using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    
    public SenderReceiver sender;
    public Thread listener;
    [Header("Registro")]
    public GameObject UIRegistro;
    public InputField CorreoRegistro;
    public InputField UsuarioRegistro;
    public InputField PassRegistro;
    public InputField ConfPassRegistro;
    [Header("Login")]
    public GameObject UILogin;
    public InputField UsuarioLogin;
    public InputField PassLogin;

    TcpClient client;

    void Start()
    {
        client = new TcpClient("81.39.109.215", 13000);
        sender = new SenderReceiver(client);
        listener = new Thread(receive);
        listener.Start();
    }
    #region RECEIVER    
    public void receive()
    {
        string str = "accede";
        while (/*sender.isConnected() && */sender.getIsRunning())
        {
            try
            {
                str = sender.sr.ReadLine();
                Debug.Log(str);
            }
            catch (IOException ex)
            {
                Debug.Log(ex.Message);
                
            }
            finally { }
        }
        //sender.stopConnection();
    }
    #endregion

    

    /*public int RollDice(int atributo,int habilidad)
    {
        int resultado=0;
        resultado = Random.Range(1, 20) + atributo + habilidad;
        return resultado;
    }*/
    public void ButtonToRegistro()
    {
        UIRegistro.SetActive(true);
        UILogin.SetActive(false);
    }

    public void ButtonToLogin()
    {
        UIRegistro.SetActive(false);
        UILogin.SetActive(true);
    }

    public void ButtonConfirmLogin()
    {
        Perfil perfil = new Perfil();
        Carga carga = new Carga();

        perfil.Nombre = UsuarioLogin.text;
        perfil.Contrasena = PassLogin.text;

        carga.peticion = "makeConnection";
        carga.json =@"" + perfil.getAsJSON()+"";
        sender.send(carga.getAsJSON());

    }

    public void ButtonConfirmRegister()
    {
        if (PassRegistro.text != "" && ConfPassRegistro.text !="" && PassRegistro.text == ConfPassRegistro.text)
        {

        }

    }
    
}




