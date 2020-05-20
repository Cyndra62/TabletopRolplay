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
    private const int Seconds = 8;

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

    [Header("Warnings")]
    public GameObject loadingLogIn;
    public GameObject connectionError;

    [Header("Menu")]
    public GameObject menu;
    public Text userNameMenu;
    public Text botonCampañaTexto;
    public List<GameObject> botonesCampañas;

    [Header("Ajustes")]
    public GameObject ajustes;
    public InputField cambioNombre;
    public GameObject confirmacion;

    [Header("Campaña")]
    public GameObject UICampaña;
    
    [Header("CrearCampaña")]
    public GameObject crearCampaña;
    public InputField nombreCampaña;
    public Dropdown juego;

    TcpClient client;
    string str;
    Perfil perfil;
    Carga carga;
    PerfilData perfilData;
    Campaña campaña;
    CampañaData campañaData;
    void Start()
    {
        try
        {
            perfil = new Perfil();
            carga = new Carga();
            perfilData = new PerfilData();
            campaña = new Campaña();
            campañaData = new CampañaData();
            client = new TcpClient("81.39.109.215", 13000);
            sender = new SenderReceiver(client);
            listener = new Thread(receive);
            listener.Start();
        }
        catch(Exception ex) 
        {
            connectionError.SetActive(true);
        }
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

        yield return new WaitForSeconds(Seconds);
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

        yield return new WaitForSeconds(Seconds);
        carga = Carga.getFromJSON(str);
        if (string.IsNullOrEmpty(carga.json)||carga.json.Equals("denied"))
        {
            loadingLogIn.SetActive(false);
        }
        else
        {
            perfil = Perfil.getFromJson(carga.json);
            perfilData = PerfilData.getFromJson(perfil.PerfilJSON);
            if (perfilData.Campañas.Count == 0)
            {
                botonCampañaTexto.text = "Vacio";
            }
            else
            {
                botonCampañaTexto.text = perfilData.Campañas[0].Nombre;
            }
            loadingLogIn.SetActive(false);
            UILogin.SetActive(false);
            menu.SetActive(true);
            userNameMenu.text = perfil.Nombre;
        }
        
    }

    public void ExitGame()
    {
#if UNITY_EDITOR     
        try
        {
            client.GetStream().Close();
            client.Close();
            UnityEditor.EditorApplication.isPlaying = false;
        }
        catch (Exception ex) { }
#else      
        client.GetStream().Close();
        client.Close();
        Application.Quit();
#endif
    }

    public void toAjustes()
    {
        menu.gameObject.SetActive(false);
        ajustes.gameObject.SetActive(true);

    }

    public void toCrearCampaña()
    {
        menu.gameObject.SetActive(false);
        crearCampaña.gameObject.SetActive(true);
    }
    public void toCampaña()
    {
        menu.gameObject.SetActive(false);
        UICampaña.gameObject.SetActive(true);

    }
    public void backToMenu()
    {
        UICampaña.gameObject.SetActive(false);
        crearCampaña.gameObject.SetActive(false);
        ajustes.gameObject.SetActive(false);
        menu.gameObject.SetActive(true);
    }

    public void ajustesActivarPopUp()
    {
        confirmacion.SetActive(true);
    }
    public void ajustesDesactivarPoPUp()
    {
        confirmacion.SetActive(false);
    }

    public void ajustesCambiarNombre()
    {
        if (!String.IsNullOrEmpty(cambioNombre.text))
        {
            StartCoroutine(ModificarNombre());
        }
    }

    IEnumerator ModificarNombre()
    {
        loadingLogIn.SetActive(true);


        perfil.Nombre = cambioNombre.text;


        carga.peticion = "updateProfile";
        carga.json = @"" + perfil.getAsJSON() + "";
        sender.send(carga.getAsJSON());

        yield return new WaitForSeconds(Seconds);
        carga = Carga.getFromJSON(str);
        if (!carga.json.Equals("accepted"))
        {
            loadingLogIn.SetActive(false);
        }
        else
        {
            loadingLogIn.SetActive(false);
            ajustes.SetActive(false);
            menu.SetActive(true);
            userNameMenu.text = perfil.Nombre;
        }
    }


    public void eliminarPerfil()
    {
        StartCoroutine(BorrarPerfil());
    }
    IEnumerator BorrarPerfil()
    {
        loadingLogIn.SetActive(true);

        carga.peticion = "deleteProfile";
        carga.json = @"" + perfil.getAsJSON() + "";
        sender.send(carga.getAsJSON());

        yield return new WaitForSeconds(Seconds);
        carga = Carga.getFromJSON(str);
        if (!carga.json.Equals("accepted"))
        {
            loadingLogIn.SetActive(false);
        }
        else
        {
            loadingLogIn.SetActive(false);
            ajustes.SetActive(false);
            UILogin.SetActive(true);
            
        }
    }

    public void guardarCampaña()
    {
        Debug.Log("Entra");
        campaña.Nombre = nombreCampaña.text;
        if(juego.value == 1)
        {
            campaña.Juego = "The Awakens";
            Debug.Log("Pone The Awakens");
        }
        else if(juego.value == 2)
        {
            campaña.Juego = "Dungeons & Dragons";
            Debug.Log("Pone D&D");
        }
        campaña.DM = perfil.Id;
        Debug.Log(campaña.DM);
        StartCoroutine(enviarCampaña());

    }

    IEnumerator enviarCampaña()
    {
        loadingLogIn.SetActive(true);

        carga.peticion = "createCampaña";
        carga.json = @"" + campaña.getAsJSON() + "";
        Debug.Log(carga.json);
        sender.send(carga.getAsJSON());
        
        yield return new WaitForSeconds(Seconds);
        carga = Carga.getFromJSON(str);
        if (!carga.json.Equals("accepted"))
        {
            loadingLogIn.SetActive(false);
        }
        else
        {
            loadingLogIn.SetActive(false);
            crearCampaña.SetActive(false);
            botonCampañaTexto.text = campaña.Nombre;
            menu.SetActive(true);

        }
    }

    public void DeleteCampaña()
    {
        if(perfil.Id == perfilData.Campañas[0].DM)
        {
            StartCoroutine(borrarCampaña());
        }
    }

    IEnumerator borrarCampaña()
    {
        loadingLogIn.SetActive(true);

        carga.peticion = "deleteCampaña";
        carga.json = @"" + perfilData.Campañas[0].getAsJSON() + "";
        Debug.Log(carga.json);
        sender.send(carga.getAsJSON());

        yield return new WaitForSeconds(Seconds);
        carga = Carga.getFromJSON(str);
        if (!carga.json.Equals("accepted"))
        {
            loadingLogIn.SetActive(false);
        }
        else
        {
            loadingLogIn.SetActive(false);
            perfilData.Campañas.Remove(perfilData.Campañas[0]);
            botonCampañaTexto.text = "vacio";

        }
    }

    public void enviarMensaje()
    {
        StartCoroutine(sendMessage());
    }

    IEnumerator sendMessage()
    {
        loadingLogIn.SetActive(true);

        carga.peticion = "sendNotificacion";
        carga.json ="Patata Dios";
        Debug.Log(carga.json);
        sender.send(carga.getAsJSON());

        yield return new WaitForSeconds(Seconds);
        carga = Carga.getFromJSON(str);
        if (!carga.json.Equals("accepted"))
        {
            loadingLogIn.SetActive(false);
        }
        else
        {
            loadingLogIn.SetActive(false);
        }
    }

    public void unirseCampaña()
    {
        StartCoroutine(joinCampaña());
    }

    IEnumerator joinCampaña()
    {
        loadingLogIn.SetActive(true);

        carga.peticion = "joinCampaña";
        carga.json = perfilData.Campañas[0].getAsJSON();
        Debug.Log(carga.json);
        sender.send(carga.getAsJSON());

        yield return new WaitForSeconds(Seconds);
        carga = Carga.getFromJSON(str);
        if (carga.assigned.Equals("false")||carga.json.Equals("denied"))
        {
            loadingLogIn.SetActive(false);
        }
        else
        {
            campaña = Campaña.getFromJson(carga.json);
            campañaData = CampañaData.getFromJson(campaña.CampañaJSON);
            loadingLogIn.SetActive(false);
            menu.SetActive(false);
            UICampaña.SetActive(true);
            
        }
    }

    public void salirCampaña()
    {
        StartCoroutine(leaveCampaña());
    }

    IEnumerator leaveCampaña()
    {
        loadingLogIn.SetActive(true);

        carga.peticion = "leaveCampaña";
        carga.json = perfil.Id.ToString();
        Debug.Log(carga.json);
        sender.send(carga.getAsJSON());

        yield return new WaitForSeconds(Seconds);
        try
        {
            carga = Carga.getFromJSON(str);
        }
        catch(Exception ex)
        {
            Debug.Log(ex);
        }
        if (carga.json.Equals("denied"))
        {
            loadingLogIn.SetActive(false);
        }
        else
        {
            loadingLogIn.SetActive(false);
            menu.SetActive(true);
            UICampaña.SetActive(false);

        }
    }
}




