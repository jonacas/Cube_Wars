using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalData
{

    public const int JUGADOR_HUMANO = 0;

    public const int DIFICULTAD = 1;
    public const int MAX_DIFICULTAD = 5;

    public const int DEFENSA_MAXIMA = 100; //con esta defensa, una unidad es invulnerable al dano

    #region COSTES ALDEANO
    public const int COSTE_PA_CREACION_ALDEANO = 10;
    public const int COSTE_MADERA_CREACION_ALDEANO = 0;
    public const int COSTE_COMIDA_CREACION_ALDEANO = 20;
    public const int COSTE_METAL_CREACION_ALDEANO = 0;
    public const int COSTE_PIEDRA_CREACION_ALDEANO = 0;
    #endregion

    #region COSTES EXPLORADOR
    public const int COSTE_PA_CREACION_EXPLORADOR = 10;
    public const int COSTE_MADERA_CREACION_EXPLORADOR = 0;
    public const int COSTE_COMIDA_CREACION_EXPLORADOR = 10;
    public const int COSTE_METAL_CREACION_EXPLORADOR = 0;
    public const int COSTE_PIEDRA_CREACION_EXPLORADOR = 0;
    #endregion

    #region COSTES GUERRERO
    public const int COSTE_PA_CREACION_GUERRERO = 20;
    public const int COSTE_MADERA_CREACION_GUERRERO = 0;
    public const int COSTE_COMIDA_CREACION_GUERRERO = 40;
    public const int COSTE_METAL_CREACION_GUERRERO = 10;
    public const int COSTE_PIEDRA_CREACION_GUERRERO = 0;
    #endregion


    #region COSTES
    public const int COSTE_MADERA_EDIFICIO_RECOLECCION = 100;
    public const int COSTE_ROCA_EDIFICIO_RECOLECCION = 30;

    public const int COSTE_MADERA_TORRE_DEFENSA = 20;
    public const int COSTE_ROCA_TORRE_DEFENSA = 20;
    public const int COSTE_METAL_TORRE_DEFENSA = 50;
    #endregion

}

//para facilitar el paso y la comprobacion de costes
public struct FacturaRecursos
{
    int pa;
    int madera;
    int comida;
    int metal;
    int piedra;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="puntosAcc">PA</param>
    /// <param name="com">Comida</param>
    /// <param name="mad">Madera</param>
    /// <param name="pie">Piedra</param>
    /// <param name="metal">Metal</param>
    public FacturaRecursos(int puntosAcc, int com, int mad, int pie, int metal)
    {
        pa = puntosAcc;
        comida = com;
        madera = mad;
        piedra = pie;
        this.metal = metal;
    }

    public int GetComida()
    {
        return comida;
    }

    public int GetMadera()
    {
        return madera;
    }

    public int GetMetal()
    {
        return metal;
    }

    public int GetPiedra()
    {
        return piedra;
    }

    public int GetPA()
    {
        return pa;
    }

}