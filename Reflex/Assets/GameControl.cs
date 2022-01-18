using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using PlayFab;
using PlayFab.ClientModels;

public class GameControl : MonoBehaviour
{
    [SerializeField]
    public GameObject target;
    public Button startBtn;
    public Text currentScoreText;
    public Text currentScoreTextEndMenu;
    public Text bestScoreTextEndMenu;
    
    public GameObject endMenu;

    
    float time = 1f;



    private Vector2 targetRandomPosition;
    private bool gameOver = false;

    public static int score, currentScore, highScore;

    private void Awake()
    {
        score = 0;
        currentScore = 0;
        highScore = PlayerPrefs.GetInt("ReflesHighScore", 0);
    }
    private void Start()
    {
        Login();

    }
    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }
    void OnSuccess(LoginResult result)
    {
        //Debug.Log("Successful login/account creating!");
        string name = null;
        if (result.InfoResultPayload.PlayerProfile != null)
        {
            name = result.InfoResultPayload.PlayerProfile.DisplayName;
        }
    }



    void OnError(PlayFabError error)
    {
        Debug.Log("Error While logging in/creating account!");
        Debug.Log(error.GenerateErrorReport());
    }

    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "ReflexLeaderboard",Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successfull leaderboard sent");
    }

    private void Update()
    {
        


        
       

        if (time < 0.3f)
        {
            time = 1f;
        }

        currentScoreText.text = currentScore.ToString();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }


    }

    private IEnumerator SpawnTarget()
    {
        if (score < 6)
        {


            targetRandomPosition = new Vector2(Random.Range(-1.5f, 1.5f), Random.Range(-3f, 3f));
            Instantiate(target, targetRandomPosition, Quaternion.identity);
            yield return new WaitForSeconds(time);
            time -= 0.01f;
            StartCoroutine("SpawnTarget");
        }
        else
        {
            
            if (currentScore > highScore)
            {
                highScore = currentScore;
                PlayerPrefs.SetInt("ReflesHighScore", highScore);
                SendLeaderboard(highScore);

            }
            StartCoroutine("EndMenu");


        }

    }

    public void StartGame()
    {
        StartCoroutine("SpawnTarget");
        startBtn.gameObject.SetActive(false);
    }

    private IEnumerator EndMenu()
    {
        currentScoreTextEndMenu.text = "Current \n" + currentScore.ToString();
        bestScoreTextEndMenu.text = "Best Score\n" + highScore.ToString();
        yield return new WaitForSeconds(2f);
        endMenu.gameObject.SetActive(true);

        int value = PlayerPrefs.GetInt("Interstitial", 0);
        PlayerPrefs.SetInt("Interstitial", value + 1);

    }


}