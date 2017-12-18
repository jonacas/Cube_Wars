using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JugadorIA : Jugador {

    ArbolMaestro arbol;

    //roles
    public RolExplorador rolExplo;
    public RolGuerrero rolGuerr;
    public RolLatente rolLat;
    public RolProtector rolPro;
    public RolRecolector rolReco;



    Ordenes reparto;

    public void CrearJugadorIA(int id, Unidad capi)
    {
        //print("CREACION  funcion crearJugadorIA");
        Crearjugador(id, capi);

        arbol = new ArbolMaestro(this);
        //print("CREACION  creadoArbolMaestro");

        rolExplo = this.GetComponent<RolExplorador>();
        rolGuerr = this.GetComponent<RolGuerrero>();
        rolLat = this.GetComponent<RolLatente>();
        rolPro = this.GetComponent<RolProtector>();
        rolReco = this.GetComponent<RolRecolector>();
    }

    public override void Turno()
    {
        puntosDeAccion = 100 + 20 * edificios.Count;
        print("TURNO DE " + idJugador);
        reparto = arbol.AsignarRecursos();

        print("ORDEN JUGADOR " + idJugador + ": Ataque-" + reparto.GetOrdenAtaque() + ": Defensa-" + reparto.GetOrdenDefensa() + ": Recoleccion-" + reparto.GetOrdenRecoleccion() + ": Exploracion-" + reparto.GetOrdenExploracion());

        StartCoroutine("EjecucionRoles");
    }

    IEnumerator EjecucionRoles()
    {
        int asignacion;

        //ejecutamos rol explorador
        asignacion = Mathf.RoundToInt(puntosDeAccion * reparto.GetOrdenExploracion());
        puntosDeAccion -= asignacion;

        if (!(asignacion < StageData.COSTE_PA_MOVER_UNIDAD))
        {
            rolExplo.fin = false;
            rolExplo.ComenzarTurno(asignacion);
            while (!rolExplo.fin)
                yield return null;

            yield return new WaitForSeconds(3);
        }


       asignacion = Mathf.RoundToInt(puntosDeAccion * reparto.GetOrdenRecoleccion());
        puntosDeAccion -= asignacion;

        if (!(asignacion < StageData.COSTE_PA_MOVER_UNIDAD))
        {
            rolReco.fin = false;
            rolReco.ComenzarTurno(ref asignacion);
            while (!rolReco.fin)
                yield return null;
        }

        turnoAcabado = true;

        yield return null;
    }
}
