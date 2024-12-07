using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    #region other Variables
    [SerializeField] public float SpeedMultiplier;
    public UnityEvent GameOverEvent;
    public bool GameOver;
    private PlayerBehaviour player;
    PlayFabManager playfab;
    #endregion
    #region Score Variables
    public int Score;
    private float scoreTimer = 0f;
    #endregion
    [Header("UI:")][Space(5)]
    #region UI
    private TMP_Text ScoreDisplay;
    public GameObject PausePanel;
    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
        playfab = FindObjectOfType<PlayFabManager>().GetComponent<PlayFabManager>();
    }

    private void Start()
    {
        ScoreDisplay = GameObject.Find("Score Display").GetComponent<TMP_Text>();
        player = GameObject.Find("Player").GetComponent<PlayerBehaviour>();
        Score = 0;
        SpeedMultiplier = 1;
        Time.timeScale = 1;
        ScoreDisplay.text = "0";
        GameOver = false; 
    }
    private void OnEnable()
    {
            
    }

    private void Update()
    {
        if (!GameOver)
        {
            if (SpeedMultiplier < 2) SpeedMultiplier += 0.02f * Time.deltaTime; 
            if (scoreTimer < 0.5f) scoreTimer += Time.deltaTime * SpeedMultiplier;
            else
            {
                AddScore(1);
                scoreTimer = 0f;
            }
            if (Input.GetKeyDown(KeyCode.Escape)) Pause();
        }
    }
    public void AddScore(int score)
    {
        Score += score;
        ScoreDisplay.text = $"{Score.ToString()}";
    }
    public void GetLeaderboardDone() => playfab.GetLeaderboard();
    public void OnGameOver()
    {
        GameOver = true;
        SpeedMultiplier = 0f;
        player.OnGameOver();
        if (Score > PlayFabManager.Instance.HighScore)
        {
            PlayFabManager.Instance.UploadHighScore(Score);
        }
    }
    public void PlayClick() => FindObjectOfType<AudioManager>().PlaySound("Click");

    public void Pause()
    {
        Time.timeScale = 0;
        PausePanel.SetActive(true);
    }
    public void UnPause() => Time.timeScale = 1f;
    public void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    public void GoTMenu() => SceneManager.LoadScene(0);
}
