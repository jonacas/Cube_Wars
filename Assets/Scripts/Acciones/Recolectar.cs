using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recolectar : Accion {

    public const int COSTE_RECOLECTAR = 100;

    public List<Node> NodosAlAlcance;

    private void Awake()
    {
        m_Unidad = GetComponent<Unidad>();
    }

	public bool Ejecutar()
    {
		Jugador jugador = Partida.GetPartidaActual().Jugadores[m_Unidad.IdJugador];
		jugador.SumarRecursos(n.resourceType, /*n.cantidadRecolectada*/);
		return true;        
    }

	public override void CancelarAccion()
	{
		
	}


	public override void EmpezarAccion()
	{
		
	}

	public override void SeleccionarResaltoDeCasilla()
	{
		
	}
}
