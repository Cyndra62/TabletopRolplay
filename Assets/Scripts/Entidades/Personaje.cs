using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personaje
{
    public string nombre;
    public string sexo;
    public string raza;
    public int edad;
    public string profesion;

    public int fuerza;
    public int carisma;
    public int percepcion;
    public int destreza;
    public int manipulacion;
    public int astucia;
    public int resistencia;
    public int apariencia;
    public int inteligencia;

    public int alerta;
    public int apañar;
    public int pelea;
    public int armasDistancia;
    public int montar;
    public int atletismo;
    public int expresion;
    public int armasMelee;
    public int empatia;
    public int intimidacion;
    public int sigilo;
    public int delincuencia;
    public int interpretacion;
    public int subterfugio;
    public int instinto;
    public int supervivencia;

    public int herreria;
    public int caza;
    public int sastreria;
    public int militar;
    public int cocina;
    public int medicina;
    public int geografia;
    public int ciencias;

    public int vida;
    public int energia;
    public int hambre;
    public int fama;
    public int experiencia;
    public int nivel;
    public int puntosDeMejora;

    public Personaje()
    {
        nombre = null;
        sexo = null;
        raza = null;
        edad = 0;
        profesion = null;

        fuerza = 0;
        carisma = 0;
        percepcion = 0;
        destreza = 0;
        manipulacion = 0;
        astucia = 0;
        resistencia = 0;
        apariencia = 0;
        inteligencia = 0;

        alerta = 0;
        apañar = 0;
        pelea = 0;
        armasDistancia = 0;
        montar = 0;
        atletismo = 0;
        expresion = 0;
        armasMelee = 0;
        empatia = 0;
        intimidacion = 0;
        sigilo = 0;
        delincuencia = 0;
        interpretacion = 0;
        subterfugio = 0;
        instinto = 0;
        supervivencia = 0;

        herreria = 0;
        caza = 0;
        sastreria = 0;
        militar = 0;
        cocina = 0;
        medicina = 0;
        geografia = 0;
        ciencias = 0;

        vida = 0;
        energia = 0;
        hambre = 0;
        fama = 0;
        experiencia = 0;
        nivel = 0;
        puntosDeMejora = 0;
}

    public string getAsJSON()
    {
        string json = JsonUtility.ToJson(this);
        Debug.Log(json);
        return json;
    }
    public static Personaje getFromJson(string PersonajeJSON)
    {
        return JsonUtility.FromJson<Personaje>(PersonajeJSON);
    }
}
