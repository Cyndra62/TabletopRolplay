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

    //constructor del objeto
    public PerfilData()
    {
        Avatar = "";
        Jugadores = new List<Jugador>();
        Campañas = new List<Campaña>();
    }

    //metodo que convierte el objeto en un json
    public string getAsJSON()
    {
        string json = JsonUtility.ToJson(this);
        Debug.Log(json);
        return json;
    }

    //metodo que convierte el json en un objeto
    public static PerfilData getFromJson(string PerfilJSON)
    {
        return JsonUtility.FromJson<PerfilData>(PerfilJSON);
    }
}
