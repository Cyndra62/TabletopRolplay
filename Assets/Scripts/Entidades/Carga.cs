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

    //constructor del objeto carga
    public Carga()
    {
        cuenta = null;
        peticion = null;
        json = null;
        assigned = "false";
    }
    
    //metodo que obtiene el json
    public string getJSON()
    {
        return json;
    }

    //metodo que transforma el objeto carga en un json
    public string getAsJSON() {
        string json;
        json = JsonUtility.ToJson(this);
        return json;
    }

    //metodo que transforma un json en un objeto carga
    public static Carga getFromJSON(string jsonCarga)
    {
        return JsonUtility.FromJson<Carga>(jsonCarga);
    }
}
