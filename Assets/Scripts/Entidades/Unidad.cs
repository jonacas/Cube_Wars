using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unidad : MonoBehaviour {

    public GameObject unitPanelPrefab;
    public UnitNamePanel UNP;

	public GameObject CasillaMarcada;

    GameObject piscina;

	List<GameObject> casillasMarcadas;

    Node nodo; //el nodo sobre el que se situa la unidad;
    public Node Nodo
    {
        get { return nodo; }
        set { nodo = value; }
    }
    
    private void Start()
    {
        //piscina = new GameObject("Piscina");
        if (IngameInterfaceManager.currentInstance != null)
        {
            GameObject panelCreated = Instantiate(unitPanelPrefab, IngameInterfaceManager.currentInstance.unitPanels_Parent) as GameObject;
            panelCreated.GetComponent<UnitNamePanel>().unitReferenced = this;
        }
    }

    protected int saludMaxima;
    public int SaludMaxima
    {
        get { return saludMaxima; }
    }

    protected int danyo;
    public int Danyo
    {
        get { return danyo; }
    }

    public int DanyoContraataque
    {
        get { return danyo / 5; }
    }

    int vida;
    public int Vida
    {
        get { return vida; }
        set { vida = value; }
    }


    protected float defensaMaxima;

    public List<Vector3> caminoActual;
    public bool caminoListo;

    internal void ResultadoAEstrella(List<Vector3> aux)
    {
		print ("Camino listo");
        caminoActual = aux;
        caminoListo = true;
    }

    public float DefensaMaxima
    {
        get { return defensaMaxima; }
    }

    float defensa;
    public float Defensa
    {
        get { return defensa; }
        set { defensa = value; }
    }

    /*FALTAN ATRIBUTOS INFLUENCIAS*/


    private int idJugador = -1;

    public int IdJugador
    {
        get { return idJugador; }
        set
        {
            if (idJugador == -1)
                idJugador = value;
            else
                Debug.LogError("Se ha intentado modificar un id ya asignado: " + idJugador + " por " + value);
        }
    }

    //identifica el tipo de unidad para el casting
    protected TipoUnidad idUnidad;
    public TipoUnidad IdUnidad
    {
		get{return idUnidad; }
    }

    protected int vision;
    public int Vision
    {
		get{return 0; }
    }
    /*Atributos de habilidad pasiva*/
    /*Esta accion se ejecuta en cada turno sin coste*/    
    protected Accion accionPasiva;

    /*Lista de acciones que se pueden realizar en los turnos*/
    protected List<Accion> acciones;
    public List<Accion> Acciones
    {
		get{ return acciones;}
    }
    //abstract public void llenarListaAccione(); 

    /*public Unidad(Vector3 pos, int saludMax, float defensaMax, int idJugador)
    {
        posicion = pos;
        vida = saludMax;
        saludMaxima = saludMax;
        this.defensa = defensaMax;
        defensaMaxima = defensaMax;
        this.idJugador = idJugador;
    }*/

    public virtual bool RecibirAtaque(int danoBruto)
    {
        int dano = danoBruto /*- (int) (danoBruto * defensa / 100)*/;
        vida -= dano;
        if (vida <= 0)
        {
            vida = 0;
            if (this.IdUnidad == TipoUnidad.Worker) {
                StageData.currentInstance.GetPartidaActual().Jugadores[idJugador].Aldeanos -= 1;
            }
            else if(this.IdUnidad == TipoUnidad.Warrior) {
                StageData.currentInstance.GetPartidaActual().Jugadores[idJugador].Guerreros -= 1;
            }

            else if (this.IdUnidad == TipoUnidad.Explorer)
            {
                StageData.currentInstance.GetPartidaActual().Jugadores[idJugador].Exploradores -= 1;
            }

            else if (this.IdUnidad == TipoUnidad.Building)
            {
                StageData.currentInstance.GetPartidaActual().Jugadores[idJugador].EdificiosRecoleccion -= 1;
            }

            else if (this.IdUnidad == TipoUnidad.DefensiveBuilding)
            {
                StageData.currentInstance.GetPartidaActual().Jugadores[idJugador].TorresDefensa -= 1;
            }
            return true;
        }
        else
            return false;
    }


    public void ResaltarCasillasAlAlcance(List<Node> alcance)
    {
        //este codigo debe resaltar las casillas del tablero que entran dentro del rango de una de las acciones
        piscina = new GameObject("Piscina");
        casillasMarcadas = new List<GameObject>();
		foreach (Node n in alcance) {
			GameObject newCasillaMarcada = Instantiate (CasillaMarcada, new Vector3 (n.position.x, CasillaMarcada.transform.position.y, n.position.z), CasillaMarcada.transform.rotation, piscina.transform);
			casillasMarcadas.Add (newCasillaMarcada);
		}

    }

    public void QuitarResaltoCasillasAlAlcance(List<Node> alcance)
    {
        Destroy(piscina);

        if(casillasMarcadas != null)
		    casillasMarcadas.Clear();
    }



    /// <summary>
    /// Solicita un camino y lo recorre cuando esta listo desde su posicion hasta la que se le proporciona
    /// </summary>
    /// <param name="final"></param>
    public virtual void SolicitarYRecorrerCamino(Vector3 final)
    {
        caminoListo = false;
        StageData.currentInstance.GetPathToTarget(this.transform.position, final, this);
    }
}