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
    private int indiceAccionEncurso;
    

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
                print("acc");
                accionEnCurso = accion;
                indiceAccionEncurso = i;
                unidadSeleccionada.Acciones[i].EmpezarAccion();
                return true;
            }
        }
        return false;
    }

    void Update()
    {
        if (IngameInterfaceManager.currentInstance.currentHudState == HUDState.actionSelected)
        {
            if (Input.GetButtonDown("ClickIzq"))
            {
                Node nodo = LanzaRaycast();
                print("Click en: " + nodo.fil + "//" + nodo.col);
                IntentarEjecutarAccion(nodo);
            }


        }
    }


    private void IntentarEjecutarAccion(Node nodo)
    {
        Atacar atacarAux;
        MoverUnidad moverAux;
        CrearUnidad crearAux;
        Construir construirAux;

        switch (unidadSeleccionada.Acciones[indiceAccionEncurso].IDAccion)
        {
            case (AccionID.attack):
                atacarAux = (Atacar)unidadSeleccionada.Acciones[indiceAccionEncurso];
                atacarAux.Ejecutar(nodo);
                break;
            case (AccionID.build):
                construirAux = (Construir)unidadSeleccionada.Acciones[indiceAccionEncurso];
                break;
            case (AccionID.create):
                print("Intentando ejecutar crear unidad");
                crearAux = (CrearUnidad)unidadSeleccionada.Acciones[indiceAccionEncurso];
                crearAux.Ejecutar(nodo, TipoUnidad.Warrior);
                break;
            case (AccionID.move):
                print("Intentando ejecutar mover");
                moverAux = (MoverUnidad)unidadSeleccionada.Acciones[indiceAccionEncurso];
                moverAux.Ejecutar(nodo);
                break;
        }

    }

    private Node LanzaRaycast()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);

        if (hit.collider != null)
        {

            return  StageData.currentInstance.GetNodeFromPosition(hit.point);
        }
        else
            return null;
    }

}
