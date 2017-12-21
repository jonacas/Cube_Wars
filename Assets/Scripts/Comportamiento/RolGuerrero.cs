using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RolGuerrero : MonoBehaviour
{
    const int NUMERO_MINIMO_DE_GUERREROS = 5;

    List<Vector3> caminoANodoDestino;
    public bool fin;
    Partida partidaActual;
    int numeroCreaciones;
    int puntosDispo;
    bool numeroAldeanosMINIMOS;
    List<Unidad> guerreros;

    int idJugador;

    void Start()
    {
        idJugador = -1;
        partidaActual = StageData.currentInstance.GetPartidaActual();
        caminoANodoDestino = new List<Vector3>();
    }

    public void SetIdJugador(int id)
    {
        if (idJugador == -1)
        {
            idJugador = id;
        }
        else
        {
            Debug.LogError("Se intenta cambiar id de jugador cuando no toca BITCH");
        }
    }

    public bool ComenzarTurno(ref int puntosAsig)
    {
        partidaActual = StageData.currentInstance.GetPartidaActual();
        // Buscamos ahora a los aldeanos disponibles.
        puntosDispo = puntosAsig; 

        print("COMIENZA ROL GUERRERO");

        guerreros = new List<Unidad>();

        foreach (Unidad u in StageData.currentInstance.GetPartidaActual().Jugadores[idJugador].unidadesDisponibles)
        {
            if (u.IdUnidad == TipoUnidad.Warrior)
            {
                guerreros.Add(u);
            }
        }

        if (guerreros.Count < NUMERO_MINIMO_DE_GUERREROS)
        {
                return false;
                fin = true;
        }
        else
        {
            int movimientosDisponibles = puntosDispo / StageData.COSTE_PA_MOVER_UNIDAD;

            if (movimientosDisponibles > 0)
            {
                StartCoroutine("PrepararOrdenesGuerrero");
                return true;
            }
            else
            {
                return false;
                fin = true;
            }

        }

        //Comprobamos si necesitamos recursos mínimos, para cumplir necesidades mínimas.
    }

    IEnumerator PrepararOrdenesGuerrero()
    {

        //asignamos a cada guerrero una posicion cercana a la capital
        List<Node> cercanias;
        cercanias = Control.GetNodosAlAlcance(StageData.currentInstance.GetPartidaActual().Jugadores[StageData.currentInstance.GetPartidaActual().JugadorActual.IndexPlayerObjetivoActual].Capital.Nodo, 3);

        IA_Guerrero guerIA;
        int nodoActual = 0;
        foreach (Unidad guer in guerreros)
        {
            //print("preparandoAldeano PROTEC");
            guerIA = (IA_Guerrero)guer;
            //print("set destino PROTEC");

            guerIA.SetDestino(cercanias[nodoActual]);

            while (!guerIA.caminoListo)
                yield return null;
        }
        yield return null;
        StartCoroutine("MoverGuerreros");
    }

    IEnumerator MoverGuerreros()
    {

        foreach (IA_Guerrero guerr in guerreros)
        {
            if (puntosDispo < StageData.COSTE_PA_ATACAR)
                break;

            guerr.AvanzarHaciaDestino(ref puntosDispo);

            while (!guerr.listo)
                yield return null;
        }

        fin = true;
    }
}
