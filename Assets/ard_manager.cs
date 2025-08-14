using UnityEngine;
using System.IO.Ports;

public class ArduinoManager : MonoBehaviour
{
    public static ArduinoManager Instance { get; private set; }

    [Header("Serial")]
    public string portName = "COM3";   // <- set this to your board's port
    public int baudRate = 9600;

    private SerialPort port;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        try
        {
            port = new SerialPort(portName, baudRate)
            {
                DtrEnable = false, // avoid auto-reset if you like
                RtsEnable = false,
                ReadTimeout = 50,
                WriteTimeout = 50
            };
            port.Open();
            Debug.Log("✅ Arduino connected on " + portName);
        }
        catch (System.Exception e)
        {
            Debug.LogError("❌ Could not open port: " + e.Message);
        }
    }

    public void Send(string message)
    {
        if (port != null && port.IsOpen)
        {
            port.WriteLine(message);
            Debug.Log("➡️ Sent: " + message);
        }
        else
        {
            Debug.LogWarning("Serial not open; cannot send: " + message);
        }
    }

    // Convenience methods:
    public void StartTimer() => Send("START_TIMER");
    public void Pause() => Send("PAUSE");
    public void Resume() => Send("RESUME");
    public void Win() => Send("WIN");
    public void Lose() => Send("LOSE");
    public void OLED(string text) => Send("OLED:" + text);

    void OnApplicationQuit()
    {
        if (port != null && port.IsOpen) port.Close();
    }
}
