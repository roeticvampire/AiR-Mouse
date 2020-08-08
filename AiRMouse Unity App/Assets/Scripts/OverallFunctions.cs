using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class OverallFunctions : MonoBehaviour
{
    public Text statusDisplay;
    public Text bottomLog;
    public InputField ipf;
    private string ipAddress;
    private bool startLooking;
    private int failCount;




    private void Awake()
    { Database.Inititalize();
        
    }

    void Start()
    {
        //client = new TcpClient();
        Input.gyro.enabled = true;

        //   Database.isConnected = true;

    }

    /// <summary>
    /// This function is used to recentre the Vertical vector for our mouse calculations
    /// </summary>
    public void RecentreGyro()
    {
        Database.Normalised = Input.gyro.gravity.normalized;
        Debug.Log("Recentred the Gyro");
    }

    void ConnectTothatFuckingServer()
    {
        try
        {
           // if (!Database.client.Connected)
                //Database.client = new TcpClient();
                //Database.client.Connect(IPAddress.Parse(ipAddress), 41900);

            var result = Database.client.BeginConnect(IPAddress.Parse(ipAddress), 41900, null, null);

            var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1));

            if (!success)
            {
                throw new Exception("Failed to connect.");
            }

            // we have connected
            Database.client.EndConnect(result);








            if (Database.client.Connected) Database.isConnected = true;
            Debug.Log("Connected hai kya ye?: " + Database.client.Connected);
            //Database.client.Close();
            Database.client.Close();
            startLooking = true;
        }
        catch (Exception err)
        {
            Debug.Log(err.ToString());
        }
        // if (!Database.client.Connected) Database.isConnected = false;
    }


    // Update is called once per frame
    void Update()
    {

        if (Database.isConnected)
        {
            statusDisplay.text = "Status: " + (Database.isConnected ? "Connected\n" : "Not Connected\n");
            statusDisplay.text += "Connected to: " + ipAddress + "\nAiR Mouse Mode: " + (Database.isAirMouseOn ? "ON\n" : "OFF\n");
            statusDisplay.text += "Y Axis: " + (Database.isYAxisInverted ? "Inverted" : "Not Inverted");

            if (Database.isAirMouseOn)
                GetAirMouseCoordinates();

            bottomLog.text = "Gyro Vertical: " + Database.dataBuffer[2].ToString();
            bottomLog.text += "\nGyro Horizontal: " + Database.dataBuffer[3].ToString();

            //sendBuffer();
        }
        else //We are not connected, just like right now
        {
            statusDisplay.text = "Arrow Key Score: " + Database.dataBuffer[0].ToString() + " , " + Database.dataBuffer[1].ToString();
            statusDisplay.text += "\nIs Y axis Inverted: " + Database.isYAxisInverted;
            statusDisplay.text += "\nMouse Score: LClick" + Database.dataBuffer[4].ToString();
            statusDisplay.text += "\nMouse Score: RClick" + Database.dataBuffer[5].ToString();
            if (Database.isAirMouseOn)
                GetAirMouseCoordinates();

            bottomLog.text = "Gyro Vertical: " + Database.dataBuffer[2].ToString();
            bottomLog.text += "\nGyro Horizontal: " + Database.dataBuffer[3].ToString();

        }

    }

    /// <summary>
    /// We use FixedUpdate here to ensure that the transmission doesn't rely on the frame rate
    /// </summary>
    void FixedUpdate()
    {
        if (Database.isConnected&&startLooking)
        {
            try
            {
                TcpClient clientu = new TcpClient();
                //clientu.Connect(IPAddress.Parse(ipAddress), 41900);

                
                var result = clientu.BeginConnect(IPAddress.Parse(ipAddress), 41900, null, null);

                var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1));

                if (!success)
                {
                    throw new Exception("Failed to connect.");
                }

                // we have connected
                clientu.EndConnect(result);
                //sendBuffer();
                if (!clientu.Connected) Database.isConnected = false;
                NetworkStream stream = clientu.GetStream();
                byte[] byteArray = new byte[Database.dataBuffer.Length * 4];
                Buffer.BlockCopy(Database.dataBuffer, 0, byteArray, 0, byteArray.Length);
                stream.Write(byteArray, 0, byteArray.Length);
                stream.Close();
                failCount = 0;
                clientu.Close();
            }
            catch
            {
                failCount++;
                if(failCount>5)
               Database.isConnected = false;
            }


            /* Debug.Log("Connected hai kya ye FixedUpdate me?: " + Database.client.Connected);
             NetworkStream stream = Database.client.GetStream();
             byte[] byteArray = new byte[Database.dataBuffer.Length * 4];
             Buffer.BlockCopy(Database.dataBuffer, 0, byteArray, 0, byteArray.Length);
             stream.Write(byteArray, 0, byteArray.Length);
             stream.Close();*/


        }
    }


    /// <summary>
    /// 
    /// </summary>
    private void sendBuffer()
    {
        //byte[] sendingBuffer = new byte[24];
        //sendingBuffer = System.Text.Encoding.UTF8.GetBytes(Database.dataBuffer);
        NetworkStream stream = Database.client.GetStream();
        byte[] byteArray = new byte[Database.dataBuffer.Length * 4];
        Buffer.BlockCopy(Database.dataBuffer, 0, byteArray, 0, byteArray.Length);
        stream.Write(byteArray, 0, byteArray.Length);
        stream.Close();
    }
    /// <summary>
    /// When the Air Mouse Mode is On, this Function keeps storing the gyro calculations in the Database.dataBuffer
    /// </summary>
    /// 



    private void GetAirMouseCoordinates()
    {
        Vector3 grav = Input.gyro.gravity;
        grav.Normalize();
        Database.dataBuffer[3] = grav.x - Database.Normalised.x;
        if (Database.isYAxisInverted)
            Database.dataBuffer[2] = -grav.y + Database.Normalised.y;
        else
            Database.dataBuffer[2] = grav.y - Database.Normalised.y;
    }

    public void InvertYAxis()
    {
        Database.isYAxisInverted = !Database.isYAxisInverted;
        Debug.Log("Is Y Axis Inverted: " + Database.isYAxisInverted);
    }
    public void AlterAirMouse()
    {
        Database.isAirMouseOn = !Database.isAirMouseOn;
        if (!Database.isAirMouseOn)
        {
            Database.dataBuffer[2] = 0f;
            Database.dataBuffer[3] = 0f;
        }
        Debug.Log("Is Air Mouse Enabled: " + Database.isAirMouseOn);
    }

    /// <summary>
    /// Alternates Left Click possibility, on disabling LClick ensures that Lclick is not stuck at 0 in Database.dataBuffer
    /// </summary>
    public void AlterLClick()
    {
        Database.isLeftClickEnabled = !Database.isLeftClickEnabled;
        if (!Database.isLeftClickEnabled)
            Database.dataBuffer[4] = 0;
        Debug.Log("Is Left Click Enabled: " + Database.isLeftClickEnabled);
    }

    /// <summary>
    /// Connect feature, major server laws required here
    /// </summary>
    public void Connect()
    {
        ipAddress = ipf.text.Trim();
        if (Database.IsValidateIP(ipAddress))
        {
            ConnectTothatFuckingServer();
        }



    }
    public void RestartServer()
    {
        Scene scene = SceneManager.GetActiveScene(); UnityEngine.SceneManagement.SceneManager.LoadScene(scene.name);
    }

}
    