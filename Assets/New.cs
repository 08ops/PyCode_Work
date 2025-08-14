using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO.Ports;
using System.Collections;

public class COMPortScanner : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] private RectTransform buttonParent;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Button playButton;

    [Header("Connection Settings")]
    [SerializeField] private int baudRate = 9600;
    [SerializeField] private int readTimeout = 1000;
    [SerializeField] private float arduinoResetTime = 1.5f;

    private string selectedPort;

    void Start()
    {
        playButton.interactable = false;
        ScanPorts();
    }

    public void ScanPorts()
    {
        // Clear existing buttons (FIXED: Removed parentheses from gameObject)
        foreach (Transform child in buttonParent)
            Destroy(child.gameObject); // Changed from Destroy(child.gameObject())

        string[] ports = SerialPort.GetPortNames();
        foreach (string port in ports)
            StartCoroutine(CheckPortCoroutine(port));
    }

    private IEnumerator CheckPortCoroutine(string portName)
    {
        SerialPort port = new SerialPort(portName, baudRate);
        port.ReadTimeout = readTimeout;
        bool portOpened = false;

        // Phase 1: Try to open port
        try
        {
            port.Open();
            portOpened = true;
        }
        catch
        {
            yield break;
        }

        // Phase 2: Wait for Arduino reset
        yield return new WaitForSeconds(arduinoResetTime);

        // Phase 3: Try to read identification
        try
        {
            while (port.BytesToRead > 0)
                port.ReadExisting();

            string response = port.ReadLine().Trim();
            if (response == "MyGameBoard")
                CreatePortButton(portName);
        }
        catch { }
        finally
        {
            if (portOpened && port.IsOpen)
                port.Close();
        }
    }

    void CreatePortButton(string portName)
    {
        GameObject newBtn = Instantiate(buttonPrefab, buttonParent);
        TMP_Text btnText = newBtn.GetComponentInChildren<TMP_Text>();
        Button btnComponent = newBtn.GetComponent<Button>();

        btnText.text = portName;
        btnComponent.onClick.AddListener(() => {
            selectedPort = portName;
            playButton.interactable = true;

            // Visual feedback
            foreach (Transform child in buttonParent)
                child.GetComponent<Image>().color = Color.gray;
            newBtn.GetComponent<Image>().color = Color.green;
        });
    }

    public void StartGame()
    {
        if (!string.IsNullOrEmpty(selectedPort))
        {
            PlayerPrefs.SetString("SelectedCOMPort", selectedPort);
            // UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
        }
    }

    public void RefreshPorts()
    {
        ScanPorts();
        playButton.interactable = false;
    }
}