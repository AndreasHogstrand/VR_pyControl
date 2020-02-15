using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HFClassicalConditioningTask : MonoBehaviour
{
    /* Scene variables */
    public GameObject Target; // This should be attached to the scene object called `Target'
    public GameObject Sensor; // This should be attached to the scene object called `Look'

    /* List of different materials to apply to the targets */
    // Square materials
    public Material material1;
    public Material material2;
    public Material material3;
    public Material material4;
    public Material material5;
    public Material material6;
    public Material material7;
    public Material material8;
    public Material material9;
    public Material material10;
    public Material material11;
    public Material material12;
    public Material material13;
    public Material material14;
    public Material material15;
    public Material material16;
    public Material material17;
    public Material material18;
    public Material material19;
    public Material material20;
    public Material material21;
    public Material material22;
    public Material material23;

    /* Setting variables */
    public int brightness = 0;
    public float targetSize = 1.3f;
    public bool isTargetSizeFixed = true;
    public bool isInterTrialTimeRandom = false;
    public float interTrialTime = 1.0f; //valid only if isInterTrialTimeRandom has been set to TRUE
    public float minInterTrialTime = 1.0f;
    public float maxInterTrialTime = 1.0f;
    public int licksToReward = 1;
    public bool isWhiteNoiseRandom = false;
    public float whiteNoiseDuration = 1.0f; //valid only if isWhiteNoiseRandom has been set to TRUE
    public float minWhiteNoiseDuration = 1.0f;
    public float maxWhiteNoiseDuration = 1.0f;
    public bool isWhiteNoiseEnabled = false;
    public float cueRewardDelay = 0.0f;
    public float wnCueDelay = 0.0f;
    public string animalName;
    public float targetSuccessRate = 0.7f;
    public int thresholdTrials = 20;
    public bool flat;
    public bool isVisualStimEnabled = true;
    public bool isAudioStimEnabled = true;
    public int numberOfTrials = 0;
    public bool useLaser = false;
    public HFLaserEvent laserEvent = HFLaserEvent.None;
    public float fractionOfTrialsStimulated = -1;
    public bool extendedScreen = false;

    // Target configuration variables
    public int numberOfTargets = 0;
    public float[] outcomeProb;
    public float[] durations;
    public float[] delays;
    public int[] visuals;
    public int[] auditory;
    public bool[] audioBefore;

    /* Private variables */

    private float[] cueRewardedTrials       = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    private float[] cueTrials               = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    private bool quitApplication = false;
    DataCollector dc;
    private bool[] fractionOfTrialsStimulatedMask;


    SensorScript sensorScript;

    /**
     * Modify this line of code to change the three position that the target can assume
     */
    private Tuple<float, float, float>[] gridPositions; 
    
    /** 
     * In each trial this variable will be used to store a clone of the `Target' scene object.
     *  Basically, the object Target is created only in order to be cloned. These clones are targetSpawned
     */
    private GameObject targetSpawned;

    /**
     * From here, a series of functions used to retrieve the settings of the GUI and to configure properly the experiment
     */
    private void SetupIsFlat()
    {
        // This function discriminates if the projection is performed on a planar or toroidal screen
        if (PlayerPrefs.GetInt("flat") == 1) flat = true;
        else flat = false;

        if (PlayerPrefs.GetInt("extendedScreen") == 1) extendedScreen = true;
        else extendedScreen = false;

        if(flat)
        {
            // Put here coordinates for the flat screen
            gridPositions = new Tuple<float, float, float>[]{
                    new Tuple<float, float, float>(0, 0, 0),    // center
                    new Tuple<float, float, float>(3, 0, 0),   // right
                    new Tuple<float, float, float>(-3, 0, 0)    // left
                };
        } else
        {
            // Put here coordinates for the VR environment
            gridPositions = new Tuple<float, float, float>[]{
                    new Tuple<float, float, float>(0, -10, 0),    // center
                    new Tuple<float, float, float>(60, -10, 0),   // right
                    new Tuple<float, float, float>(-60, -10, 0)    // left
                };
        }

    }

    private void SetupBackgroundColor()
    {
        brightness = PlayerPrefs.GetInt("brightness");

        if (flat)
        {
            Camera main = GameObject.Find("CameraMain").GetComponent<Camera>();
            main.backgroundColor = new Color((float)(brightness) / 255, (float)(brightness) / 255, (float)(brightness) / 255, 1.0f);
        }
        else
        {
            Debug.Log("Brightness is: " + brightness);
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

    }

    private void SetupInterTrialTime()
    {
        interTrialTime = PlayerPrefs.GetFloat("interTrialTime");
        minInterTrialTime = PlayerPrefs.GetFloat("minInterTrialTime");
        maxInterTrialTime = PlayerPrefs.GetFloat("maxInterTrialTime");
        if (PlayerPrefs.GetInt("isInterTrialTimeRandom") == 1) isInterTrialTimeRandom = true;
        else isInterTrialTimeRandom = false;

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

    private void SetupNumberOfTrials()
    {
        numberOfTrials = PlayerPrefs.GetInt("numberOfTrials");
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

        if (PlayerPrefs.GetInt("isWnEnabled") == 1) isWhiteNoiseEnabled = true;
        else isWhiteNoiseEnabled = false;

        
    }

    private void SetupCueRewardDelay()
    {
        cueRewardDelay = PlayerPrefs.GetFloat("cueRewardDelay");
    }

    private void SetupWnCueDelay()
    {
        wnCueDelay = PlayerPrefs.GetFloat("wnCueDelay");
    }

    public void SetupLaser()
    {
        if (PlayerPrefs.GetInt("useLaser") == 1) useLaser = true;
        else useLaser = false;

        int laserEventIdx = PlayerPrefs.GetInt("laserEvent");
        switch (laserEventIdx)
        {
            case 0:
                laserEvent = HFLaserEvent.None;
                break;
            case 1:
                laserEvent = HFLaserEvent.TrialStart;
                break;
            case 2:
                laserEvent = HFLaserEvent.VisualTargetOn;
                break;
            case 3:
                laserEvent = HFLaserEvent.AuditoryOn;
                break;
            case 4:
                laserEvent = HFLaserEvent.VisualTargetOff;
                break;
            case 5:
                laserEvent = HFLaserEvent.Reward;
                break;
            case 6:
                laserEvent = HFLaserEvent.RewardToneOn;
                break;
            case 7:
                laserEvent = HFLaserEvent.Punishment;
                break;
            case 8:
                laserEvent = HFLaserEvent.PunishmentToneOn;
                break;
            case 9:
                laserEvent = HFLaserEvent.Outcome;
                break;
            case 10:
                laserEvent = HFLaserEvent.OutcomeToneOn;
                break;
        }

        fractionOfTrialsStimulated = PlayerPrefs.GetFloat("fractionOfTrialsStimulated");

        GenerateRandomSequence(fractionOfTrialsStimulated);
    }

    private void SetupTargets()
    {
        numberOfTargets = HFTargetConfigurator.shared.NumberOfTargets;

        visuals = new int[10];
        visuals[0] = HFTargetConfigurator.shared.T1VisualStim;
        visuals[1] = HFTargetConfigurator.shared.T2VisualStim;
        visuals[2] = HFTargetConfigurator.shared.T3VisualStim;
        visuals[3] = HFTargetConfigurator.shared.T4VisualStim;
        visuals[4] = HFTargetConfigurator.shared.T5VisualStim;
        visuals[5] = HFTargetConfigurator.shared.T6VisualStim;
        visuals[6] = HFTargetConfigurator.shared.T7VisualStim;
        visuals[7] = HFTargetConfigurator.shared.T8VisualStim;
        visuals[8] = HFTargetConfigurator.shared.T9VisualStim;
        visuals[9] = HFTargetConfigurator.shared.T10VisualStim;

        auditory = new int[10];
        auditory[0] = HFTargetConfigurator.shared.T1AudioStim;
        auditory[1] = HFTargetConfigurator.shared.T2AudioStim;
        auditory[2] = HFTargetConfigurator.shared.T3AudioStim;
        auditory[3] = HFTargetConfigurator.shared.T4AudioStim;
        auditory[4] = HFTargetConfigurator.shared.T5AudioStim;
        auditory[5] = HFTargetConfigurator.shared.T6AudioStim;
        auditory[6] = HFTargetConfigurator.shared.T7AudioStim;
        auditory[7] = HFTargetConfigurator.shared.T8AudioStim;
        auditory[8] = HFTargetConfigurator.shared.T9AudioStim;
        auditory[9] = HFTargetConfigurator.shared.T10AudioStim;

        outcomeProb = new float[10];
        outcomeProb[0] = HFTargetConfigurator.shared.T1OutcomeProb;
        outcomeProb[1] = HFTargetConfigurator.shared.T2OutcomeProb;
        outcomeProb[2] = HFTargetConfigurator.shared.T3OutcomeProb;
        outcomeProb[3] = HFTargetConfigurator.shared.T4OutcomeProb;
        outcomeProb[4] = HFTargetConfigurator.shared.T5OutcomeProb;
        outcomeProb[5] = HFTargetConfigurator.shared.T6OutcomeProb;
        outcomeProb[6] = HFTargetConfigurator.shared.T7OutcomeProb;
        outcomeProb[7] = HFTargetConfigurator.shared.T8OutcomeProb;
        outcomeProb[8] = HFTargetConfigurator.shared.T9OutcomeProb;
        outcomeProb[9] = HFTargetConfigurator.shared.T10OutcomeProb;

        durations = new float[10];
        durations[0] = HFTargetConfigurator.shared.T1Duration;
        durations[1] = HFTargetConfigurator.shared.T2Duration;
        durations[2] = HFTargetConfigurator.shared.T3Duration;
        durations[3] = HFTargetConfigurator.shared.T4Duration;
        durations[4] = HFTargetConfigurator.shared.T5Duration;
        durations[5] = HFTargetConfigurator.shared.T6Duration;
        durations[6] = HFTargetConfigurator.shared.T7Duration;
        durations[7] = HFTargetConfigurator.shared.T8Duration;
        durations[8] = HFTargetConfigurator.shared.T9Duration;
        durations[9] = HFTargetConfigurator.shared.T10Duration;

        delays = new float[10];
        delays[0] = HFTargetConfigurator.shared.T1Delays;
        delays[1] = HFTargetConfigurator.shared.T2Delays;
        delays[2] = HFTargetConfigurator.shared.T3Delays;
        delays[3] = HFTargetConfigurator.shared.T4Delays;
        delays[4] = HFTargetConfigurator.shared.T5Delays;
        delays[5] = HFTargetConfigurator.shared.T6Delays;
        delays[6] = HFTargetConfigurator.shared.T7Delays;
        delays[7] = HFTargetConfigurator.shared.T8Delays;
        delays[8] = HFTargetConfigurator.shared.T9Delays;
        delays[9] = HFTargetConfigurator.shared.T10Delays;
    }

    /**
     * This function will print in the Debug console all the settings that are been applied. To use only for debug purposes
     */
    public void PrintStartingConfiguration()
    {
        Debug.Log("brightness = " + brightness);
        Debug.Log("targetSize = " + targetSize);
        Debug.Log("isTargetSizeFixed = " + isTargetSizeFixed);
        Debug.Log("isInterTrialTimeRandom = " + isInterTrialTimeRandom);
        Debug.Log("interTrialTime = " + interTrialTime);
        Debug.Log("minInterTrialTime = " + minInterTrialTime);
        Debug.Log("maxInterTrialTime = " + maxInterTrialTime);
        Debug.Log("animalName = " + animalName);
        Debug.Log("licksToReward = " + licksToReward);
        Debug.Log("whiteNoise = " + whiteNoiseDuration);
        Debug.Log("minWhiteNoise = " + minWhiteNoiseDuration);
        Debug.Log("maxWhiteNoise = " + maxWhiteNoiseDuration);
        Debug.Log("isWhiteNoiseRandom = " + isWhiteNoiseRandom);
        Debug.Log("cueRewardDelay = " + cueRewardDelay);
        Debug.Log("wnCueDelay = " + wnCueDelay);
        Debug.Log("isVisualStimEnabled = " + isVisualStimEnabled);
        Debug.Log("isAudioStimEnabled = " + isAudioStimEnabled);
        Debug.Log("useLaser = " + useLaser);
        Debug.Log("laserEvent = " + laserEvent);
        Debug.Log("fractionOfTrialsStimulated = " + fractionOfTrialsStimulated);

        Debug.Log("numberOfTargets = " + HFTargetConfigurator.shared.NumberOfTargets);
        Debug.Log("visual = {" + 
            HFTargetConfigurator.shared.T1VisualStim + ", " + 
            HFTargetConfigurator.shared.T2VisualStim + ", " + 
            HFTargetConfigurator.shared.T3VisualStim + ", " + 
            HFTargetConfigurator.shared.T4VisualStim + ", " + 
            HFTargetConfigurator.shared.T5VisualStim + ", " + 
            HFTargetConfigurator.shared.T6VisualStim + ", " + 
            HFTargetConfigurator.shared.T7VisualStim + ", " + 
            HFTargetConfigurator.shared.T8VisualStim + ", " + 
            HFTargetConfigurator.shared.T9VisualStim + ", " + 
            HFTargetConfigurator.shared.T10VisualStim + "}");

        Debug.Log("auditory = {" + 
            HFTargetConfigurator.shared.T1AudioStim + ", " + 
            HFTargetConfigurator.shared.T2AudioStim + ", " + 
            HFTargetConfigurator.shared.T3AudioStim + ", " + 
            HFTargetConfigurator.shared.T4AudioStim + ", " + 
            HFTargetConfigurator.shared.T5AudioStim + ", " + 
            HFTargetConfigurator.shared.T6AudioStim + ", " + 
            HFTargetConfigurator.shared.T7AudioStim + ", " + 
            HFTargetConfigurator.shared.T8AudioStim + ", " + 
            HFTargetConfigurator.shared.T9AudioStim + ", " + 
            HFTargetConfigurator.shared.T10AudioStim + "}");

        Debug.Log("outcomeProb = {" + 
            HFTargetConfigurator.shared.T1OutcomeProb + ", " + 
            HFTargetConfigurator.shared.T2OutcomeProb + ", " +
            HFTargetConfigurator.shared.T3OutcomeProb + ", " +
            HFTargetConfigurator.shared.T4OutcomeProb + ", " +
            HFTargetConfigurator.shared.T5OutcomeProb + ", " +
            HFTargetConfigurator.shared.T6OutcomeProb + ", " +
            HFTargetConfigurator.shared.T7OutcomeProb + ", " +
            HFTargetConfigurator.shared.T8OutcomeProb + ", " +
            HFTargetConfigurator.shared.T9OutcomeProb + ", " +
            HFTargetConfigurator.shared.T10OutcomeProb + "}");

        Debug.Log("durations = {" + 
            HFTargetConfigurator.shared.T1Duration + ", " + 
            HFTargetConfigurator.shared.T2Duration + ", " + 
            HFTargetConfigurator.shared.T3Duration + ", " + 
            HFTargetConfigurator.shared.T4Duration + ", " + 
            HFTargetConfigurator.shared.T5Duration + ", " + 
            HFTargetConfigurator.shared.T6Duration + ", " + 
            HFTargetConfigurator.shared.T7Duration + ", " + 
            HFTargetConfigurator.shared.T8Duration + ", " + 
            HFTargetConfigurator.shared.T9Duration + ", " + 
            HFTargetConfigurator.shared.T10Duration + "}");

        Debug.Log("delays = {" + 
            HFTargetConfigurator.shared.T1Delays + ", " + 
            HFTargetConfigurator.shared.T2Delays + ", " + 
            HFTargetConfigurator.shared.T3Delays + ", " + 
            HFTargetConfigurator.shared.T4Delays + ", " + 
            HFTargetConfigurator.shared.T5Delays + ", " + 
            HFTargetConfigurator.shared.T6Delays + ", " + 
            HFTargetConfigurator.shared.T7Delays + ", " + 
            HFTargetConfigurator.shared.T8Delays + ", " + 
            HFTargetConfigurator.shared.T9Delays + ", " +
            HFTargetConfigurator.shared.T10Delays + "}");

        Debug.Log("pump type = {" +
            HFTargetConfigurator.shared.T1PumpType + ", " +
            HFTargetConfigurator.shared.T2PumpType + ", " +
            HFTargetConfigurator.shared.T3PumpType + ", " +
            HFTargetConfigurator.shared.T4PumpType + ", " +
            HFTargetConfigurator.shared.T5PumpType + ", " +
            HFTargetConfigurator.shared.T6PumpType + ", " +
            HFTargetConfigurator.shared.T7PumpType + ", " +
            HFTargetConfigurator.shared.T8PumpType + ", " +
            HFTargetConfigurator.shared.T9PumpType + ", " +
            HFTargetConfigurator.shared.T10PumpType + "}");

    }

    // Use this for initialization
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = CommonUtils.frameRate;


        // Call all the functions to setup the environment
        SetupIsFlat();
        SetupBackgroundColor();
        SetupInterTrialTime();
        SetupTargetSize();
        SetupAnimalName();
        SetupLickToReward();
        SetupWhiteNoise();
        SetupCueRewardDelay();
        SetupWnCueDelay();
        SetupNumberOfTrials();
        SetupTargets();
        SetupLaser();

        // Print the configuration, if needed
        PrintStartingConfiguration();

        // Retrieve a reference for the scripts, starting from the GameObjects of the scene
        Sensor.GetComponentInChildren<Renderer>().receiveShadows = false;
        sensorScript = Sensor.GetComponentInChildren<SensorScript>();

        // Init a new DataCollector
        dc = new DataCollector
        {
            taskName = "HeadFixedClassicalConditioningTask",
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

        // Request to the start the imaging session (if available)
        sensorScript.StartRecording();



        // The experiment continues until the user does not explicitly request for termination
        while (((trialCnt < numberOfTrials) || numberOfTrials == -1) && (!quitApplication))
        {
            // A new recording element is created. All the following instructions regarding `dataInfo' are meant to collect data during the experiment
            DataCollector.Element dataInfo = dc.NewElem();
            dataInfo.TrialStart = DateTime.Now;

            // The sensorScript is set to initialize a new trial
            sensorScript.InitNewTrial();

            if (isWhiteNoiseEnabled) // If the white noise is enabled
            {
                // Calculate the duration for the white noise
                float wn = whiteNoiseDuration;
                if (isWhiteNoiseRandom) wn *= UnityEngine.Random.Range(minWhiteNoiseDuration, maxWhiteNoiseDuration);
                
                // Request to the sensor script to start the white noise
                sensorScript.Tone(ToneFrequency.WhiteNoiseStart);
                dataInfo.WhiteNoiseOn = DateTime.Now;

                if (useLaser && laserEvent == HFLaserEvent.TrialStart)
                {
                    if (fractionOfTrialsStimulatedMask[trialCnt])
                    {
                        sensorScript.TriggerLaser();
                        dataInfo.LaserStartTime = DateTime.Now;
                        dataInfo.IsLaserActivated = true;
                        dataInfo.fractionOfStimulatedTrials = fractionOfTrialsStimulated;
                    }
                }

                // Wait for the amount of time chosen
                while (wn > 0)
                {
                    wn -= Time.deltaTime;
                    yield return 0;
                }

                // Request to the sensor script to stop the white noise
                sensorScript.Tone(ToneFrequency.WhiteNoiseStop);
                dataInfo.WhiteNoiseOff = DateTime.Now;
            }

            // Wait for the amount of time chosen in the GUI between the white noise and the cue
            yield return new WaitForSecondsRealtime(wnCueDelay);

            int cueType = UnityEngine.Random.Range(0, numberOfTargets);

            dataInfo.VisualCueType = HFTargetConfigurator.shared.GetVisualStimIndex(cueType);
            dataInfo.AuditoryCueType = HFTargetConfigurator.shared.GetAudioStimIndex(cueType);
            dataInfo.OutcomeProbability = HFTargetConfigurator.shared.GetOutcomeProbability(cueType);

            cueTrials[cueType]++;

            // Set the target size in the variable `actualTargetSize'
            float actualTargetSize = targetSize;
            if (!isTargetSizeFixed) actualTargetSize *= UnityEngine.Random.Range(0.4f, 1.1f); //min size = 0.6; max size = 1.65 //Note: it should be always higher than the mouse indicator size
            dataInfo.TargetSize = actualTargetSize;

            // Set size and position and spawn a new target
            Vector3 spawnPosition;
            Quaternion spawnRotation;
            float timeoutCounter = HFTargetConfigurator.shared.GetDuration(cueType);
            int posIdx = 0;
            if (extendedScreen) posIdx = UnityEngine.Random.Range(0, 3);

            if (flat)
            {
                spawnPosition = new Vector3(gridPositions[posIdx].Item1, gridPositions[posIdx].Item2, gridPositions[posIdx].Item3);
                spawnRotation = new Quaternion();
                dataInfo.TargetPos.X = gridPositions[posIdx].Item1;
                dataInfo.TargetPos.Y = gridPositions[posIdx].Item2;
                dataInfo.TargetPos.Z = gridPositions[posIdx].Item3;

            } else
            {
                spawnPosition = new Vector3(250f, 1.6f, 250f);
                spawnRotation = Quaternion.Euler(gridPositions[posIdx].Item1, gridPositions[posIdx].Item2, gridPositions[posIdx].Item3);
                dataInfo.TargetPos.Pitch = gridPositions[posIdx].Item2;
                dataInfo.TargetPos.Yaw = gridPositions[posIdx].Item1;
                dataInfo.TargetPos.Roll = gridPositions[posIdx].Item3;
            }

            if (HFTargetConfigurator.shared.GetSoundBefore(cueType))
            {
                // Preallocation of the target
                targetSpawned = Instantiate(Target, spawnPosition, spawnRotation);
                targetSpawned.GetComponentInChildren<Renderer>().enabled = false;

                // Arrives here if for that cue the order should be audio -> delay -> video
                sensorScript.Tone(GetFrequency(HFTargetConfigurator.shared.GetAudioStimIndex(cueType)));
                dataInfo.AuditoryOn = DateTime.Now;

                // Activate laser stimulation iff: a) user requested to use laser, b) the event attached is "auditory on", c) the audio stimulus is not disabled
                if (useLaser && (laserEvent == HFLaserEvent.AuditoryOn) && (!(HFTargetConfigurator.shared.GetAudioStimIndex(cueType) == 0)))
                {
                    if (fractionOfTrialsStimulatedMask[trialCnt])
                    {
                        sensorScript.TriggerLaser();
                        dataInfo.LaserStartTime = DateTime.Now;
                        dataInfo.IsLaserActivated = true;
                        dataInfo.fractionOfStimulatedTrials = fractionOfTrialsStimulated;

                    }
                }

                yield return new WaitForSecondsRealtime(HFTargetConfigurator.shared.GetDelays(cueType));
                targetSpawned.GetComponentInChildren<Renderer>().material = GetMaterial(HFTargetConfigurator.shared.GetVisualStimIndex(cueType));
                targetSpawned.GetComponentInChildren<Renderer>().enabled = !(HFTargetConfigurator.shared.GetVisualStimIndex(cueType) == 0);
                if (isVisualStimEnabled) targetSpawned.transform.localScale = new Vector3(targetSize, targetSize, 1.0f);
                dataInfo.VisualTargetOnTime = DateTime.Now;

                // Activate laser stimulation iff: a) user requested to use laser, b) the event attached is "visual on", c) the visual stimulus is not disabled
                if (useLaser && (laserEvent == HFLaserEvent.VisualTargetOn) && (!(HFTargetConfigurator.shared.GetVisualStimIndex(cueType) == 0)))
                {
                    if (fractionOfTrialsStimulatedMask[trialCnt])
                    {
                        sensorScript.TriggerLaser();
                        dataInfo.LaserStartTime = DateTime.Now;
                        dataInfo.IsLaserActivated = true;
                        dataInfo.fractionOfStimulatedTrials = fractionOfTrialsStimulated;

                    }
                }

            }
            else
            {
                // Arrives here if for that cue the order should be video -> delay -> audio

                targetSpawned = Instantiate(Target, spawnPosition, spawnRotation);
                targetSpawned.GetComponentInChildren<Renderer>().enabled = !(HFTargetConfigurator.shared.GetVisualStimIndex(cueType) == 0);
                targetSpawned.GetComponentInChildren<Renderer>().material = GetMaterial(HFTargetConfigurator.shared.GetVisualStimIndex(cueType));
                if (isVisualStimEnabled) targetSpawned.transform.localScale = new Vector3(targetSize, targetSize, 1.0f);

                dataInfo.VisualTargetOnTime = DateTime.Now;

                // Activate laser stimulation iff: a) user requested to use laser, b) the event attached is "target on", c) the visual stimulus is not disabled
                if (useLaser)
                {
                    if ((laserEvent == HFLaserEvent.VisualTargetOn) && (!(HFTargetConfigurator.shared.GetVisualStimIndex(cueType) == 0)))
                    {
                        if (fractionOfTrialsStimulatedMask[trialCnt])
                        {
                            sensorScript.TriggerLaser();
                            dataInfo.LaserStartTime = DateTime.Now;
                            dataInfo.IsLaserActivated = true;
                            dataInfo.fractionOfStimulatedTrials = fractionOfTrialsStimulated;

                        }
                    }
                }

                yield return new WaitForSecondsRealtime(HFTargetConfigurator.shared.GetDelays(cueType));
                sensorScript.Tone(GetFrequency(HFTargetConfigurator.shared.GetAudioStimIndex(cueType)));
                dataInfo.AuditoryOn = DateTime.Now;

                // Activate laser stimulation iff: a) user requested to use laser, b) the event attached is "auditory on", c) the audio stimulus is not disabled
                if (useLaser && (laserEvent == HFLaserEvent.AuditoryOn) && (!(HFTargetConfigurator.shared.GetAudioStimIndex(cueType) == 0)))
                {
                    if (fractionOfTrialsStimulatedMask[trialCnt])
                    {
                        sensorScript.TriggerLaser();
                        dataInfo.LaserStartTime = DateTime.Now;
                        dataInfo.IsLaserActivated = true;
                        dataInfo.fractionOfStimulatedTrials = fractionOfTrialsStimulated;

                    }
                }

            }

            // Set a destroy timer equals to 1 second. This duration is not adjustable, because should be coordinated with the sound (that is embedded in the arduino script).
            Destroy(targetSpawned, timeoutCounter);

            // Wait for the cue to disappear
            while (timeoutCounter > 0)
            {
                timeoutCounter -= Time.deltaTime;
                yield return 0;
            }

            dataInfo.TargetDisappearingTime = DateTime.Now;
            if (useLaser && (laserEvent == HFLaserEvent.VisualTargetOff))
            {
                if (fractionOfTrialsStimulatedMask[trialCnt])
                {
                    sensorScript.TriggerLaser();
                    dataInfo.LaserStartTime = DateTime.Now;
                    dataInfo.IsLaserActivated = true;
                    dataInfo.fractionOfStimulatedTrials = fractionOfTrialsStimulated;

                }
            }

            // Wait for the amount of time chosen in the GUI between the white noise and the cue
            yield return new WaitForSecondsRealtime(cueRewardDelay);
            dataInfo.CueOutcomeDelay = cueRewardDelay;

            if (HFTargetConfigurator.shared.GetPumpType(cueType) == 0) dataInfo.OutcomeType = false;
            else dataInfo.OutcomeType = true;

            // Procedure of online calculation of the ratio.
            if ((cueRewardedTrials[cueType] / cueTrials[cueType]) < HFTargetConfigurator.shared.GetOutcomeProbability(cueType)) // If the trial should be rewarded
            {
                cueRewardedTrials[cueType]++;
                dataInfo.IsOutcomeDelivered = true;

                // Request to the sensor script to play the `Success' or `Missed' tone and to deliver the reward/punishment
                if(HFTargetConfigurator.shared.GetPumpType(cueType) == 0)
                {
                    sensorScript.Tone(ToneFrequency.SuccessTone);
                    sensorScript.Reward();
                    dataInfo.OutcomeTime = DateTime.Now;

                    // activate laser if it was requested on reward or rewardTone
                    if (useLaser && (laserEvent == HFLaserEvent.Reward || laserEvent == HFLaserEvent.RewardToneOn || laserEvent == HFLaserEvent.Outcome || laserEvent == HFLaserEvent.OutcomeToneOn))
                    {
                        if (fractionOfTrialsStimulatedMask[trialCnt])
                        {
                            sensorScript.TriggerLaser();
                            dataInfo.LaserStartTime = DateTime.Now;
                            dataInfo.IsLaserActivated = true;
                            dataInfo.fractionOfStimulatedTrials = fractionOfTrialsStimulated;

                        }
                    }

                } else
                {
                    sensorScript.Tone(ToneFrequency.MissedTone);
                    sensorScript.Punishment();
                    dataInfo.OutcomeTime = DateTime.Now;

                    // activate laser if it was requested on penalty or penaltyTone
                    if (useLaser && (laserEvent == HFLaserEvent.Punishment || laserEvent == HFLaserEvent.PunishmentToneOn || laserEvent == HFLaserEvent.Outcome || laserEvent == HFLaserEvent.OutcomeToneOn))
                    {
                        if (fractionOfTrialsStimulatedMask[trialCnt])
                        {
                            sensorScript.TriggerLaser();
                            dataInfo.LaserStartTime = DateTime.Now;
                            dataInfo.IsLaserActivated = true;
                            dataInfo.fractionOfStimulatedTrials = fractionOfTrialsStimulated;

                        }
                    }
                }

                

                // Check if in the remaining time of the trial (aka ITI) the reward is consumed or not
                bool consumed = false;
                float iti = interTrialTime;
                if (isInterTrialTimeRandom) iti = UnityEngine.Random.Range(minInterTrialTime, maxInterTrialTime);

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
            else // If the trial should NOT be rewarded
            {
                // Wait for the ITI
                dataInfo.ITIOn = DateTime.Now;
                if (isInterTrialTimeRandom) yield return new WaitForSecondsRealtime(UnityEngine.Random.Range(minInterTrialTime, maxInterTrialTime));
                else yield return new WaitForSecondsRealtime(interTrialTime);
                dataInfo.ITIOff = DateTime.Now;

            }

            // Save the licks timestamps and the head positions
            dataInfo.LicksTimeOptical = new List<DateTime>(sensorScript.licksTimeOptical);
            dataInfo.LicksTimeElectrical = new List<DateTime>(sensorScript.licksTimeElectrical);
            dataInfo.HeadPositions = new List<DataCollector.Position>(sensorScript.headPositions);

            dataInfo.TrialEnd = DateTime.Now;
            trialCnt++;
        }

        sensorScript.StopRecording();
        Debug.Log("Task completed");
    }

    private ToneFrequency GetFrequency(int audioStimIndex)
    {
        switch (audioStimIndex)
        {
            case 0:
                return ToneFrequency.None;
            case 1:
                return ToneFrequency.KHz1_5;
            case 2:
                return ToneFrequency.KHz3;
            case 3:
                return ToneFrequency.KHz4_5;
            case 4:
                return ToneFrequency.KHz7;
            case 5:
                return ToneFrequency.KHz9_5;
            default:
                return ToneFrequency.None;
        }
    }

    private ref Material GetMaterial(int visualStimIndex)
    {
        switch (visualStimIndex)
        {
            case 1:
                return ref material1;
            case 2:
                return ref material2;
            case 3:
                return ref material3;
            case 4:
                return ref material4;
            case 5:
                return ref material5;
            case 6:
                return ref material6;
            case 7:
                return ref material7;
            case 8:
                return ref material8;
            case 9:
                return ref material9;
            case 10:
                return ref material10;
            case 11:
                return ref material11;
            case 12:
                return ref material12;
            case 13:
                return ref material13;
            case 14:
                return ref material14;
            case 15:
                return ref material15;
            case 16:
                return ref material16;
            case 17:
                return ref material17;
            case 18:
                return ref material18;
            case 19:
                return ref material19;
            case 20:
                return ref material20;
            case 21:
                return ref material21;
            case 22:
                return ref material22;
            case 23:
                return ref material23;

            default:
                return ref material13; //In any other cases, the target should not have a material. This is handled outside from this function
        }
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

