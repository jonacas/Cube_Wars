using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Esta clase contiene un arbol de decisiones dinamico que decidirá el comportamiento general de un jugador a lo largo de la partida.
/// </summary>
/// 

/*La idea principal es que el arbol decida entre cuatro roles posibles.
 * -Recolector: se centra en recoger recursos
 * -Protector: Construye defensas
 * -Latente: Prepara un ataque
 * -Destructor: Ataca
 * 
 * El oden "estandard" es:
 * - Empieza como recolector
 * - Cuando tiene recursos, construye defensas
 * - Cuando esta protegido, prepara ataque
 * - Cuando el ataque esta listo, ataca
 * 
 * Se usara un gradiente, que determinara la cantidad de recursos por turno que se dedicaran a cada rol. La importancia de cada
 * rol variara en cada turno en funcion de la situacion.*/

/*Se podrá alterar el orden de los tres ultimos roles para crear enemigos mas torpes y realistas
 *La ejecucion seguira una lista cpn los comportamientos de principio a fin. Se le dara mas importancia a los primeros, que solo seran sobrepasados
 *si se ha garantizado su cumplimiento (se recogen suficientes recursos o hay bastantes defensas).
 */

public class ArbolMaestro {

    private const int EXPLORADOR = 0;
    private const int RECOLECTOR = 1;
    private const int PROTECTOR = 2;
    private const int LATENTE = 3;
    private const int DESTRUCTOR = 4;

    //estas variables deciden a partir de cuantos puntos de accion un rol es viable

    //coste de over un explorador
    private const int MINIMO_VIABLE_EXPLORACION = 10;

    /// <summary>
    /// Esta lista contiene los comportamientos en el orden en el que seran "comprobados"
    /// </summary>
    List<int> personalidad;
    Node[] mapaInfluencias;
    Jugador jug;
    Partida partidaActual;



    //VARIABLES AUXILIARES PARA CONTROL DE ROLES
    int puntosAccionIniciales;
    //EXPLORACION
    int recursosConocidos;
    int capitalesConocidas;


    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="jugador">REFERENCIA al jugador que controla</param>
    public ArbolMaestro(ref Jugador jugador)
    {
        List<int> aux = new List<int> { 2, 3, 4 };
        personalidad = new List<int> { 0, 1};
        int seleccion;
        partidaActual = StageData.currentInstance.GetPartidaActual();

        //se genera el comportamiento del jugador
        //explorador y recolector siempre van delante porque son necesario para que el personaje pueda realizar el resto dse tareas
        while (aux.Count > 0)
        {
            seleccion = Random.Range(0, aux.Count);
            personalidad.Add(aux[seleccion]);
            aux.Remove(seleccion);
        }

        jug = jugador;
    }

    /// <summary>
    /// Decide la cantidad de PA que le corresponden a cada rol en funcion de la situacion
    /// </summary>
    /// <param name="puntosAccion">PA de los que se dispone al INICIO del turno</param>
    /// <returns></returns>
    public Ordenes AsignarRecursos()
    {
        puntosAccionIniciales = jug.PuntosDeAccion;
        int puntosRestantes = jug.PuntosDeAccion;
        float explo, reco, prote, late, atac;

        for(int i = 0; i < personalidad.Count; i++)
        {
            switch (personalidad[i])
            {
                case EXPLORADOR:
                    explo = decidirRecursosExploracion(ref puntosRestantes);
                    break;
                case RECOLECTOR:
                   // resultado[i] = 
                break;

                case PROTECTOR:
                break;

                case LATENTE:
                break;

                case DESTRUCTOR:
                break;
            }

        }
        return new Ordenes(0,0,0,0,0);
    }


    private float decidirRecursosExploracion(ref int puntosDisponibles)
    {
        if (puntosDisponibles <= MINIMO_VIABLE_EXPLORACION)
            return 0;

        int asignacion = 0;
        //este rol se ocupara de explorar
        //su prioridad se basa en el numero de recursos que conoce y la localizacion de las capitales enemigas
        //si las conoce, movera poco y no empleara todos los recursos

        //si no hay exploradores, creamos uno
        if (jug.Exploradores <= 0 && CrearUnidad.COSTE_PA_CREACION_EXPLORADOR <= puntosDisponibles)
        {
            if (partidaActual.GetTurnos() <= 2)
            {
                puntosDisponibles -= CrearUnidad.COSTE_PA_CREACION_EXPLORADOR * 3;
                asignacion += CrearUnidad.COSTE_PA_CREACION_EXPLORADOR * 3;
            }
            puntosDisponibles -= CrearUnidad.COSTE_PA_CREACION_EXPLORADOR;
            asignacion += CrearUnidad.COSTE_PA_CREACION_EXPLORADOR;
        }

        if (recursosConocidos >= 5 && capitalesConocidas == partidaActual.numJugadores)
        {
            puntosDisponibles -= MoverUnidad.COSTE_MOVER_EXPLORADOR * ((5 - recursosConocidos) + (partidaActual.numJugadores - capitalesConocidas));
            asignacion += MoverUnidad.COSTE_MOVER_EXPLORADOR * ((5 - recursosConocidos) + (partidaActual.numJugadores - capitalesConocidas));
        }

        //ajuste de la asignacion, la exploracion no debe superar el 50% despues del segundo turno
        float definitivo = asignacion / (float)puntosAccionIniciales;
        if (partidaActual.GetTurnos() < 2)
        {
            if (definitivo > 1)
                definitivo = 1f;
        }
        else
        {
            if (definitivo > 0.5f)
                definitivo = 0.5f;
        }
        return definitivo;
    }

    private int decidirRecursosRecoleccion(int puntosDisponibles)
    {
        if (puntosDisponibles <= MINIMO_VIABLE_EXPLORACION)
            return 0;

        //se debe recorrrer la lista de unidades y comprobar que entrada de recursos se percibe
        //si hay carencias, se debe generar una orden con las prioiridades de acorde a estas

        return 0;
    }
}
