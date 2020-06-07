using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Token
{
    public string Img;
    public float X;
    public float Y;
    public string Perfil;

    //Constructor del objeto
    public Token()
    {
        X = 0;
        Y = 0;
        Img = "";
        Perfil = null;
    }
    //metodo para convertir el objeto Token en un json
    public string getAsJSON()
    {
        string json = JsonUtility.ToJson(this);
        Debug.Log(json);
        return json;
    }

    //metodo para convertir el json en un objeto Token
    public static Token getFromJson(string Token)
    {
        return JsonUtility.FromJson<Token>(Token);
    }
}
