using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrearUnidad : Accion
{

    #region COSTES ALDEANO
    public const int COSTE_PA_CREACION_ALDEANO = 10;
    public const int COSTE_MADERA_CREACION_ALDEANO = 0;
    public const int COSTE_COMIDA_CREACION_ALDEANO = 20;
    public const int COSTE_METAL_CREACION_ALDEANO = 0;
    public const int COSTE_PIEDRA_CREACION_ALDEANO = 0;
    #endregion

    #region COSTES EXPLORADOR
    public const int COSTE_PA_CREACION_EXPLORADOR = 10;
    public const int COSTE_MADERA_CREACION_EXPLORADOR = 0;
    public const int COSTE_COMIDA_CREACION_EXPLORADOR = 10;
    public const int COSTE_METAL_CREACION_EXPLORADOR = 0;
    public const int COSTE_PIEDRA_CREACION_EXPLORADOR = 0;
    #endregion

    #region COSTES GUERRERO
    public const int COSTE_PA_CREACION_GUERRERO = 20;
    public const int COSTE_MADERA_CREACION_GUERRERO = 0;
    public const int COSTE_COMIDA_CREACION_GUERRERO = 40;
    public const int COSTE_METAL_CREACION_GUERRERO = 10;
    public const int COSTE_PIEDRA_CREACION_GUERRERO = 0;
    #endregion

    //dado que solo pueden crear unidades los edificios, el alcance es siempre el mismo
    List<Node> alcance;


    private void Awake()
    {
        m_Unidad = GetComponent<Unidad>();
        switch (m_Unidad.IdUnidad)
        {
            case TipoUnidad.Building:
                break;
        }
        idAccion = AccionID.create;
    }


    /// <summary>
    /// Crea una unidad y hace que aparezca en una posicion al azar alrededor del edificio
    /// </summary>
    /// <param name="tipo">Tipo de la unidad que va a crearse</param>
    /// <returns>Devuelve true si se ha podido realizar, false si no</returns>
    public bool Ejecutar(TipoUnidad tipo)
    {
        //se debe coger el jugador de la instancia de partida del stageData
        //Jugador jug = SGetComponent<Unidad>().IdJugador
        return false;
    }

    public override void CancelarAccion()
    {
        //throw new System.NotImplementedException();
    }

    public override void EmpezarAccion()
    {

    }

    public override void SeleccionarResaltoDeCasilla()
    {

    }
}
