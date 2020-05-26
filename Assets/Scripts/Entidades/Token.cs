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

    public Token()
    {
        X = 0;
        Y = 0;
        Img = "";
        Perfil = null;
    }
    public string getAsJSON()
    {
        string json = JsonUtility.ToJson(this);
        Debug.Log(json);
        return json;
    }
    public static Token getFromJson(string CampañaJSON)
    {
        return JsonUtility.FromJson<Token>(CampañaJSON);
    }
}
