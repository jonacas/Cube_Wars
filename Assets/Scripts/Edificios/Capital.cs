using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capital : Unidad {
       
    bool posicionAsignada, primerUpdate;

    private void Awake()
    {
        saludMaxima = StageData.SALUD_MAX_CAPITAL;
        Vida = StageData.SALUD_MAX_CAPITAL;
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
            StageData.currentInstance.SetInfluenceToNode(5, this.Nodo, this.IdJugador);
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
