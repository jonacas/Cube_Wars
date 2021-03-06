﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AccionID
{
    move, attack, create, build
}

public abstract class Accion : MonoBehaviour
{

    /// ID DE LA ACCION
    /// especifica el tipo de accion para no hacer castings con todos los tipos posibles
    protected AccionID idAccion;
	public AccionID IDAccion{ get{return idAccion; }}
    protected Unidad m_Unidad;

    protected int costeAccion;
	public int CosteAccion { get{ return costeAccion;} } //coste en puntosDeAccion
    protected int duracionEnTurnos;
	public int DuracionEnTurnos{ get{ return duracionEnTurnos;} }
    protected int alcance;
    public int Alcance { get { return alcance; } set { alcance = value; } }

    /// <summary>
    /// Esta funcion se ejecutara siempre que el usuario, despues de seleccionar una accion, elija otra o la cancele
    /// </summary>
    public abstract void CancelarAccion();
    public abstract void EmpezarAccion();
    public abstract List<Node> VerNodosAlAlcance();
}
