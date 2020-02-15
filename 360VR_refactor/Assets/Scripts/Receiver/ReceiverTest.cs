using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.IO;

public class ReceiverTest : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    private SerialPort serialPort;
    private readonly string pyControlPortName = @"\\.\COM5";
    private char incomingData;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
        meshRenderer.enabled = true;
        serialPort = new SerialPort(pyControlPortName, 9600, 0, 8, StopBits.One);
        try
        {
                serialPort.Open(); // open the port
                serialPort.ReadTimeout = 40; //define the timeout
        }
        catch (IOException e)
        {
            Debug.LogWarning(e);
        }
    }

    // Update is called once per frame
    void Update()
    {
        incomingData = (char)serialPort.ReadChar();
        if(incomingData == 'a')
        {
            meshRenderer.enabled = true;
            incomingData = '\0';
        }
        if (incomingData == 'b')
        {
            meshRenderer.enabled = false;
            incomingData = '\0';
        }
    }
}
