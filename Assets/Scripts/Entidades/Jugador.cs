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

    //constructor del jugador
    public Jugador()
    {
        Id = 0;
        Nombre = null;
        Perfil = 0;
        Campaña = 0;
        JugadorJSON = null;
    }

    //metodo que convierte el objeto en un json
    public string getAsJSON()
    {
        string json = JsonUtility.ToJson(this);
        Debug.Log(json);
        return json;
    }

    //metodo que convierte el json en un objeto
    public static Jugador getFromJson(string CampañaJSON)
    {
        return JsonUtility.FromJson<Jugador>(CampañaJSON);
    }

    //metodo que transforma el JugadorJSON en un objeto personaje
    public Personaje getPersonaje()
    {
        return JsonUtility.FromJson<Personaje>(JugadorJSON);
    }

}
