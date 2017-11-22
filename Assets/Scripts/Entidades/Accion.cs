using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IAccion{

    /// ID DE LA ACCION
    /// especifica el tipo de accion para no hacer castings con todos los tipos posibles
    int idAccion {get;}

    int costeAccion { get; } //coste en puntosDeAccion
    int duracionEnTurnos { get; }
    int alcance { get; }

    int GetCoste();
    int GetDuracion();
    int GetAlcance();
    int GetIdAccion();
}
