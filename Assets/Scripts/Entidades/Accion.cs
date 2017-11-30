using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Accion : MonoBehaviour
{

    /// ID DE LA ACCION
    /// especifica el tipo de accion para no hacer castings con todos los tipos posibles
    protected int idAccion;
	public int IDAccion{ get{return 0; }}

    protected int costeAccion;
	public int CosteAccion { get{ return 0;} } //coste en puntosDeAccion
    protected int duracionEnTurnos;
	public int DuracionEnTurnos{ get{ return 0;} }
    protected int alcance;
	public int Alcance{ get{return 0; } }


    /// <summary>
    /// Esta funcion se ejecutara siempre que el usuario, despues de seleccionar una accion, elija otra o la cancele
    /// </summary>
    public abstract void CancelarAccion();
}
