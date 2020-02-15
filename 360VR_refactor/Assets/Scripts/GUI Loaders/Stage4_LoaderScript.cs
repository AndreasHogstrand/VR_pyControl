using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

/**
 * This class handles the GUI for the Head Fixed task. 
 * It is attached to the Canvas of the scene `Stage4_loader.unity'
 */
public class Stage4_LoaderScript : MonoBehaviour {

    // UI Elements linked to sliders, labels, checkbox, etc. in the Unity scene
    public GameObject loadingImage;
    public Button runBtn;

    // Environment settings
    public Slider brightness;
    public Text brightnessLbl;
    public Toggle flat;
    public Slider numberOfTrials;
    public Text numberOfTrialsLbl;
    public Toggle isNumTrialsUnlimited;
    public Slider targetSize;
    public Text targetSizeLbl;
    public Toggle isTargetSizeRandom;
    public Toggle useExtendedTargets;

    // Task global settings
    public Slider whiteNoise;
    public Text whiteNoiseLbl;
    public Toggle isWhiteNoiseRandom;
    public Slider minWhiteNoiseDuration;
    public Text minWhiteNoiseDurationLbl;
    public Slider maxWhiteNoiseDuration;
    public Text maxWhiteNoiseDurationLbl;
    public Toggle disableWn;
    public Slider interTrialTime;
    public Text interTrialTimeLbl;
    public Slider minInterTrialTime;
    public Text minInterTrialTimeLbl;
    public Slider maxInterTrialTime;
    public Text maxInterTrialTimeLbl;
    public Toggle isInterTrialTimeRandom;
    public Slider licksToReward;
    public InputField animalName;
    public Text licksToRewardLbl;
    public Slider cueRewardDelay;
    public Text cueRewardDelayLbl;
    public Slider wnCueDelay;
    public Text wnCueDelayLbl;
    public Dropdown laserEvent;
    public Text fractionOfTrialsStimulatedLbl;
    public Toggle useLaser;
    public Slider fractionOfTrialsStimulated;

    // Task specific settings
    public Dropdown numberOfTargets;
    public GameObject target1Row;
    public GameObject target2Row;
    public GameObject target3Row;
    public GameObject target4Row;
    public GameObject target5Row;
    public GameObject target6Row;
    public GameObject target7Row;
    public GameObject target8Row;
    public GameObject target9Row;
    public GameObject target10Row;

    // Target1
    public Dropdown visualStim1;
    public Dropdown audioStim1;
    public Toggle soundBefore1;
    public Slider outcomeProb1;
    public Text outcomeProb1Lbl;
    public Slider duration1;
    public Text duration1Lbl;
    public Slider cueDelays1;
    public Text cueDelays1Lbl;
    public Dropdown pumpType1;

    // Target2
    public Dropdown visualStim2;
    public Dropdown audioStim2;
    public Toggle soundBefore2;
    public Slider outcomeProb2;
    public Text outcomeProb2Lbl;
    public Slider duration2;
    public Text duration2Lbl;
    public Slider cueDelays2;
    public Text cueDelays2Lbl;
    public Dropdown pumpType2;

    // Target3
    public Dropdown visualStim3;
    public Dropdown audioStim3;
    public Toggle soundBefore3;
    public Slider outcomeProb3;
    public Text outcomeProb3Lbl;
    public Slider duration3;
    public Text duration3Lbl;
    public Slider cueDelays3;
    public Text cueDelays3Lbl;
    public Dropdown pumpType3;

    // Target4
    public Dropdown visualStim4;
    public Dropdown audioStim4;
    public Toggle soundBefore4;
    public Slider outcomeProb4;
    public Text outcomeProb4Lbl;
    public Slider duration4;
    public Text duration4Lbl;
    public Slider cueDelays4;
    public Text cueDelays4Lbl;
    public Dropdown pumpType4;

    // Target5
    public Dropdown visualStim5;
    public Dropdown audioStim5;
    public Toggle soundBefore5;
    public Slider outcomeProb5;
    public Text outcomeProb5Lbl;
    public Slider duration5;
    public Text duration5Lbl;
    public Slider cueDelays5;
    public Text cueDelays5Lbl;
    public Dropdown pumpType5;

    // Target6
    public Dropdown visualStim6;
    public Dropdown audioStim6;
    public Toggle soundBefore6;
    public Slider outcomeProb6;
    public Text outcomeProb6Lbl;
    public Slider duration6;
    public Text duration6Lbl;
    public Slider cueDelays6;
    public Text cueDelays6Lbl;
    public Dropdown pumpType6;

    // Target7
    public Dropdown visualStim7;
    public Dropdown audioStim7;
    public Toggle soundBefore7;
    public Slider outcomeProb7;
    public Text outcomeProb7Lbl;
    public Slider duration7;
    public Text duration7Lbl;
    public Slider cueDelays7;
    public Text cueDelays7Lbl;
    public Dropdown pumpType7;

    // Target8
    public Dropdown visualStim8;
    public Dropdown audioStim8;
    public Toggle soundBefore8;
    public Slider outcomeProb8;
    public Text outcomeProb8Lbl;
    public Slider duration8;
    public Text duration8Lbl;
    public Slider cueDelays8;
    public Text cueDelays8Lbl;
    public Dropdown pumpType8;

    // Target9
    public Dropdown visualStim9;
    public Dropdown audioStim9;
    public Toggle soundBefore9;
    public Slider outcomeProb9;
    public Text outcomeProb9Lbl;
    public Slider duration9;
    public Text duration9Lbl;
    public Slider cueDelays9;
    public Text cueDelays9Lbl;
    public Dropdown pumpType9;

    // Target10
    public Dropdown visualStim10;
    public Dropdown audioStim10;
    public Toggle soundBefore10;
    public Slider outcomeProb10;
    public Text outcomeProb10Lbl;
    public Slider duration10;
    public Text duration10Lbl;
    public Slider cueDelays10;
    public Text cueDelays10Lbl;
    public Dropdown pumpType10;

    void Start(){

        // Delete all the previous settings (if any)
        MainMenuScript.CleanupSettings();
        onNumberOfTargetChange(0);

    }

    private void Update()
    {
        // Press Esc to go to the main menu
        if (Input.GetKeyDown(KeyCode.Escape)) SceneManager.LoadScene(0);
    }

    /**
    * This function gathers information from the UI elements and save their values in the PlayerPrefs before launching the scene.
    */
    public void LoadScene(int level)
    {
        // the target size and if it is random
        if (isTargetSizeRandom.isOn) PlayerPrefs.SetInt("isTargetSizeRandom", 1);
        else PlayerPrefs.SetInt("isTargetSizeRandom", 0);
        PlayerPrefs.SetFloat("targetSize", targetSize.value);

        // the brightness of the background
        PlayerPrefs.SetInt("brightness", (int)brightness.value);


        if (!isNumTrialsUnlimited.isOn) PlayerPrefs.SetInt("numberOfTrials", (int)numberOfTrials.value);
        else PlayerPrefs.SetInt("numberOfTrials", -1);

        // the ITI, if it is random and (in this case) its min/max
        if (isInterTrialTimeRandom.isOn) PlayerPrefs.SetInt("isInterTrialTimeRandom", 1);
        else PlayerPrefs.SetInt("isInterTrialTimeRandom", 0);
        PlayerPrefs.SetFloat("interTrialTime", interTrialTime.value);
        PlayerPrefs.SetFloat("minInterTrialTime", minInterTrialTime.value);
        PlayerPrefs.SetFloat("maxInterTrialTime", maxInterTrialTime.value);

        // the name of the animal (used to create a folder, so *do not* use special characters)
        PlayerPrefs.SetString("animalName", animalName.text.ToLower());

        // if the white noise duration is random, its duration and (in case of random) the min/max values or if it is disabled
        if (isWhiteNoiseRandom.isOn) PlayerPrefs.SetInt("isWhiteNoiseRandom", 1);
        else PlayerPrefs.SetInt("isWhiteNoiseRandom", 0);
        PlayerPrefs.SetFloat("whiteNoise", whiteNoise.value);
        PlayerPrefs.SetFloat("minWhiteNoiseDuration", minWhiteNoiseDuration.value);
        PlayerPrefs.SetFloat("maxWhiteNoiseDuration", maxWhiteNoiseDuration.value);
        if (disableWn.isOn) PlayerPrefs.SetInt("isWnEnabled", 0);
        else PlayerPrefs.SetInt("isWnEnabled", 1);

        // the duration of the delay between the end of the white noise and the appearing of the cue
        PlayerPrefs.SetFloat("wnCueDelay", wnCueDelay.value);

        // the duration of the delay between the disapperaing of the cue and the reward
        PlayerPrefs.SetFloat("cueRewardDelay", cueRewardDelay.value);

        // the numbers of detected licks to consider a reward as consumed
        PlayerPrefs.SetInt("licksToReward", (int)licksToReward.value);

        // set visible the loading image
        loadingImage.SetActive(true);

        // set if play in a flat screen or not
        if(flat.isOn) PlayerPrefs.SetInt("flat", 1);
        else PlayerPrefs.SetInt("flat", 0);

        // If use the laser stimulation or not
        if (!isNumTrialsUnlimited.isOn)
        {
            if (useLaser.isOn) PlayerPrefs.SetInt("useLaser", 1);
            else PlayerPrefs.SetInt("useLaser", 0);
            PlayerPrefs.SetInt("laserEvent", laserEvent.value);
            PlayerPrefs.SetFloat("fractionOfTrialsStimulated", (float)(fractionOfTrialsStimulated.value / 100.0));
        }
        else
        {
            PlayerPrefs.SetInt("useLaser", 0);
            PlayerPrefs.SetFloat("fractionOfTrialsStimulated", 1.0f);
        }

        // save settings of the targets
        HFTargetConfigurator.shared.NumberOfTargets = numberOfTargets.value;
        HFTargetConfigurator.shared.T1AudioStim = audioStim1.value;
        HFTargetConfigurator.shared.T1VisualStim = visualStim1.value;
        HFTargetConfigurator.shared.T1SoundBefore = soundBefore1.isOn;
        HFTargetConfigurator.shared.T1OutcomeProb = outcomeProb1.value/100.0f;
        HFTargetConfigurator.shared.T1Duration = duration1.value;
        HFTargetConfigurator.shared.T1Delays = cueDelays1.value;
        HFTargetConfigurator.shared.T1PumpType = pumpType1.value;

        HFTargetConfigurator.shared.T2AudioStim = audioStim2.value;
        HFTargetConfigurator.shared.T2VisualStim = visualStim2.value;
        HFTargetConfigurator.shared.T2SoundBefore = soundBefore2.isOn;
        HFTargetConfigurator.shared.T2OutcomeProb = outcomeProb2.value / 100.0f;
        HFTargetConfigurator.shared.T2Duration = duration2.value;
        HFTargetConfigurator.shared.T2Delays = cueDelays2.value;
        HFTargetConfigurator.shared.T2PumpType = pumpType2.value;

        HFTargetConfigurator.shared.T3AudioStim = audioStim3.value;
        HFTargetConfigurator.shared.T3VisualStim = visualStim3.value;
        HFTargetConfigurator.shared.T3SoundBefore = soundBefore3.isOn;
        HFTargetConfigurator.shared.T3OutcomeProb = outcomeProb3.value / 100.0f;
        HFTargetConfigurator.shared.T3Duration = duration3.value;
        HFTargetConfigurator.shared.T3Delays = cueDelays3.value;
        HFTargetConfigurator.shared.T3PumpType = pumpType3.value;

        HFTargetConfigurator.shared.T4AudioStim = audioStim4.value;
        HFTargetConfigurator.shared.T4VisualStim = visualStim4.value;
        HFTargetConfigurator.shared.T4SoundBefore = soundBefore4.isOn;
        HFTargetConfigurator.shared.T4OutcomeProb = outcomeProb4.value / 100.0f;
        HFTargetConfigurator.shared.T4Duration = duration4.value;
        HFTargetConfigurator.shared.T4Delays = cueDelays4.value;
        HFTargetConfigurator.shared.T4PumpType = pumpType4.value;

        HFTargetConfigurator.shared.T5AudioStim = audioStim5.value;
        HFTargetConfigurator.shared.T5VisualStim = visualStim5.value;
        HFTargetConfigurator.shared.T5SoundBefore = soundBefore5.isOn;
        HFTargetConfigurator.shared.T5OutcomeProb = outcomeProb5.value / 100.0f;
        HFTargetConfigurator.shared.T5Duration = duration5.value;
        HFTargetConfigurator.shared.T5Delays = cueDelays5.value;
        HFTargetConfigurator.shared.T5PumpType = pumpType5.value;

        HFTargetConfigurator.shared.T6AudioStim = audioStim6.value;
        HFTargetConfigurator.shared.T6VisualStim = visualStim6.value;
        HFTargetConfigurator.shared.T6SoundBefore = soundBefore6.isOn;
        HFTargetConfigurator.shared.T6OutcomeProb = outcomeProb6.value / 100.0f;
        HFTargetConfigurator.shared.T6Duration = duration6.value;
        HFTargetConfigurator.shared.T6Delays = cueDelays6.value;
        HFTargetConfigurator.shared.T6PumpType = pumpType6.value;

        HFTargetConfigurator.shared.T7AudioStim = audioStim7.value;
        HFTargetConfigurator.shared.T7VisualStim = visualStim7.value;
        HFTargetConfigurator.shared.T7SoundBefore = soundBefore7.isOn;
        HFTargetConfigurator.shared.T7OutcomeProb = outcomeProb7.value / 100.0f;
        HFTargetConfigurator.shared.T7Duration = duration7.value;
        HFTargetConfigurator.shared.T7Delays = cueDelays7.value;
        HFTargetConfigurator.shared.T7PumpType = pumpType7.value;

        HFTargetConfigurator.shared.T8AudioStim = audioStim8.value;
        HFTargetConfigurator.shared.T8VisualStim = visualStim8.value;
        HFTargetConfigurator.shared.T8SoundBefore = soundBefore8.isOn;
        HFTargetConfigurator.shared.T8OutcomeProb = outcomeProb8.value / 100.0f;
        HFTargetConfigurator.shared.T8Duration = duration8.value;
        HFTargetConfigurator.shared.T8Delays = cueDelays8.value;
        HFTargetConfigurator.shared.T8PumpType = pumpType8.value;

        HFTargetConfigurator.shared.T9AudioStim = audioStim9.value;
        HFTargetConfigurator.shared.T9VisualStim = visualStim9.value;
        HFTargetConfigurator.shared.T9SoundBefore = soundBefore9.isOn;
        HFTargetConfigurator.shared.T9OutcomeProb = outcomeProb9.value / 100.0f;
        HFTargetConfigurator.shared.T9Duration = duration9.value;
        HFTargetConfigurator.shared.T9Delays = cueDelays9.value;
        HFTargetConfigurator.shared.T9PumpType = pumpType9.value;

        HFTargetConfigurator.shared.T10AudioStim = audioStim10.value;
        HFTargetConfigurator.shared.T10VisualStim = visualStim10.value;
        HFTargetConfigurator.shared.T10SoundBefore = soundBefore10.isOn;
        HFTargetConfigurator.shared.T10OutcomeProb = outcomeProb10.value / 100.0f;
        HFTargetConfigurator.shared.T10Duration = duration10.value;
        HFTargetConfigurator.shared.T10Delays = cueDelays10.value;
        HFTargetConfigurator.shared.T10PumpType = pumpType10.value;

        if (useExtendedTargets.isOn) PlayerPrefs.SetInt("extendedScreen", 1);
        else PlayerPrefs.SetInt("extendedScreen", 0);

        // load the scene "Stage4" or "Stage4flat"
        if (flat.isOn) SceneManager.LoadScene(level+1);
        else SceneManager.LoadScene(level);
    }

    /**
     * Function handler of the brightness slider
     */
    public void onBrightnessChange(float newVal)
    {
        brightnessLbl.text = newVal.ToString();
    }

    /**
     * Function handler of the number of trials slider
     */
    public void onNumberOfTrialsChange(float newVal)
    {
        numberOfTrialsLbl.text = newVal.ToString();
    }

    /**
     * Function handler of the ITI random checkbox
     */
    public void onIsInterTrialTimeRandomChange(bool newVal)
    {
        // if the random checkbox has been set, then the fixed slider should be deactivated and
        // the random sliders should be activated
        if (newVal)
        {
            interTrialTime.interactable = false;
            minInterTrialTime.interactable = true;
            maxInterTrialTime.interactable = true;
            minInterTrialTimeLbl.enabled = true;
            maxInterTrialTimeLbl.enabled = true;
            interTrialTimeLbl.enabled = false;
        }
        // otherwise, the fixed slider should be activated, and the random sliders should be deactivated
        else
        {
            interTrialTime.interactable = true;
            minInterTrialTime.interactable = false;
            maxInterTrialTime.interactable = false;
            minInterTrialTimeLbl.enabled = false;
            maxInterTrialTimeLbl.enabled = false;
            interTrialTimeLbl.enabled = true;
        }
    }

    /**
     * Function handler of the fixed ITI slider
     */
    public void onInterTrialChange(float newVal)
    {
        interTrialTimeLbl.text = newVal.ToString();
    }

    /**
     * Function handler of the minimum random ITI slider
     */
    public void onMinInterTrialChange(float newVal)
    {
        minInterTrialTimeLbl.text = newVal.ToString();
        if (newVal > maxInterTrialTime.value) maxInterTrialTime.value = newVal;
    }

    /**
     * Function handler of the maximum random ITI slider
     */
    public void onMaxInterTrialChange(float newVal)
    {
        maxInterTrialTimeLbl.text = newVal.ToString();
        if (newVal < minInterTrialTime.value) minInterTrialTime.value = newVal;

    }

    /**
     * Function handler of the unlimited number of trials checkbox
     */
    public void onUnlimitedChange(bool newVal)
    {
        numberOfTrials.interactable = !newVal;
        numberOfTrialsLbl.enabled = !newVal;
        laserEvent.interactable = !newVal;
        useLaser.interactable = !newVal;
        fractionOfTrialsStimulatedLbl.enabled = !newVal;
        fractionOfTrialsStimulated.interactable = !newVal;
        numberOfTrialsLbl.enabled = !newVal;
        populateLaserEventDropdown();
    }

    /**
     * Function handler of the random size checkbox for the target
     */
    public void onIsTargetSizeRandomChange(bool newVal)
    {
        targetSize.interactable = !newVal;
        targetSizeLbl.enabled = !newVal;
    }

    /**
     * Function handler of the target size slider
     */
    public void onTargetSizeChange(float newVal)
    {
        targetSizeLbl.text = newVal.ToString();
    }

    /**
     * Function handler of the white noise fixed duration slider
     */
    public void onWhiteNoiseChange(float newVal)
    {
        whiteNoiseLbl.text = newVal.ToString();
    }

    /**
     * Function handler of the white noise random checkbox
     * it works in the same way as the ITI
     */
    public void onIsWhiteNoiseRandomChange(bool newVal)
    {
        if (newVal)
        {
            whiteNoise.interactable = false;
            minWhiteNoiseDuration.interactable = true;
            maxWhiteNoiseDuration.interactable = true;
            minWhiteNoiseDurationLbl.enabled = true;
            maxWhiteNoiseDurationLbl.enabled = true;
            whiteNoiseLbl.enabled = false;
        }
        else
        {
            whiteNoise.interactable = true;
            minWhiteNoiseDuration.interactable = false;
            maxWhiteNoiseDuration.interactable = false;
            minWhiteNoiseDurationLbl.enabled = false;
            maxWhiteNoiseDurationLbl.enabled = false;
            whiteNoiseLbl.enabled = true;
        }
    }

    /**
     * Function handler of the white noise disable checkbox
     */
    public void onIsWhiteNoiseDisableChange(bool newVal)
    {
        // If the checkbox has been set, the white noise will be disabled,
        // so all the controls about the white noise should be disabled.
        if (newVal)
        {
            whiteNoise.interactable = false;
            minWhiteNoiseDuration.interactable = false;
            maxWhiteNoiseDuration.interactable = false;
            minWhiteNoiseDurationLbl.enabled = false;
            maxWhiteNoiseDurationLbl.enabled = false;
            whiteNoiseLbl.enabled = false;
            isWhiteNoiseRandom.interactable = false;
        }
        // If the checkbox has been unset, the white noise will be present,
        // so all the controls about the white noise should be brought as they
        // were before setting the `disable white noise' checkbox
        else
        {

            isWhiteNoiseRandom.interactable = true;
            onIsWhiteNoiseRandomChange(isWhiteNoiseRandom.isOn);
        }
    }

    /**
     * Function handler of the minimum random ITI-success slider
     */
    public void onMinWhiteNoiseChange(float newVal)
    {
        minWhiteNoiseDurationLbl.text = newVal.ToString();
        if (newVal > maxWhiteNoiseDuration.value) maxWhiteNoiseDuration.value = newVal;
    }

    /**
     * Function handler of the maximum random white noise slider
     */
    public void onMaxWhiteNoiseChange(float newVal)
    {
        maxWhiteNoiseDurationLbl.text = newVal.ToString();
        if (newVal < minWhiteNoiseDuration.value) minWhiteNoiseDuration.value = newVal;

    }

    /**
     * Function handler of the delay between cue and reward slider
     */
    public void onCueRewardDelayChange(float newVal)
    {
        cueRewardDelayLbl.text = newVal.ToString();
    }

    /**
     * Function handler of the delay between white noise and cue slider
     */
    public void onWnCueDelayChange(float newVal)
    {
        wnCueDelayLbl.text = newVal.ToString();
    }

    /**
     * Function handler of the `licks to reward` slider
     */
    public void onLicksToRewardChange(float newVal)
    {
        licksToRewardLbl.text = newVal.ToString();
    }

    /**
     * Function handler of the fraction of trials stimulated slider
     */
    public void onFractionOfTrialsStimulatedChange(float newVal)
    {
        fractionOfTrialsStimulatedLbl.text = newVal.ToString() + " %";
    }

    public void populateLaserEventDropdown()
    {
        List<string> names = new List<string>();

        names.Add(HFLaserEvent.GetName(typeof(HFLaserEvent), 0));
        names.Add(HFLaserEvent.GetName(typeof(HFLaserEvent), 1));
        names.Add(HFLaserEvent.GetName(typeof(HFLaserEvent), 2));
        names.Add(HFLaserEvent.GetName(typeof(HFLaserEvent), 3));
        names.Add(HFLaserEvent.GetName(typeof(HFLaserEvent), 4));
        names.Add(HFLaserEvent.GetName(typeof(HFLaserEvent), 5));
        names.Add(HFLaserEvent.GetName(typeof(HFLaserEvent), 6));
        names.Add(HFLaserEvent.GetName(typeof(HFLaserEvent), 7));
        names.Add(HFLaserEvent.GetName(typeof(HFLaserEvent), 8));
        names.Add(HFLaserEvent.GetName(typeof(HFLaserEvent), 9));
        names.Add(HFLaserEvent.GetName(typeof(HFLaserEvent), 10));

        laserEvent.ClearOptions();
        laserEvent.AddOptions(names);
    }

    public void onNumberOfTargetChange(int value)
    {
        runBtn.interactable = true;
        ActivateRowsTill(value);
    }

    //Target 1 Handling functions
    public void onOutcomeProb1Change(float val)
    {
        outcomeProb1Lbl.text = val.ToString() + "%";
    }
    public void onDuration1Change(float val)
    {
        duration1Lbl.text = val.ToString();
    }

    public void onCueDelays1Change(float val)
    {
        cueDelays1Lbl.text = val.ToString();
    }

    //Target 2 Handling functions
    public void onOutcomeProb2Change(float val)
    {
        outcomeProb2Lbl.text = val.ToString() + "%";
    }
    public void onDuration2Change(float val)
    {
        duration2Lbl.text = val.ToString();
    }

    public void onCueDelays2Change(float val)
    {
        cueDelays2Lbl.text = val.ToString();
    }

    //Target 3 Handling functions
    public void onOutcomeProb3Change(float val)
    {
        outcomeProb3Lbl.text = val.ToString() + "%";
    }
    public void onDuration3Change(float val)
    {
        duration3Lbl.text = val.ToString();
    }

    public void onCueDelays3Change(float val)
    {
        cueDelays3Lbl.text = val.ToString();
    }

    //Target 4 Handling functions
    public void onOutcomeProb4Change(float val)
    {
        outcomeProb4Lbl.text = val.ToString() + "%";
    }
    public void onDuration4Change(float val)
    {
        duration4Lbl.text = val.ToString();
    }

    public void onCueDelays4Change(float val)
    {
        cueDelays4Lbl.text = val.ToString();
    }

    //Target 5 Handling functions
    public void onOutcomeProb5Change(float val)
    {
        outcomeProb5Lbl.text = val.ToString() + "%";
    }
    public void onDuration5Change(float val)
    {
        duration5Lbl.text = val.ToString();
    }

    public void onCueDelays5Change(float val)
    {
        cueDelays5Lbl.text = val.ToString();
    }

    //Target 6 Handling functions
    public void onOutcomeProb6Change(float val)
    {
        outcomeProb6Lbl.text = val.ToString() + "%";
    }
    public void onDuration6Change(float val)
    {
        duration6Lbl.text = val.ToString();
    }

    public void onCueDelays6Change(float val)
    {
        cueDelays6Lbl.text = val.ToString();
    }

    //Target 7 Handling functions
    public void onOutcomeProb7Change(float val)
    {
        outcomeProb7Lbl.text = val.ToString() + "%";
    }
    public void onDuration7Change(float val)
    {
        duration7Lbl.text = val.ToString();
    }

    public void onCueDelays7Change(float val)
    {
        cueDelays7Lbl.text = val.ToString();
    }

    //Target 8 Handling functions
    public void onOutcomeProb8Change(float val)
    {
        outcomeProb8Lbl.text = val.ToString() + "%";
    }
    public void onDuration8Change(float val)
    {
        duration8Lbl.text = val.ToString();
    }

    public void onCueDelays8Change(float val)
    {
        cueDelays8Lbl.text = val.ToString();
    }

    //Target 9 Handling functions
    public void onOutcomeProb9Change(float val)
    {
        outcomeProb9Lbl.text = val.ToString() + "%";
    }
    public void onDuration9Change(float val)
    {
        duration9Lbl.text = val.ToString();
    }

    public void onCueDelays9Change(float val)
    {
        cueDelays9Lbl.text = val.ToString();
    }

    //Target 10 Handling functions
    public void onOutcomeProb10Change(float val)
    {
        outcomeProb10Lbl.text = val.ToString() + "%";
    }
    public void onDuration10Change(float val)
    {
        duration10Lbl.text = val.ToString();
    }

    public void onCueDelays10Change(float val)
    {
        cueDelays10Lbl.text = val.ToString();
    }

    private void SetTarget1(bool enabled)
    {
        target1Row.SetActive(enabled);
        
        /* Uncomment to only disable things, without hiding
        visualStim1.interactable = enabled;
        audioStim1.interactable = enabled;
        soundBefore1.interactable = enabled;
        outcomeProb1.interactable = enabled;
        outcomeProb1Lbl.enabled = enabled;
        duration1.interactable = enabled;
        duration1Lbl.enabled = enabled;
        cueDelays1.interactable = enabled;
        cueDelays1Lbl.enabled = enabled;
        */
    }

    private void SetTarget2(bool enabled)
    {
        target2Row.SetActive(enabled);
    }

    private void SetTarget3(bool enabled)
    {
        target3Row.SetActive(enabled);
    }

    private void SetTarget4(bool enabled)
    {
        target4Row.SetActive(enabled);
    }

    private void SetTarget5(bool enabled)
    {
        target5Row.SetActive(enabled);
    }

    private void SetTarget6(bool enabled)
    {
        target6Row.SetActive(enabled);
    }

    private void SetTarget7(bool enabled)
    {
        target7Row.SetActive(enabled);
    }

    private void SetTarget8(bool enabled)
    {
        target8Row.SetActive(enabled);
    }

    private void SetTarget9(bool enabled)
    {
        target9Row.SetActive(enabled);
    }

    private void SetTarget10(bool enabled)
    {
        target10Row.SetActive(enabled);
    }

    private void ActivateRowsTill(int lastRow)
    {
        switch (lastRow)
        {
            case 0:
                runBtn.interactable = false;

                SetTarget1(false);
                SetTarget2(false);
                SetTarget3(false);
                SetTarget4(false);
                SetTarget5(false);
                SetTarget6(false);
                SetTarget7(false);
                SetTarget8(false);
                SetTarget9(false);
                SetTarget10(false);
                break;

            case 1:
                SetTarget1(true);
                SetTarget2(false);
                SetTarget3(false);
                SetTarget4(false);
                SetTarget5(false);
                SetTarget6(false);
                SetTarget7(false);
                SetTarget8(false);
                SetTarget9(false);
                SetTarget10(false);
                break;

            case 2:
                SetTarget1(true);
                SetTarget2(true);
                SetTarget3(false);
                SetTarget4(false);
                SetTarget5(false);
                SetTarget6(false);
                SetTarget7(false);
                SetTarget8(false);
                SetTarget9(false);
                SetTarget10(false);
                break;

            case 3:
                SetTarget1(true);
                SetTarget2(true);
                SetTarget3(true);
                SetTarget4(false);
                SetTarget5(false);
                SetTarget6(false);
                SetTarget7(false);
                SetTarget8(false);
                SetTarget9(false);
                SetTarget10(false);
                break;

            case 4:
                SetTarget1(true);
                SetTarget2(true);
                SetTarget3(true);
                SetTarget4(true);
                SetTarget5(false);
                SetTarget6(false);
                SetTarget7(false);
                SetTarget8(false);
                SetTarget9(false);
                SetTarget10(false);
                break;

            case 5:
                SetTarget1(true);
                SetTarget2(true);
                SetTarget3(true);
                SetTarget4(true);
                SetTarget5(true);
                SetTarget6(false);
                SetTarget7(false);
                SetTarget8(false);
                SetTarget9(false);
                SetTarget10(false);
                break;

            case 6:
                SetTarget1(true);
                SetTarget2(true);
                SetTarget3(true);
                SetTarget4(true);
                SetTarget5(true);
                SetTarget6(true);
                SetTarget7(false);
                SetTarget8(false);
                SetTarget9(false);
                SetTarget10(false);
                break;

            case 7:
                SetTarget1(true);
                SetTarget2(true);
                SetTarget3(true);
                SetTarget4(true);
                SetTarget5(true);
                SetTarget6(true);
                SetTarget7(true);
                SetTarget8(false);
                SetTarget9(false);
                SetTarget10(false);
                break;

            case 8:
                SetTarget1(true);
                SetTarget2(true);
                SetTarget3(true);
                SetTarget4(true);
                SetTarget5(true);
                SetTarget6(true);
                SetTarget7(true);
                SetTarget8(true);
                SetTarget9(false);
                SetTarget10(false);
                break;

            case 9:
                SetTarget1(true);
                SetTarget2(true);
                SetTarget3(true);
                SetTarget4(true);
                SetTarget5(true);
                SetTarget6(true);
                SetTarget7(true);
                SetTarget8(true);
                SetTarget9(true);
                SetTarget10(false);
                break;

            case 10:
                SetTarget1(true);
                SetTarget2(true);
                SetTarget3(true);
                SetTarget4(true);
                SetTarget5(true);
                SetTarget6(true);
                SetTarget7(true);
                SetTarget8(true);
                SetTarget9(true);
                SetTarget10(true);
                break;

            default:
                ActivateRowsTill(0);
                break;
        }
    }
    

}


