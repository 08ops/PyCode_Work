using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public ardui arduino; // Drag your Arduino manager here

public class pythonLevelwithGrading : MonoBehaviour
{
    public float timeRemaining = 10f;
    // UI References
    public TMP_Text timerText;
    public TMP_InputField codeInput;
    public Button submitButton;
    public Button helpButton;

    // Main buttons
    public Button pauseButton;
    public Button resumeButton;
    public Button nextLevelButton;
    public Button retryButton; // First retry button
    public Button retryButton2; // Second retry button
    public Button menuButton;

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
    private float _timeRemaining;
    private bool isPaused;
    private bool levelCompleted;
    private bool timerRunning = true;

    void Start()
    {
        _timeRemaining = timeRemaining;
        // Core button bindings
        submitButton.onClick.AddListener(ValidateCode);
        helpButton.onClick.AddListener(ShowTutorial);

        // Main functionality buttons
        pauseButton.onClick.AddListener(PauseGame);
        resumeButton.onClick.AddListener(ResumeGame);
        nextLevelButton.onClick.AddListener(LoadNextLevel);
        retryButton.onClick.AddListener(RetryLevel);
        retryButton2.onClick.AddListener(RetryLevel); // Second retry button
        menuButton.onClick.AddListener(ReturnToMenu);

        // Bind any extra buttons
        BindExtraButtons(extraPauseButtons, PauseGame);
        BindExtraButtons(extraResumeButtons, ResumeGame);

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

    void Update()
    {
        if (timerRunning && !isPaused && !levelCompleted)
        {
            _timeRemaining -= Time.deltaTime;
            UpdateTimerDisplay(_timeRemaining);

            if (_timeRemaining <= 0)
            {
                _timeRemaining = 0;
                timerRunning = false;
                LevelFailed();
            }
        }
    }

    void InitializeGame()
    {
        _timeRemaining = 180f;
        timerRunning = true;
        isPaused = false;
        levelCompleted = false;
        HideAllPanels();
        UpdateTimerDisplay(_timeRemaining);
    }

    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }

    void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    void ShowTutorial()
    {
        PauseGame();
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
        float timeUsed = 180f - _timeRemaining;
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
    }

    void UpdateTimerDisplay(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
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