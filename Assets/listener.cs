using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO.Ports;
using System.Collections;
using System.Collections.Generic;

public class Listener : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private RectTransform buttonParent; // PortsPanel
    [SerializeField] private GameObject buttonPrefab;    // PortButton (prefab)
    [SerializeField] private Button playButton;          // PlayButton

    [Header("Serial Settings")]
    [SerializeField] private int baudRate = 9600;
    [SerializeField] private int readTimeout = 1000;
    [SerializeField] private float arduinoResetTime = 2f;

    private string selectedPort;
    private bool portFound = false;
    private readonly List<Button> portButtons = new List<Button>();

    void Start()
    {
        playButton.interactable = false;
        ScanPorts();
    }

    public void ScanPorts()
    {
        // Clear old
        foreach (Transform child in buttonParent)
            Destroy(child.gameObject);
        portButtons.Clear();
        portFound = false;
        selectedPort = null;
        playButton.interactable = false;

        // Scan all ports
        foreach (string port in SerialPort.GetPortNames())
            StartCoroutine(CheckPortRoutine(port));
    }

    private IEnumerator CheckPortRoutine(string portName)
    {
        SerialPort port = null;
        bool portOpened = false;

        // Phase 1: open
        try
        {
            port = new SerialPort(portName, baudRate) { ReadTimeout = readTimeout, NewLine = "\n" };
            port.Open();
            portOpened = true;
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"Failed to open {portName}: {e.Message}");
            yield break;
        }

        // Phase 2: wait for board reset
        yield return new WaitForSeconds(arduinoResetTime);

        // Phase 3: identify
        try
        {
            port.Write("IDENTIFY\n");
            float waitUntil = Time.time + 1.0f;
            while (port.BytesToRead == 0 && Time.time < waitUntil)
                yield return null;

            if (port.BytesToRead > 0)
            {
                string response = port.ReadLine().Trim();
                Debug.Log($"[{portName}] → {response}");
                if (response == "MyGameBoard")
                {
                    bool auto = !portFound;
                    if (auto)
                    {
                        selectedPort = portName;
                        portFound = true;
                        playButton.interactable = true;
                        Debug.Log($"Auto-selected {portName}");
                    }
                    CreatePortButton(portName, auto);
                }
            }
        }
        finally
        {
            if (portOpened && port.IsOpen) port.Close();
        }
    }

    private void CreatePortButton(string portName, bool autoSelected)
    {
        // Correct UI instantiation into your panel
        GameObject btnObj = Instantiate(buttonPrefab, buttonParent);
        btnObj.transform.SetParent(buttonParent, false);
        btnObj.SetActive(true);

        TMP_Text label = btnObj.GetComponentInChildren<TMP_Text>(true);
        if (label) label.text = autoSelected ? $"{portName} (Auto)" : portName;

        Button button = btnObj.GetComponent<Button>();
        Image bg = btnObj.GetComponent<Image>();
        if (autoSelected && bg) bg.color = new Color(0.2f, 0.8f, 0.2f);

        button.onClick.AddListener(() =>
        {
            selectedPort = portName;
            playButton.interactable = true;
            // reset all
            foreach (var b in portButtons)
            {
                var img = b.GetComponent<Image>();
                if (img) img.color = Color.white;
            }
            // highlight this
            var imgThis = button.GetComponent<Image>();
            if (imgThis) imgThis.color = new Color(0.2f, 0.8f, 0.2f);
        });

        portButtons.Add(button);
        Debug.Log($"Spawned button under: {btnObj.transform.parent.name}");
    }

    public void StartGame()
    {
        if (string.IsNullOrEmpty(selectedPort)) return;
        PlayerPrefs.SetString("SelectedCOMPort", selectedPort);
        Debug.Log($"Starting game with port: {selectedPort}");
        // UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
}
