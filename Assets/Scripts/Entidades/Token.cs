using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Token
{
    string Img;
    int X;
    int Y;
    string Perfil;
    string IsNPC;

    public Token()
    {
        X = 0;
        Y = 0;
        Img = "";
        Perfil = null;
        IsNPC = "false";
    }
}
