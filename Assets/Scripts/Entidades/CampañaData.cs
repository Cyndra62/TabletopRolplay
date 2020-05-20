using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CampañaData
{
    List<Token> TokensPlayer;
    List<string> ImagenesMapas;
    List<Token> TokensNPC;

    public CampañaData()
    {
        TokensPlayer = new List<Token>();
        ImagenesMapas = new List<string>();
        TokensNPC = new List<Token>();
    }

    public string getAsJSON()
    {
        string json = JsonUtility.ToJson(this);
        Debug.Log(json);
        return json;
    }

    public static CampañaData getFromJson(string CampañaJSON)
    {
        return JsonUtility.FromJson<CampañaData>(CampañaJSON);
    }
}
