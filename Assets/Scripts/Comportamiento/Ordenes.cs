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
    float exploracion;
    float oa;
    float od;
    float op;
    float or;

    public Ordenes(float exploracion, float r, float d, float a, float p)
    {
        this.exploracion = exploracion;
        oa = a;
        op = p;
        od = d;
        or = r;
    }

    public float GetOrdenAtaque()
    {
        return oa;
    }

        public float GetOrdenDefensa()
    {
        return od;
    }


        public float GetOrdenPreparacion()
    {
        return op;
    }

        public float GetOrdenRecoleccion()
    {
        return or;
    }

        public float GetOrdenExploracion()
        {
            return exploracion;
        }

}