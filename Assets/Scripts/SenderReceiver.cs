using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class SenderReceiver
{
    private const int portNum = 13000;
    public TcpClient client;
    public NetworkStream ns;
    public Thread listener;
    public StreamWriter sw;
    public StreamReader sr;
    public volatile bool isRunning;
    //constructor del senderReceiver
    public SenderReceiver(TcpClient client)
    {
        Lanzar(client);
    }

    //metodo que lanza la conexion con con el servidor
    public void Lanzar(TcpClient client)
    {

        ns = client.GetStream();
        sr = new StreamReader(ns, Encoding.UTF8);
        sw = new StreamWriter(ns, Encoding.UTF8);

        setIsRunning(true);
        Debug.Log("INICIADO");
        /*listener = new Thread(receive);
        listener.Start();*/
    }

    //metodo que coloca si esta actibo o no
    public void setIsRunning(bool running)
    {
        lock (this) {
            isRunning = running;
        }
        
    }

    /*public void receive()
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
    }*/

    //metodo que para la conexion
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

    //Metodo que envia un mensaje al servidor
    public void send(string message)
    {
        sw.WriteLine(message);
        sw.Flush();
        Thread.Sleep(20);
    }

    //metodo que obtiene si esta funcionando o no
    public bool getIsRunning()
    {
        lock (this)
        {
            return isRunning;
        }
    }
    
    //metodo que comprueba si el servidor esta conectado o no, si no esta conectado, devuelve un false
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

    //metodo que convierte una imagen en un string
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

    //metodo que convierte un string en una imagen
    public Texture2D stringToIMG(string cadena)
    {
        byte[] bytes = Convert.FromBase64String(cadena);
        Texture2D mytexture = new Texture2D(2, 2);
        mytexture.LoadImage(bytes);
        return mytexture;
    }



}
