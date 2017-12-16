using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JugadorIA : Jugador {

    ArbolMaestro arbol;

    //roles
    RolExplorador rolExplo;
    RolGuerrero rolGuerr;
    RolLatente rolLat;
    RolProtector rolPro;
    RolRecolector rolReco;

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
        print("TURNO DE " + idJugador);
        Ordenes reparto = arbol.AsignarRecursos();
        turnoAcabado = true;

        print("ORDEN JUGADOR " + idJugador + ": Ataque-" + reparto.GetOrdenAtaque() + ": Defensa-" + reparto.GetOrdenDefensa() + ": Recoleccion-" + reparto.GetOrdenRecoleccion() + ": Exploracion-" + reparto.GetOrdenExploracion());

    }
}
