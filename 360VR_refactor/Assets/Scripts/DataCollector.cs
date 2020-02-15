using System.IO;
using System.IO.Ports;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

/**
 * This class has the responsability to collect all the information about the experiment
 * and print them into one or more output files
 */
public class DataCollector
{

    /**
     * This class is used to encapsulate the information about the position, containing both YPR and XYZ notation.
     * In some cases, it is useful to track a timestamp, too. That's why a `Timestamp' property is present
     */
    public class Position
    {
        public float Pitch { get; set; }
        public float Yaw { get; set; }
        public float Roll { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public DateTime Timestamp { get; set; }

        public override string ToString()
        {
            return ("Y"+Yaw+"P"+Pitch+"R"+Roll+"X"+X+"Y"+Y+"Z"+Z);
        }
    }

    /**
     * This class is used to store the information about one trial
     */
    public class Element
	{
        // Progressive integer number, indicating the number of the trial
        public int TrialNumber { get; set; } = -1;

        // When the trials starts
        public DateTime TrialStart { get; set; }

        // When the trial ends
        public DateTime TrialEnd { get; set; }

        // When a target appears
        public DateTime VisualTargetOnTime { get; set; }

        // When a target disappears
        public DateTime TargetDisappearingTime { get; set; }
         
        // If a reward has been delivered or not (aka a collision has been triggered)
        public bool IsOutcomeDelivered { get; set; } = false;

        // If a reward has been consumed or not (information from the lickometer)
        public bool IsRewardConsumed { get; set; } = false;

        // Timestamps of the licks (information from the optical lickometer)
        public List<DateTime> LicksTimeOptical { get; set; }

        // Timestamps of the licks (information from the electrical lickometer)
        public List<DateTime> LicksTimeElectrical { get; set; }

        // Position of the target (considering 1 target for 1 trial)
        public Position TargetPos { get; set; }

        // Decimal number indicating the size of the target
        public float TargetSize { get; set; }

        // List of all the positions sampled from the razor sensor
        public List<Position> HeadPositions { get; set; }

        // Time in milliseconds between a tone and a reward (if any)
        public float ToneRewardDelay { get; set; } = 0f;

        // Position of the center of the rewarded Tile (or null if every tile is rewarded)
        public Position RewardedTile { get; set; } = null;

        // Type of texture applied to the target
        public int VisualCueType { get; set; } = -1;

        // If the laser has been activated in the trial
        public bool IsLaserActivated { get; set; } = false;

        public DateTime AuditoryOn { get; set; }

        public DateTime OutcomeTime { get; set; }

        public DateTime LaserStartTime { get; set; }

        public int AuditoryCueType { get; set; } = -1;

        public DateTime WhiteNoiseOn { get; set; }

        public DateTime WhiteNoiseOff { get; set; }

        public DateTime ITIOn { get; set; }

        public DateTime ITIOff { get; set; }

        public float OutcomeProbability { get; set; } = 1f;
        public float CueOutcomeDelay { get; set; } = -1f;

        public float fractionOfStimulatedTrials { get; set; } = -1f;

        public bool OutcomeType { get; set; } = false; // set this variable to False to have "reward" in the report, or True for "punishment"

        //Constructor: init the lists
        public Element() {
            this.LicksTimeOptical = new List<DateTime>();
            this.LicksTimeElectrical = new List<DateTime>();
            this.TargetPos = new Position();
        }

    }

    // Save each trial into a single variable of a list of elements
    private readonly List<Element> list;

    /***** Variables in common among all trials *****/

    // Name of the animal (used for the folder in which to save the output file)
    private string animalName = "UntitledName";
    public string AnimalName
    {
        get { return animalName; }
        set { if (value != "") animalName = value; }
    }
    
    // Name of the task (used for the folder in which to save the output file)
    public string taskName = "UntitledTask";

    /*************/

    // Constructor: init the list of elements
    public DataCollector()
    {
        this.list = new List<Element>();
    }

    // Function to create a new element and set properly `trialNumber'. It returns a pointer to the newly created element
    public Element NewElem()
    {
        this.list.Add(new Element());
        this.list[this.list.Count - 1].TrialNumber = this.list.Count - 1;
        return this.list[this.list.Count - 1];
    }
	
	// Overriding of the function .ToString()
    // The returned string is in CSV-like format
	public override string ToString(){
        string ret = 
            "\"trial number\"," +
            "\"trial start\"," +
            "\"white noise start\"," +
            "\"white noise stop\"," +
            "\"visual target on\"," +
            "\"auditory on\"," +
            "\"visual target off\"," +
            "\"outcome time\"," +
            "\"outcome delivered\"," +
            "\"reward consumed\"," +
            "\"ITI start\"," +
            "\"ITI stop\"," +
            "\"tone-reward delay\"," +
            "\"cue-reward delay\"," +
            "\"licks optical timestamp\"," +
            "\"licks electrical timestamp\"," +
            "\"target position\"," +
            "\"rewarded position\"," +
            "\"auditory cue type\"," +
            "\"visual cue type\"," +
            "\"outcome probability\"," +
            "\"outcome type\"," +
            "\"laser\"," +
            "\"laser start time\"," +
            "\"fraction of stimulated trials\"," +
            "\"target size\"," +
            "\"trial end\"" +
            "\n";

        foreach (Element e in this.list)
        {
            // Calculates all timespans in millis
            double startSpan = (e.TrialStart - list[0].TrialStart).TotalMilliseconds;
            double visualTargetOnSpan = (e.VisualTargetOnTime - list[0].TrialStart).TotalMilliseconds;
            double visualTargetOffSpan = (e.TargetDisappearingTime - list[0].TrialStart).TotalMilliseconds;
            double endSpan = (e.TrialEnd - list[0].TrialStart).TotalMilliseconds;
            double wnStartSpan = (e.WhiteNoiseOn - list[0].TrialStart).TotalMilliseconds;
            double wnStopSpan = (e.WhiteNoiseOff - list[0].TrialStart).TotalMilliseconds;
            double auditoryOnSpan = (e.AuditoryOn - list[0].TrialStart).TotalMilliseconds;
            double outcomeSpan = (e.OutcomeTime - list[0].TrialStart).TotalMilliseconds;
            double ITIOnSpan = (e.ITIOn - list[0].TrialStart).TotalMilliseconds;
            double ITIOffSpan = (e.ITIOff - list[0].TrialStart).TotalMilliseconds;
            double laserSpan = (e.LaserStartTime - list[0].TrialStart).TotalMilliseconds;

            ret += 
                "\"" + e.TrialNumber.ToString() + "\"," +
                "\"" + startSpan.ToString() + "\"," +
                "\"" + wnStartSpan.ToString() + "\"," +
                "\"" + wnStopSpan.ToString() + "\"," +
                "\"" + visualTargetOnSpan.ToString() + "\"," +
                "\"" + auditoryOnSpan.ToString() + "\"," +
                "\"" + visualTargetOffSpan.ToString() + "\"," +
                "\"" + outcomeSpan.ToString() + "\"," +
                "\"" + e.IsOutcomeDelivered.ToString() + "\"," +
                "\"" + e.IsRewardConsumed.ToString() + "\"," +
                "\"" + ITIOnSpan.ToString() + "\"," +
                "\"" + ITIOffSpan.ToString() + "\"," +
                "\"" + e.ToneRewardDelay.ToString() + "\"," +
                "\"" + e.CueOutcomeDelay.ToString() + "\",";

            ret += "\"<";
            foreach (DateTime d in e.LicksTimeOptical)
            {
                double span = (d - list[0].TrialStart).TotalMilliseconds;
                ret += span.ToString() + " ";
            }

            ret.Remove(ret.Length - 1);
            ret += ">\",";

            ret += "\"<";
            foreach (DateTime d in e.LicksTimeElectrical)
            {
                double span = (d - list[0].TrialStart).TotalMilliseconds;
                ret += span.ToString() + " ";
            }
            ret.Remove(ret.Length - 1);
            ret += ">\",";

            ret +=
                "\"" + e.TargetPos.ToString() + "\"," +
                "\"" + (e.RewardedTile==null ? "ALL" : e.RewardedTile.ToString()) + "\"," +
                "\"" + e.AuditoryCueType.ToString() + "\"," +
                "\"" + e.VisualCueType.ToString() + "\"," +
                "\"" + e.OutcomeProbability.ToString() + "\"," +
                "\"" + (e.OutcomeType ? "Punishment" : "Reward") + "\"," +
                "\"" + (e.IsLaserActivated ? "ON" : "OFF") + "\"," +
                "\"" + laserSpan.ToString() + "\"," +
                "\"" + e.fractionOfStimulatedTrials.ToString() + "\"," +
                "\"" + e.TargetSize.ToString() + "\"," +
                "\"" + endSpan.ToString() + "\"" +
                "\n";
        }

		return ret;
    }

    /**
     * Function to save the data of the experiment into two files. The first is a CSV, containing all but the list of headPositions; 
     * while the second is a txt file with all the position sampled by the razor sensor, time-marked.
     * All the files are saved into the directory: `.\Report\<animalName>\<taskName>\' where the `.' directory is the same where the game executable is located.
     * The two files are called by the timestamp at which the experiment starts, in the format yyyyMMdd_HHmm
     */
    public void SaveToFile()
    {
        //Debug.Log("called SaveToFile");
        if(list.Count == 0) { Debug.Log("Error: report list is empty"); return; }
        string filename = list[0].TrialStart.ToString("yyyyMMdd_HHmm");
        Directory.CreateDirectory("Report\\"+AnimalName + "\\" + taskName);

        StreamWriter outputReport = File.CreateText("Report\\" + AnimalName + "\\" + taskName + "\\" + filename + ".csv");
        outputReport.Write(this.ToString());
        outputReport.Flush();

        StreamWriter outputPositions = File.CreateText("Report\\" + AnimalName + "\\" + taskName + "\\" + filename + "_positions.txt");
        foreach (Element e in list)
        {
            if (e.HeadPositions == null) break;
            foreach (Position p in e.HeadPositions)
            {
                double span = (p.Timestamp - list[0].TrialStart).TotalMilliseconds;
                outputPositions.WriteLine(p.ToString() + "@" + span.ToString());
            }
        }

        outputPositions.Flush();
    }
		
}
