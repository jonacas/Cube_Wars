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

    private const int RECOLECTOR = 0;
    private const int PROTECTOR = 1;
    private const int LATENTE = 2;
    private const int DESTRUCTOR = 3;

    /// <summary>
    /// Esta lista contiene los comportamientos en el orden en el que seran "comprobados"
    /// </summary>
    List<int> personalidad;
    Node[] mapaInfluencias;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="jugador">REFERENCIA al jugador que controla</param>
    public ArbolMaestro(ref Jugador jugador)
    {
        personalidad = new List<int> { 0, 1, 2, 3 };
    }

    /// <summary>
    /// Decide la cantidad de PA que le corresponden a cada rol en funcion de la situacion
    /// </summary>
    /// <param name="puntosAccion">PA de los que se dispone al INICIO del turno</param>
    /// <returns></returns>
    public int[] AsignarRecursos(int puntosAccion)
    {
        int puntosRestantes = puntosAccion;
        int[] resultado = new int[4];

        for(int i = 0; i < personalidad.Count; i++)
        {
            switch (personalidad[i])
            {
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
        return new int[1];
    }


    private int decidirRecursosRecoleccion(int puntosDisponibles)
    {
        if (puntosDisponibles <= 0)
            return 0;

        //se debe recorrrer la lista de unidades y comprobar que entrada de recursos se percibe
        //si hay carencias, se debe generar una orden con las prioiridades de acorde a estas

        return 0;
    }
}
