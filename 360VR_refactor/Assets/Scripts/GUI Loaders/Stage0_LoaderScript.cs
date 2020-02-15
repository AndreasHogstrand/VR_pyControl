﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

/**
 * This class handles the GUI for the Habituation task. 
 * It is attached to the Canvas of the scene `Stage0_loader.unity'
 */
public class Stage0_LoaderScript : MonoBehaviour {

    // UI Elements linked to sliders, labels, checkbox, etc. in the Unity scene
	public GameObject loadingImage;
    public Slider brightness;
    public Slider mouseIndicatorSize;
    public Slider numberOfTrials;
    public Slider targetTimeout;
    public Slider collisionTime;
    public Slider interTrialTimeMiss;
    public Slider minInterTrialTimeMiss;
    public Slider maxInterTrialTimeMiss;
    public Slider interTrialTimeReward;
    public Slider minInterTrialTimeReward;
    public Slider maxInterTrialTimeReward;
    public Slider licksToReward;
    public Slider targetSize;
    public Slider whiteNoise;
    public Text mouseIndicatorLbl;
    public Text brightnessLbl;
    public Text numberOfTrialsLbl;
    public Text targetTimeoutLbl;
    public Text collisionTimeLbl;
    public Text interTrialTimeMissLbl;
    public Text minInterTrialTimeMissLbl;
    public Text maxInterTrialTimeMissLbl;
    public Text minInterTrialTimeRewardLbl;
    public Text maxInterTrialTimeRewardLbl;
    public Text interTrialTimeRewardLbl;
    public Text targetSizeLbl;
    public Text whiteNoiseLbl;
    public Text licksToRewardLbl;
    public Toggle isInterTrialTimeMissRandom;
    public Toggle isInterTrialTimeRewardRandom;
    public Toggle isTargetSizeRandom;
    public Toggle isNumTrialsUnlimited;
    public InputField animalName;
    public Dropdown laserEvent;
    public Text fractionOfTrialsStimulatedLbl;
    public Toggle useLaser;
    public Slider fractionOfTrialsStimulated;

    void start(){

        // Delete all the previous settings (if any)
        MainMenuScript.CleanupSettings();
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

        // the size of the green ball
        PlayerPrefs.SetInt("mouseSize", (int)(mouseIndicatorSize.value*100));

        // the maximum number of trials and if they are unlimited
        if (!isNumTrialsUnlimited.isOn) PlayerPrefs.SetInt("numberOfTrials", (int)numberOfTrials.value);
        else PlayerPrefs.SetInt("numberOfTrials", -1);

        // the maximum lifetime of a target
        PlayerPrefs.SetFloat("targetTimeout", targetTimeout.value);

        // the time that triggers a collision
        PlayerPrefs.SetFloat("collisionTime", collisionTime.value);

        // the ITI in case of a miss trial, if it is random and (in this case) its min/max
        if(isInterTrialTimeMissRandom.isOn) PlayerPrefs.SetInt("isInterTrialTimeMissRandom", 1);
        else PlayerPrefs.SetInt("isInterTrialTimeMissRandom", 0);
        PlayerPrefs.SetFloat("interTrialTimeMiss", interTrialTimeMiss.value);
        PlayerPrefs.SetFloat("minInterTrialTimeMiss", minInterTrialTimeMiss.value);
        PlayerPrefs.SetFloat("maxInterTrialTimeMiss", maxInterTrialTimeMiss.value);

        // the ITI in case of a successful trial, if it is random and (in this case) its min/max
        if (isInterTrialTimeRewardRandom.isOn) PlayerPrefs.SetInt("isInterTrialTimeRewardRandom", 1);
        else PlayerPrefs.SetInt("isInterTrialTimeRewardRandom", 0);
        PlayerPrefs.SetFloat("interTrialTimeReward", interTrialTimeReward.value);
        PlayerPrefs.SetFloat("minInterTrialTimeReward", minInterTrialTimeReward.value);
        PlayerPrefs.SetFloat("maxInterTrialTimeReward", maxInterTrialTimeReward.value);

        // the name of the animal (used to create a folder, so *do not* use special characters)
        PlayerPrefs.SetString("animalName", animalName.text.ToLower());

        // the numbers of detected licks to consider a reward as consumed
        PlayerPrefs.SetInt("licksToReward", (int)licksToReward.value);

        // the duration of the white noise
        PlayerPrefs.SetFloat("whiteNoise", whiteNoise.value);

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

        // set visible the loading image
        loadingImage.SetActive(true);

        // load the scene "Stage0"
        SceneManager.LoadScene(level);
    }

    /**
     * Function handler of the brightness slider
     */
    public void onBrightnessChange(float newVal)
    {
        brightnessLbl.text = newVal.ToString();
    }

    /**
     * Function handler of the mouse indicator slider
     */
    public void onMouseIndicatorChange(float newVal)
    {
        mouseIndicatorLbl.text = ( (float)((int)(newVal*100))/100 ).ToString();
    }

    /**
     * Function handler of the number of trials slider
     */
    public void onNumberOfTrialsChange(float newVal)
    {
        numberOfTrialsLbl.text = newVal.ToString();
    }

    /**
     * Function handler of the target timeout slider
     */
    public void onTargetTimeoutChange(float newVal)
    {
        targetTimeoutLbl.text = newVal.ToString();
    }

    /**
     * Function handler of the collision time slider
     */
    public void onCollisionTimeChange(float newVal)
    {
        collisionTimeLbl.text = newVal.ToString();
    }

    /**
     * Function handler of the ITI-miss random checkbox
     */
    public void onIsInterTrialTimeMissRandomChange(bool newVal)
    {
        // if the random checkbox has been set, then the fixed slider should be deactivated and
        // the random sliders should be activated
        if (newVal)
        {
            interTrialTimeMiss.interactable = false;
            minInterTrialTimeMiss.interactable = true;
            maxInterTrialTimeMiss.interactable = true;
            minInterTrialTimeMissLbl.enabled = true;
            maxInterTrialTimeMissLbl.enabled = true;
            interTrialTimeMissLbl.enabled = false;
        }
        // otherwise, the fixed slider should be activated, and the random sliders should be deactivated
        else
        {
            interTrialTimeMiss.interactable = true;
            minInterTrialTimeMiss.interactable = false;
            maxInterTrialTimeMiss.interactable = false;
            minInterTrialTimeMissLbl.enabled = false;
            maxInterTrialTimeMissLbl.enabled = false;
            interTrialTimeMissLbl.enabled = true;
        }
    }

    /**
     * Function handler of the fixed ITI-miss slider
     */
    public void onInterTrialMissChange(float newVal)
    {
        interTrialTimeMissLbl.text = newVal.ToString();
    }

    /**
     * Function handler of the minimum random ITI-miss slider
     */
    public void onMinInterTrialMissChange(float newVal)
    {
        minInterTrialTimeMissLbl.text = newVal.ToString();
        if (newVal > maxInterTrialTimeMiss.value) maxInterTrialTimeMiss.value = newVal;
    }

    /**
     * Function handler of the maximum random ITI-miss slider
     */
    public void onMaxInterTrialMissChange(float newVal)
    {
        maxInterTrialTimeMissLbl.text = newVal.ToString();
        if (newVal < minInterTrialTimeMiss.value) minInterTrialTimeMiss.value = newVal;

    }

    /**
     * Function handler of the ITI-success random checkbox
     */
    public void onIsInterTrialTimeRewardRandomChange(bool newVal)
    {
        // if the random checkbox has been set, then the fixed slider should be deactivated and
        // the random sliders should be activated
        if (newVal)
        {
            interTrialTimeReward.interactable = false;
            minInterTrialTimeReward.interactable = true;
            maxInterTrialTimeReward.interactable = true;
            minInterTrialTimeRewardLbl.enabled = true;
            maxInterTrialTimeRewardLbl.enabled = true;
            interTrialTimeRewardLbl.enabled = false;
        }
        // otherwise, the fixed slider should be activated, and the random sliders should be deactivated
        else
        {
            interTrialTimeReward.interactable = true;
            minInterTrialTimeReward.interactable = false;
            maxInterTrialTimeReward.interactable = false;
            minInterTrialTimeRewardLbl.enabled = false;
            maxInterTrialTimeRewardLbl.enabled = false;
            interTrialTimeRewardLbl.enabled = true;
        }
    }

    /**
     * Function handler of the fixed ITI-success slider
     */
    public void onInterTrialRewardChange(float newVal)
    {
        interTrialTimeRewardLbl.text = newVal.ToString();
    }

    /**
     * Function handler of the minimum random ITI-success slider
     */
    public void onMinInterTrialRewardChange(float newVal)
    {
        minInterTrialTimeRewardLbl.text = newVal.ToString();
        if (newVal > maxInterTrialTimeReward.value) maxInterTrialTimeReward.value = newVal;
    }

    /**
     * Function handler of the maximum random ITI-success slider
     */
    public void onMaxInterTrialRewardChange(float newVal)
    {
        maxInterTrialTimeRewardLbl.text = newVal.ToString();
        if (newVal < minInterTrialTimeReward.value) minInterTrialTimeReward.value = newVal;

    }

    /**
     * Function handler of the `licks to reward` slider
     */
    public void onLicksToRewardChange(float newVal)
    {
        licksToRewardLbl.text = newVal.ToString();
    }

    /**
     * Function handler of the unlimited number of trials checkbox
     */
    public void onUnlimitedChange(bool newVal)
    {
        numberOfTrials.interactable = !newVal;
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
    }

    /**
     * Function handler of the target size slider
     */
    public void onTargetSizeChange(float newVal)
    {
        targetSizeLbl.text = newVal.ToString();
    }

    /**
     * Function handler of the white noise duration slider
     */
    public void onWhiteNoiseChange(float newVal)
    {
        whiteNoiseLbl.text = newVal.ToString();
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

        names.Add(VGOLaserEvent.GetName(typeof(VGOLaserEvent), 0));
        names.Add(VGOLaserEvent.GetName(typeof(VGOLaserEvent), 1));
        names.Add(VGOLaserEvent.GetName(typeof(VGOLaserEvent), 2));
        names.Add(VGOLaserEvent.GetName(typeof(VGOLaserEvent), 3));
        names.Add(VGOLaserEvent.GetName(typeof(VGOLaserEvent), 4));
        names.Add(VGOLaserEvent.GetName(typeof(VGOLaserEvent), 5));
        names.Add(VGOLaserEvent.GetName(typeof(VGOLaserEvent), 6));

        laserEvent.ClearOptions();
        laserEvent.AddOptions(names);
    }

}
