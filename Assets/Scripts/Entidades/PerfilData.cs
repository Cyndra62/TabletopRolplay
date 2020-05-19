using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PerfilData
{
    public string Avatar;
    public List <Jugador> Jugadores;
    public List<Campaña> Campañas;

    public PerfilData()
    {
        Avatar = null;
        Jugadores = new List<Jugador>();
        Campañas = new List<Campaña>();
    }

    public string getAsJSON()
    {
        string json = JsonUtility.ToJson(this);
        Debug.Log(json);
        return json;
    }

    public static PerfilData getFromJson(string PerfilJSON)
    {
        return JsonUtility.FromJson<PerfilData>(PerfilJSON);
    }
}
