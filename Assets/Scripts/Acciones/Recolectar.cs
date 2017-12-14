using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recolectar : Accion {

    public List<Node> NodosAlAlcance;

    private void Awake()
    {
        m_Unidad = GetComponent<Unidad>();
        Alcance = 1;
        NodosAlAlcance = Control.GetNodosAlAlcance(m_Unidad.Nodo, Alcance);
    }

	bool Ejecutar(Node n)
    {
        Unidad unidadActual = GetComponent<Unidad>();
        Jugador jugador = Partida.GetPartidaActual().Jugadores[unidadActual.IdJugador];

		if(NodosAlAlcance.Contains(n))
			jugador.SumarRecursos(n.resourceType, /*n.cantidadRecolectada*/);
		return true;        
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
		print("SeleccionarResaltoCasilla" + Alcance);
		NodosAlAlcance = Control.GetNodosAlAlcance(m_Unidad.Nodo, Alcance);
		for (int i = NodosAlAlcance.Count - 1; i >= 0; i--)
		{
			if (NodosAlAlcance[i].unidad != null ||
				NodosAlAlcance[i].resourceType == TipoRecurso.NullResourceType)
			{
				print(NodosAlAlcance[i].unidad + "   " + NodosAlAlcance[i].resourceType + "  Eliminado");
				NodosAlAlcance.Remove(NodosAlAlcance[i]);
			}
		}

		m_Unidad.ResaltarCasillasAlAlcance(NodosAlAlcance);
	}
}
