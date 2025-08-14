using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO.Ports;
using System.Collections.Generic;

public class listen : MonoBehaviour
{
    [Header("UI References")]
    public GameObject buttonPrefab;
    public Transform buttonParent;
    public Button playButton;

    private string selectedPort;
    private List<string> currentPorts = new List<string>();

    void Start()
    {
        playButton.interactable = false; // Disable until port selected
        InvokeRepeating(nameof(RefreshPortList), 0f, 2f); // Refresh every 2 seconds
    }

    void RefreshPortList()
    {
        string[] ports = SerialPort.GetPortNames();

        if (HasPortListChanged(ports))
        {
            ClearButtons();
            foreach (string port in ports)
            {
                GameObject btnObj = Instantiate(buttonPrefab, buttonParent);
                btnObj.GetComponentInChildren<TextMeshProUGUI>().text = port;

                btnObj.GetComponent<Button>().onClick.AddListener(() => SelectPort(port));
            }
            currentPorts = new List<string>(ports);
        }
    }

    bool HasPortListChanged(string[] newPorts)
    {
        if (newPorts.Length != currentPorts.Count)
            return true;

        for (int i = 0; i < newPorts.Length; i++)
        {
            if (newPorts[i] != currentPorts[i])
                return true;
        }
        return false;
    }

    void ClearButtons()
    {
        foreach (Transform child in buttonParent)
            Destroy(child.gameObject);
    }

    void SelectPort(string port)
    {
        selectedPort = port;
        playButton.interactable = true; // Enable play button
    }

    public void PlayGame()
    {
        if (!string.IsNullOrEmpty(selectedPort))
        {
            PlayerPrefs.SetString("SelectedCOMPort", selectedPort);
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
        }
    }
    public class ConnectionManager : MonoBehaviour
    {
        public ArduinoTimerClient arduino; // drag the ArduinoLink here in the Inspector

        // Call this when the user clicks a port button (pass "COM5", "COM7", etc.)
        public void OnUserPickedPort(string portName)
        {
            arduino.portName = portName;
            arduino.Open();
        }
    }

}
