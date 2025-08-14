using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // UI References
    public TMP_Text timerText;
    public TMP_InputField codeInput;
    public Button submitButton;
    public Button helpButton;

    [SerializeField] bool timeOn;
    // Main buttons
    public Button pauseButton;
    public Button resumeButton;
    public Button nextLevelButton;
    public Button retryButton; // First retry button
    public Button retryButton2; // Second retry button
    public Button retryButton3; // Second retry button
    public Button menuButton;
    public Button inGameResume;


    // Optional button arrays
    public Button[] extraPauseButtons;
    public Button[] extraResumeButtons;

    // Panels
    public GameObject tutorialPanel;
    public GameObject successPanel;
    public GameObject failurePanel;
    public GameObject pausePanel;

    // Stars
    public Image[] starIcons;
    public Sprite filledStar;
    public Sprite emptyStar;

    // Game State
    private float timeRemaining = 180f;
    private bool isPaused;
    private bool levelCompleted;
    private bool timerRunning = true;

    string levelName;

    void Start()
    {
        levelName = SceneManager.GetActiveScene().name;
        // Core button bindings
        submitButton.onClick.AddListener(ValidateCode);
        helpButton.onClick.AddListener(ShowTutorial);

        // Main functionality buttons
        pauseButton.onClick.AddListener(PauseGame);
        resumeButton.onClick.AddListener(ResumeGame);
        inGameResume.onClick.AddListener(HideTutorial);
        retryButton.onClick.AddListener(RetryLevel);
        retryButton2.onClick.AddListener(RetryLevel); // Second retry button
        retryButton3.onClick.AddListener(RetryLevel);
        menuButton.onClick.AddListener(ReturnToMenu);

        // Bind any extra buttons
        BindExtraButtons(extraPauseButtons, PauseGame);
        BindExtraButtons(extraResumeButtons, ResumeGame);
        timeOn = false;
        InitializeGame();
    }

    void BindExtraButtons(Button[] buttons, UnityEngine.Events.UnityAction action)
    {
        if (buttons == null || buttons.Length == 0) return;

        foreach (Button btn in buttons)
        {
            if (btn != null) btn.onClick.AddListener(action);
        }
    }

    public void TimerVisible()
    {
        timeOn = !timeOn;
    }

    void Update()
    {
        if (timerRunning && !isPaused && !levelCompleted)
        {
            timeRemaining -= Time.deltaTime;
            if (timeOn)
            {
                UpdateTimerDisplay(timeRemaining);
            }
            else
            {
                timerText.gameObject.SetActive(false);
            }

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                timerRunning = false;
                LevelFailed();
            }
        }
    }

    void InitializeGame()
    {
        timeRemaining = 180f;
        timerRunning = true;
        isPaused = false;
        levelCompleted = false;
        HideAllPanels();
        //UpdateTimerDisplay(timeRemaining);
    }

    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    public void ShowTutorial()
    {
        tutorialPanel.SetActive(true);
        codeInput.interactable = false;
        submitButton.interactable = false;
    }

    void HideTutorial()
    {
        tutorialPanel.SetActive(false);
        codeInput.interactable = true;
        submitButton.interactable = true;
        ResumeGame();
    }

    void ValidateCode()
    {
        if (IsCodeValid(codeInput.text.Trim()))
        {
            timerRunning = false;
            LevelCompleted();
        }
    }

    bool IsCodeValid(string code)
    {
        return code.Contains("print(\"Hello, World!\")") ||
               code.Contains("print('Hello, World!')");
    }

    void LevelCompleted()
    {
        levelCompleted = true;
        successPanel.SetActive(true);
        AwardStars(CalculateStars());
    }

    void LevelFailed()
    {
        levelCompleted = true;
        failurePanel.SetActive(true);
    }

    int CalculateStars()
    {
        float timeUsed = 180f - timeRemaining;
        if (timeUsed <= 60f) return 3;
        if (timeUsed <= 120f) return 2;
        return 1;
    }

    void AwardStars(int stars)
    {
        for (int i = 0; i < starIcons.Length; i++)
        {
            starIcons[i].sprite = i < stars ? filledStar : emptyStar;
            starIcons[i].gameObject.SetActive(true);
        }
        PlayerPrefs.SetInt(levelName, stars);
    }

    void UpdateTimerDisplay(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timerText.gameObject.SetActive(true);

        timerText.text = $"{minutes:00}:{seconds:00}";

    }

    void HideAllPanels()
    {
        tutorialPanel.SetActive(false);
        successPanel.SetActive(false);
        failurePanel.SetActive(false);
        pausePanel.SetActive(false);
    }

    void LoadNextLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void RetryLevel() // Fully implemented retry method
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void ReturnToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}