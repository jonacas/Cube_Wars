using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalData
{

    public const int DIFICULTAD = 1;
    public const int MAX_DIFICULTAD = 5;

    public const int DEFENSA_MAXIMA = 100; //con esta defensa, una unidad es invulnerable al dano

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