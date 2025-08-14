using System.IO.Ports;
using UnityEngine;
using System;

public class ArduinoTimerClient : MonoBehaviour
{
    [Header("Serial Settings")]
    public string portName = "COM6"; // Set your COM port via Inspector
    public int baudRate = 115200;

    [Header("Debug")]
    public bool debug = true; // Enable/disable debug logs

    private SerialPort serial;

    // ------------------- Open Port -------------------
    void Start()
    {
        Open();
    }
    public void Open()
    {
        if (serial != null && serial.IsOpen) return;

        try
        {
            serial = new SerialPort(portName, baudRate);
            serial.NewLine = "\n"; // Matches Arduino Serial.println
            serial.ReadTimeout = 500; // short timeout for Unity reads
            serial.Open();

            if (serial.IsOpen)
                Log($"Port {portName} opened successfully.");
            else
                LogWarning($"Port {portName} failed to open.");
        }
        catch (Exception ex)
        {
            LogWarning($"Failed to open port {portName}: {ex.Message}");
        }
    }

    // ------------------- Close Port -------------------
    public void Close()
    {
        if (serial != null && serial.IsOpen)
        {
            serial.Close();
            Log($"Port {portName} closed.");
        }
    }

    // ------------------- Send Command -------------------
    public void SendCommand(string cmd)
    {
        if (serial != null && serial.IsOpen)
        {
            try
            {
                serial.WriteLine(cmd);
                serial.BaseStream.Flush();
                Log($"Sent: {cmd}");
            }
            catch (Exception ex)
            {
                LogWarning($"Failed to send '{cmd}': {ex.Message}");
            }
        }
        else
        {
            LogWarning($"Port not open. Cannot send: {cmd}");
        }
    }

    // ------------------- Send START with seconds -------------------
    public void StartTimer(int seconds)
    {
        SendCommand($"START {seconds}");
    }

    // ------------------- Read response from Arduino -------------------
    public string ReadResponse()
    {
        if (serial != null && serial.IsOpen)
        {
            try
            {
                return serial.ReadLine().Trim();
            }
            catch (TimeoutException)
            {
                return null; // no data available
            }
            catch (Exception ex)
            {
                LogWarning($"Read error: {ex.Message}");
            }
        }
        return null;
    }

    // ------------------- Check if port is open -------------------
    public bool IsOpen()
    {
        return serial != null && serial.IsOpen;
    }

    // ------------------- Auto Close -------------------
    private void OnApplicationQuit()
    {
        Close();
    }

    // ------------------- Logging -------------------
    private void Log(string msg)
    {
        if (debug) Debug.Log($"[ArduinoTimerClient] {msg}");
    }

    private void LogWarning(string msg)
    {
        if (debug) Debug.LogWarning($"[ArduinoTimerClient] {msg}");
    }
}
