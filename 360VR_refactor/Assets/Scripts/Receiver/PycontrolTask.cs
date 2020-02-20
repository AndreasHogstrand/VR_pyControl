using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.IO;

public class PycontrolTask : MonoBehaviour, VGOTargetDelegate
{
    /* Scene variables */
    public GameObject Target; // This should be attached to the scene object called `Target'

    /* Private variables */
    private VGOTarget targetScript;
    private SerialPort serialPort;
    private readonly string pyControlPortName = @"\\.\COM5";
    private char incomingData;

    private int success = 0;
    public float targetTimeout = 4.0f;

    /** 
     * In each trial this variable will be used to store a clone of the `Target' scene object.
     *  Basically, the object Target is created only in order to be cloned. These clones are targetSpawned
     */
    private GameObject targetSpawned;

    /**
     * If requested, this task can run either using a 2-by-4 grid or in a free space. In case the gride is used, 
     * then the following is the variable that represents it
     */
    private readonly Tuple<float, float, float>[] gridPositions = {
        new Tuple<float, float, float>(-60,-30,0),  //0
        new Tuple<float, float, float>(-20,-30,0),  //1
        new Tuple<float, float, float>(-60,30,0),   //2
        new Tuple<float, float, float>(-20,30,0),   //3
        new Tuple<float, float, float>(20,-30,0),   //4
        new Tuple<float, float, float>(60,-30,0),   //5
        new Tuple<float, float, float>(20,30,0),    //6
        new Tuple<float, float, float>(60,30,0)     //7
    };

    /**
     * Implementation of the function for the Delegation pattern
     */
    public void VGOCollisionOccurred(DateTime ts)
    {
        Debug.Log("Collision occurred at " + ts);
        success = 1;
    }

    IEnumerator WaitForCommand()
    {
        incomingData = '\0';
        while (true)
        {
            if (serialPort.BytesToRead > 0)
            {
                incomingData = (char)serialPort.ReadChar();
                StartCoroutine(SpawnTarget());
                break;
            }
            yield return 0;
        }
    }

    IEnumerator SpawnTarget()
    {
        float timeoutCounter = targetTimeout;

        Quaternion spawnRotation = Quaternion.Euler(gridPositions[0].Item2, gridPositions[0].Item1, gridPositions[0].Item3);
        targetSpawned = Instantiate(Target, new Vector3(250f, 1.6f, 250f), spawnRotation);

        // Find and set the delegate to the target, in order to handle *here* if a collision occurs.
        targetSpawned.GetComponentInChildren<VGOTarget>().Delegate = this;

        // Set a destroy timer equals to the chose timeoutTarget. If the target has already been deallocated before its timeout, this function has no effect.
        Destroy(targetSpawned, targetTimeout);

        // Wait for at maximum the targetTimeout
        while ((timeoutCounter > 0) && (success != 1))
        {
            timeoutCounter -= Time.deltaTime;
            yield return 0;
        }
        StartCoroutine(WaitForCommand());
    }

    private Dictionary<char, IEnumerator> commandDict = new Dictionary<char, IEnumerator>();

    // Start is called before the first frame update
    void Start()
    {
        commandDict.Add('a', SpawnTarget());

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = CommonUtils.frameRate;

        // Retrieve a reference for the scripts, starting from the GameObjects of the scene
        targetScript = Target.GetComponentInChildren<VGOTarget>();

        serialPort = new SerialPort(pyControlPortName, 9600, 0, 8, StopBits.One);
        try
        {
            serialPort.Open(); // open the port
            serialPort.ReadTimeout = 1; //define the timeout
        }
        catch (IOException e)
        {
            Debug.LogWarning(e);
        }

        StartCoroutine(WaitForCommand());
    }
}