using System;
using System.IO;
using System.IO.Ports;
using System.Collections.Generic;
using UnityEngine;

/**
 * This type is an enumeration of th all different types of sound that might be played
 */
public enum ToneFrequency { SuccessTone, MissedTone, WhiteNoiseStart, WhiteNoiseStop, KHz1_5, KHz3, KHz4_5, KHz7, KHz9_5, None }

/**
 * This class has the responsability to handle all the communication with the external Arduino boards
 */
public class SensorScript : MonoBehaviour
{
    private const float turnSmoothing = 15f;
    public float yawOffset = 0.0f;
    public float pitchOffset = 0.0f;

    /* Board file descriptors */
    private wrmhl movementSensor = new wrmhl(); 
    private SerialPort rewardBoard;
    private SerialPort lickometerOpticalSensor;
    private SerialPort lickometerElectricalSensor;
    private SerialPort externalTTLBoard;

    /* Board COM ports identifiers. Chenge here to adapt to different computers */
    private readonly string movementPortName = @"\\.\COM9";
    private readonly string rewardPortName = @"\\.\COM3";
    private readonly string lickometerOpticalPortName = @"\\.\COM11";
    private readonly string lickometerElectricalPortName = @"\\.\COM6";
    private readonly string externalTTLPortName = @"\\.\COM5";

    /* Board baud rates for serial communication */
    private readonly int movementBaudRate = 57600;
    private readonly int lickometerOpticalBaudRate = 57600;
    private readonly int lickometerElectricalBaudRate = 57600;
    private readonly int rewardBaudRate = 57600;
    private readonly int externalTTLBaudRate = 57600;

    /* Timestamp collection lists */
    public List<DateTime> licksTimeOptical;
    public List<DateTime> licksTimeElectrical;
    public List<DataCollector.Position> headPositions;

    /* Guard variables (board is correctly connected or not) */
    public bool isMovementBoardConnected = false;
    public bool isRewardBoardConnected = false;
    public bool isLickometerOpticalBoardConnected = false;
    public bool isLickometerElectricalBoardConnected = false;
    public bool isExternalTTLBoardConnected = false;

    /* Position variables */
    public float Yaw { get; set; } = 0.0f;
    public float Pitch { get; set; } = 0.0f;
    public float Roll { get; set; } = 0.0f;

    /* Lickometer related variables */
    public int numberOfLicksAfterReward = 0;
    private bool rewardDelivered = false;

    void Start()
    {
        // Try to connect the movement board
        movementSensor.set(movementPortName, movementBaudRate, 40, 1);

        try
        {
            movementSensor.connect();
            isMovementBoardConnected = true;
        } catch (IOException e)
        {
            Debug.LogWarning(e);
        }

        // Try to connect the optical-lickometer board
        lickometerOpticalSensor = new SerialPort(lickometerOpticalPortName, lickometerOpticalBaudRate);
        try
        {
            OpenConnection(lickometerOpticalSensor);
            isLickometerOpticalBoardConnected = true;
        } catch (IOException e) {
            Debug.LogWarning(e);
        }

        // Try to connect the electrical-lickometer board
        lickometerElectricalSensor = new SerialPort(lickometerElectricalPortName, lickometerElectricalBaudRate);
        try
        {
            OpenConnection(lickometerElectricalSensor);
            isLickometerElectricalBoardConnected = true;
        }
        catch (IOException e)
        {
            Debug.LogWarning(e);
        }

        // Try to connect the reward board
        rewardBoard = new SerialPort(rewardPortName, rewardBaudRate);
        try
        {
            OpenConnection(rewardBoard);
            isRewardBoardConnected = true;
        }
        catch (IOException e)
        {
            Debug.LogWarning(e);
        }

        // Try to connect the external TTL board
        externalTTLBoard = new SerialPort(externalTTLPortName, externalTTLBaudRate);
        try
        {
            OpenConnection(externalTTLBoard);
            isExternalTTLBoardConnected = true;
        }
        catch (IOException e)
        {
            Debug.LogWarning(e);
        }

    }

    // Update is called once per frame
    private void Update()
    {
        YawPitchCalibrationRoutine();
        if(!isMovementBoardConnected) MovementKeyboardRoutine();
        LickometerOpticalSensorRoutine();
        LickometerElectricalSensorRoutine();
        /*
         * This code allows to retrieve a YPR coordinates of the sensor by pressing the `q' key 
         
        if (Input.GetKeyUp("q"))
        {
            StreamWriter outputPositions = File.AppendText("SENSOR_POSITIONS.txt");
            outputPositions.WriteLine("Yaw = " + Yaw + "; Pitch = " + Pitch + "; Roll = " + Roll);
            outputPositions.Flush();
            outputPositions.Close();
        }
        */
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovementSensorRoutine();
    }
    

    private void MovementSensorRoutine()
    {
        //Sensor position update

        string movementIncomingData = null;
        try
        {
            movementIncomingData = movementSensor.readQueue();
        } catch (NullReferenceException)
        {
            return;
        }

        if (movementIncomingData != null)
        {
            try
            {
                string[] Data = movementIncomingData.Split('=');
                string t = Data[0].Split('#')[0];
                string[] Angles = Data[1].Split(',');
                Yaw = Convert.ToSingle(Angles[0]);
                Pitch = Convert.ToSingle(Angles[1]);
                Roll = Convert.ToSingle(Angles[2]);

            } catch (IndexOutOfRangeException)
            {
                Debug.Log("Unexpected data format received from the sensor");
                return;
            }


            DataCollector.Position pos = new DataCollector.Position
            {
                Yaw = Yaw,
                Pitch = Pitch,
                Roll = Roll,
                Timestamp = DateTime.Now
            };

            headPositions?.Add(pos);
        }

        Quaternion target = Quaternion.Euler(-(Pitch - pitchOffset), Yaw - yawOffset, -Roll);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * turnSmoothing);

    }

    private void MovementKeyboardRoutine()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Yaw -= 15;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Yaw += 15;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Pitch += 15;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Pitch -= 15;
        }
        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            Roll += 15;
        }
        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            Roll -= 15;
        }

        Quaternion target = Quaternion.Euler(-(Pitch - pitchOffset), Yaw - yawOffset, -Roll);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * turnSmoothing);
    }

    private void LickometerOpticalSensorRoutine()
    {
        if (!lickometerOpticalSensor.IsOpen) return;
        string lickIncomingData = lickometerOpticalSensor.ReadExisting();

        if ((lickIncomingData != null) && (lickIncomingData != ""))
        {
            Debug.Log("lickometer sent: " + lickIncomingData);
            if (lickIncomingData.Contains("l"))
            {
                Debug.Log("lick detected");
                licksTimeOptical?.Add(DateTime.Now);
                if (rewardDelivered) numberOfLicksAfterReward++; //UNCOMMENT THIS LINE TO DRIVE THE CONSUMED REWARD FROM THE OPTICAL
            }
        }

    }

    private void LickometerElectricalSensorRoutine()
    {
        if (!lickometerElectricalSensor.IsOpen) return;
        string lickIncomingData = lickometerElectricalSensor.ReadExisting();

        if ((lickIncomingData != null) && (lickIncomingData != ""))
        {
            Debug.Log("lickometer sent: " + lickIncomingData);
            if (lickIncomingData.Contains("l"))
            {
                Debug.Log("lick detected");
                licksTimeElectrical?.Add(DateTime.Now);
                //if (rewardDelivered) numberOfLicksAfterReward++; //UNCOMMENT THIS LINE TO DRIVE THE CONSUMED REWARD FROM THE ELECTRICAL 
            }
        }

    }

    private void YawPitchCalibrationRoutine()
    {
        if (Input.GetKey("escape"))
            Application.Quit();
        if (Input.GetKey("y"))
            yawOffset = Yaw;
        if (Input.GetKey("p"))
            pitchOffset = Pitch;
    }
    
    void OnApplicationQuit()
    { // close the Thread and Serial Port
        movementSensor.close();
        if (rewardBoard.IsOpen) rewardBoard.Close();
        if (lickometerOpticalSensor.IsOpen) lickometerOpticalSensor.Close();
        if (lickometerElectricalSensor.IsOpen) lickometerElectricalSensor.Close();
        if (externalTTLBoard.IsOpen) externalTTLBoard.Close();
    }
    
    public void Reward()
    {
        if (!rewardBoard.IsOpen) return;
        rewardBoard.Write("a");
        rewardDelivered = true;
    }
    public void Punishment()
    {
        if (!rewardBoard.IsOpen) return;
        rewardBoard.Write("p");
        rewardDelivered = true;
    }
    public void Buzzer()
    {
        if (!rewardBoard.IsOpen) return;
        rewardBoard.Write("b");
    }
    public void TTLStart()
    {
        if (!rewardBoard.IsOpen) return;
        rewardBoard.Write("s");
    }
    public void TTL()
    {
        if (!rewardBoard.IsOpen) return;
        rewardBoard.Write("t");
    }

    public void Consumed()
    {
        if (!rewardBoard.IsOpen) return;
        rewardBoard.Write("c");
    }

    public void Tone(ToneFrequency freq)
    {
        if (!rewardBoard.IsOpen) return;
        switch (freq)
        {
            case ToneFrequency.SuccessTone:
                rewardBoard.Write("S");
                break;
            case ToneFrequency.MissedTone:
                rewardBoard.Write("M");
                break;
            case ToneFrequency.WhiteNoiseStart:
                rewardBoard.Write("w");
                break;
            case ToneFrequency.WhiteNoiseStop:
                rewardBoard.Write("W");
                break;
            case ToneFrequency.KHz1_5:
                rewardBoard.Write("1");
                break;
            case ToneFrequency.KHz3:
                rewardBoard.Write("3");
                break;
            case ToneFrequency.KHz4_5:
                rewardBoard.Write("4");
                break;
            case ToneFrequency.KHz7:
                rewardBoard.Write("7");
                break;
            case ToneFrequency.KHz9_5:
                rewardBoard.Write("9");
                break;
            case ToneFrequency.None:
                break;
        }
    }
    
    public void TriggerLaser()
    {
        if (!externalTTLBoard.IsOpen) return;
        externalTTLBoard.Write("L");
    }

    public void StartRecording()
    {
        if (!externalTTLBoard.IsOpen) return;
        externalTTLBoard.Write("S");
    }

    public void StopRecording()
    {
        if (!externalTTLBoard.IsOpen) return;
        externalTTLBoard.Write("T");
    }

    public void InitNewTrial()
    {
        licksTimeOptical = new List<DateTime>();
        licksTimeElectrical = new List<DateTime>();
        headPositions = new List<DataCollector.Position>();
        rewardDelivered = false;
        numberOfLicksAfterReward = 0;
        if (externalTTLBoard.IsOpen) externalTTLBoard.Write("B");
        
    }

    public void OpenConnection(SerialPort sp)
    {
        
        if (sp != null)
        { //See if there is a serialport connected
            if (sp.IsOpen)
            { //if the port is already open, then close it.(it shouldnt be open)
                sp.Close();

                Debug.Log("Closing port as it was already open");
            }
            else
            {
                sp.Open(); // open the port
                sp.ReadTimeout = 40; //define the timeout
            }
        }
        else
        { //errorcodes
            if (sp.IsOpen)
            {
                Debug.Log("Port is already open");
            }
            else
            {
                Debug.Log("Port == Null");
            }
        }
    
    }
}
