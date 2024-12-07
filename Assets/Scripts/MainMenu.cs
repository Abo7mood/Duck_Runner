using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject UsernameInput;
    [SerializeField] private TMP_Text HighscoreDisp;

    private void Start()
    {
        HighScoreDisplay();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Play();
            PlayClick();
        }
    }
    public void HighScoreDisplay()
    {
        string v = $"HighScore : {PlayFabManager.Instance.HighScore}";
        HighscoreDisp.text = v;
    }

    public void SetName() => PlayFabManager.Instance.SetName(UsernameInput.GetComponent<TMP_InputField>().text);
    public void LeaderboardUIcall() => PlayFabManager.Instance.GetLeaderboard();
    #region Loading Screen
    public void Play()
    {
        if (PlayFabManager.Instance.Name != null)
            StartCoroutine(Loading());
        else SetName();
    }
    public void PlayClick() => FindObjectOfType<AudioManager>().PlaySound("Click");

        IEnumerator Loading()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            slider.value = operation.progress / 0.9f;
            yield return null;
        }
    }
    #endregion

}
