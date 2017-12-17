using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_Explorador : Unidad {

    const int ACCION_MOVER = 0;
    const int VISION_EXPLO = 4;

    public bool heLlegado, listo;
    private List<Vector3> caminoTotalANodoDestino;
    int posActual;

    void Awake()
    {        
        acciones = new List<Accion>();
        acciones.Add(GetComponent<MoverUnidad>());
        saludMaxima = StageData.SALUD_MAX_ALDEANO;
        Vida = StageData.SALUD_MAX_ALDEANO;
        defensaMaxima = StageData.DEFENSA_MAX_ALDEANO;
        Defensa = StageData.DEFENSA_MAX_ALDEANO;
        idUnidad = TipoUnidad.Explorer;
        Nodo = StageData.currentInstance.GetNodeFromPosition(transform.position);
        heLlegado = false;
        //StageData.currentInstance.SetInfluenceToNode(VISION_EXPLO, Nodo, IdJugador);
    }

    /*private void Start()
    {
        Nodo = StageData.currentInstance.GetNodeFromPosition(transform.position);
    }*/

    /*void Update()
    {
            Nodo = StageData.currentInstance.GetNodeFromPosition(transform.position);
            Nodo.unidad = this;
    }*/


    public void AccionMover(List<Vector3> camino)
    {
        MoverUnidad mv = (MoverUnidad)acciones[ACCION_MOVER];
        print(mv == null);
       // mv.Ejecutar(StageData.currentInstance.GetNodeFromPosition(camino[camino.Count - 1]), camino);
    }

    public override void SolicitarYRecorrerCamino(Vector3 final)
    {
        base.SolicitarYRecorrerCamino(final);
        StartCoroutine("EsperarCamino");
    }

    public IEnumerator EsperarCamino()
    {
        //espera a que el camino este listo y lo guarda
        while (!caminoListo)
            yield return null;
        caminoTotalANodoDestino = caminoActual;
        posActual = 0;
        //print("Espera camino terminada");
        //AccionMover(caminoActual);
    }    

    public void SetDestino(Node destino)
    {
        heLlegado = false;
        SolicitarYRecorrerCamino(destino.position);
    }

    public void AvanzarHaciaDestino(ref int puntosDisponibles)
    {
        listo = false;
        List<Node> alcance;
        //alcance = acciones[ACCION_MOVER].VerNodosAlAlcance();
        
        //si no atacamos, movemos
        if (posActual + 6 > caminoTotalANodoDestino.Count - 1)
        {
            caminoActual = caminoTotalANodoDestino.GetRange(posActual, caminoTotalANodoDestino.Count - 1);
            heLlegado = true;
        }
        else
            caminoActual = caminoTotalANodoDestino.GetRange(posActual, 6);
        print(caminoActual.Count);


        //comprobamos a que posiciones podemos movernos
       Vector3 destino = Vector3.zero;
        int incrementoPos = 0;
        for (int i = caminoActual.Count - 1; i >= 0; i--)
        {
            if (StageData.currentInstance.GetNodeFromPosition(caminoActual[i]).unidad == null)// && StageData.currentInstance.GetNodeFromPosition(caminoActual[i]).resourceType == TipoRecurso.NullResourceType)
            {
                destino = caminoActual[i];
                incrementoPos = i;
                break;
            }
        }

        //si no puede moverse, ha llegado
       if (destino == Vector3.zero)
        {
            print("Avanzar hacia destino" + caminoActual.Count);
            heLlegado = true;
            listo = true;
            return;
        }

        //si puede moverse, lo hace
        MoverUnidad mv = (MoverUnidad)acciones[ACCION_MOVER];
        print(StageData.currentInstance.GetNodeFromPosition(destino));
        if (mv.Ejecutar(StageData.currentInstance.GetNodeFromPosition(destino)))
        {
            posActual += incrementoPos;
            puntosDisponibles -= StageData.COSTE_PA_MOVER_UNIDAD;
        }
        listo = true;
    }

    public bool HaLlegado()
    {
        return heLlegado;
    }

    public bool AccionTerminada()
    {
        return listo;
    }

}
