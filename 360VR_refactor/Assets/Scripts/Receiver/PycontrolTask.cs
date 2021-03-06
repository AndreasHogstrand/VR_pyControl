﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.IO;

public class PycontrolTask : MonoBehaviour, VGOTargetDelegate
{
    /* Scene variables */
    public GameObject Target; // This should be attached to the scene object called `Target'
    public GameObject PhotodiodeTargetObject; //This should be attached to the prefab object called `PhotodiodeTarget'

    private PhotodiodeTarget photodiodeTarget;

    /* Private variables */
    private VGOTarget targetScript;
    private SerialPort serialPort;
    private readonly string pyControlPortName = @"\\.\COM5";
    private int incomingData;

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
        StartCoroutine(photodiodeTarget.ShowTarget());
        serialPort.Write("a");
        success = 1;
    }

    IEnumerator ProcessCommand(int command)
    {
        if ((command > 96) && (command < 105))
        {
            StartCoroutine(SpawnTarget(command - 97));
            yield break;
        }
        else
        {
            Debug.Log("Invalid command received");
            StartCoroutine(WaitForCommand());
            yield break;
        }
    }

    IEnumerator WaitForCommand()
    {
        incomingData = '\0';
        while (true)
        {
            if (serialPort.BytesToRead > 0)
            {
                incomingData = serialPort.ReadChar();
                StartCoroutine(ProcessCommand(incomingData));
                yield break;
            }
            yield return 0;
        }
    }

    IEnumerator SpawnTarget(int gridPos)
    {
        success = 0;
        float timeoutCounter = targetTimeout;

        Quaternion spawnRotation = Quaternion.Euler(gridPositions[gridPos].Item2, gridPositions[gridPos].Item1, gridPositions[gridPos].Item3);
        targetSpawned = Instantiate(Target, new Vector3(250f, 1.6f, 250f), spawnRotation);
        StartCoroutine(photodiodeTarget.ShowTarget());
        Debug.Log("Target spawned at " + DateTime.Now);

        // Find and set the delegate to the target, in order to handle *here* if a collision occurs.
        targetSpawned.GetComponentInChildren<VGOTarget>().Delegate = this;

        // Wait for at maximum the targetTimeout
        while ((timeoutCounter > 0) && (success != 1))
        {
            timeoutCounter -= Time.deltaTime;
            yield return 0;
        }

        //If target was not destroyed by collision
        if (success != 1)
        {
            Destroy(targetSpawned);
            StartCoroutine(photodiodeTarget.ShowTarget());
        }

        StartCoroutine(WaitForCommand());
    }

    private Dictionary<char, IEnumerator> commandDict = new Dictionary<char, IEnumerator>();

    // Start is called before the first frame update
    void Start()
    {
        commandDict.Add('a', SpawnTarget(0));
        commandDict.Add('b', SpawnTarget(1));
        commandDict.Add('c', SpawnTarget(2));
        commandDict.Add('d', SpawnTarget(3));
        commandDict.Add('e', SpawnTarget(4));
        commandDict.Add('f', SpawnTarget(5));
        commandDict.Add('g', SpawnTarget(6));
        commandDict.Add('h', SpawnTarget(7));

        photodiodeTarget = PhotodiodeTargetObject.GetComponent<PhotodiodeTarget>();

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
        }

        StartCoroutine(WaitForCommand());
    }
}