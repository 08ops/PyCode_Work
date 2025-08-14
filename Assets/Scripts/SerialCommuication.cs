using System.IO.Ports;
using UnityEngine;

public class SerialCommunication : MonoBehaviour
{
    public SerialPort serialPort;
    public string portName = "COM3"; // Change to your ESP32's COM port
    public int baudRate = 115200;

    void Start()
    {
        serialPort = new SerialPort(portName, baudRate);
        serialPort.Open();
    }

    public void SendTimeToESP32(string message)
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.WriteLine(message);
        }
    }

    void OnDestroy()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}
