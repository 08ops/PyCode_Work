using UnityEngine;
using UnityEngine.UI;

public class Level1Bindings : MonoBehaviour
{
    [Header("Optional UI Buttons")]
    public Button pauseButton;
    public Button resumeButton;

    [Header("Auto start at scene load")]
    public bool startTimerOnAwake = true;

    void Start()
    {
        if (startTimerOnAwake)
            ArduinoManager.Instance?.StartTimer(); // starts 3:00 on OLED

        if (pauseButton != null)
            pauseButton.onClick.AddListener(() => ArduinoManager.Instance?.Pause());

        if (resumeButton != null)
            resumeButton.onClick.AddListener(() => ArduinoManager.Instance?.Resume());
    }
}
