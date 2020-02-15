using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VGOLaserEvent { None, TrialStart, TargetOn, TargetOff, Reward, RewardToneOn, MissToneOn }
public enum HFLaserEvent { None, TrialStart, VisualTargetOn, AuditoryOn, VisualTargetOff, Reward, RewardToneOn, Punishment, PunishmentToneOn, Outcome, OutcomeToneOn  }

public class CommonUtils
{
    public static int frameRate = 600;
}