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

    public float GetPrioridad()
    {
        return pioridadOrden;
    }
}

public struct OrdenAtaque
{
    float prioridadOrden;

    public OrdenAtaque(float p)
    {
        prioridadOrden = p;
    }

    public float GetPrioridad()
    {
        return prioridadOrden;
    }

}

public struct OrdenDefensa
{
    float prioridadOrden;

    public OrdenDefensa(float p)
    {
        prioridadOrden = p;
    }

    public float GetPrioridad()
    {
        return prioridadOrden;
    }

}

public struct OrdenPreparacion
{
    float prioridadOrden;

    public OrdenPreparacion(float p)
    {
        prioridadOrden = p;
    }

    public float GetPrioridad()
    {
        return prioridadOrden;
    }

}

public struct Ordenes
{
    OrdenAtaque oa;
    OrdenDefensa od;
    OrdenPreparacion op;
    OrdenRecoleccion or;

    public Ordenes(OrdenAtaque a, OrdenDefensa d, OrdenRecoleccion r, OrdenPreparacion p)
    {
        oa = a;
        op = p;
        od = d;
        or = r;
    }

    public OrdenAtaque GetOrdenAtaque()
    {
        return oa;
    }

        public OrdenDefensa GetOrdenDefensa()
    {
        return od;
    }


        public OrdenPreparacion GetOrdenPreparacion()
    {
        return op;
    }

        public OrdenRecoleccion GetOrdenRecoleccion()
    {
        return or;
    }

}