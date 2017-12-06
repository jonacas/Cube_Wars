using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverUnidad :  Accion{

    List<Vector3> m_Ruta;
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

    public void Ejecutar(Node destino)
    {
        
        SeleccionarResaltoDeCasilla();
        print("Ejecutar entrado");
        print(NodosAlAlcance.Count);
        //print(destino == null);
        if (NodosAlAlcance.Contains(destino))
        {
            SolicitarYRecorrerCamino(destino.position);            
        }
    }

    void Mover()
    {
        m_Ruta = m_Unidad.caminoActual;
        StartCoroutine("RecorrerRuta");
        CancelarAccion();
    }

    IEnumerator RecorrerRuta()
    {
        while(posicionActualRuta < m_Ruta.Count-1)
        {
			
			transform.position = Vector3.MoveTowards(transform.position, m_Ruta[posicionActualRuta + 1], Time.deltaTime * MOVE_SPEED);
			if (Vector3.Distance(transform.position, m_Ruta[posicionActualRuta + 1]) < margen)
				posicionActualRuta++;
			else
				yield return null;        
        }
        posicionActualRuta = 0;
        m_Unidad.Nodo = StageData.currentInstance.GetNodeFromPosition(m_Unidad.Posicion);
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

    public void SolicitarYRecorrerCamino(Vector3 final)
    {
        StageData.currentInstance.GetPathToTarget(this.transform.position, final, m_Unidad);
        StartCoroutine("EsperarCamino");
    }

    public IEnumerator EsperarCamino()
    {
        while (!m_Unidad.caminoListo)
            yield return null;
        print("Espera camino terminada");
        Mover();
    }

}
