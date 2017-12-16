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

    //variables para controlar ataques
    int objetivoActual;
    float debilidadObjetivo;
    bool ataqueEnCurso;



    //VARIABLES AUXILIARES PARA CONTROL DE ROLES
    int puntosAccionIniciales;
    //EXPLORACION
    int recursosConocidos;
    int capitalesConocidas;


    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="jugador">REFERENCIA al jugador que controla</param>
    public ArbolMaestro(Jugador jugador)
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
            aux.Remove(aux[seleccion]);
        }

        jug = jugador;
        recursosConocidos = 0;
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
        explo = reco = prote = late = atac = 0;

        for(int i = 0; i < personalidad.Count; i++)
        {
            switch (personalidad[i])
            {
                case EXPLORADOR:
                    explo = decidirRecursosExploracion(ref puntosRestantes);
                    break;

                case RECOLECTOR:
                    reco = decidirRecursosRecoleccion(ref puntosRestantes);
                break;

                case PROTECTOR:
                prote = decidirRecursosProteccion(ref puntosRestantes);
                break;

                case LATENTE:
                late = decidirRecursosLatente(ref puntosRestantes);
                break;

                case DESTRUCTOR:
                    //si es el ultimo, recibira los puntos restantes, si no, el 80%
                if (i == personalidad.Count)
                    atac = puntosRestantes / puntosAccionIniciales;

                else
                {
                    atac = 0.8f * puntosRestantes / puntosAccionIniciales;
                    puntosRestantes = Mathf.RoundToInt( 0.2f * puntosRestantes);
                }
                break;
            }

        }
        return new Ordenes(explo,reco,prote,late, atac);
    }

    private float decidirRecursosLatente(ref int puntosRestantes)
    {
        //se decide un objetivo (el jugador mas debil) y se prepara el ataque

        elegirObjetivo();

        //el aaque se preparara emplenado todos los recursos disponibles mientras la fuerza del jugador sea inferior a la del oponente
        int asignacion;
        if (debilidadObjetivo > 1)
        {
            asignacion = puntosRestantes;
            puntosRestantes -= asignacion;
        }
        else
        {
            asignacion = Mathf.RoundToInt (0.5f * puntosRestantes * debilidadObjetivo);
            puntosRestantes -= asignacion;
        }
        return asignacion / puntosAccionIniciales;
    }


    private float decidirRecursosProteccion(ref int puntosRestantes)
    {
        //se calcula la influencia media del jugador alrededor de sus edificios
        //si no supera una media, se dedican recursos
		if (puntosRestantes < StageData.COSTE_PA_CONSTRUIR_TORRE)
            return 0f;

        int suma;
        int asignacion =0 ;
        foreach (Unidad un in jug.edificios)
        {
            List<Node> cercanias;
            cercanias = Control.GetNodosAlAlcance(un.Nodo, 5);
            suma = 0;
            foreach (Node n in cercanias)
            {
				suma += n.GetPlayerInfluence(jug.idJugador);
            }

            float media = suma / cercanias.Count;

            //si la media no llega al minimo, se proporcionan recursos para construir una torre
            if (media < 2.5f)
            {
				if (puntosRestantes >= StageData.COSTE_PA_CONSTRUIR_TORRE)
                {
					puntosRestantes -= StageData.COSTE_PA_CONSTRUIR_TORRE;
					asignacion += StageData.COSTE_PA_CONSTRUIR_TORRE;
                }
                else
                {
                    asignacion += puntosRestantes;
                    puntosRestantes -= asignacion;
                }
            }
        }

        return (float)asignacion / (float)puntosAccionIniciales;
    }


    private float decidirRecursosExploracion(ref int puntosDisponibles)
    {
        if (puntosDisponibles <= MINIMO_VIABLE_EXPLORACION)
            return 0f;

        int asignacion = 0;
        //este rol se ocupara de explorar
        //su prioridad se basa en el numero de recursos que conoce y la localizacion de las capitales enemigas
        //si las conoce, movera poco y no empleara todos los recursos

        //si no hay exploradores, creamos uno
        if (jug.Exploradores <= 0 && GlobalData.COSTE_PA_CREACION_EXPLORADOR <= puntosDisponibles)
        {
            if (partidaActual.GetTurnos() <= 2)
            {
                puntosDisponibles -= GlobalData.COSTE_PA_CREACION_EXPLORADOR * 3;
                asignacion += GlobalData.COSTE_PA_CREACION_EXPLORADOR * 3;
            }
            puntosDisponibles -= GlobalData.COSTE_PA_CREACION_EXPLORADOR;
            asignacion += GlobalData.COSTE_PA_CREACION_EXPLORADOR;
        }

        if (recursosConocidos <= 5)
        {
            puntosDisponibles -= StageData.COSTE_PA_MOVER_UNIDAD * (5 - recursosConocidos);
            asignacion += StageData.COSTE_PA_MOVER_UNIDAD * (5 - recursosConocidos);
        }

        //ajuste de la asignacion, la exploracion no debe superar el 50% despues del segundo turno
        float definitivo = asignacion;// / (float)puntosAccionIniciales;
       /* if (partidaActual.GetTurnos() < 2)
        {
            if (definitivo > 1)
                definitivo = 1f;
        }
        else
        {
            if (definitivo > 0.5f)
                definitivo = 0.5f;
        }*/
        return definitivo;
    }

    private float decidirRecursosRecoleccion(ref int puntosDisponibles)
    {
        //la recoleccion se mantendra al maximo hasta que se tengan tres recursos bajo control del jugador
		if (puntosDisponibles <= StageData.COSTE_PA_CONSTRUIR_RECURSOS)
            return 0f;

        int asignacion = 0;
        int aux;
        //si hay menos de tres edificios y son los primeros turnos, se asignan todos los puntos
        if (jug.EdificiosRecoleccion < 3 && partidaActual.GetTurnos() < 6)
        {
            asignacion = puntosDisponibles;
            puntosDisponibles -= asignacion;
        }


        //si la partida esta mas avanzada, se ajustaran los recursos
        else if (jug.EdificiosRecoleccion < 3)
        {
            aux = 3 - jug.EdificiosRecoleccion;
            while (aux > 0)
            {
				puntosDisponibles -= StageData.COSTE_PA_CONSTRUIR_RECURSOS;
                if (puntosDisponibles < 0)
                {
					puntosDisponibles += StageData.COSTE_PA_CONSTRUIR_RECURSOS;
                    aux = -1;
                }
                else
                {
					asignacion += StageData.COSTE_PA_CONSTRUIR_RECURSOS;
                }
            }
        }

        float definitivo = asignacion / puntosAccionIniciales;
        if (definitivo > 1)
            definitivo = 1f;
        return definitivo;
        //se debe recorrrer la lista de unidades y comprobar que entrada de recursos se percibe
        //si hay carencias, se debe generar una orden con las prioiridades de acorde a estas

        return 0;
    }
    
    private void elegirObjetivo()
    {
        int[] influenciasJugadores = new int[partidaActual.numJugadores];
        int[] guerrerosEnemigo = new int[partidaActual.numJugadores];
        for (int k = 0; k < partidaActual.numJugadores; k++)
        {
            influenciasJugadores[k] = 0;
            guerrerosEnemigo[k] = 0;
        }

        for (int i = 0; i < StageData.currentInstance.CG.filas; i++)
        {
            for (int j = 0; j < StageData.currentInstance.CG.columnas; j++)
            {
                for (int k = 0; k < partidaActual.numJugadores; k++)
                {//se suma la influencia en el nodo del jugador
                    if(jug.influencias[i, j].GetPlayerInfluence(k) != -1)
                        influenciasJugadores[k] += jug.influencias[i, j].GetPlayerInfluence(k);
                }
                //acceder a unidad y si es un guerrero sumarlo al array
            }
        }

        //calculamos cual es el mas debil
        //debilidad = (unidadesEnemigas * 2 / influenciaEnem) * (influenciaEnem / influenciaJug)
        //               esta division calcula la diferencia de fuerzas                                    esta division calcula lo dispersas que estan sus fuerzas
        float debilidad = float.PositiveInfinity;
        float aux;
        int seleccion = 0;
        for (int k = 0; k < partidaActual.numJugadores; k++)
        {
            if (k == jug.idJugador)
                continue;

            aux = (guerrerosEnemigo[k] * 2 / (guerrerosEnemigo[jug.idJugador] + 1)) / (((float)influenciasJugadores[k] / (float)influenciasJugadores[jug.idJugador]));
            if (debilidad > aux)
            {
                debilidad = aux;
                seleccion = k;
            }
        }

        objetivoActual = seleccion;
        debilidadObjetivo = debilidad;
    }

}
