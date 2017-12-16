using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capital : Unidad {

    private const int SALUD_MAX = 500;
    private const int DEFENSA_MAX = 100;
    private const int COMIDA_POR_TURNO = 20;

    int nivel;
    bool posicionAsignada,primerUpdate;

    private void Awake()
    {
        saludMaxima = SALUD_MAX;
        Vida = SALUD_MAX;
        acciones = new List<Accion>();
        acciones.Add(GetComponent<CrearUnidad>());
    }



    void Update()
    {
        if (!primerUpdate)
        {
            this.Nodo = StageData.currentInstance.GetNodeFromPosition(this.transform.position);
            Nodo.unidad = this;
            primerUpdate = true;
        }

       /* if (Input.GetKeyDown(KeyCode.Space))
        {
            List<Node> destinos = Control.GetNodosAlAlcance(this.Nodo, 2);
            CrearUnidad cr = (CrearUnidad)acciones[0];

            for (int i = 0; i < destinos.Count; i++)
            {
                if(destinos[i].unidad == null && destinos[i].resourceType == TipoRecurso.NullResourceType)
                {
                    cr.Ejecutar(destinos[i], TipoUnidad.Warrior);
                    break;
                }
            }
        }*/
    }

    public override bool RecibirAtaque(int danoBruto)
    {
        bool destruido = base.RecibirAtaque(danoBruto);

        //codigo para informar de destruccion de capital
        if (destruido)
            StageData.currentInstance.GetPartidaActual().DescativarJugadorYComprobarVictoria(this.IdJugador);
        return destruido;
    }



    public void llenarListaAcciones()
    {
        throw new System.NotImplementedException();
    }
}
