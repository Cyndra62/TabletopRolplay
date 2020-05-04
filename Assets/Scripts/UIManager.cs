using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;



public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public SenderReciever senderReciever;
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
    /*TcpClient client = new TcpClient("81.39.109.215", 13000);*/

    void Start()
    {
        //No poner Reader
        senderReciever = new SenderReciever();
        senderReciever.Launch();
    }

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
        /*StreamReader sReader = new StreamReader(client.GetStream(), Encoding.ASCII);
        Debug.Log(sReader.ReadLine());

        StreamWriter sWriter = new StreamWriter(client.GetStream(), Encoding.ASCII);
        sWriter.WriteLine("{'nombre':'wololo', 'apellido':'wololo'}" );
        sWriter.Flush();*/
    }
    public void ButtonConfirmRegister()
    {
        if (PassRegistro.text != "" && ConfPassRegistro.text !="" && PassRegistro.text == ConfPassRegistro.text)
        {

        }

    }
    
}


