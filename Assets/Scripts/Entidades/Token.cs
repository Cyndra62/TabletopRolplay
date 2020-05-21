using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Token
{
    public string Img;
    public int X;
    public int Y;
    public string Perfil;
    public string IsNPC;

    public Token()
    {
        X = 0;
        Y = 0;
        Img = "";
        Perfil = null;
        IsNPC = "false";
    }
}
