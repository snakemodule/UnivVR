using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;



public class UniviewConnection
{
    private static UniviewConnection instance;

    public static UniviewConnection Instance {
        get
        {
            if (instance == null)
            {
                instance = new UniviewConnection();
            }
            return instance;
        }
    }

    private Socket socket;


    private UniviewConnection()
    {
        TcpClient client = new TcpClient();
        socket = client.Client;
        String ip = "127.0.0.1"; //192.168.111.125  127.0.0.1
        System.Net.IPAddress ipAddress = System.Net.IPAddress.Parse(ip);
        System.Net.IPEndPoint remoteEndPoint = new IPEndPoint(ipAddress, 22000);
        socket.Connect(remoteEndPoint);
        byte[] data = new byte[50];
        //socket.Receive(data);
        //String recv = System.Text.Encoding.Default.GetString(data);
        //Debug.Log(recv);
    }

    public void sendCommand(String command)
    {
        try
        {
            byte[] data = System.Text.Encoding.ASCII.GetBytes(command);
            socket.Send(data);
        }
        catch (SocketException se)
        {

        }
    }

}

