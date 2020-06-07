using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CampañaData
{
    public List<Token> TokensPlayer;
    public List<string> ImagenesMapas;
    public List<Token> TokensNPC;

    //constructor del objeto
    public CampañaData()
    {
        TokensPlayer = new List<Token>();
        ImagenesMapas = new List<string>();
        TokensNPC = new List<Token>();
    }
    //metodo que convierte el objeto en un json
    public string getAsJSON()
    {
        string json = JsonUtility.ToJson(this);
        Debug.Log(json);
        return json;
    }
    //metodo que convierte el json en un objeto
    public static CampañaData getFromJson(string CampañaJSON)
    {
        return JsonUtility.FromJson<CampañaData>(CampañaJSON);
    }
}
