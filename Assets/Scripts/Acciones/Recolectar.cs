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

    bool Ejecutar(TipoRecurso tipo, int cantidad)
    {
        Unidad unidadActual = GetComponent<Unidad>();
        Jugador jugador = Partida.GetPartidaActual().Jugadores[unidadActual.IdJugador];

        jugador.SumarRecursos(tipo, cantidad);

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
