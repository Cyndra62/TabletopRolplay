using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Campaña
{
    public string Nombre;
    public string Juego;
    public int Id;
    public int DM;
    public string CampañaJSON;

    //metodo que transforma el objeto en un json
    public string getAsJSON()
    {
        string json = JsonUtility.ToJson(this);
        Debug.Log(json);
        return json;
    }

    //constructor del objeto
    public Campaña()
    {
        Nombre = null;
        Juego = null;
        Id = 0;
        DM = 0;
        CampañaJSON = null;
    }

    //metodo que transforma el json en un objeto
    public static Campaña getFromJson(string campañaJSON)
    {
        return JsonUtility.FromJson<Campaña>(campañaJSON);
    }


}
