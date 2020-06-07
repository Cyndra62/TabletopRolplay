using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Mensaje
{
    public string Receptor;
    public string Emisor;
    public string Messaje;
    public string Tipo;

    //constructor del objeto
    public Mensaje()
    {
        Receptor = null;
        Emisor = null;
        Messaje = null;
        Tipo = null;
    }

    //metodo que convierte el objeto en un json
    public string getAsJSON()
    {
        string json = JsonUtility.ToJson(this);
        Debug.Log(json);
        return json;
    }

    //metodo que convierte el json en un objeto
    public static Mensaje getFromJson(string Mensaje)
    {
        return JsonUtility.FromJson<Mensaje>(Mensaje);
    }
}
