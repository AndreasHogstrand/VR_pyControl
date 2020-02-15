using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/**
 * This class handles the behaiour of the main menu, that should be the first scene loaded.
 */
public class MainMenuScript : MonoBehaviour {

    // UI Elements linked to the Unity scene
    public GameObject loadingImage;
    public Dropdown levelDropdown;
    public Button configBtn;

    // This variable will contain the number of the scene to load
    private int level = -1;

    // At startup time, the dropdown must be filled with the list of all different available levels
    void Start()
    {
        addScenes();
    }

    /**
     * This function defines and add the list of available levels
     */
    public void addScenes()
    {
        List<string> names = new List<string>{ "LEVEL", "Habituation for visual guided oriented", "Visual guided oriented task", "Visual guided oriented task one-or-all rewarded", "Visual guided oriented task with fixation", "Head fixed classical conditioning task" };
        levelDropdown.AddOptions(names);
    }

    /**
     * This function is attached to the dropdown and it is called each time the user choose a different level from the GUI
     */
    public void onValueChange(int val)
    {
        level = val-1;
        if (level == -1) configBtn.interactable = false;
        else configBtn.interactable = true;
    }

    /**
     * This function is attached to the `Configure' button and will trigger the loading of the chosen scene.
     * A convention has been decided: the order of the scenes in the building settings should be
     *      0) Main menu
     *      1) stage0_loader
     *      2) stage0
     *      3) stage1_loader
     *      4) stage1
     *      and so on.
     */
    public void onConfigurePress()
    {
        loadingImage.SetActive(true);
        SceneManager.LoadScene(1 + 2 * level);
    }

    /**
     * This function should be called in the `start' function of each stage loader. It will clean every current settings, if any.
     */
    public static void CleanupSettings()
    {
        PlayerPrefs.DeleteKey("isTargetSizeRandom");
        PlayerPrefs.DeleteKey("brightness");
        PlayerPrefs.DeleteKey("mouseSize");
        PlayerPrefs.DeleteKey("numberOfTrials");
        PlayerPrefs.DeleteKey("targetTimeout");
        PlayerPrefs.DeleteKey("collisionTime");
        PlayerPrefs.DeleteKey("isInterTrialTimeMissRandom");
        PlayerPrefs.DeleteKey("interTrialTimeMiss");
        PlayerPrefs.DeleteKey("minInterTrialTimeMiss");
        PlayerPrefs.DeleteKey("maxInterTrialTimeMiss");
        PlayerPrefs.DeleteKey("isInterTrialTimeRewardRandom");
        PlayerPrefs.DeleteKey("interTrialTimeReward");
        PlayerPrefs.DeleteKey("minInterTrialTimeReward");
        PlayerPrefs.DeleteKey("maxInterTrialTimeReward");
        PlayerPrefs.DeleteKey("animalName");
        PlayerPrefs.DeleteKey("licksToReward");
        PlayerPrefs.DeleteKey("targetSize");
        PlayerPrefs.DeleteKey("whiteNoise");
        PlayerPrefs.DeleteKey("isWhiteNoiseRandom");
        PlayerPrefs.DeleteKey("minWhiteNoiseDuration");
        PlayerPrefs.DeleteKey("maxWhiteNoiseDuration");
        PlayerPrefs.DeleteKey("targetRewardDelay");
        PlayerPrefs.DeleteKey("useGrid");
        PlayerPrefs.DeleteKey("isInterTrialTimeRandom");
        PlayerPrefs.DeleteKey("interTrialTime");
        PlayerPrefs.DeleteKey("minInterTrialTime");
        PlayerPrefs.DeleteKey("maxInterTrialTime");
        PlayerPrefs.DeleteKey("cueRewardDelay");
        PlayerPrefs.DeleteKey("wnCueDelay");
        PlayerPrefs.DeleteKey("flat");
        PlayerPrefs.DeleteKey("isWnEnabled");
        PlayerPrefs.DeleteKey("useLaser");
        PlayerPrefs.DeleteKey("laserEvent");
        PlayerPrefs.DeleteKey("fractionOfTrialsStimulated");
        PlayerPrefs.DeleteAll();
    }


}
