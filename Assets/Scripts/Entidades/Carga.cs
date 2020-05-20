using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Carga
{
    public string cuenta;
    public string peticion;
    public string json;
    public string assigned;

    public Carga()
    {
        cuenta = null;
        peticion = null;
        json = null;
        assigned = "false";
    }
    
    public string getJSON()
    {
        return json;
    }

    public string getAsJSON() {
        string json;
        json = JsonUtility.ToJson(this);
        return json;
    }
    public static Carga getFromJSON(string jsonCarga)
    {
        return JsonUtility.FromJson<Carga>(jsonCarga);
    }
}
