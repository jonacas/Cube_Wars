﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverUnidad :  Accion{

    List<Vector3> ruta;
    int posicionActualRuta = 0;
	private const float MOVE_SPEED = 10f;
    public List<Node> NodosAlAlcance;

    float margen = 1.0f; //Margen para indicar que se está lo suficientemente cerca de un punto.

    private void Awake()
    {       
        m_Unidad = GetComponent<Unidad>();
        switch (m_Unidad.IdUnidad)
        {
            case TipoUnidad.Warrior: //en caso de que al final se añadan otras unidades, pues ya sabes loko
                Alcance = 4;
                break;
            case TipoUnidad.Worker:
                Alcance = 6;
                break;
            case TipoUnidad.Explorer:
                Alcance = 2;
                break;
        }
        idAccion = AccionID.move;
    }

    private void Start()
    {
        StageData.currentInstance.LimpiarGrafo(StageData.currentInstance.CG.nodeMap);
        NodosAlAlcance = Control.GetNodosAlAlcance(StageData.currentInstance.GetNodeFromPosition(transform.position), 3);
        m_Unidad.Nodo = StageData.currentInstance.GetNodeFromPosition(transform.position);
        StageData.currentInstance.LimpiarGrafo(StageData.currentInstance.CG.nodeMap);
    }

    public bool Ejecutar(Node destino, List<Vector3> ruta)
    {
        SeleccionarResaltoDeCasilla();
        print("Ejecutar entrado");
        print(NodosAlAlcance.Count);
        //print(destino == null);
        if (NodosAlAlcance.Contains(destino))
        {
            StageData.currentInstance.GetNodeFromPosition(ruta[ruta.Count - 1]).unidad = null;
            print("Mover Unidad");
            //Debug.LogError("ERROR EN ACCION MOVER: Falta que las unidades sobre los nodos se actualicen");
            Unidad unidadActual = GetComponent<Unidad>();
            /*if (Partida.GetPartidaActual().Jugadores[unidadActual.IdJugador].RestarPuntosDeAccion(costeAccion))
            {*/
            this.ruta = ruta; //IMPORTANTE CONTROLAR DESDE FUERA QUE LLEGUE UNA RUTA VIABLE, ES DECIR, QUE EL OBJETIVO ESTÉ AL ALCANCE DE LA UNIDAD QUE SE QUIERE MOVER. AQUI NO SE CONTROLA ESE ERROR.
            StartCoroutine("RecorrerRuta");

            CancelarAccion();

            m_Unidad.Nodo = StageData.currentInstance.GetNodeFromPosition(ruta[ruta.Count - 1]);
            StageData.currentInstance.GetNodeFromPosition(ruta[ruta.Count - 1]).unidad = this.m_Unidad;
            return true;
            /*}
            else
                return false;*/
        }
        return false;
    }

    IEnumerator RecorrerRuta()
    {
        while(posicionActualRuta < ruta.Count-1)
        {
			
			transform.position = Vector3.MoveTowards(transform.position, ruta[posicionActualRuta + 1], Time.deltaTime * MOVE_SPEED);
			if (Vector3.Distance(transform.position, ruta[posicionActualRuta + 1]) < margen)
				posicionActualRuta++;
			else
				yield return null;
        
        }
        posicionActualRuta = 0;
        StageData.currentInstance.LimpiarGrafo(StageData.currentInstance.CG.nodeMap);
        NodosAlAlcance = Control.GetNodosAlAlcance(StageData.currentInstance.GetNodeFromPosition(transform.position), 3);
    }   

    public override void CancelarAccion()
    {
        m_Unidad.QuitarResaltoCasillasAlAlcance(NodosAlAlcance);
    }

    public override void EmpezarAccion()
    {
        SeleccionarResaltoDeCasilla();
        m_Unidad.ResaltarCasillasAlAlcance(NodosAlAlcance);
    }

    public override void SeleccionarResaltoDeCasilla()
    {
        print("SeleccionarResaltoCasilla");
        NodosAlAlcance = Control.GetNodosAlAlcance(m_Unidad.Nodo, Alcance);
        foreach (Node n in NodosAlAlcance)
        {
            if (n.unidad != null ||
                n.resourceType != TipoRecurso.NullResourceType)
            {
                print(n.unidad + "   " + n.resourceType + "  Eliminado");
                NodosAlAlcance.Remove(n);
            }
        }

        m_Unidad.ResaltarCasillasAlAlcance(NodosAlAlcance);
    }



}
