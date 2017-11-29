using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Coleccion de ordenes para cada comportamiento.
/// </summary>
/// 

public struct OrdenRecoleccion
{
    float pioridadOrden;

    float prioridadComida;
    float prioridadMadera;
    float prioridadRoca;
    float prioridadMetal;

    public OrdenRecoleccion(float pComida, float pMadera, float pRoca, float pMetal, float pOrden)
    {
        prioridadComida = pComida;
        prioridadMadera = pMadera;
        prioridadRoca = pRoca;
        prioridadMetal = pMetal;
        pioridadOrden = pOrden;
    }

    public float GetPrioridadComida()
    {
        return prioridadComida;
    }

    public float GetPrioridadMadera()
    {
        return prioridadMadera;
    }

    public float GetPrioridadRoca()
    {
        return prioridadRoca;
    }

    public float GetPrioridadMetal()
    {
        return prioridadMetal;
    }
}

