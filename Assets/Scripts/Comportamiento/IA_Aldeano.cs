using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_Aldeano : Unidad {

    const int ACCION_MOVER = 0, ACCION_CONSTRUIR = 1;

    public bool heLlegado, listo;
    private List<Vector3> caminoTotalANodoDestino;
    int posActual;
    Node nodoRecurso;    

    void Awake()
    {
        //Nodo = StageData.currentInstance.GetNodeFromPosition(transform.position);
        saludMaxima = StageData.SALUD_MAX_ALDEANO;
        Vida = StageData.SALUD_MAX_ALDEANO;
        acciones = new List<Accion>();
        acciones.Add(this.GetComponent<MoverUnidad>());
        acciones.Add(this.GetComponent<Construir>());
        idUnidad = TipoUnidad.Worker;
        heLlegado = true;
		caminoTotalANodoDestino = new List<Vector3> ();
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
        //espera a que el camino este listo y lo guarda
        while (!caminoListo)
            yield return null;
        caminoTotalANodoDestino = caminoActual;
		Debug.Log ("Tamaño camino a recorrer: " + caminoTotalANodoDestino);
        posActual = 0;
        //print("Espera camino terminada");
        //AccionMover(caminoActual);
    }    


    public void AccionConstruir(Node objetivo)
    {
        Construir co = (Construir)acciones[ACCION_CONSTRUIR];
        print(co == null);
        co.Ejecutar(objetivo);
    }

    public void SetDestino(Node destino)
    {
        //guarda el nodo donde esta el recurso y fija su destino a uno circunandte
        nodoRecurso = destino;
        List<Node> nodosCircun = Control.GetNodosAlAlcance(destino, 2);

        foreach (Node n in nodosCircun)
        {
            if (Nodo.resourceType == TipoRecurso.NullResourceType)
            {
                heLlegado = false;
                SolicitarYRecorrerCamino(n.position);
                break;
            }
        }
    }

    bool CrearEdificio()
    {
        Construir creadorEdificios = (Construir) acciones[ACCION_CONSTRUIR];
        List<Node> nodosAptos = creadorEdificios.VerNodosAlAlcance();

        foreach (Node n in nodosAptos)
        {
            if (creadorEdificios.Ejecutar(n) /* && tiene recursos suficientes*/)
            {                
                print("He construidoooo");
                ((JugadorIA)StageData.currentInstance.GetPartidaActual().JugadorActual).rolRecolector.recursosSinExplotar.Remove(n.position);
                return true;
            }
        }

        return false;
    }

    /* public void AvanzarHaciaDestinoMasLejano(ref int puntosDisponibles)
     {
         listo = false;
         List<Node> alcance;
         alcance = Control.GetNodosAlAlcance(StageData.currentInstance.GetNodeFromPosition(this.transform.position), acciones[ACCION_MOVER].Alcance);
         //Codigo de recolectar

         caminoActual = caminoAObjetivo.GetRange(posActual, acciones[ACCION_MOVER].Alcance - 1);


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
             puntosDisponibles -= StageData.COSTE_PA_MOVER_UNIDAD;
         }
         listo = true;
         //Falta Construir, que no tengo muy claro donde ponerlo

     }
 */


    public void AvanzarHaciaDestino()
    {
        listo = false;
        List<Node> alcance;
        bool construir = false;
        //alcance = acciones[ACCION_MOVER].VerNodosAlAlcance();

        //si no atacamos, movemos
        if (posActual + 4 > caminoTotalANodoDestino.Count - 1)
        {
            //si esta al alcance, debe construir
            caminoActual = caminoTotalANodoDestino.GetRange(posActual, caminoTotalANodoDestino.Count - 1);
            construir = true;
        }
        else
            caminoActual = caminoTotalANodoDestino.GetRange(posActual, 4);
        print(caminoActual.Count);


        if (construir)
        {
            Construir accionConstr = (Construir)acciones[ACCION_CONSTRUIR];
            if (accionConstr.Ejecutar(nodoRecurso))
            {
                heLlegado = true;
            }
        }

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
            CrearEdificio();
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
