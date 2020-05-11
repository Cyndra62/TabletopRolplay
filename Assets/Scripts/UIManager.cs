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
    public GameObject loadingLogIn;
    [Header("Menu")]
    public GameObject menu;
    public Text userNameMenu;

    TcpClient client;
    string str;
    Perfil perfil;
    Carga carga;
    void Start()
    {
        perfil= new Perfil();
        carga = new Carga();
        client = new TcpClient("81.39.109.215", 13000);
        sender = new SenderReceiver(client);
        listener = new Thread(receive);
        listener.Start();

    }
    #region RECEIVER    
    public void receive()
    {
        str = "accede";
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
        
        StartCoroutine(logIn());

    }

    public void ButtonConfirmRegister()
    {
        if (!string.IsNullOrEmpty(PassRegistro.text) && !string.IsNullOrEmpty(ConfPassRegistro.text) && PassRegistro.text == ConfPassRegistro.text)
        {
            StartCoroutine(Register());
        }

    }

    IEnumerator Register()
    {
        loadingLogIn.SetActive(true);


        perfil.Nombre = UsuarioRegistro.text;
        perfil.Contraseña = PassRegistro.text;
        perfil.Correo = CorreoRegistro.text;

        carga.peticion = "makeRegister";
        carga.json = @"" + perfil.getAsJSON() + "";
        sender.send(carga.getAsJSON());

        yield return new WaitForSeconds(2);
        carga = Carga.getFromJSON(str);
        if (!carga.json.Equals("accepted"))
        {
            loadingLogIn.SetActive(false);
        }
        else
        {
            loadingLogIn.SetActive(false);
            UIRegistro.SetActive(false);
            UILogin.SetActive(true);
            userNameMenu.text = perfil.Nombre;
        }
    }
    IEnumerator logIn()
    {
        loadingLogIn.SetActive(true);
        

        perfil.Nombre = UsuarioLogin.text;
        perfil.Correo = null;
        perfil.Contraseña = PassLogin.text;

        carga.peticion = "makeConnection";
        carga.json = @"" + perfil.getAsJSON() + "";
        sender.send(carga.getAsJSON());

        yield return new WaitForSeconds(2);
        carga = Carga.getFromJSON(str);
        if (string.IsNullOrEmpty(carga.json))
        {
            loadingLogIn.SetActive(false);
        }
        else
        {
            perfil = Perfil.getFromJson(carga.json);
            
            loadingLogIn.SetActive(false);
            UILogin.SetActive(false);
            menu.SetActive(true);
            userNameMenu.text = perfil.Nombre;
        }
        
    }
}




