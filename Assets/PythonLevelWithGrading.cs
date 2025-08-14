using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PythonLevelWithGrading : MonoBehaviour
{
    [Header("Arduino Client")]
    public ArduinoTimerClient arduino;

    [Header("UI")]
    public TMP_Text timerText;
    public TMP_InputField codeInput;

    public Button submitButton;
    public Button pauseButton;
    public Button resumeButton;
    public Button retryButton;
    public Button menuButton;

    [Header("Panels")]
    public GameObject tutorialPanel;
    public GameObject successPanel;
    public GameObject failurePanel;
    public GameObject pausePanel;

    private float _timeRemaining = 180f; // 3 minutes countdown
    private bool isPaused = false;
    private bool levelCompleted = false;

 
    void Start()
    {
        // Initialize UI
        UpdateTimerDisplay(_timeRemaining);

        HideAllPanels();

        // Open Arduino port
        if (arduino != null) arduino.Open();

        // Bind buttons
        submitButton.onClick.AddListener(ValidateCode);
        pauseButton.onClick.AddListener(PauseGame);
        resumeButton.onClick.AddListener(ResumeGame);
        retryButton.onClick.AddListener(RetryLevel);
        menuButton.onClick.AddListener(ReturnToMenu);

        // Start countdown on Arduino
        StartTimerOnArduino();
    }

    void Update()
    {
        if (!isPaused && !levelCompleted)
        {
            _timeRemaining -= Time.deltaTime;
            if (_timeRemaining < 0) _timeRemaining = 0;

            UpdateTimerDisplay(_timeRemaining);

            if (_timeRemaining <= 0 && !levelCompleted)
            {
                LevelFailed();
            }
        }
    }

    private void StartTimerOnArduino()
    {
        if (arduino != null && arduino.IsOpen())
            arduino.SendCommand($"START 180"); // send 3 min
    }

    void PauseGame()
    {
        if (!isPaused)
        {
            isPaused = true;
            pausePanel.SetActive(true);
            Time.timeScale = 0;
            if (arduino != null && arduino.IsOpen())
                arduino.SendCommand("PAUSE");
        }
    }

    void ResumeGame()
    {
        if (isPaused)
        {
            isPaused = false;
            pausePanel.SetActive(false);
            Time.timeScale = 1;
            if (arduino != null && arduino.IsOpen())
                arduino.SendCommand("RESUME");
        }
    }

    void RetryLevel()
    {
        // Reset Unity timer
        _timeRemaining = 180f;
        levelCompleted = false;
        isPaused = false;
        Time.timeScale = 1;
        HideAllPanels();
        UpdateTimerDisplay(_timeRemaining);

        // Reset Arduino timer
        if (arduino != null && arduino.IsOpen())
            arduino.SendCommand("RESET");

        // Start countdown again
        StartTimerOnArduino();

        // Reload scene to ensure full reset
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void ValidateCode()
    {
        string code = codeInput.text.Trim();
        if (code.Contains("print(\"Hello, World!\")") || code.Contains("print('Hello, World!')"))
        {
            LevelCompleted();
        }
        else
        {
            LevelFailed();
        }
    }

    void LevelCompleted()
    {
        levelCompleted = true;
        successPanel.SetActive(true);
        if (arduino != null && arduino.IsOpen())
            arduino.SendCommand("RESET"); // stop Arduino timer on success
    }

    void LevelFailed()
    {
        levelCompleted = true;
        failurePanel.SetActive(true);
        if (arduino != null && arduino.IsOpen())
            arduino.SendCommand("RESET"); // stop Arduino timer on failure
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


    void ReturnToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
