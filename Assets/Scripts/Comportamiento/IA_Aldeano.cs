using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_Aldeano : Unidad {

    private int SALUD_MAX_ALDEANO = 100;
    private int VISION_ALDEANO = 4;
    private int CASILLAS_MOVIMIENTO_ALDEANO = 4;

    //ACCIONES DE LA UNIDAD
    private const int ACCION_MOVER = 0;
    private const int ACCION_RECOLECTAR = 1;
    private const int ACCION_CONSTRUIR = 2;


    private bool heLlegado, listo;
    private List<Vector3> caminoAObjetivo;
    int posActual;

    void Awake()
    {
        //Nodo = StageData.currentInstance.GetNodeFromPosition(transform.position);
        saludMaxima = SALUD_MAX_ALDEANO;
        Vida = SALUD_MAX_ALDEANO;
        vision = VISION_ALDEANO;
        acciones = new List<Accion>();
        acciones.Add(this.GetComponent<MoverUnidad>());
        acciones.Add(this.GetComponent<Construir>());
        idUnidad = TipoUnidad.Worker;

        //FALTA RELLENAR INFLUENCIAS
    }

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
        while (!caminoListo)
            yield return null;
        print("Espera camino terminada");
        AccionMover(caminoActual);
    }

    public void AccionConstruir(Node objetivo)
    {
        Construir co = (Construir)acciones[ACCION_CONSTRUIR];
        print(co == null);
        co.Ejecutar(objetivo);
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
        alcance = Control.GetNodosAlAlcance(StageData.currentInstance.GetNodeFromPosition(this.transform.position), VISION_ALDEANO);
        //Codigo de recolectar

        caminoActual = caminoAObjetivo.GetRange(posActual, VISION_ALDEANO - 1);


        Vector3 destino = Vector3.zero;
        int incrementoPos = 0;
        for (int i = caminoActual.Count - 1; i >= 0; i--) {
            if (StageData.currentInstance.GetNodeFromPosition(caminoActual[i]).unidad == null && StageData.currentInstance.GetNodeFromPosition(caminoActual[i]).resourceType != TipoRecurso.NullResourceType)
            {
                destino = caminoActual[i];
                incrementoPos = i;
                break;
            }

        }
        if (destino == Vector3.zero) {
            heLlegado = true;
            listo = true;
            return;
        }

        MoverUnidad mv = (MoverUnidad)acciones[ACCION_MOVER];
        if (mv.Ejecutar(StageData.currentInstance.GetNodeFromPosition(destino))) {

            posActual += incrementoPos;
            puntosDisponibles -= MoverUnidad.COSTE_MOVER;
        }
        listo = true;


        //Falta Construir, que no tengo muy claro donde ponerlo
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
