using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;



public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
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
    //TcpClient client = new TcpClient("81.39.109.215", 13000);
    [Header("Instancias Clases")]
    public FichaPJScript fichaPJ;
    public SenderReceiver senderReceiver;

    public Thread listener;

    void Start()
    {
        listener = new Thread(receive);
        listener.Start();
    }
    #region RECEIVER    
    public void receive()
    {
        string str = "accede";
        while (senderReceiver.isConnected() && senderReceiver.getIsRunning())
        {
            try
            {
                str = senderReceiver.sr.ReadLine();

            }
            catch (IOException ex)
            {
                Debug.Log(ex.Message);
                senderReceiver.stopConnection();
            }
            finally { }
        }
    }

    #endregion
    // Update is called once per frame
    void Update()
    {
        //No poner Reader
    }

    public int RollDice(int atributo,int habilidad)
    {
        int resultado=0;
        resultado = Random.Range(1, 20) + atributo + habilidad;
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
        //StreamReader sReader = new StreamReader(client.GetStream(), Encoding.ASCII);
       // Debug.Log(sReader.ReadLine());

        //StreamWriter sWriter = new StreamWriter(client.GetStream(), Encoding.ASCII);
       // sWriter.WriteLine("{'nombre':'wololo', 'apellido':'wololo'}" );
        // sWriter.Flush();
    }
    public void ButtonConfirmRegister()
    {
        if (PassRegistro.text != "" && ConfPassRegistro.text !="" && PassRegistro.text == ConfPassRegistro.text)
        {

        }

    }
    
    
}


