using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CPSManager : MonoBehaviour
{
    int count = 0;
    int bestCount;

    float startTime = 1;
    public Button clickBtn;
    public GameObject pauseMenu;


    public Text counterText;
    public Text timertext;
    public Text currentCountText;
    public Text BestCountText;


    bool allowed = false;


    private void Awake()
    {
        timertext.text ="1.00";
        bestCount = PlayerPrefs.GetInt("BestCPS", 0);
    }









    private void Start()
    {
        Login();
        Fun();
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
                    StatisticName = "CPSLeader",Value = score
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
        if(allowed && startTime > 0)
        {
            startTime -= 1 * Time.deltaTime;
            timertext.text = startTime.ToString("F2");

        }

        if (startTime <= 0)
        {
            clickBtn.gameObject.SetActive(false);
            pauseMenu.gameObject.SetActive(true);
            counterText.gameObject.SetActive(false);
            timertext.text = "0.00";


            BestCountText.text = "Best CPS: " + PlayerPrefs.GetInt("BestCPS", 0).ToString();
            currentCountText.text = "Current CPS: " + count.ToString();




            if (count > bestCount)
            {
                PlayerPrefs.SetInt("BestCPS", count);
                SendLeaderboard(count);
            }





            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(0);
            }

        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
            

    }
    public void Button()
    {
        allowed = true;

        counterText.gameObject.SetActive(true);

        count++;
        counterText.text = count.ToString();
    }
    void Fun()
    {
        int value = PlayerPrefs.GetInt("Interstitial", 0);
        PlayerPrefs.SetInt("Interstitial", value + 1);
        Debug.Log("DT MANAGER" + value);
    }
    
}
