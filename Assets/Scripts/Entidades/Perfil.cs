using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Perfil
{
    // Start is called before the first frame update
    public int Id;
    public string Nombre;
    public string Correo;
    public string Contraseña;
    public string PerfilJSON;

    public Perfil()
    {
        Id = 0;
        Nombre = null;
        Correo = null;
        Contraseña = null;
        PerfilJSON = null;
    }

    public string getAsJSON()
    {

        string json = JsonUtility.ToJson(this);
        Debug.Log(json);
        return json;
        
        
    }

    public static Perfil getFromJson(string PerfilJSON)
    {
        return JsonUtility.FromJson<Perfil>(PerfilJSON);
    }


}
