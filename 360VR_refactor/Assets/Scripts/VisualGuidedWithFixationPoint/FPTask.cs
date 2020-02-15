using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPTask : MonoBehaviour, FPTargetDelegate {

    /* Scene variables */
    public GameObject Target; // This should be attached to the scene object called `Target'
    public GameObject Sensor; // This should be attached to the scene object called `Look'
    public Material fixationPointMaterial;
    public Material targetMaterial;

    /* Setting variables */
    public int brightness = 0;
    public float mouseIndicatorSize = 0;
    public float collisionTime = 0.5f;
    public float targetTimeout = 10.0f;
    public int numberOfTrials = 10;
    public float targetSize = 1.3f;
    public bool isTargetSizeFixed = true;
    public bool isInterTrialTimeMissRandom = false;
    public float interTrialTimeMiss = 1.0f; //valid only if isInterTrialTimeMissRandom has been set to TRUE
    public float minInterTrialTimeMiss = 1.0f;
    public float maxInterTrialTimeMiss = 1.0f;
    public bool isInterTrialTimeRewardRandom = false;
    public float interTrialTimeReward = 1.0f; //valid only if isInterTrialTimeRewardRandom has been set to TRUE
    public float minInterTrialTimeReward = 1.0f;
    public float maxInterTrialTimeReward = 1.0f;
    public int licksToReward = 1;
    public bool isWhiteNoiseRandom = false;
    public float whiteNoiseDuration = 1.0f; //valid only if isWhiteNoiseRandom has been set to TRUE
    public float minWhiteNoiseDuration = 1.0f;
    public float maxWhiteNoiseDuration = 1.0f;
    public float targetRewardDelay = 0.0f;
    public string animalName;
    public float targetSuccessRate = 0.7f;
    public int thresholdTrials = 20;
    public bool useGrid = false;
    public bool useLaser = false;
    public VGOLaserEvent laserEvent = VGOLaserEvent.None;
    public float fractionOfTrialsStimulated = -1;

    /* Private variables */
    private int success = 0;
    private bool quitApplication = false;
    private bool[] fractionOfTrialsStimulatedMask;

    private FPTarget targetScript;
    private SensorScript sensorScript;

    DataCollector dc;

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
     * From here, a series of functions used to retrieve the settings of the GUI and to configure properly the experiment
     */
    private void SetupBackgroundColor()
    {
        brightness = PlayerPrefs.GetInt("brightness");
        Camera front = GameObject.Find("Front Camera").GetComponent<Camera>();
        Camera left = GameObject.Find("Left Camera").GetComponent<Camera>();
        Camera right = GameObject.Find("Right Camera").GetComponent<Camera>();
        Camera bottom = GameObject.Find("Bottom Camera").GetComponent<Camera>();
        Camera back = GameObject.Find("Back Camera").GetComponent<Camera>();
        Camera top = GameObject.Find("Top Camera").GetComponent<Camera>();
        front.backgroundColor = new Color((float)(brightness) / 255, (float)(brightness) / 255, (float)(brightness) / 255, 1.0f);
        left.backgroundColor = new Color((float)(brightness) / 255, (float)(brightness) / 255, (float)(brightness) / 255, 1.0f);
        right.backgroundColor = new Color((float)(brightness) / 255, (float)(brightness) / 255, (float)(brightness) / 255, 1.0f);
        bottom.backgroundColor = new Color((float)(brightness) / 255, (float)(brightness) / 255, (float)(brightness) / 255, 1.0f);
        back.backgroundColor = new Color((float)(brightness) / 255, (float)(brightness) / 255, (float)(brightness) / 255, 1.0f);
        top.backgroundColor = new Color((float)(brightness) / 255, (float)(brightness) / 255, (float)(brightness) / 255, 1.0f);
    }

    private void SetupIndicatorSize()
    {
        mouseIndicatorSize = (float)(PlayerPrefs.GetInt("mouseSize")) / 100;
        GameObject mouseIndicator = GameObject.Find("Sphere");
        mouseIndicator.transform.localScale = new Vector3(mouseIndicatorSize, mouseIndicatorSize, mouseIndicatorSize);
    }

    private void SetupInterTrialTime()
    {
        interTrialTimeMiss = PlayerPrefs.GetFloat("interTrialTimeMiss");
        minInterTrialTimeMiss = PlayerPrefs.GetFloat("minInterTrialTimeMiss");
        maxInterTrialTimeMiss = PlayerPrefs.GetFloat("maxInterTrialTimeMiss");
        if (PlayerPrefs.GetInt("isInterTrialTimeMissRandom") == 1) isInterTrialTimeMissRandom = true;
        else isInterTrialTimeMissRandom = false;

        interTrialTimeReward = PlayerPrefs.GetFloat("interTrialTimeReward");
        minInterTrialTimeReward = PlayerPrefs.GetFloat("minInterTrialTimeReward");
        maxInterTrialTimeReward = PlayerPrefs.GetFloat("maxInterTrialTimeReward");
        if (PlayerPrefs.GetInt("isInterTrialTimeRewardRandom") == 1) isInterTrialTimeRewardRandom = true;
        else isInterTrialTimeRewardRandom = false;
    }

    private void SetupCollisionTime()
    {
        collisionTime = PlayerPrefs.GetFloat("collisionTime");
    }

    private void SetupTargetTimeout()
    {
        targetTimeout = PlayerPrefs.GetFloat("targetTimeout");
    }

    private void SetupNumberOfTrials()
    {
        numberOfTrials = PlayerPrefs.GetInt("numberOfTrials");
    }

    public void SetupTargetSize()
    {
        if (PlayerPrefs.GetInt("isTargetSizeRandom") == 1) isTargetSizeFixed = false;
        else
        {
            isTargetSizeFixed = true;
            targetSize = PlayerPrefs.GetFloat("targetSize");
        }

    }

    public void SetupAnimalName()
    {
        animalName = PlayerPrefs.GetString("animalName");
    }

    private void SetupLickToReward()
    {
        licksToReward = PlayerPrefs.GetInt("licksToReward");
    }

    private void SetupWhiteNoise()
    {
        whiteNoiseDuration = PlayerPrefs.GetFloat("whiteNoise");

        minWhiteNoiseDuration = PlayerPrefs.GetFloat("minWhiteNoiseDuration");
        maxWhiteNoiseDuration = PlayerPrefs.GetFloat("maxWhiteNoiseDuration");
        if (PlayerPrefs.GetInt("isWhiteNoiseRandom") == 1) isWhiteNoiseRandom = true;
        else isWhiteNoiseRandom = false;
    }

    private void SetupToneRewardDelay()
    {
        targetRewardDelay = PlayerPrefs.GetFloat("targetRewardDelay");
    }

    private void SetupUseGrid()
    {
        if (PlayerPrefs.GetInt("useGrid") == 1) useGrid = true;
        else useGrid = false;
    }

    public void SetupLaser()
    {
        if (PlayerPrefs.GetInt("useLaser") == 1) useLaser = true;
        else useLaser = false;

        int laserEventIdx = PlayerPrefs.GetInt("laserEvent");
        switch (laserEventIdx)
        {
            case 0:
                laserEvent = VGOLaserEvent.None;
                break;
            case 1:
                laserEvent = VGOLaserEvent.TrialStart;
                break;
            case 2:
                laserEvent = VGOLaserEvent.TargetOn;
                break;
            case 3:
                laserEvent = VGOLaserEvent.TargetOff;
                break;
            case 4:
                laserEvent = VGOLaserEvent.Reward;
                break;
            case 5:
                laserEvent = VGOLaserEvent.RewardToneOn;
                break;
            case 6:
                laserEvent = VGOLaserEvent.MissToneOn;
                break;
        }

        fractionOfTrialsStimulated = PlayerPrefs.GetFloat("fractionOfTrialsStimulated");

        GenerateRandomSequence(fractionOfTrialsStimulated);
    }

    /**
     * This function will print in the Debug console all the settings that are been applied. To use only for debug purposes
     */
    public void PrintStartingConfiguration()
    {
        Debug.Log("brightness = " + brightness);
        Debug.Log("mouseIndicatorSize = " + mouseIndicatorSize);
        Debug.Log("collisionTime = " + collisionTime);
        Debug.Log("targetTimeout = " + targetTimeout);
        Debug.Log("numberOfTrials = " + numberOfTrials);
        Debug.Log("targetSize = " + targetSize);
        Debug.Log("isTargetSizeFixed = " + isTargetSizeFixed);
        Debug.Log("targetSize = " + targetSize);
        Debug.Log("isInterTrialTimeMissRandom = " + isInterTrialTimeMissRandom);
        Debug.Log("interTrialTimeMiss = " + interTrialTimeMiss);
        Debug.Log("minInterTrialTimeMiss = " + minInterTrialTimeMiss);
        Debug.Log("maxInterTrialTimeMiss = " + maxInterTrialTimeMiss);
        Debug.Log("isInterTrialTimeRewardRandom = " + isInterTrialTimeRewardRandom);
        Debug.Log("interTrialTimeReward = " + interTrialTimeReward);
        Debug.Log("minInterTrialTimeReward = " + minInterTrialTimeReward);
        Debug.Log("maxInterTrialTimeReward = " + maxInterTrialTimeReward);
        Debug.Log("animalName = " + animalName);
        Debug.Log("licksToReward = " + licksToReward);
        Debug.Log("whiteNoise = " + whiteNoiseDuration);
        Debug.Log("minWhiteNoise = " + minWhiteNoiseDuration);
        Debug.Log("maxWhiteNoise = " + maxWhiteNoiseDuration);
        Debug.Log("isWhiteNoiseRandom = " + isWhiteNoiseRandom);
        Debug.Log("toneRewardDelay = " + targetRewardDelay);
        Debug.Log("useGrid = " + useGrid);
        Debug.Log("useLaser = " + useLaser);
        Debug.Log("laserEvent = " + laserEvent);
        Debug.Log("fractionOfTrialsStimulated = " + fractionOfTrialsStimulated);
    }


    // Use this for initialization
    void Start()
    {

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = CommonUtils.frameRate;

        // Call all the functions to setup the environment
        SetupBackgroundColor();
        SetupIndicatorSize();
        SetupInterTrialTime();
        SetupCollisionTime();
        SetupTargetTimeout();
        SetupNumberOfTrials();
        SetupTargetSize();
        SetupAnimalName();
        SetupLickToReward();
        SetupWhiteNoise();
        SetupToneRewardDelay();
        SetupUseGrid();
        SetupLaser();

        // Print the configuration, if needed
        PrintStartingConfiguration();

        // Retrieve a reference for the scripts, starting from the GameObjects of the scene
        sensorScript = Sensor.GetComponentInChildren<SensorScript>();
        targetScript = Target.GetComponentInChildren<FPTarget>();

        // Init a new DataCollector
        dc = new DataCollector
        {
            taskName = "VisualGuidedOrientedTask",
            AnimalName = animalName
        };

        // Start a thread the waits for the user to start the experiment
        StartCoroutine(WaitForStart());
    }

    // Update is called once per frame
    void Update()
    {
        // If the user presses Escape the variable `quitApplication' is set. 
        // This will trigger the end of the experiment when the current trial ends.
        if (Input.GetKey(KeyCode.Return)) quitApplication = true;
        Application.targetFrameRate = CommonUtils.frameRate;

    }

    /**
     * Thread that waits for the start of the experiment
     */
    IEnumerator WaitForStart()
    {
        // The thread will not go forward, until the spacebar is not pressed
        while (!Input.GetKey("space")) yield return 0;

        // Then, it starts the thread that implements the experiment
        StartCoroutine(StartExperiment());
    }

    /**
     * This function implements the task Habituation
     */
    IEnumerator StartExperiment()
    {
        int trialCnt = 0;

        // The collision time chosen is injected in the target script
        targetScript.CollisionDuration = collisionTime;

        // Request to the start the imaging session (if available)
        sensorScript.StartRecording();

        // The experiment continues when:
        // the user has not explicitly requested to terminate AND
        // the current trial is less than the limit OR the user requested to have unlimited trials
        while (((trialCnt < numberOfTrials) || numberOfTrials == -1) && (!quitApplication))
        {
            // A new recording element is created. All the following instructions regarding `dataInfo' are meant to collect data during the experiment
            DataCollector.Element dataInfo = dc.NewElem();
            dataInfo.TrialStart = DateTime.Now;

            // The sensorScript is set to initialize a new trial
            sensorScript.InitNewTrial();

            // Calculate the duration of the white noise
            float wn = whiteNoiseDuration;
            if (!isTargetSizeFixed) wn *= UnityEngine.Random.Range(minWhiteNoiseDuration, maxWhiteNoiseDuration);

            // Request to the sensor script to start the white noise
            sensorScript.Tone(ToneFrequency.WhiteNoiseStart);
            dataInfo.WhiteNoiseOn = DateTime.Now;

            if (useLaser && (laserEvent == VGOLaserEvent.TrialStart))
            {
                if (fractionOfTrialsStimulatedMask[trialCnt])
                {
                    sensorScript.TriggerLaser();
                    dataInfo.IsLaserActivated = true;
                    dataInfo.LaserStartTime = DateTime.Now;
                }
            }

            // Wait for the amount of time chosen in GUI for the white noise
            while (wn > 0)
            {
                wn -= Time.deltaTime;
                yield return 0;
            }

            // Request to the sensor script to stop the white noise
            sensorScript.Tone(ToneFrequency.WhiteNoiseStop);
            dataInfo.WhiteNoiseOff = DateTime.Now;

            dataInfo.VisualTargetOnTime = DateTime.Now;
            if (useLaser && (laserEvent == VGOLaserEvent.TargetOn))
            {
                if (fractionOfTrialsStimulatedMask[trialCnt])
                {
                    sensorScript.TriggerLaser();
                    dataInfo.IsLaserActivated = true;
                    dataInfo.LaserStartTime = DateTime.Now;
                }
            }

            // Set the target size in the variable `actualTargetSize'
            float actualTargetSize = targetSize;
            if (!isTargetSizeFixed) actualTargetSize *= UnityEngine.Random.Range(0.4f, 1.1f); //min size = 0.6; max size = 1.65 //Note: it should be always higher than the mouse indicator size
            dataInfo.TargetSize = actualTargetSize;

            // Reset the `sucess variable'
            success = 0;

            // Set size and position and spawn a new target
            float fixationPointTimeoutCounter = targetTimeout;

            dataInfo.TargetPos.X = 250f;
            dataInfo.TargetPos.Y = 1.6f;
            dataInfo.TargetPos.Z = 250f;
            Target.transform.localScale = new Vector3(actualTargetSize, actualTargetSize, 1);
            targetSpawned = Instantiate(Target, new Vector3(250f, 1.6f, 250f), Quaternion.Euler(0, 0, 0));
            targetSpawned.GetComponentInChildren<Renderer>().material = fixationPointMaterial;

            // Find and set the delegate to the target, in order to handle *here* if a collision occurs.
            targetSpawned.GetComponentInChildren<FPTarget>().Delegate = this;

            // Set a destroy timer equals to the chose timeoutTarget. If the target has already been deallocated before its timeout, this function has no effect.
            Destroy(targetSpawned, fixationPointTimeoutCounter);

            // Wait for at maximum the targetTimeout
            while ((fixationPointTimeoutCounter > 0) && (success != 1))
            {
                fixationPointTimeoutCounter -= Time.deltaTime;
                yield return 0;
            }

            // Either for the timeout or for the collision, at this point, the target does not exist anymore.
            dataInfo.TargetDisappearingTime = DateTime.Now;
            if (useLaser && (laserEvent == VGOLaserEvent.TargetOff))
            {
                if (fractionOfTrialsStimulatedMask[trialCnt])
                {
                    sensorScript.TriggerLaser();
                    dataInfo.IsLaserActivated = true;
                }
            }

            if (success == 1) // If there was a collision
            {
                dataInfo.ToneRewardDelay = targetRewardDelay;
                dataInfo.IsOutcomeDelivered = true;


                // Request to the sensor script to play the `Success' tone and to deliver the Reward
                sensorScript.Tone(ToneFrequency.SuccessTone);
                if (useLaser && (laserEvent == VGOLaserEvent.RewardToneOn))
                {
                    if (fractionOfTrialsStimulatedMask[trialCnt])
                    {
                        sensorScript.TriggerLaser();
                        dataInfo.IsLaserActivated = true;
                        dataInfo.LaserStartTime = DateTime.Now;
                    }
                }

                // Wait for the amount of time indicated from the setting `targetRewardDelay'
                yield return new WaitForSecondsRealtime(targetRewardDelay);

                success = 0;
                /************/
                // Set size and position and spawn a new target
                float targetTimeoutCounter = targetTimeout;

                Quaternion spawnRotation;
                if (useGrid)
                {
                    // If the grid space has been chosen, then the position is one among the 8 different tiles
                    int gridPosIdx = UnityEngine.Random.Range(0, 8);
                    spawnRotation = Quaternion.Euler(gridPositions[gridPosIdx].Item2, gridPositions[gridPosIdx].Item1, gridPositions[gridPosIdx].Item3);
                    dataInfo.TargetPos.Yaw = gridPositions[gridPosIdx].Item1;
                    dataInfo.TargetPos.Pitch = gridPositions[gridPosIdx].Item2;
                    dataInfo.TargetPos.Roll = gridPositions[gridPosIdx].Item3;
                }
                else
                {
                    // If the free space has been chosen, then the position is random (but inside a fixed box)

                    float yaw = UnityEngine.Random.Range(-70, 71);
                    float pitch = UnityEngine.Random.Range(-40, 41);
                    spawnRotation = Quaternion.Euler(pitch, yaw, 0f);
                    dataInfo.TargetPos.Yaw = yaw;
                    dataInfo.TargetPos.Pitch = pitch;
                    dataInfo.TargetPos.Roll = 0f;
                }

                dataInfo.TargetPos.X = 250f;
                dataInfo.TargetPos.Y = 1.6f;
                dataInfo.TargetPos.Z = 250f;
                Target.transform.localScale = new Vector3(actualTargetSize, actualTargetSize, 1);
                targetSpawned = Instantiate(Target, new Vector3(250f, 1.6f, 250f), spawnRotation);
                targetSpawned.GetComponentInChildren<Renderer>().material = targetMaterial;

                // Find and set the delegate to the target, in order to handle *here* if a collision occurs.
                targetSpawned.GetComponentInChildren<FPTarget>().Delegate = this;


                // Set a destroy timer equals to the chose timeoutTarget. If the target has already been deallocated before its timeout, this function has no effect.
                Destroy(targetSpawned, targetTimeoutCounter);

                // Wait for at maximum the targetTimeout
                while ((targetTimeoutCounter > 0) && (success != 1))
                {
                    targetTimeoutCounter -= Time.deltaTime;
                    yield return 0;
                }

                // Either for the timeout or for the collision, at this point, the target does not exist anymore.
                dataInfo.TargetDisappearingTime = DateTime.Now;
                if (useLaser && (laserEvent == VGOLaserEvent.TargetOff))
                {
                    if (fractionOfTrialsStimulatedMask[trialCnt])
                    {
                        sensorScript.TriggerLaser();
                        dataInfo.IsLaserActivated = true;
                    }
                }

                if(success == 1)
                {

                    // Deliver reward
                    sensorScript.Reward();
                    dataInfo.OutcomeTime = DateTime.Now;
                    if (useLaser && (laserEvent == VGOLaserEvent.Reward))
                    {
                        if (fractionOfTrialsStimulatedMask[trialCnt])
                        {
                            sensorScript.TriggerLaser();
                            dataInfo.IsLaserActivated = true;
                            dataInfo.LaserStartTime = DateTime.Now;
                        }
                    }

                    // Check if in the remaining time of the trial (aka ITI-reward) the reward is consumed or not
                    bool consumed = false;
                    float iti = interTrialTimeReward;
                    if (isInterTrialTimeRewardRandom) iti = UnityEngine.Random.Range(minInterTrialTimeReward, maxInterTrialTimeReward);
                    dataInfo.ITIOn = DateTime.Now;

                    while (iti > 0)
                    {
                        if (sensorScript.numberOfLicksAfterReward >= licksToReward) consumed = true;
                        iti -= Time.deltaTime;
                        yield return 0;
                    }
                    dataInfo.ITIOff = DateTime.Now;


                    if (consumed)
                    {
                        // If a reward has been consumed, then request to the sensor script to send the signal of `Consumed'
                        dataInfo.IsRewardConsumed = true;
                        sensorScript.Consumed();
                    }

                }
                else
                {
                    // Request to the sensor script to play the `Missed' tone
                    sensorScript.Tone(ToneFrequency.MissedTone);
                    if (useLaser && (laserEvent == VGOLaserEvent.MissToneOn))
                    {
                        if (fractionOfTrialsStimulatedMask[trialCnt])
                        {
                            sensorScript.TriggerLaser();
                            dataInfo.IsLaserActivated = true;
                            dataInfo.LaserStartTime = DateTime.Now;
                        }
                    }

                    // Wait for the ITI-miss
                    dataInfo.ITIOn = DateTime.Now;
                    if (isInterTrialTimeMissRandom) yield return new WaitForSecondsRealtime(UnityEngine.Random.Range(minInterTrialTimeMiss, maxInterTrialTimeMiss));
                    else yield return new WaitForSecondsRealtime(interTrialTimeMiss);
                    dataInfo.ITIOff = DateTime.Now;
                }
                /*************/

                

            }
            else // If there was NOT a collision
            {
                // Request to the sensor script to play the `Missed' tone
                sensorScript.Tone(ToneFrequency.MissedTone);
                if (useLaser && (laserEvent == VGOLaserEvent.MissToneOn))
                {
                    if (fractionOfTrialsStimulatedMask[trialCnt])
                    {
                        sensorScript.TriggerLaser();
                        dataInfo.IsLaserActivated = true;
                        dataInfo.LaserStartTime = DateTime.Now;
                    }
                }

                // Wait for the ITI-miss
                dataInfo.ITIOn = DateTime.Now;
                if (isInterTrialTimeMissRandom) yield return new WaitForSecondsRealtime(UnityEngine.Random.Range(minInterTrialTimeMiss, maxInterTrialTimeMiss));
                else yield return new WaitForSecondsRealtime(interTrialTimeMiss);
                dataInfo.ITIOff = DateTime.Now;

            }

            // Save the licks timestamps and the head positions
            dataInfo.LicksTimeOptical = new List<DateTime>(sensorScript.licksTimeOptical);
            dataInfo.HeadPositions = new List<DataCollector.Position>(sensorScript.headPositions);

            dataInfo.TrialEnd = DateTime.Now;
            trialCnt++;
        }

        // Stop the imaging session (if available)
        sensorScript.StopRecording();

        Debug.Log("Training completed");
    }

    /**
     * Implementation of the function for the Delegation pattern
     */
    public void FPCollisionOccurred(DateTime ts)
    {
        Debug.Log("Collision occurred at " + ts);
        success = 1;
    }

    private void GenerateRandomSequence(float percentage)
    {
        if (numberOfTrials < 0) return;
        int fractionOfTrials = (int)((float)numberOfTrials * percentage);
        fractionOfTrialsStimulatedMask = new bool[numberOfTrials];

        for (int i = 0; i < numberOfTrials; i++) fractionOfTrialsStimulatedMask[i] = false;

        for (int i = 0; i < fractionOfTrials; i++)
        {
            int newIndex;
            do
            {
                newIndex = UnityEngine.Random.Range(0, numberOfTrials);

            } while (fractionOfTrialsStimulatedMask[newIndex]);

            fractionOfTrialsStimulatedMask[newIndex] = true;
        }

        Debug.Log("fractionOfTrials = " + fractionOfTrials);
        for (int i = 0; i < numberOfTrials; i++)
        {
            Debug.Log(fractionOfTrialsStimulatedMask[i].ToString());
        }


    }

    public void OnApplicationQuit()
    {
        // Before terminating the application, stop the imaging recording (if available) and save the report file
        sensorScript.StopRecording();
        dc.SaveToFile();
    }
}
