using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayFabMain : MonoBehaviour
{

    public GameObject exitMenu;
    [Header("For Top 11 DT")]
    public GameObject rowPrefab;
    public Transform rowParent;

    [Header("For Top 11 CPS")]
    public GameObject rowPrefabCPS;
    public Transform rowParentCPS;

    
    [Header("For Top 11 Reflex")]
    public GameObject rowPrefabReflex;
    public Transform rowParentReflex;

    [Header("For Top 3 Reflex")]
    public GameObject rowPrefabThreeReflex;
    public Transform rowParentThreeReflex;

    [Header("Windows")]
    public GameObject nameWindow;
    public GameObject leaderboardWindow;
    public GameObject leaderboardWindowCPS;
    public GameObject leaderboardWindowReflex;

    [Header("Display name window")]
    public InputField nameInput;

    string loggedInPlayFabID;

    private void Start()
    {
        Login();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            exitMenu.gameObject.SetActive(true);
        }
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
        loggedInPlayFabID = result.PlayFabId;
        //Debug.Log("Successful login/account creating!");
        string name = null;
        if (result.InfoResultPayload.PlayerProfile != null)
        {
            name = result.InfoResultPayload.PlayerProfile.DisplayName;
        }

        if (name == null)
        {
            nameWindow.SetActive(true);
        }
        else
        {
            //leaderboardWindow.SetActive(true);
        }
        GetLeaderboardReflex();
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Error While logging in/creating account!");
        Debug.Log(error.GenerateErrorReport());
    }

    public void SubmitNameButton()
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = nameInput.text,
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
    }
    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        nameWindow.SetActive(false);

    }
    

    public void GetLeaderboardAroundPlayer()
    {
        var request = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = "DoubleTapLeader",
            MaxResultsCount = 11
        };
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnleaderboardAroundPlayerGet, OnError);
    }

    void OnleaderboardAroundPlayerGet (GetLeaderboardAroundPlayerResult result)
    {
        foreach (Transform item in rowParent)
        {
            Destroy(item.gameObject);
        }



        foreach (var item in result.Leaderboard)
        {
            GameObject newGo = Instantiate(rowPrefab, rowParent);
            Text[] texts = newGo.GetComponentsInChildren<Text>();
            texts[0].text = (item.Position + 1).ToString();
            texts[1].text = item.DisplayName;
            texts[2].text = (item.StatValue * -1).ToString();

            if(item.PlayFabId == loggedInPlayFabID)
            {
                texts[0].color = Color.green;
                texts[1].color = Color.green;
                texts[2].color = Color.green;
            }
        }
    }


































    

    public void GetLeaderboardAroundPlayerCPS()
    {
        var request = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = "CPSLeader",
            MaxResultsCount = 11
        };
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnleaderboardAroundPlayerGetCPS, OnError);
    }

    void OnleaderboardAroundPlayerGetCPS(GetLeaderboardAroundPlayerResult result)
    {
        foreach (Transform item in rowParentCPS)
        {
            Destroy(item.gameObject);
        }



        foreach (var item in result.Leaderboard)
        {
            GameObject newGo = Instantiate(rowPrefabCPS, rowParentCPS);
            Text[] texts = newGo.GetComponentsInChildren<Text>();
            texts[0].text = (item.Position + 1).ToString();
            texts[1].text = item.DisplayName;
            texts[2].text = (item.StatValue).ToString();

            if (item.PlayFabId == loggedInPlayFabID)
            {
                texts[0].color = Color.green;
                texts[1].color = Color.green;
                texts[2].color = Color.green;
            }
        }
    }

















    public void GetLeaderboardReflex()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "ReflexLeaderboard",
            StartPosition = 0,
            MaxResultsCount = 3
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGetReflex, OnError);
    }

    void OnLeaderboardGetReflex(GetLeaderboardResult result)
    {
        foreach (Transform item in rowParentThreeReflex)
        {
            Destroy(item.gameObject);
        }



        foreach (var item in result.Leaderboard)
        {
            GameObject newGo = Instantiate(rowPrefabThreeReflex, rowParentThreeReflex);
            Text[] texts = newGo.GetComponentsInChildren<Text>();
            texts[0].text = (item.Position + 1).ToString();
            texts[1].text = item.DisplayName;
            texts[2].text = (item.StatValue).ToString();
        }
    }

    public void GetLeaderboardAroundPlayerReflex()
    {
        var request = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = "ReflexLeaderboard",
            MaxResultsCount = 11
        };
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnleaderboardAroundPlayerGetReflex, OnError);
    }

    void OnleaderboardAroundPlayerGetReflex(GetLeaderboardAroundPlayerResult result)
    {
        foreach (Transform item in rowParentReflex)
        {
            Destroy(item.gameObject);
        }



        foreach (var item in result.Leaderboard)
        {
            GameObject newGo = Instantiate(rowPrefabReflex, rowParentReflex);
            Text[] texts = newGo.GetComponentsInChildren<Text>();
            texts[0].text = (item.Position + 1).ToString();
            texts[1].text = item.DisplayName;
            texts[2].text = (item.StatValue).ToString();

            if (item.PlayFabId == loggedInPlayFabID)
            {
                texts[0].color = Color.green;
                texts[1].color = Color.green;
                texts[2].color = Color.green;
            }
        }
    }













    public void ShowLeaderboardBtn()
    {
        GetLeaderboardAroundPlayer();
        leaderboardWindow.gameObject.SetActive(true);
    }
    public void HideLeaderboardBtn()
    {
        leaderboardWindow.gameObject.SetActive(false);

    }
    public void ShowLeaderboardBtnCPS()
    {
        GetLeaderboardAroundPlayerCPS();
        leaderboardWindowCPS.gameObject.SetActive(true);
    }
    public void HideLeaderboardBtnCPS()
    {
        leaderboardWindowCPS.gameObject.SetActive(false);

    }

    public void ShowLeaderboardBtnReflex()
    {
        GetLeaderboardAroundPlayerReflex();
        leaderboardWindowReflex.gameObject.SetActive(true);
    }
    public void HideLeaderboardBtnReflex()
    {
        leaderboardWindowReflex.gameObject.SetActive(false);

    }

    public void ExtitGame()
    {
        Application.Quit();
    }
    public void CloseExitWindow()
    {
        exitMenu.gameObject.SetActive(false);
    }
}
