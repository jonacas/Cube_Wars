using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrearUnidad : Accion
{
    //dado que solo pueden crear unidades los edificios, el alcance es siempre el mismo
    List<Node> NodosAlAlcance;


    private void Awake()
    {
        m_Unidad = GetComponent<Unidad>();
        idAccion = AccionID.create;
        Alcance = 2;
        NodosAlAlcance = new List<Node>();
    }


    /// <summary>
    /// Crea una unidad y hace que aparezca en una posicion al azar alrededor del edificio
    /// </summary>
    /// <param name="tipo">Tipo de la unidad que va a crearse</param>
    /// <returns>Devuelve true si se ha podido realizar, false si no</returns>
    public bool Ejecutar(Node destino, TipoUnidad tipo)
    {
        //se debe coger el jugador de la instancia de partida del stageData
        //Jugador jug = SGetComponent<Unidad>().IdJugador

        //SeleccionarResaltoDeCasilla();
        
        if (NodosAlAlcance.Contains(destino))
        {
            GameObject gameObjectUnidad;
            switch (tipo)
            {
                case TipoUnidad.Warrior:
                    if (StageData.currentInstance.GetPartidaActual().JugadorActual.idJugador != StageData.ID_JUGADOR_HUMANO)
                    {
                        gameObjectUnidad = Instantiate(StageData.currentInstance.guerreroIAPrefab, destino.position, StageData.currentInstance.ExplorerPrefab.transform.rotation);
                    }
                    else
                    {
                        gameObjectUnidad = Instantiate(StageData.currentInstance.WarriorPrefab, destino.position, StageData.currentInstance.ExplorerPrefab.transform.rotation);
                    }
                    SetUnidadANodoYViceversa(gameObjectUnidad.GetComponent<Unidad>());
                    StageData.currentInstance.GetPartidaActual().Jugadores[m_Unidad.IdJugador].unidadesDisponibles.Add(gameObjectUnidad.GetComponent<Unidad>());
                    StageData.currentInstance.GetPartidaActual().JugadorActual.Guerreros++;
                    gameObject.GetComponent<Unidad>().IdJugador = StageData.currentInstance.GetPartidaActual().JugadorActual.idJugador;
                    break;
                case TipoUnidad.Worker:
                    if (StageData.currentInstance.GetPartidaActual().JugadorActual.idJugador != StageData.ID_JUGADOR_HUMANO)
                    {
                        gameObjectUnidad = Instantiate(StageData.currentInstance.aldeanoIAprefab, destino.position, StageData.currentInstance.ExplorerPrefab.transform.rotation);
                    }
                    else
                    {
                        gameObjectUnidad = Instantiate(StageData.currentInstance.WorkerPrefab, destino.position, StageData.currentInstance.ExplorerPrefab.transform.rotation);
                    }
                    SetUnidadANodoYViceversa(gameObjectUnidad.GetComponent<Unidad>());
                    //StageData.currentInstance.GetPartidaActual().JugadorActual.Aldeanos++;
                    StageData.currentInstance.GetPartidaActual().Jugadores[m_Unidad.IdJugador].unidadesDisponibles.Add(gameObjectUnidad.GetComponent<Unidad>());
                    gameObjectUnidad.GetComponent<Unidad>().IdJugador = StageData.currentInstance.GetPartidaActual().JugadorActual.idJugador;
                    break;
                case TipoUnidad.Explorer:

                    if (StageData.currentInstance.GetPartidaActual().JugadorActual.idJugador != StageData.ID_JUGADOR_HUMANO)
                    {
                        gameObjectUnidad = Instantiate(StageData.currentInstance.explorerIAPrefab, destino.position, StageData.currentInstance.ExplorerPrefab.transform.rotation);
                    }
                    else
                    {
                        gameObjectUnidad = Instantiate(StageData.currentInstance.ExplorerPrefab, destino.position, StageData.currentInstance.ExplorerPrefab.transform.rotation);
                    }
                    StageData.currentInstance.GetPartidaActual().JugadorActual.Exploradores++;
                    SetUnidadANodoYViceversa(gameObjectUnidad.GetComponent<Unidad>());
                    StageData.currentInstance.GetPartidaActual().Jugadores[m_Unidad.IdJugador].unidadesDisponibles.Add(gameObjectUnidad.GetComponent<Unidad>());
                    gameObjectUnidad.GetComponent<Unidad>().IdJugador = StageData.currentInstance.GetPartidaActual().JugadorActual.idJugador;
                    break;
            }
            CancelarAccion();
            return true;
        }
        return false;
    }

    void SetUnidadANodoYViceversa(Unidad unidad)
    {
        Node nodoActual = StageData.currentInstance.GetNodeFromPosition(unidad.transform.position);
        unidad.Nodo = nodoActual;
        nodoActual.unidad = unidad;
    }

    public override void CancelarAccion()
    {
        m_Unidad.QuitarResaltoCasillasAlAlcance(NodosAlAlcance);
    }

    public override void EmpezarAccion()
    {
        VerNodosAlAlcance();
        m_Unidad.ResaltarCasillasAlAlcance(NodosAlAlcance);
    }

    public override List<Node> VerNodosAlAlcance()
    {
        NodosAlAlcance = Control.GetNodosAlAlcance(m_Unidad.Nodo, Alcance);
        for (int i = NodosAlAlcance.Count - 1; i >= 0; i--)
        {
            if (NodosAlAlcance[i].unidad != null ||
                NodosAlAlcance[i].resourceType != TipoRecurso.NullResourceType)
            {
                NodosAlAlcance.Remove(NodosAlAlcance[i]);
            }
        }
        return NodosAlAlcance;
    }    
}
