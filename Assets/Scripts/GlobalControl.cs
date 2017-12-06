using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalControl : MonoBehaviour {

    public static GlobalControl currentInstance;

    public Unidad unidadSeleccionada;
    public  GameObject botonAtck, botonMove, botonConstr, botonCrearUn;

    private HUDState estado;
    private IngameInterfaceManager managerInterfaz;
    private AccionID accionEnCurso;
    

    void Awake()
    {
        managerInterfaz = IngameInterfaceManager.currentInstance;

        estado = HUDState.nothingSelected;
        currentInstance = this;

       /* botonAtck = GameObject.Find("ButtonAttack");
        botonMove = GameObject.Find("ButtonMove");
        botonConstr = GameObject.Find("ButtonBuild");
        botonCrearUn = GameObject.Find("ButtonCreateUnit");*/

    }

    public void UnidadSeleccionada(Unidad un)
    {
        unidadSeleccionada = un;
        print("Seleccionado " + un.gameObject.name);
        marcarAccionesPosibles();
    }

    public void Deseleccionar(Unidad un)
    {
        if (unidadSeleccionada == un)
        {
            print("DeSeleccionado " + un.gameObject.name);
            unidadSeleccionada = null;
        }
    }


    private void marcarAccionesPosibles()
    {
        //desactivamos botones
      /*  botonAtck.SetActive(false);
        botonConstr.SetActive(false);
        botonCrearUn.SetActive(false);
        botonMove.SetActive(false);


        foreach (Accion acc in unidadSeleccionada.Acciones)
        {
            switch (acc.IDAccion)
            {
                case AccionID.attack:
                    botonAtck.SetActive(true);
                    break;
                case AccionID.create:
                    botonConstr.SetActive(true);
                    break;
                case AccionID.build:
                    botonCrearUn.SetActive(true);
                    break;
                case AccionID.move:
                    botonMove.SetActive(true);
                    break;
            }

        }*/

    }
    /// <summary>
    /// Comprueba si es posible realizar la accion seleccionada
    /// </summary>
    /// <param name="accion">Tipo de la accion que se comprueba</param>
    /// <returns>True si la accion puede ejecutarse, false si no</returns>
    public bool SeleccionarAccion(AccionID accion)
    {
        for(int i = 0; i < unidadSeleccionada.Acciones.Count; i++)
        {
            if (unidadSeleccionada.Acciones[i].IDAccion == accion)
            {
                accionEnCurso = accion;
                unidadSeleccionada.Acciones[i].EmpezarAccion();
                return true;
            }
        }
        return false;
    }


}
