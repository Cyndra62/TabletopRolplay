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

    //constructor del perfil
    public Perfil()
    {
        Id = 0;
        Nombre = null;
        Correo = null;
        Contraseña = null;
        PerfilJSON = null;
    }

    //metodo que convierte el perfil en un json
    public string getAsJSON()
    {

        string json = JsonUtility.ToJson(this);
        Debug.Log(json);
        return json;
        
        
    }

    //metodo que convierte un json en un objeto perfil
    public static Perfil getFromJson(string PerfilJSON)
    {
        return JsonUtility.FromJson<Perfil>(PerfilJSON);
    }


}
