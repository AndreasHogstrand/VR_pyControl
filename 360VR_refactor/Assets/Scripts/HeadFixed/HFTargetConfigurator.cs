using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HFTargetConfigurator
{
    public static HFTargetConfigurator shared = new HFTargetConfigurator();
    private HFTargetConfigurator() {}

    // Common properties
    public int NumberOfTargets { get; set; } = -1;

    // Target 1
    public int T1VisualStim { get; set; } = -1;
    public int T1AudioStim { get; set; } = -1;
    public bool T1SoundBefore { get; set; } = false;
    public float T1OutcomeProb { get; set; } = 0.0f;
    public float T1Duration { get; set; } = 0.0f;
    public float T1Delays { get; set; } = 0.0f;
    public int T1PumpType { get; set; } = -1;

    // Target 2
    public int T2VisualStim { get; set; } = -1;
    public int T2AudioStim { get; set; } = -1;
    public bool T2SoundBefore { get; set; } = false;
    public float T2OutcomeProb { get; set; } = 0.0f;
    public float T2Duration { get; set; } = 0.0f;
    public float T2Delays { get; set; } = 0.0f;
    public int T2PumpType { get; set; } = -1;

    // Target 3
    public int T3VisualStim { get; set; } = -1;
    public int T3AudioStim { get; set; } = -1;
    public bool T3SoundBefore { get; set; } = false;
    public float T3OutcomeProb { get; set; } = 0.0f;
    public float T3Duration { get; set; } = 0.0f;
    public float T3Delays { get; set; } = 0.0f;
    public int T3PumpType { get; set; } = -1;

    // Target 4
    public int T4VisualStim { get; set; } = -1;
    public int T4AudioStim { get; set; } = -1;
    public bool T4SoundBefore { get; set; } = false;
    public float T4OutcomeProb { get; set; } = 0.0f;
    public float T4Duration { get; set; } = 0.0f;
    public float T4Delays { get; set; } = 0.0f;
    public int T4PumpType { get; set; } = -1;

    // Target 5
    public int T5VisualStim { get; set; } = -1;
    public int T5AudioStim { get; set; } = -1;
    public bool T5SoundBefore { get; set; } = false;
    public float T5OutcomeProb { get; set; } = 0.0f;
    public float T5Duration { get; set; } = 0.0f;
    public float T5Delays { get; set; } = 0.0f;
    public int T5PumpType { get; set; } = -1;

    // Target 6
    public int T6VisualStim { get; set; } = -1;
    public int T6AudioStim { get; set; } = -1;
    public bool T6SoundBefore { get; set; } = false;
    public float T6OutcomeProb { get; set; } = 0.0f;
    public float T6Duration { get; set; } = 0.0f;
    public float T6Delays { get; set; } = 0.0f;
    public int T6PumpType { get; set; } = -1;

    // Target 7
    public int T7VisualStim { get; set; } = -1;
    public int T7AudioStim { get; set; } = -1;
    public bool T7SoundBefore { get; set; } = false;
    public float T7OutcomeProb { get; set; } = 0.0f;
    public float T7Duration { get; set; } = 0.0f;
    public float T7Delays { get; set; } = 0.0f;
    public int T7PumpType { get; set; } = -1;

    // Target 8
    public int T8VisualStim { get; set; } = -1;
    public int T8AudioStim { get; set; } = -1;
    public bool T8SoundBefore { get; set; } = false;
    public float T8OutcomeProb { get; set; } = 0.0f;
    public float T8Duration { get; set; } = 0.0f;
    public float T8Delays { get; set; } = 0.0f;
    public int T8PumpType { get; set; } = -1;

    // Target 9
    public int T9VisualStim { get; set; } = -1;
    public int T9AudioStim { get; set; } = -1;
    public bool T9SoundBefore { get; set; } = false;
    public float T9OutcomeProb { get; set; } = 0.0f;
    public float T9Duration { get; set; } = 0.0f;
    public float T9Delays { get; set; } = 0.0f;
    public int T9PumpType { get; set; } = -1;

    // Target 10
    public int T10VisualStim { get; set; } = -1;
    public int T10AudioStim { get; set; } = -1;
    public bool T10SoundBefore { get; set; } = false;
    public float T10OutcomeProb { get; set; } = 0.0f;
    public float T10Duration { get; set; } = 0.0f;
    public float T10Delays { get; set; } = 0.0f;
    public int T10PumpType { get; set; } = -1;

    public float GetOutcomeProbability(int targetIndex)
    {
        switch (targetIndex)
        {
            case 0:
                return T1OutcomeProb;
            case 1:
                return T2OutcomeProb;
            case 2:
                return T3OutcomeProb;
            case 3:
                return T4OutcomeProb;
            case 4:
                return T5OutcomeProb;
            case 5:
                return T6OutcomeProb;
            case 6:
                return T7OutcomeProb;
            case 7:
                return T8OutcomeProb;
            case 8:
                return T9OutcomeProb;
            case 9:
                return T10OutcomeProb;
            default:
                throw new Exception();
        }
    }

    public float GetPumpType(int targetIndex)
    {
        switch (targetIndex)
        {
            case 0:
                return T1PumpType;
            case 1:
                return T2PumpType;
            case 2:
                return T3PumpType;
            case 3:
                return T4PumpType;
            case 4:
                return T5PumpType;
            case 5:
                return T6PumpType;
            case 6:
                return T7PumpType;
            case 7:
                return T8PumpType;
            case 8:
                return T9PumpType;
            case 9:
                return T10PumpType;
            default:
                throw new Exception();
        }
    }

    public float GetDelays(int targetIndex)
    {
        switch (targetIndex)
        {
            case 0:
                return T1Delays;
            case 1:
                return T2Delays;
            case 2:
                return T3Delays;
            case 3:
                return T4Delays;
            case 4:
                return T5Delays;
            case 5:
                return T6Delays;
            case 6:
                return T7Delays;
            case 7:
                return T8Delays;
            case 8:
                return T9Delays;
            case 9:
                return T10Delays;
            default:
                throw new Exception();
        }
    }

    public float GetDuration(int targetIndex)
    {
        switch (targetIndex)
        {
            case 0:
                return T1Duration;
            case 1:
                return T2Duration;
            case 2:
                return T3Duration;
            case 3:
                return T4Duration;
            case 4:
                return T5Duration;
            case 5:
                return T6Duration;
            case 6:
                return T7Duration;
            case 7:
                return T8Duration;
            case 8:
                return T9Duration;
            case 9:
                return T10Duration;
            default:
                throw new Exception();
        }
    }

    public bool GetSoundBefore(int targetIndex)
    {
        switch (targetIndex)
        {
            case 0:
                return T1SoundBefore;
            case 1:
                return T2SoundBefore;
            case 2:
                return T3SoundBefore;
            case 3:
                return T4SoundBefore;
            case 4:
                return T5SoundBefore;
            case 5:
                return T6SoundBefore;
            case 6:
                return T7SoundBefore;
            case 7:
                return T8SoundBefore;
            case 8:
                return T9SoundBefore;
            case 9:
                return T10SoundBefore;
            default:
                throw new Exception();
        }
    }

    public int GetVisualStimIndex(int targetIndex)
    {
        switch (targetIndex)
        {
            case 0:
                return T1VisualStim;
            case 1:
                return T2VisualStim;
            case 2:
                return T3VisualStim;
            case 3:
                return T4VisualStim;
            case 4:
                return T5VisualStim;
            case 5:
                return T6VisualStim;
            case 6:
                return T7VisualStim;
            case 7:
                return T8VisualStim;
            case 8:
                return T9VisualStim;
            case 9:
                return T10VisualStim;
            default:
                throw new Exception();
        }
    }

    public int GetAudioStimIndex(int targetIndex)
    {
        switch (targetIndex)
        {
            case 0:
                return T1AudioStim;
            case 1:
                return T2AudioStim;
            case 2:
                return T3AudioStim;
            case 3:
                return T4AudioStim;
            case 4:
                return T5AudioStim;
            case 5:
                return T6AudioStim;
            case 6:
                return T7AudioStim;
            case 7:
                return T8AudioStim;
            case 8:
                return T9AudioStim;
            case 9:
                return T10AudioStim;
            default:
                throw new Exception();
        }
    }
}
