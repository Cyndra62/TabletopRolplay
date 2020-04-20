using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginButton : MonoBehaviour
{
    public InputField nombreUsuario;
    public Text texto;
    // Start is called before the first frame update
    void Start()
    {

    }
    
   public void ObtencionNombre()
    {
     string Texto =  nombreUsuario.text;
        texto.text = Texto;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
