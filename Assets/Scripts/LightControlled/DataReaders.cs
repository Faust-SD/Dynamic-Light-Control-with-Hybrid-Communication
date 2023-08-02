/*
文件名: DataReaders.cs
作者: Faust-SD
联系方式: 2950068710z@gmail.com
个人空间: https://space.bilibili.com/179771012
日期: Created on Tue Aug  2 00:00:01 2023
版本: v1.0.1
版权所有: (C) Copyright 2023 Faust-SD, All Rights Reserved.
许可协议: MIT License
*/
using System.IO;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;
using UnityEngine;
using System.Collections.Generic;

public class DataReaders : MonoBehaviour
{
    // Communication types
    public enum CommunicationType
    {
        Serial,
        UDP
    }

    // Variables for communication
    private Thread communicationThread;
    private SerialPort serialPort;
    private UdpClient udpClient;
    public ConcurrentQueue<float> dataQueue = new ConcurrentQueue<float>();

    public string serialPortName = "COM3";
    public int baudRate = 115200;
    public int udpPort = 8000;
    [SerializeField]
    public List<CommunicationType> communicationTypesPriority = new List<CommunicationType>() { CommunicationType.Serial, CommunicationType.UDP };
    private CommunicationType currentType;

    // Constants
    private const int retryDelay = 1000;

    // Start is called before the first frame update
    void Start()
    {
        StartCommunication();
    }

    private void StartCommunication()
    {
        // Try to start communication for each type in priority
        foreach (CommunicationType type in communicationTypesPriority)
        {
            currentType = type;
            if (currentType == CommunicationType.Serial)
            {
                StartSerial();
            }
            else if (currentType == CommunicationType.UDP)
            {
                StartUDP();
            }
        }
    }

    // Start Serial communication
    private void StartSerial()
    {
        communicationThread = new Thread(() =>
        {
            while (true)
            {
                try
                {
                    serialPort = new SerialPort(serialPortName, baudRate);
                    serialPort.Open();

                    while (true)
                    {
                        string data = serialPort.ReadLine();
                        if (int.TryParse(data.Trim(), out int Out))
                        {
                            dataQueue.Enqueue(Out);
                        }
                    }
                }
                catch (IOException)
                {
                    Debug.Log("Serial port error. Retrying connection in " + retryDelay / 1000 + " seconds.");
                    Thread.Sleep(retryDelay);
                }
                catch (ThreadAbortException)
                {
                    Debug.Log("Serial thread aborted. Closing connection.");
                    break;
                }
            }
        });
        communicationThread.Start();
    }

    // Start UDP communication
    private void StartUDP()
    {
        communicationThread = new Thread(() =>
        {
            while (true)
            {
                try
                {
                    udpClient = new UdpClient(udpPort);
                    IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);

                    while (true)
                    {
                        byte[] data = udpClient.Receive(ref anyIP);
                        string text = Encoding.UTF8.GetString(data);
                        if (int.TryParse(text.Trim(), out int Out))
                        {
                            dataQueue.Enqueue(Out);
                        }
                    }
                }
                catch (SocketException)
                {
                    Debug.Log("UDP error. Retrying connection in " + retryDelay / 1000 + " seconds.");
                    Thread.Sleep(retryDelay);
                }
                catch (ThreadAbortException)
                {
                    Debug.Log("UDP thread aborted. Closing connection.");
                    break;
                }
            }
        });
        communicationThread.Start();
    }

    // OnDisable is called when the MonoBehaviour will be disabled or destroyed
    private void OnDisable()
    {
        // Abort the communication thread
        if (communicationThread != null)
        {
            communicationThread.Abort();
            communicationThread = null;
        }

        // Close the serial port
        if (serialPort != null)
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
            serialPort = null;
        }

        // Close the UDP client
        if (udpClient != null)
        {
            udpClient.Close();
            udpClient = null;
        }
    }
}