using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class SenderReciever : MonoBehaviour
{
    private const int portNum = 13000;
    public TcpClient client;
    public NetworkStream ns;
    public Thread listener;
    public StreamWriter sw;
    public StreamReader sr;
    public volatile bool isRunning;

    public SenderReciever()
    {
        
    }
    public void Launch()
    {
        client = new TcpClient("81.39.109.215", portNum);
        ns = client.GetStream();
        sr = new StreamReader(ns, Encoding.UTF8);
        sw = new StreamWriter(ns, Encoding.UTF8);

        setIsRunning(true);

        listener = new Thread(receive);
        listener.Start();
    }

    public void setIsRunning(bool running)
    {
        lock (this) {
            isRunning = running;
        }
        
    }

    public void receive()
    {
        string str = "accede";
        while(isConnected() && getIsRunning()){
            try {
                str = sr.ReadLine();
                
            }
            catch(IOException ex){
                Debug.Log(ex.Message);
                stopConnection();
            }
            finally { }
        }
    }
    public void stopConnection()
    {
        lock (this)
        {
            setIsRunning(false);
            try
            {
                ns.Close();
                client.GetStream().Close();
                client.Close();
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
            finally {
                Debug.Log("Aborted Connection");
            }
        }
    }
    public void send(string message)
    {
        sw.WriteLine(message);
        sw.Flush();
        Thread.Sleep(20);
    }
    public bool getIsRunning()
    {
        lock (this)
        {
            return isRunning;
        }
    }
    
    public bool IsConnected {
        get
        {
            try
            {
                if (client != null && client.Client != null && client.Client.Connected)
                {
                    if (client.Client.Poll(0, SelectMode.SelectRead))
                    {
                        byte[] buffer=new byte[1];
                        if (client.Client.Receive(buffer, SocketFlags.Peek) == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    return true;
                }
                else { return false; }
            }
            catch { return false; }
        }
    }
    public bool isConnected()
    {
        lock (this)
        {
            return IsConnected;
        }
    }

    public string imgToString(string ruta)
    {
        Texture2D mytexture = null;
        string cadena;
        byte[] bytes;
        if (File.Exists(ruta))
        {
            bytes = File.ReadAllBytes(ruta);
            mytexture = new Texture2D(2, 2);
            mytexture.LoadImage(bytes);
            bytes = mytexture.EncodeToJPG();
            cadena = Convert.ToBase64String(bytes);
            return cadena;
        }
        else { return null; }
    }

    /*public async Texture2D stringInput()
    {

    }*/



}
