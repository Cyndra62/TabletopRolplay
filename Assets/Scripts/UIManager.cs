using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using UnityEngine.UI;
using System;
using System.Linq.Expressions;
using System.Security.Cryptography;

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
    public Text botonPersonajeTexto;

    [Header("Fichas")]
    public GameObject crearPersonajes;
    [Header("VistasFichas")]
    public GameObject vistaAtributos;
    public GameObject vistaHabilidades;
    public GameObject vistaProfesiones;
    public GameObject vistaRecursos;
    public GameObject vistaFacultades;
    [Header("Datos Personales fichas")]
    public InputField nombre;
    public Dropdown sexo;
    public Dropdown raza;
    public InputField Edad;
    public InputField Profesion;
    [Header("Atributos fichas")]
    public InputField fuerza;
    public InputField carisma;
    public InputField percepcion;
    public InputField destreza;
    public InputField manipulacion;
    public InputField astucia;
    public InputField resistencia;
    public InputField apariencia;
    public InputField inteligencia;
    [Header("Habilidades fichas")]
    public InputField alerta;
    public InputField apanar;
    public InputField pelea;
    public InputField armasDistancia;
    public InputField montar;
    public InputField atletismo;
    public InputField expresion;
    public InputField armasMelee;
    public InputField empatia;
    public InputField intimidacion;
    public InputField sigilo;
    public InputField delincuencia;
    public InputField interpretacion;
    public InputField subterfugio;
    public InputField instinto;
    public InputField supervivencia;
    [Header("Profesiones fichas")]
    public InputField herreria;
    public InputField caza;
    public InputField sastreria;
    public InputField militar;
    public InputField cocina;
    public InputField medicina;
    public InputField geografia;
    public InputField ciencias;
    [Header("Recursos fichas")]
    public InputField vida;
    public InputField energia;
    public InputField hambre;
    public InputField fama;
    public InputField experiencia;
    public InputField nivel;
    public InputField puntosDeMejora;
    

    [Header("Ajustes")]
    public GameObject ajustes;
    public InputField cambioNombre;
    public GameObject confirmacion;

    [Header("Campaña")]
    public GameObject UICampaña;
    public GameObject PopUpUsuario;
    public InputField invitaUsuario;
    public GameObject chatBoxContent;
    public Text Message;
    public InputField mensajeAEnviar;
    
    [Header("CrearCampaña")]
    public GameObject crearCampaña;
    public InputField nombreCampaña;
    public Dropdown juego;

    TcpClient client;
    string str = "accede";
    Perfil perfil;
    Carga carga;
    PerfilData perfilData;
    Campaña campaña;
    CampañaData campañaData;
    Personaje personaje;
    Mensaje mensaje;
    Jugador jugador;
    void Start()
    {
        try
        {
            perfil = new Perfil();
            carga = new Carga();
            perfilData = new PerfilData();
            campaña = new Campaña();
            campañaData = new CampañaData();
            client = new TcpClient("79.152.249.254", 13000);
            sender = new SenderReceiver(client);
            listener = new Thread(receive);
            jugador = new Jugador();
            mensaje = new Mensaje();
            personaje = new Personaje();
            listener.Start();
        }
        catch(Exception ex) 
        {
            connectionError.SetActive(true);
        }
    }
    public void Update()
    {
        ProcessData();
        
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

    

    public int RollDice(int atributo,int habilidad)
    {
        int resultado=0;
        resultado = UnityEngine.Random.Range(1, 20) + atributo + habilidad;
        return resultado;
    }
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
            if (perfilData.Jugadores.Count == 0)
            {
                botonPersonajeTexto.text = "Vacio";
            }
            else
            {
                botonPersonajeTexto.text = perfilData.Jugadores[0].Nombre;
                personaje = perfilData.Jugadores[0].getPersonaje();
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

        carga.peticion = "chatMessage";
        carga.assigned = "true";
        mensaje.Emisor = perfil.Nombre;
        mensaje.Messaje = mensajeAEnviar.text;
        carga.json = mensaje.getAsJSON();
        Debug.Log(carga.json);
        sender.send(carga.getAsJSON());

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

    public void toCrearPersonaje()
    {
        carga.peticion = "createCharacter";
        nombre.interactable = true;
        crearPersonajes.SetActive(true);
        menu.SetActive(false);
    }
    public void changeAtributos()
    {
        vistaAtributos.SetActive(true);
        vistaHabilidades.SetActive(false);
        vistaProfesiones.SetActive(false);
        vistaRecursos.SetActive(false);
        vistaFacultades.SetActive(false);
    }
    public void changeHabilidades()
    {
        vistaAtributos.SetActive(false);
        vistaHabilidades.SetActive(true);
        vistaProfesiones.SetActive(false);
        vistaRecursos.SetActive(false);
        vistaFacultades.SetActive(false);
    }
    public void changeProfesiones()
    {
        vistaAtributos.SetActive(false);
        vistaHabilidades.SetActive(false);
        vistaProfesiones.SetActive(true);
        vistaRecursos.SetActive(false);
        vistaFacultades.SetActive(false);
    }
    public void changeRecursos()
    {
        vistaAtributos.SetActive(false);
        vistaHabilidades.SetActive(false);
        vistaProfesiones.SetActive(false);
        vistaRecursos.SetActive(true);
        vistaFacultades.SetActive(false);
    }
    public void changeFacultades()
    {
        vistaAtributos.SetActive(false);
        vistaHabilidades.SetActive(false);
        vistaProfesiones.SetActive(false);
        vistaRecursos.SetActive(false);
        vistaFacultades.SetActive(true);
    }

    public void salvarDatosPj()
    {
        personaje.nombre = nombre.text;
        if (sexo.value == 1)
        {
            personaje.sexo = "Masculino";
        }
        else
        {
            personaje.sexo = "Femenino";
        }

        switch (raza.value)
        {
            case 1:
                personaje.raza = "Precursor";
                break;
            case 2:
                personaje.raza = "Leonico";
                break;
            case 3:
                personaje.raza = "Ursido";
                break;
            case 4:
                personaje.raza = "Miatta";
                break;
            case 5:
                personaje.raza = "Konigder";
                break;
            case 6:
                personaje.raza = "Lupxano";
                break;
        }
        int.TryParse(Edad.text, out personaje.edad);
        personaje.profesion = Profesion.text;
        int.TryParse(fuerza.text, out personaje.fuerza);
        int.TryParse(carisma.text, out personaje.carisma);
        int.TryParse(percepcion.text, out personaje.percepcion);
        int.TryParse(destreza.text, out personaje.destreza);
        int.TryParse(manipulacion.text, out personaje.manipulacion);
        int.TryParse(astucia.text, out personaje.astucia);
        int.TryParse(resistencia.text, out personaje.resistencia);
        int.TryParse(apariencia.text, out personaje.apariencia);
        int.TryParse(inteligencia.text, out personaje.inteligencia);

        int.TryParse(alerta.text, out personaje.alerta);
        int.TryParse(apanar.text, out personaje.apañar);
        int.TryParse(pelea.text, out personaje.pelea);
        int.TryParse(armasDistancia.text, out personaje.armasDistancia);
        int.TryParse(montar.text, out personaje.montar);
        int.TryParse(atletismo.text, out personaje.atletismo);
        int.TryParse(expresion.text, out personaje.expresion);
        int.TryParse(armasMelee.text, out personaje.armasMelee);
        int.TryParse(empatia.text, out personaje.empatia);
        int.TryParse(intimidacion.text, out personaje.intimidacion);
        int.TryParse(sigilo.text, out personaje.sigilo);
        int.TryParse(delincuencia.text, out personaje.delincuencia);
        int.TryParse(interpretacion.text, out personaje.interpretacion);
        int.TryParse(subterfugio.text, out personaje.subterfugio);
        int.TryParse(instinto.text, out personaje.instinto);
        int.TryParse(supervivencia.text, out personaje.supervivencia);

        int.TryParse(herreria.text, out personaje.herreria);
        int.TryParse(caza.text, out personaje.caza);
        int.TryParse(sastreria.text, out personaje.sastreria);
        int.TryParse(militar.text, out personaje.militar);
        int.TryParse(cocina.text, out personaje.cocina);
        int.TryParse(medicina.text, out personaje.medicina);
        int.TryParse(geografia.text, out personaje.geografia);
        int.TryParse(ciencias.text, out personaje.ciencias);

        int.TryParse(vida.text, out personaje.vida);
        int.TryParse(energia.text, out personaje.energia);
        int.TryParse(hambre.text, out personaje.hambre);
        int.TryParse(fama.text, out personaje.fama);
        int.TryParse(experiencia.text, out personaje.experiencia);
        int.TryParse(nivel.text, out personaje.nivel);
        int.TryParse(puntosDeMejora.text, out personaje.puntosDeMejora);

        jugador.Nombre = nombre.text;
        jugador.Perfil = perfil.Id;
        jugador.Campaña = perfilData.Campañas[0].Id;
        jugador.JugadorJSON = personaje.getAsJSON();
    }
    public void guardarFicha()
    {
        salvarDatosPj();
        StartCoroutine(enviarJugador());
    }

    IEnumerator enviarJugador()
    {
        loadingLogIn.SetActive(true);

        carga.json = jugador.getAsJSON();
        Debug.Log(carga.json);
        sender.send(carga.getAsJSON());

        yield return new WaitForSeconds(Seconds);
        try
        {
            carga = Carga.getFromJSON(str);
        }
        catch (Exception ex)
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
            crearPersonajes.SetActive(false);

        }
    }

    public void verPersonaje()
    {
        carga.peticion = "updateCharacter";
        nombre.interactable = false;
        nombre.text = personaje.nombre;
        switch (personaje.raza)
        {
            case "Precursor":
                raza.value = 1;
                break;
            case "Leonico":
                raza.value = 2;
                break;
            case "Ursido":
                raza.value = 3;
                break;
            case "Miatta":
                raza.value = 4;
                break;
            case "Konigder":
                raza.value = 5;
                break;
            case "Lupxano":
                raza.value = 6;
                break;
        }
        switch (personaje.sexo)
        {
            case "Masculino":
                sexo.value = 1;
                break;
            case "Femenino":
                sexo.value = 2;
                break;
        }
        Edad.text = personaje.edad.ToString();
        Profesion.text = personaje.profesion;

        fuerza.text = personaje.fuerza.ToString();
        carisma.text = personaje.carisma.ToString();
        percepcion.text = personaje.percepcion.ToString();
        destreza.text = personaje.destreza.ToString();
        manipulacion.text = personaje.manipulacion.ToString();
        astucia.text = personaje.astucia.ToString();
        resistencia.text = personaje.resistencia.ToString();
        apariencia.text = personaje.apariencia.ToString();
        inteligencia.text = personaje.inteligencia.ToString();

        alerta.text = personaje.alerta.ToString();
        apanar.text = personaje.apañar.ToString();
        pelea.text = personaje.pelea.ToString();
        armasDistancia.text = personaje.armasDistancia.ToString();
        atletismo.text = personaje.atletismo.ToString();
        montar.text = personaje.montar.ToString();
        expresion.text = personaje.expresion.ToString();
        armasMelee.text = personaje.armasMelee.ToString();
        empatia.text = personaje.empatia.ToString();
        intimidacion.text = personaje.intimidacion.ToString();
        sigilo.text = personaje.sigilo.ToString();
        delincuencia.text = personaje.delincuencia.ToString();
        interpretacion.text = personaje.interpretacion.ToString();
        subterfugio.text = personaje.subterfugio.ToString();
        instinto.text = personaje.instinto.ToString();
        supervivencia.text = personaje.supervivencia.ToString();

        herreria.text = personaje.herreria.ToString();
        caza.text = personaje.caza.ToString();
        sastreria.text = personaje.sastreria.ToString();
        militar.text = personaje.militar.ToString();
        cocina.text = personaje.cocina.ToString();
        medicina.text = personaje.medicina.ToString();
        geografia.text = personaje.geografia.ToString();
        ciencias.text = personaje.ciencias.ToString();

        vida.text = personaje.vida.ToString();
        energia.text = personaje.energia.ToString();
        hambre.text = personaje.hambre.ToString();
        fama.text = personaje.fama.ToString();
        experiencia.text = personaje.experiencia.ToString();
        nivel.text = personaje.nivel.ToString();
        puntosDeMejora.text = personaje.puntosDeMejora.ToString();

        crearPersonajes.SetActive(true);
        menu.SetActive(false);
    }
    public void volverMenu()
    {
        menu.SetActive(true);
        crearPersonajes.SetActive(false);
    }

    public void borrarPersonaje()
    {
        StartCoroutine(borrarJugador());
    }
    IEnumerator borrarJugador()
    {
        loadingLogIn.SetActive(true);
        carga.peticion = "deleteCharacter";
        carga.json = perfilData.Jugadores[0].getAsJSON();
        Debug.Log(carga.json);
        sender.send(carga.getAsJSON());

        yield return new WaitForSeconds(Seconds);
        try
        {
            carga = Carga.getFromJSON(str);
        }
        catch (Exception ex)
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
            botonPersonajeTexto.text = "Vacio";
            personaje = null;
            personaje = new Personaje();
            crearPersonajes.SetActive(false);

        }
    }

    public void ProcessData()
    {
        if (!String.Equals(str, "accede"))
        {
            Carga Aux = new Carga();
            Aux = Carga.getFromJSON(str);
            if (String.Equals(Aux.peticion, "chatMessage"))
            {
                Text nuevoTexto = GameObject.Instantiate(Message);
                nuevoTexto.transform.parent = chatBoxContent.transform;
                mensaje = Mensaje.getFromJson(Aux.json);
                nuevoTexto.text = mensaje.Emisor+": " + mensaje.Messaje;
                str = "accede";

            }
            if (String.Equals(Aux.peticion, "sendNotificacion"))
            {
                mensaje = Mensaje.getFromJson(Aux.json);
                if (String.Equals(mensaje.Receptor, perfil.Nombre))
                {
                    perfilData.Campañas.Add(Campaña.getFromJson(mensaje.Messaje));
                    botonCampañaTexto.text = perfilData.Campañas[0].Nombre;
                }
                str = "accede";

            }
        }
    }

    public void activarPopupInvitar()
    {
        PopUpUsuario.SetActive(true);
    }

    public void desactivarPopUpInvitar()
    {
        PopUpUsuario.SetActive(false);
    }
    public void InvitarJugador()
    {
        Carga cargaInvitar = new Carga();
        cargaInvitar.peticion = "sendNotificacion";
        mensaje.Emisor = perfil.Nombre;
        mensaje.Receptor = invitaUsuario.text;
        mensaje.Messaje = perfilData.Campañas[0].getAsJSON();
        mensaje.Tipo = "invitacion";
        cargaInvitar.json = mensaje.getAsJSON();

        Debug.Log(carga.json);
        sender.send(cargaInvitar.getAsJSON());
    }

    public void tirarDados()
    {

        carga.peticion = "chatMessage";
        carga.assigned = "true";
        mensaje.Emisor = perfil.Nombre;
        mensaje.Messaje = ""+ RollDice(0,0);
        carga.json = mensaje.getAsJSON();
        Debug.Log(carga.json);
        sender.send(carga.getAsJSON());

    }

}




