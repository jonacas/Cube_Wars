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

        SeleccionarResaltoDeCasilla();
        
        if (NodosAlAlcance.Contains(destino))
        {
            //Instantiate(StageData.currentInstance.WarriorPrefab, destino.position, StageData.currentInstance.WarriorPrefab.transform.rotation);
            switch (tipo)
            {
                case TipoUnidad.Warrior:
                    Instantiate(StageData.currentInstance.WarriorPrefab, destino.position, StageData.currentInstance.WarriorPrefab.transform.rotation);
                    break;
                case TipoUnidad.Worker:
                    Instantiate(StageData.currentInstance.WorkerPrefab, destino.position, StageData.currentInstance.WorkerPrefab.transform.rotation);
                    break;
                case TipoUnidad.Explorer:
                    Instantiate(StageData.currentInstance.ExplorerPrefab, destino.position, StageData.currentInstance.ExplorerPrefab.transform.rotation);
                    break;
            }
            CancelarAccion();
            return true;
        }
        return false;
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
        NodosAlAlcance = Control.GetNodosAlAlcance(m_Unidad.Nodo, Alcance);
        for (int i = NodosAlAlcance.Count - 1; i >= 0; i--)
        {
            if (NodosAlAlcance[i].unidad != null ||
                NodosAlAlcance[i].resourceType != TipoRecurso.NullResourceType)
            {
                NodosAlAlcance.Remove(NodosAlAlcance[i]);
            }
        }
    }

	public List<Node> GetNodosAlAlcance()
	{
		if (NodosAlAlcance.Count == 0) {
			SeleccionarResaltoDeCasilla ();
		}
		return NodosAlAlcance;
	}
}
