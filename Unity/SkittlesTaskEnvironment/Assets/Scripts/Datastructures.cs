using UnityEngine;
using System;


#region DataStructures
public enum ParticipantDataHeader
{
    MousePos_X,
    MousePos_Y,
    PivotAngle,
    PivotAngVel,
    BallPos_X,
    BallPos_Z,
    BallVel_X,
    BallVel_Z,
    Detached,
    DistToTarget,
    UnityTime
}
public struct ParticipantData
{
    public Vector2 MousePos;
    public float PivotAngle;
    public float PivotAngVel;
    public Vector3 BallPos;
    public Vector3 BallVel;
    public bool Detached;
    public float DistToTarget;
    public float UnityTime;
    public string[] DataString;

    public ParticipantData(string[] data)
    {
        MousePos = new Vector2(float.Parse(data[(int)ParticipantDataHeader.MousePos_X]), float.Parse(data[(int)ParticipantDataHeader.MousePos_Y]));
        PivotAngle = float.Parse(data[(int)ParticipantDataHeader.PivotAngle]);
        PivotAngVel = float.Parse(data[(int)ParticipantDataHeader.PivotAngVel]);
        BallPos = new Vector3(float.Parse(data[(int)ParticipantDataHeader.BallPos_X]), 0, float.Parse(data[(int)ParticipantDataHeader.BallPos_X]));
        BallVel = new Vector3(float.Parse(data[(int)ParticipantDataHeader.BallVel_X]), 0, float.Parse(data[(int)ParticipantDataHeader.BallVel_Z]));
        Detached = data[(int)ParticipantDataHeader.Detached] == "True" ? true : false;
        DistToTarget = float.Parse(data[(int)ParticipantDataHeader.DistToTarget]);
        UnityTime = float.Parse(data[(int)ParticipantDataHeader.UnityTime]);
        DataString = data;
    }
}
#endregion
#region Classes
public static class ParticipantInfo
{
    public static int Number;
    public static int Session;
    public static int Block;
    public static int TrialNum;
    public static string Result;

    public static string ToFileName()
    {

        DateTime time = DateTime.Now;
        string date = time.ToString("ddMM'-'HHmmss");

        string output = "";

        output += "Skittles_PartID_" + Number.ToString() + "_";
        output += "Session_" + Session.ToString() + "_";
        output += "Block_" + Block.ToString() + "_";
        output += "TrialNum_" + TrialNum.ToString() + "_";
        output += "Result_" + Result + "_";
        output += date;
        output += ".csv";

        return output;
    }
}

public static class GameStates
{
    public static Vector2 MousePos;
    public static bool MouseDown;

    // Note that Angle/Ball Change gets updated 1 frame after MousePosition.
    public static float PivotAngle;
    public static float PivotAngVel;
    public static Vector3 BallPos;
    public static Vector3 BallVel;

    public static bool Detached;
    public static float DistToTarget;
    public static float UnityTime;


    public static string GetHeader()
    {
        string output = "MousePos_X,MousePos_Z,MouseDown,PivotAngle,PivotAngleVel,BallPos_X,BallPos_Z,BallVel_X,BallVel_Z,Detached,DistToTarget,UnityTime";
        return output;
    }
    public static string GetString()
    {
        string output = "";

        output += MousePos.x.ToString("F4")+",";
        output += MousePos.y.ToString("F4") + ",";
        output += (MouseDown ? "True" : "False") + ",";
        output += PivotAngle + ",";
        output += PivotAngVel + ",";
        output += BallPos.x.ToString("F4") + ",";
        output += BallPos.z.ToString("F4") + ",";
        output += BallVel.x.ToString("F4") + ",";
        output += BallVel.z.ToString("F4")+",";
        output += (Detached ? "True" : "False") + ",";
        output += DistToTarget.ToString("F4") + ",";
        output += Time.timeSinceLevelLoad.ToString("F4");

        return output;
    }
    #endregion
}
