using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
public static class Database
{
    public static bool isPressed;
    //0:Keypress Up/Down    1:Keypress Left/Right   2:AirMouse Vertical 3: AirMouse Horizontal  4: LeftClick    5: RightClick
    public static float[] dataBuffer= new float[6];
    public static bool isConnected;
    public static bool isLeftClickEnabled;
    public static bool isYAxisInverted;
    public static bool isAirMouseOn;
    static float[] emptyBuffer = { 0, 0, 0, 0, 0, 0 };
    public static Vector3 Normalised;
    public static TcpClient client;

    // Initializing all values as per initial input
    public static void Inititalize()
    {
        client = new TcpClient();
        dataBuffer[0]= 0f;
        dataBuffer[1]= 0f;
        dataBuffer[2]= 0f;
        dataBuffer[3]= 0f;
        dataBuffer[4]= 0f;
        dataBuffer[5]= 0f;
        isConnected = false;
        isLeftClickEnabled = true;
        isYAxisInverted = false;
        isAirMouseOn = false;
        Normalised = new Vector3(0, 0, -1);
    }
    
    public static float[] GetBuffer()
    {
        return dataBuffer;
    }

   
    public static bool IsValidateIP(string Address)
    {
        //Match pattern for IP address    
        string Pattern = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";
        //Regular Expression object    
        Regex check = new Regex(Pattern);

        //check to make sure an ip address was provided    
        if (string.IsNullOrEmpty(Address))
            //returns false if IP is not provided    
            return false;
        else
            //Matching the pattern    
            return check.IsMatch(Address, 0);
    }
}
