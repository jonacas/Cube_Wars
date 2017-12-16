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
        Crearjugador(id, capi);

        arbol = new ArbolMaestro(this);

        rolExplo = this.GetComponent<RolExplorador>();
        rolGuerr = this.GetComponent<RolGuerrero>();
        rolLat = this.GetComponent<RolLatente>();
        rolPro = this.GetComponent<RolProtector>();
        rolReco = this.GetComponent<RolRecolector>();
    }

    public override void Turno()
    {
    }
}
