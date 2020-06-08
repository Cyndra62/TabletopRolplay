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
    private const int Seconds = 4; //Tiempo de espera para recivir informacion del servidor


    public SenderReceiver sender;
    public Thread listener;
    //Todos los objetos de la interfaz a continuacion
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
    public RawImage image;

    [Header("Campaña")]
    public GameObject UICampaña;
    public GameObject PopUpUsuario;
    public InputField invitaUsuario;
    public GameObject chatBoxContent;
    public Text Message;
    public InputField mensajeAEnviar;
    public InputField numDados;
    public InputField tipoDado;
    public InputField modificador;
    public GameObject PopupDados;
    public GameObject ficha;
    public GameObject botonInvitarUsuario;
    
    [Header("CrearCampaña")]
    public GameObject crearCampaña;
    public InputField nombreCampaña;
    public Dropdown juego;
    //Variables necesarias para el funcionamiento de las funciones
    TcpClient client;
    string str = "accede";
    public Perfil perfil;
    public Carga carga;
    public PerfilData perfilData;
    public Campaña campaña;
    public CampañaData campañaData;
    public Personaje personaje;
    public Mensaje mensaje;
    public Jugador jugador;


    /*
     * El metodo Start se ejecuta al iniciar el programa. En este caso inicializa todos los objetos (Perfil, Carga, etc...) y
     * tambien inicializa la escucha del servidor con la ip y en el puerto indicado.
    */
    void Start()
    {
        try
        {
            perfil = new Perfil();
            carga = new Carga();
            perfilData = new PerfilData();
            campaña = new Campaña();
            campañaData = new CampañaData();
            client = new TcpClient("81.39.98.108", 13000);
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

    /*
     * El metodo update se ejecuta a cada frame. En este caso lo utilizamos para que el metodo ProcessData se ejecute continuamente
     */
    public void Update()
    {
        ProcessData();
        
    }

    /*
     * En esta region esta el metodo recieve, el cual se encarga de leer la informacion que llega del servidor, colocandola en la variable str para poder utilizar los datos
     * en el resto de metodos.
     */

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

    
    // En este metodo permitimos tirar un numero de dados segun los que quieras tirar y el tipo de dado que quieras tirar, al cual sumamos el modificador que hayas utilizado.
    public string RollDice(int numDados,int tipo,int modificador)
    {
        string resultado = "";
        string tiradas = "";
        int total = 0;
        resultado = "Numero de dados = " + numDados + "d" + tipo + " + " + " : ";
        for (int i = 0; i< numDados; i++)
        {
            int valor = UnityEngine.Random.Range(1, 20);
            tiradas = tiradas +" + "+ valor;
            total = total + valor;
        }
        total = total + modificador;
        resultado += tiradas +" + "+modificador+ " = " + total ;
        
        return resultado;
    }

    //metodo para activar la pantalla de registro
    public void ButtonToRegistro()
    {
        UIRegistro.SetActive(true);
        UILogin.SetActive(false);
    }

    //metodo para activar la pantalla de login
    public void ButtonToLogin()
    {
        UIRegistro.SetActive(false);
        UILogin.SetActive(true);
    }

    //metodo del boton que confirma el login. Inicia la Courutine logIn()
    public void ButtonConfirmLogin()
    {
        
        StartCoroutine(logIn());

    }

    //metodo del boton que confirma el registro. Comprueba si todos los datos estan rellenos y si lo estan comienza la Couritine Register()
    public void ButtonConfirmRegister()
    {
        if (!string.IsNullOrEmpty(PassRegistro.text) && !string.IsNullOrEmpty(ConfPassRegistro.text) && PassRegistro.text == ConfPassRegistro.text)
        {
            StartCoroutine(Register());  
        }

    }

    // metodo IEnumerator, obtiene los datos del registro de la interfaz y los monta dentro del objeto perfil. Despues los envia al servidor el cual responde si se ha aceptado el registro o no.
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

    /*
     * metodo IEnumerator, obtiene los datos del login de la interfaz y los monta dentro del objeto perfil. Despues los envia al servidor y espera a la respuesta de si le permite logearse o no. Si le deja
     * el metodo tambien administra toda la informacion recibida insertandola en los objetos correspondientes
     */
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
            if (!String.IsNullOrEmpty(perfilData.Avatar)||!String.Equals(perfilData.Avatar,""))
            {
                image.texture = stringToIMG(perfilData.Avatar);
            }
            loadingLogIn.SetActive(false);
            cambioNombre.text = perfil.Nombre;
            UILogin.SetActive(false);
            menu.SetActive(true);
            userNameMenu.text = perfil.Nombre;
        }
        
    }

    /*
     * Este metodo convierte un string de una imagen en una cadena.
     */
    public Texture2D stringToIMG(string cadena)
    {
        byte[] bytes = Convert.FromBase64String(cadena);
        Texture2D mytexture = new Texture2D(2, 2);
        mytexture.LoadImage(bytes);
        return mytexture;
    }

    // Este metodo permite salir de la aplicacion, si nos encontramos en el editor de unity quita el play, y si estamos en build, cierra el programa.
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

    //Este metodo activa la pantalla de ajustes.
    public void toAjustes()
    {
        menu.gameObject.SetActive(false);
        ajustes.gameObject.SetActive(true);

    }

    //Este metodo activa la pantalla de crear campaña
    public void toCrearCampaña()
    {
        menu.gameObject.SetActive(false);
        crearCampaña.gameObject.SetActive(true);
    }

    //Este metodo activa la UI de la campaña.
    public void toCampaña()
    {
        menu.gameObject.SetActive(false);
        UICampaña.gameObject.SetActive(true);

    }

    //Este metodo permite volver al menu
    public void backToMenu()
    {
        UICampaña.gameObject.SetActive(false);
        crearCampaña.gameObject.SetActive(false);
        ajustes.gameObject.SetActive(false);
        crearCampaña.gameObject.SetActive(false);
        menu.gameObject.SetActive(true);
    }

    //Este metodo activa el popup de confirmacion.
    public void ajustesActivarPopUp()
    {
        confirmacion.SetActive(true);
    }

    //Este metodo desactiva el popup de confirmacion
    public void ajustesDesactivarPoPUp()
    {
        confirmacion.SetActive(false);
    }

    //Este metodo comprueba si el cambio de nombre del perfil esta vacio o no. Si no esta vacio (Puede ser el mismo), inicia la courutine Modificar Perfil.
    public void ajustesCambiarPerfil()
    {
        if (!String.IsNullOrEmpty(cambioNombre.text))
        {
            StartCoroutine(ModificarPerfil());
        }
    }
    // Metodo Courutine, este metodo modifica el objeto perfil y mediante el objeto carga lo envia al servidor. Espera a que el servidor le responda si se han producido los cambios o no.
    IEnumerator ModificarPerfil()
    {
        loadingLogIn.SetActive(true);


        perfil.Nombre = cambioNombre.text;
        /*Texture2D texture = (Texture2D)image.texture;
        byte[] bytes;
        bytes = texture.EncodeToPNG();
        perfilData.Avatar = Convert.ToBase64String(bytes);    (Esto esta comentado debido a que no nos ha dado tiempo a terminar de implementarlo y da errores, vasicamente, convierte una imagen en un string.)
        */
        perfil.PerfilJSON = perfilData.getAsJSON();

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

    //Este metodo inicia la courutine BorrarPerfil()
    public void eliminarPerfil()
    {
        StartCoroutine(BorrarPerfil());
    }

    //Metodo Courutine, envia al servidor la peticion de borrar un perfil y el perfil en cuestion. Despues te develve a la pantalla de login.
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

    //Este metodo coje los datos de la interfaz y monta un objeto campaña. Despues inicia la Courutine EnviarCampaña.
    public void guardarCampaña()
    {
        Debug.Log("Entra");
        if (!String.IsNullOrEmpty(campaña.Nombre))
        {
            campaña.Nombre = nombreCampaña.text;
            if (juego.value == 1)
            {
                campaña.Juego = "The Awakens";
                Debug.Log("Pone The Awakens");
            }
            else if (juego.value == 2)
            {
                campaña.Juego = "Dungeons & Dragons";
                Debug.Log("Pone D&D");
            }
            campaña.DM = perfil.Id;
            Debug.Log(campaña.DM);
            StartCoroutine(enviarCampaña());
        }

    }

    //Manda al servidor un objeto carga con la informacion de la campaña. Espera a la respuesta del servidor y si es satisfactoria te devuelve al menu. Si no es satisfactoria desaparece la pantalla de carga.
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
            perfilData.Campañas.Add(campaña);
        }
    }

    //Este metodo inicia la Courutine BorrarCampaña
    public void DeleteCampaña()
    {
            StartCoroutine(borrarCampaña());
    }

    //Metodo Courutine, este metodo manda al servidor un objeto carga con la peticion de borrar y los datos de la campaña. Si es satisfactorio borra la campaña. Si no, no lo hace.
    IEnumerator borrarCampaña()
    {
        loadingLogIn.SetActive(true);

        carga.peticion = "deleteCampaña";
        carga.json = "{\"Perfil\": "+perfil.Id+", \"Campaña\": "+perfilData.Campañas[0].Id+"}";
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

    //Este metodo envia un mensaje del chat al servidor. Este luego se encarga de repartirlo al resto de usuarios.
    public void enviarMensaje()
    {

        carga.peticion = "chatMessage";
        carga.assigned = "true";
        mensaje.Emisor = perfil.Nombre;
        mensaje.Messaje = mensajeAEnviar.text;
        mensajeAEnviar.text = "";
        carga.json = mensaje.getAsJSON();
        Debug.Log(carga.json);
        sender.send(carga.getAsJSON());

    }


    //Este metodo inicia la courutina joinCampaña, ademas si el usuario es el DM de la campaña (el creador), le permite invitar a otros usuarios a esta.
    public void unirseCampaña()
    {
        StartCoroutine(joinCampaña());
        if(perfilData.Campañas[0].DM == perfil.Id)
        {
            botonInvitarUsuario.SetActive(true);
        }
    }

    //Metodo Courutine, Este metodo inicia la conexion con la campaña. Si esta todo correcto activa la UI de la campaña.
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

    //Este metodo sirve para salir de la campaña. No importa lo que devuelva el servidor ya que siempre se puede salir. Tambien borra todos los GameObject Token, de la escena, para evitar problemas.
    public void salirCampaña()
    {

        carga.peticion = "leaveCampaña";
        carga.json = perfil.getAsJSON();
        Debug.Log(carga.json);
        sender.send(carga.getAsJSON());
        carga.assigned = "false";
        GameObject[] fichas = GameObject.FindGameObjectsWithTag("Token");
        foreach (GameObject ficha in fichas)
        {
            Destroy(ficha);
        }
        menu.SetActive(true);
        UICampaña.SetActive(false);
    }

    //Este metodo activa la pantalla de creacion de jugadores/personajes
    public void toCrearPersonaje()
    {
        carga.peticion = "createCharacter";
        nombre.interactable = true;
        crearPersonajes.SetActive(true);
        menu.SetActive(false);
    }

    //Este metodo activa la pantalla de los atributos.
    public void changeAtributos()
    {
        vistaAtributos.SetActive(true);
        vistaHabilidades.SetActive(false);
        vistaProfesiones.SetActive(false);
        vistaRecursos.SetActive(false);
        vistaFacultades.SetActive(false);
    }

    //Este metodo activa la pantalla de las habilidades
    public void changeHabilidades()
    {
        vistaAtributos.SetActive(false);
        vistaHabilidades.SetActive(true);
        vistaProfesiones.SetActive(false);
        vistaRecursos.SetActive(false);
        vistaFacultades.SetActive(false);
    }

    //Este metodo activa la pantalla de las profesiones
    public void changeProfesiones()
    {
        vistaAtributos.SetActive(false);
        vistaHabilidades.SetActive(false);
        vistaProfesiones.SetActive(true);
        vistaRecursos.SetActive(false);
        vistaFacultades.SetActive(false);
    }

    //Este metodo activa la pantalla de los recursos
    public void changeRecursos()
    {
        vistaAtributos.SetActive(false);
        vistaHabilidades.SetActive(false);
        vistaProfesiones.SetActive(false);
        vistaRecursos.SetActive(true);
        vistaFacultades.SetActive(false);
    }

    //Este metodo activa la pantalla de las facultades (No implementadas debido a su extension y a que no existen todas aun en el juego de rol de mesa The Awakens)
    public void changeFacultades()
    {
        vistaAtributos.SetActive(false);
        vistaHabilidades.SetActive(false);
        vistaProfesiones.SetActive(false);
        vistaRecursos.SetActive(false);
        vistaFacultades.SetActive(true);
    }

    //Este metodo guarda todos los datos del jugador/personaje dentro del objeto personaje.
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

    //Este metodo activa el metodo de salvarDatosPj y la Courutine de enviarJugador
    public void guardarFicha()
    {
        salvarDatosPj();
        StartCoroutine(enviarJugador());
    }

    //Este metodo envia los datos del jugador/personaje al servidor para ser almacenados. Si esta todo correcto, te devuelve al menu.
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
            botonPersonajeTexto.text = jugador.Nombre;
            menu.SetActive(true);
            crearPersonajes.SetActive(false);

        }
    }

    //Este metodo te permite ver uno de tus personajes extrayendo la informacion de el del servidor. Activando la pantalla de creacion de personajes por si necesitas modificar algun dato.
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
        UICampaña.SetActive(false);
    }

    //Este metodo permite volver al menu o a la UI de la campaña desde la creacion de personajes. Segun si entraste desde la campaña o desde el menu principal
    public void volverMenu()
    {
        if(string.Equals(carga.assigned,"true"))
        {
            UICampaña.SetActive(true);
            crearPersonajes.SetActive(false);
        }
        else
        {
            menu.SetActive(true);
            crearPersonajes.SetActive(false);
        }
        
    }

    //Este metodo inicia la Courutine de borrarJugador
    public void borrarPersonaje()
    {
        StartCoroutine(borrarJugador());
    }

    //Metodo Courutine el cual manda al servidor una peticion de borrar un personaje y la informacion de este, si todo va bien el personaje se elimina tanto del servidor como del local.
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

    //Metodo para procesar la informacion que se recive del servidor conrinuamente, como las notificaciones, el chat, o el movimiento de los token segun la peticion recivida del servidor
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
            if (String.Equals(Aux.peticion, "tokenMove"))
            {
                Token fichaAux = Token.getFromJson(Aux.json);
                invocarToken(fichaAux);
                str = "accede";
            }
            if (String.Equals(Aux.peticion, "leaveCampaña"))
            {
                if (String.Equals(Aux.json, "kick"))
                {
                    carga.assigned = "false";
                    GameObject[] fichas = GameObject.FindGameObjectsWithTag("Token");
                    foreach (GameObject ficha in fichas)
                    {
                        Destroy(ficha);
                    }
                    menu.SetActive(true);
                    UICampaña.SetActive(false);
                    str = "accede";
                }
                else
                {
                    Perfil aux = Perfil.getFromJson(Aux.json);
                    Destroy(GameObject.Find(aux.Nombre));
                    str = "accede";
                }
                
            }
            

        }
    }

    //metodo para activar el popup de la invitacion
    public void activarPopupInvitar()
    {
        PopUpUsuario.SetActive(true);
    }

    //metodo para desactivar el popup de la invitacion
    public void desactivarPopUpInvitar()
    {
        PopUpUsuario.SetActive(false);
    }

    //metodo para invitar a un jugador a la campaña. Manda la informacion en una carga al servidor con los datos del emisor, receptor y el mensaje (En este caso el mensaje es la informacion de la campaña)
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

    //Este metodo activa el metodo de RollDice y manda el resultado mediante un mensaje de chat.
    public void tirarDados()
    {

        carga.peticion = "chatMessage";
        carga.assigned = "true";
        mensaje.Emisor = perfil.Nombre;
        mensaje.Messaje = ""+ RollDice(int.Parse(numDados.text),int.Parse(tipoDado.text),int.Parse(modificador.text));
        carga.json = mensaje.getAsJSON();
        Debug.Log(carga.json);
        sender.send(carga.getAsJSON());

    }

    //metodo que activa el popup de los dados
    public void activarPoPUpDados()
    {
        PopupDados.SetActive(true);
    }

    //metodo que desactiva el popup de los dados
    public void desactivarPoPUpDados()
    {
        PopupDados.SetActive(false);
    }

    //metodo que invoca el token del jugador
    public void botonInvocarToken()
    {
        invocarToken(null);
    }

    //metodo que invoca un token depende de la informacion que le des, si no le das nada invoca el token del propio jugador. Si le das algo intentara buscar el token en cuestion y si no lo encuentra lo creara.
    public void invocarToken(Token invFicha)
    {
        if (invFicha == null)
        {
            GameObject token = GameObject.Instantiate(ficha);
            token.transform.position = new Vector3(-3, -4, 0);
            token.GetComponent<TokenMovement>().token.Perfil = perfil.Nombre;
            token.GetComponent<SpriteRenderer>().color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            token.name = perfil.Nombre;
        }
        else if (GameObject.Find(invFicha.Perfil)==null)
        {
            GameObject token = GameObject.Instantiate(ficha);
            token.transform.position = new Vector3(-3, -4, 0);
            token.GetComponent<TokenMovement>().token = invFicha;
            token.GetComponent<SpriteRenderer>().color = UnityEngine.Random.ColorHSV(0f,1f,1f,1f,0.5f,1f);
            token.name = invFicha.Perfil;
        }
        else
        {
            GameObject.Find(invFicha.Perfil).GetComponent<TokenMovement>().token = invFicha;
        }
        
    }

    //Metodo que envia la informacion del token al servidor, en este caso el nombre del perfil al que pertenece y su posicion
    public void sendInfoToken(Token token)
    {   
        carga.peticion = "tokenMove";
        carga.assigned = "true";
        carga.json = token.getAsJSON();
        Debug.Log(carga.json);
        sender.send(carga.getAsJSON());
    }

    //metodo que apaga la aplicacion de golpe en caso de error de conexion
    public void apagar()
    {
        Application.Quit();
    }


}
