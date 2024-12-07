using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayFabManager : MonoBehaviour
{
    public static PlayFabManager Instance;
    public int HighScore;
    public string Name;
    public GameObject listingPrefab;
    public Transform listingContainer;
    private void Awake()
    {
        Name = null;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        if (!loggedin)login();
            
    }

    public List<TMP_Text> ScoresList = new List<TMP_Text>(10);
    public List<TMP_Text> NamesList = new List<TMP_Text>(10);
    public TMP_Text NameErrortxt;
    public GameObject inputField;
    public bool loggedin = false;
    public bool isNameValid;





    /// <summary>
    /// Login
    /// </summary>
    /// 
    private string _randomStringGenerator;
    public void login()
    {
        _randomStringGenerator = PlayerPrefs.GetString("DeviceID");

        if (string.IsNullOrEmpty(_randomStringGenerator)||string.IsNullOrWhiteSpace(_randomStringGenerator))
        {
            _randomStringGenerator = RandomStringGenerator(15);
            PlayerPrefs.SetString("DeviceID", _randomStringGenerator);
            Debug.Log(_randomStringGenerator + " " + "GeneratingNewId");
        }
        else
            Debug.Log("ID IS ALREADY GENERATED !!!!!!");
        var request = new LoginWithCustomIDRequest
        {
            CustomId = _randomStringGenerator,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams { GetPlayerProfile = true }
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnErrorLogin);
    }
    string RandomStringGenerator(int length)
    {
        string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        string generated_string = "";

        for (int i = 0; i < length; i++)
            generated_string += characters[Random.Range(0, length)];

        return generated_string;
    }
    void OnSuccess(LoginResult result)
    {
        Debug.Log("Loggedin");
        loggedin = true;
      
        if (result.InfoResultPayload.PlayerProfile != null) 
        { 
            Name = result.InfoResultPayload.PlayerProfile.DisplayName;
            if (Name == null) { if (inputField != null) inputField.SetActive(true); }
            else { if (inputField != null) inputField.SetActive(false); }
        }
        GetHighscore();
    }
    void OnErrorLogin(PlayFabError error)
    {
        login();
        Debug.Log("error");
    }


    /// <summary>
    ///             Highscore update
    /// </summary>
    /// <param name="highscore"></param>
    public void UploadHighScore(int highscore)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "Platform leaderboard", Value = highscore
                }
            }
        };
        var request2 = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string> { { "HighScore",  highscore.ToString() } }
        };
        PlayFabClientAPI.UpdateUserData(request2, OnUpdateUserData, OnErrorLogin);
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnErrorLogin);
        HighScore = highscore;

    }
    void OnUpdateUserData(UpdateUserDataResult result)
    {
        Debug.Log("Score saved In profile");
    }
    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("score updated");
    }

    /// <summary>
    ///             Leaderboard Get
    /// </summary>
    public void GetLeaderboard()
    {

        var request = new GetLeaderboardRequest
        {
            StatisticName = "Platform leaderboard",
            StartPosition = 0,
            MaxResultsCount = 50,
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeadeboardGet, OnErrorLogin);
    }
    void OnLeadeboardGet(GetLeaderboardResult result)
    {
        for (int i = 0; i < listingContainer.childCount; i++)
        {
            Destroy(listingContainer.GetChild(i).gameObject);
        }
        foreach (var item in result.Leaderboard)
        {

            GameObject tempListing = Instantiate(listingPrefab, listingContainer);
            ScoreboardListing LL = tempListing.GetComponent<ScoreboardListing>();
            LL.playerPositionText.text = (1 + item.Position).ToString();
            LL.playerNameText.text = item.DisplayName;
            LL.playerScoreText.text = item.StatValue.ToString();
           

        }
        //foreach (var item in result.Leaderboard)
        //{
        //    ScoresList[item.Position].text = item.StatValue.ToString();
        //    NamesList[item.Position].text = item.DisplayName;
        //}
    }
    //public void LinkLeaderboardtoUI()
    //{
    //    for (int i = 0; i < 50; i++)
    //    {
    //        ScoresList[i] = GameObject.Find("Score"+i).GetComponent<TMP_Text>();
    //        NamesList[i] = GameObject.Find("Name"+i).GetComponent<TMP_Text>();
    //    }
    //}

    /// <summary>
    ///             Name Set
    /// </summary>
    /// <param name="Name"></param>
    public void SetName(string name)
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = name
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnNameSave, OnErrorName);
    }
    void OnNameSave(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("name saved");
        Name = result.DisplayName;
        inputField.SetActive(false);
        isNameValid = true;
        MainMenu.Instance.Play();
    }
    void OnErrorName(PlayFabError error)
    {
        isNameValid = false;
        NameErrortxt.gameObject.SetActive(true);
        NameErrortxt.text = error.ErrorMessage;
    }
    /// <summary>
    ///             Get HighScore
    /// </summary>
    public void GetHighscore()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnHighscoreGet, OnErrorLogin);
    }
    void OnHighscoreGet(GetUserDataResult result)
    {
        if (result.Data != null && result.Data.ContainsKey("HighScore")) { HighScore = int.Parse(result.Data["HighScore"].Value); }
        else HighScore = 0;
        MainMenu.Instance.HighScoreDisplay();
    }
    
}