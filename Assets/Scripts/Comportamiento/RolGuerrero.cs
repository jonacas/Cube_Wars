using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RolGuerrero : MonoBehaviour {

    public enum EstadoAtaque
    {
        Planificacion, Preparacion, Ataque, Retirada
    }

    private int DISTANCIA_PREPARACION_CAPITAL = 5;
    public const int COSTE_ATACAR = 20;
    private const int MINIMO_GUERREROS = 3;

    private Jugador jug;
    private List<IA_Guerrero> guerrerosDisponibles;
    private List<Vector3> caminoCapial;
    private Unidad un;
    private bool fin;
    private List<Node> casillasDestinoPreparacion;
    public int objetivoActual;


    private int puntosDispAux;

    public EstadoAtaque estado;


	// Use this for initialization
	void Awake () {
        jug = this.gameObject.GetComponent<Jugador>();
        guerrerosDisponibles = new List<IA_Guerrero>();
        un = this.gameObject.GetComponent<Unidad>();
	}


    public void Ataca(int objetivo, int puntosDisponibles, bool empezarDeCero = false)
    {
        puntosDispAux = puntosDisponibles;
        if(empezarDeCero)
            estado = EstadoAtaque.Planificacion;
        fin = false;
        switch (estado)
        {
            case EstadoAtaque.Planificacion:
                   objetivoActual = objetivo;
                 guerrerosDisponibles.Clear();

                //contamos los guerreros y si hay poca el ataque se cancela

                //se obtienen los guerreros de los que se dispone
                for (int i = 0; i < jug.unidadesDisponibles.Count; i++)
                {
                    if (jug.unidadesDisponibles[i].IdUnidad == TipoUnidad.Warrior)
                    {
                        guerrerosDisponibles.Add((IA_Guerrero)jug.unidadesDisponibles[i]);
                    }
                }

                StartCoroutine("dirigirTropas");
                puntosDispAux = puntosDisponibles;
                break;

            case EstadoAtaque.Preparacion:
               puntosDispAux = puntosDisponibles;
                StartCoroutine("PrepararTropas");
                break;

            case EstadoAtaque.Ataque:
                puntosDispAux = puntosDisponibles;
                StartCoroutine("Atacar");
                break;
        }

    }

    IEnumerator dirigirTropas()
    {
        un.caminoListo = false;
        StageData.currentInstance.GetPathToTarget(this.transform.position, jug.capitalesEnemigas[objetivoActual].transform.position, un);

        while (!un.caminoListo)
            yield return null;

        caminoCapial = un.caminoActual;

        //se preparan las tropas a una distancia segura
        //elegimos alrededor de que nodo se concentraran
        Node nodoDestino = StageData.currentInstance.GetNodeFromPosition(caminoCapial[caminoCapial.Count - DISTANCIA_PREPARACION_CAPITAL]);

        //obtenemos nodos suficientes alrededor para colocar a los guerreros
        casillasDestinoPreparacion = new List<Node>();
        int radioNecesario = guerrerosDisponibles.Count / 8;
        while (casillasDestinoPreparacion.Count <= guerrerosDisponibles.Count)
        {
            radioNecesario++;
            casillasDestinoPreparacion = Control.GetNodosAlAlcance(nodoDestino, radioNecesario);
        }

        //asignamos a cada unidad un destino
        for (int i = 0; i < guerrerosDisponibles.Count; i++)
        {
            guerrerosDisponibles[i].SetDestino(casillasDestinoPreparacion[i]);
            while (!guerrerosDisponibles[i].caminoListo)
                yield return null;
        }

        estado = EstadoAtaque.Preparacion;
        //al terminar, volvemos a ejecutar para empezar la accion en si
        Ataca(objetivoActual, puntosDispAux);
        fin = true;
    }

    IEnumerator PrepararTropas()
    {
        //las unidades avanzan hacia la zona de ataque
        int guerrerosEnPosicion = 0;
        while (puntosDispAux > 20)
        {
            for (int i = guerrerosDisponibles.Count - 1; i >= 0; i--)
            {

                if (guerrerosDisponibles[i] == null)
                {
                    guerrerosDisponibles.Remove(guerrerosDisponibles[i]);
                }
                else
                {
                    if (!guerrerosDisponibles[i].HaLlegado())
                    {
                        guerrerosDisponibles[i].AvanzarHaciaDestino(ref puntosDispAux);
                        while (!guerrerosDisponibles[i].AccionTerminada())
                            yield return null;
                    }
                    else
                        guerrerosEnPosicion++;
                }
            }

            //si el 70 o mas de los guerreros han llegado, comienza el ataque
            if ((float)guerrerosEnPosicion / (float)guerrerosDisponibles.Count > 0.7f)
            {
                StartCoroutine("ColocarTropasEnCapital");
                StopCoroutine("PrepararTropas");
            }
        }
        fin = true;
    }

    IEnumerator ColocarTropasEnCapital()
    {
        Node nodoDestino = StageData.currentInstance.GetNodeFromPosition(jug.capitalesEnemigas[objetivoActual].transform.position);

        //obtenemos nodos suficientes alrededor para colocar a los guerreros
        casillasDestinoPreparacion = new List<Node>();
        int radioNecesario = guerrerosDisponibles.Count / 8;
        while (casillasDestinoPreparacion.Count <= guerrerosDisponibles.Count)
        {
            radioNecesario++;
            casillasDestinoPreparacion = Control.GetNodosAlAlcance(nodoDestino, radioNecesario);
        }

        //asignamos a cada unidad un destino
        for (int i = 0; i < guerrerosDisponibles.Count; i++)
        {
            guerrerosDisponibles[i].SetDestino(casillasDestinoPreparacion[i]);
            while (!guerrerosDisponibles[i].caminoListo)
                yield return null;
        }

        estado = EstadoAtaque.Ataque;
        //al terminar, volvemos a ejecutar para empezar la accion en si
        Ataca(objetivoActual, puntosDispAux);
        fin = true;
    }

    IEnumerator Atacar()
    {
        int guerrerosEnCapital = 0;

        while (puntosDispAux > COSTE_ATACAR)
        {
            for (int i = guerrerosDisponibles.Count - 1; i >= 0; i--)
            {

                if (guerrerosDisponibles[i] == null)
                {
                    guerrerosDisponibles.Remove(guerrerosDisponibles[i]);
                }
                else
                {
                    guerrerosDisponibles[i].AvanzarHaciaDestino(ref puntosDispAux);
                    while (!guerrerosDisponibles[i].AccionTerminada())
                        yield return null;
                }
            }

            if (guerrerosDisponibles.Count <= MINIMO_GUERREROS)
            {
                estado = EstadoAtaque.Preparacion;
            }
        }
        fin = true;
    }


    public bool HaTerminado()
    {
        return fin;
    }
}
