using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RolProtector : MonoBehaviour {

    const int CREAR_UNIDAD_INDEX = 3;

    Partida partidaActual;
    int numeroCreaciones;

    void Awake()
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
        while (numeroCreaciones > 0 && aldeanoActual < aldeanosCreadores.Count) {
           //Continuar desde aquí
        }


    }

    List<Unidad> GetCreadoresDeTorres() {

        List<Unidad> creadoresDeTorres = partidaActual.JugadorActual.unidadesDisponibles; //COJO TODAS LAS UNIDADES
        for (int i = creadoresDeTorres.Count - 1; i >= 0; i--) {
            if (creadoresDeTorres[i].IdUnidad != TipoUnidad.Worker) {  //ME DESHAGO DE AQUELLAS QUE NO SEAN ALDEANOS
                creadoresDeTorres.Remove(creadoresDeTorres[i]);
            }
        }
        List<Unidad> creadoresOrdenados = new List<Unidad>();
        Unidad objetivo = partidaActual.Jugadores[partidaActual.JugadorActual.IndexPlayerObjetivoActual].Capital;
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
