using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Accion : MonoBehaviour
{

    /// ID DE LA ACCION
    /// especifica el tipo de accion para no hacer castings con todos los tipos posibles
    protected int idAccion;
    public int IDAccion{ get;}

    protected int costeAccion;
    public int CosteAccion { get; } //coste en puntosDeAccion
    protected int duracionEnTurnos;
    public int DuracionEnTurnos{ get; }
    protected int alcance;
    public int Alcance{ get; }
}
