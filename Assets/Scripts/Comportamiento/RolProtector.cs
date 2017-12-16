using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RolProtector : MonoBehaviour {
    
    Partida partidaActual;
    int numeroCreaciones;

    void Start()
    {
        partidaActual = StageData.currentInstance.GetPartidaActual();
    }

    public bool ComenzarTurno(int puntosAsignados)
    {
        //Decidir cual mover y cuanto moverlo
        numeroCreaciones = puntosAsignados / StageData.COSTE_PA_CONSTRUIR_TORRE; // CALCULO CUANTAS CREACIONES PUEDO HACER CON LOS PUNTOS ASIGNADOS
        if (numeroCreaciones > 0)
        {
            CrearTorres();
            return true;
        }
        else
            return false;
    }

    void CrearTorres() {

        List<Unidad> aldeanosCreadores = GetCreadoresDeTorres();
        int aldeanoActual = 0;
        Construir contructor = new Construir();
        while (numeroCreaciones > 0 && aldeanoActual < aldeanosCreadores.Count) { //Proceso para crear
            List<Node> NodosAlAlcance = OrdenarNodos(Control.GetNodosAlAlcance(StageData.currentInstance.GetNodeFromPosition(aldeanosCreadores[aldeanoActual].transform.position), 2));//Obtenemos los nodos en los que podemos crear
            foreach (Node n in NodosAlAlcance) { //RecorremosEsos nodos ahora que estan ordenados por distancia
                if (contructor.Ejecutar(n)) {//Si hemos conseguido crear pasamos al siguiente nodo
                    numeroCreaciones--;
                    break;

                }

            }
            aldeanoActual++;
            if (aldeanoActual >= aldeanosCreadores.Count) {
                print("NO SE PUEDEN CREAR MAS TORRES  PORQUE NO CABEN MAS UNIDADES");

            }
        }


    }

    List<Node> OrdenarNodos(List<Node> Nodos) {

        List<Node> NodosOrdenados = new List<Node>();
        Unidad objetivo = partidaActual.Jugadores[partidaActual.JugadorActual.idJugador].Capital;
        Node aux;
        while (Nodos.Count > 0) {
            aux = Nodos[0];
            foreach (Node n in Nodos)
            {
                if(Vector3.Distance(n.position,objetivo.transform.position) < Vector3.Distance(aux.position,objetivo.transform.position)){

                    aux = n;
                }
            }
            NodosOrdenados.Add(aux);
            Nodos.Remove(aux);
        }
        return NodosOrdenados;
    }

    List<Unidad> GetCreadoresDeTorres() {

        List<Unidad> creadoresDeTorres = partidaActual.JugadorActual.unidadesDisponibles; //COJO TODAS LAS UNIDADES
        for (int i = creadoresDeTorres.Count - 1; i >= 0; i--) {
            if (creadoresDeTorres[i].IdUnidad != TipoUnidad.Worker) {  //ME DESHAGO DE AQUELLAS QUE NO SEAN ALDEANOS
                creadoresDeTorres.Remove(creadoresDeTorres[i]);
            }
        }
        List<Unidad> creadoresOrdenados = new List<Unidad>();
        Unidad objetivo = partidaActual.Jugadores[partidaActual.JugadorActual.idJugador].Capital;
        Unidad aux;
        while (creadoresDeTorres.Count > 0) { //ORDENACION
            aux = creadoresDeTorres[0];
            foreach (Unidad u in creadoresDeTorres) {

                if (Vector3.Distance(u.transform.position, objetivo.transform.position) < Vector3.Distance(aux.transform.position, objetivo.transform.position)) {
                    aux = u;
                }
            }

            creadoresOrdenados.Add(aux);
            creadoresDeTorres.Remove(aux);

        }

        return creadoresOrdenados;
    }

}
