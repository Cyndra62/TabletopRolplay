using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 [Serializable]
public class Jugador 
{
    public int Id;
    public string Nombre;
    public int Perfil;
    public int Campaña;
    public string JugadorJSON;

    public Jugador()
    {
        Id = 0;
        Nombre = null;
        Perfil = 0;
        Campaña = 0;
        JugadorJSON = null;
    }

    public string getAsJSON()
    {
        string json = JsonUtility.ToJson(this);
        Debug.Log(json);
        return json;
    }
    public static Jugador getFromJson(string CampañaJSON)
    {
        return JsonUtility.FromJson<Jugador>(CampañaJSON);
    }

    public Personaje getPersonaje()
    {
        return JsonUtility.FromJson<Personaje>(JugadorJSON);
    }

}
