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

    public string getAsJSON()
    {
        string json = JsonUtility.ToJson(this);
        Debug.Log(json);
        return json;
    }
    public Campaña()
    {
        Nombre = null;
        Juego = null;
        Id = 0;
        DM = 0;
        CampañaJSON = null;
    }

    public static Campaña getFromJson(string campañaJSON)
    {
        return JsonUtility.FromJson<Campaña>(campañaJSON);
    }


}
