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

    public Mensaje()
    {
        Receptor = null;
        Emisor = null;
        Messaje = null;
        Tipo = null;
    }
    public string getAsJSON()
    {
        string json = JsonUtility.ToJson(this);
        Debug.Log(json);
        return json;
    }

    public static Mensaje getFromJson(string Mensaje)
    {
        return JsonUtility.FromJson<Mensaje>(Mensaje);
    }
}
