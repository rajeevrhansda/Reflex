using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DTUIManager : MonoBehaviour
{
    private PlayerFab _playerFab;
    float startTime;
    public Text countText;
    public Text bestText;

    public Button startBtn;
    public Button stopBtn;

    public GameObject pauseMenu;
    public Text result;


    float bestTime;

    bool timeActive = false;

    private void Awake()
    {
        _playerFab = FindObjectOfType<PlayerFab>();
        bestTime = PlayerPrefs.GetFloat("BestDT",100000000000000000000000000000000.00f);
        if(bestTime>100)
        {
            bestText.text = "First Time";
        }
        else
        {
            bestText.text = "Best Time: "+PlayerPrefs.GetFloat("BestDT", 100000000000000000000000000000000.00f).ToString("F4");
        }
        
        startBtn.gameObject.SetActive(true);
        stopBtn.gameObject.SetActive(false);

        pauseMenu.gameObject.SetActive(false);
    }
    private void Update()
    {
        if(timeActive)
        {
            
            startTime += 1 * Time.deltaTime;
            countText.text = startTime.ToString("F4");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }

    }

    public void StoptTimeBtn()
    {
        timeActive = false;
        if(bestTime > startTime)
        {
            PlayerPrefs.SetFloat("BestDT", startTime);
            bestText.text = "Best Time: " + startTime.ToString("F4");

            float num1 = startTime * 10000;
            int num2 = (int)num1;

            _playerFab.SendLeaderboard(num2*-1);
        }
        else
        {
            bestText.text = "Best Time: " + PlayerPrefs.GetFloat("BestDT", 100000000000000000000000000000000.00f).ToString("F4");
        }
        startBtn.gameObject.SetActive(false);
        stopBtn.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(true);
        
        result.text ="Your time is: "+startTime.ToString("F4");

        int value = PlayerPrefs.GetInt("Interstitial", 0);
        PlayerPrefs.SetInt("Interstitial", value + 1);
        Debug.Log("DT MANAGER" + value);





    }

    public void StartTimeBtn()
    {
        timeActive = true;
        startBtn.gameObject.SetActive(false);
        stopBtn.gameObject.SetActive(true);

    }
    public void LoadScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
    public void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }
}
