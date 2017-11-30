using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalData
{

    public const int DIFICULTAD = 1;
    public const int MAX_DIFICULTAD = 5;

    public const int DEFENSA_MAXIMA = 100; //con esta defensa, una unidad es invulnerable al dano


    #region ID UNIDADES
    public const int ID_EXPLORADOR = 0;
    public const int ID_TORRE_DEFENSA = 1;
    #endregion

    #region ID RECURSOS
    public const int ID_MADERA = 0;
    public const int ID_METAL = 1;
    public const int ID_PIEDRA = 2;
    public const int ID_COMIDA = 3;
    #endregion

    #region ID ACCIONES
    public const int ACCION_ATACAR = 0;
    public const int ACCION_MOVER = 1;
    public const int ACCION_CONSTRUIR = 2;
    #endregion
}