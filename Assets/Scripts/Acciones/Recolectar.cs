using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recolectar : Accion {

    Unidad m_Unidad;

    private void Awake()
    {
        m_Unidad = GetComponent<Unidad>();
    }

    bool Ejecutar(StageData.ResourceType tipo, int cantidad)
    {
        Unidad unidadActual = GetComponent<Unidad>();
        Jugador jugador = Partida.GetPartidaActual().Jugadores[unidadActual.IdJugador];

        jugador.SumarRecursos(tipo, cantidad);

        return true;        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (AccionEmpezada &&
            StageData.currentInstance.LastClickedNode.resourceType != StageData.ResourceType.NullResourceType
            /*&& COMPROBAR QUE ESTÁ AL ALCANCE*/)
            {
                Ejecutar(StageData.currentInstance.LastClickedNode.resourceType, 50/*NI PUTA IDEA*/);
            }
        }
    }

    public override void CancelarAccion()
    {
        //codigo para des-resaltar las casillas del alcance
        AccionEmpezada = false;
    }

    public override void EmpezarAccion()
    {
        m_Unidad.ResaltarCasillasAlAlcance(Alcance);
        AccionEmpezada = true;
    }    
}
