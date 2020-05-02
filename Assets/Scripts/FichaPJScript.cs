using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FichaPJScript : MonoBehaviour
{
    [Header("Vistas")]
    public GameObject vistaAtributos;
    public GameObject vistaHabilidades;
    public GameObject vistaProfesiones;
    public GameObject vistaRecursos;
    public GameObject vistaFacultades;
    [Header("Datos Personales")]
    public InputField nombre;
    public Dropdown sexo;
    public Dropdown raza;
    public InputField Edad;
    public InputField Profesion;
    [Header("Atributos")]
    public InputField fuerza;
    public InputField carisma;
    public InputField percepcion;
    public InputField destreza;
    public InputField manipulacion;
    public InputField astucia;
    public InputField resistencia;
    public InputField apariencia;
    public InputField inteligencia;
    [Header("Habilidades")]
    public InputField alerta;
    public InputField apanar;
    public InputField pelea;
    public InputField armasDistancia;
    public InputField montar;
    public InputField atletismo;
    public InputField expresion;
    public InputField armasMelee;
    public InputField empatia;
    public InputField intimidacion;
    public InputField sigilo;
    public InputField delincuencia;
    public InputField interpretacion;
    public InputField subterfugio;
    public InputField instinto;
    public InputField supervivencia;
    [Header("Profesiones")]
    public InputField herreria;
    public InputField caza;
    public InputField sastreria;
    public InputField militar;
    public InputField cocina;
    public InputField medicina;
    public InputField geografia;
    public InputField ciencias;
    [Header("Recursos")]
    public InputField vida;
    public InputField energia;
    public InputField hambre;
    public InputField fama;
    public InputField experiencia;
    public InputField nivel;
    public InputField puntosDeMejora;
    public void changeAtributos()
    {
        vistaAtributos.SetActive(true);
        vistaHabilidades.SetActive(false);
        vistaProfesiones.SetActive(false);
        vistaRecursos.SetActive(false);
        vistaFacultades.SetActive(false);
    }
    public void changeHabilidades()
    {
        vistaAtributos.SetActive(false);
        vistaHabilidades.SetActive(true);
        vistaProfesiones.SetActive(false);
        vistaRecursos.SetActive(false);
        vistaFacultades.SetActive(false);
    }
    public void changeProfesiones()
    {
        vistaAtributos.SetActive(false);
        vistaHabilidades.SetActive(false);
        vistaProfesiones.SetActive(true);
        vistaRecursos.SetActive(false);
        vistaFacultades.SetActive(false);
    }
    public void changeRecursos()
    {
        vistaAtributos.SetActive(false);
        vistaHabilidades.SetActive(false);
        vistaProfesiones.SetActive(false);
        vistaRecursos.SetActive(true);
        vistaFacultades.SetActive(false);
    }
    public void changeFacultades()
    {
        vistaAtributos.SetActive(false);
        vistaHabilidades.SetActive(false);
        vistaProfesiones.SetActive(false);
        vistaRecursos.SetActive(false);
        vistaFacultades.SetActive(true);
    }

    public void guardarFicha()
    {

    }
}
