using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RolLatente : MonoBehaviour {

    const int CREAR_UNIDAD_INDEX = 0;

    
	int numeroCreaciones;

    public bool fin;
    

	public bool ComenzarTurno(ref int puntosAsignados)
	{
        fin = false;
		numeroCreaciones = puntosAsignados / StageData.COSTE_PA_CREAR_GUERRERO; // CALCULO CUANTAS CREACIONES PUEDO HACER CON LOS PUNTOS ASIGNADOS
		if (numeroCreaciones > 0) {
			StartCoroutine("CrearGuerreros");
			return true;
		}
		else
			return false;
	}

	IEnumerator CrearGuerreros()
	{
        List<Unidad> edificiosCreadores = GetCreadorDeUnidadesAdecuado(); // MIRO TODOS LOS EDIFICIOS QUE PUEDEN CONSTRUIR UNIDADES Y LOS ORDENO POR PRIORIDAD
		int edificioActual = 0; // SE UTILIZA PARA SABER CUAL ES EL EDIFICIO QUE VA A CONSTRUIR, PARA CONTROLAR QUE SI UN EDIFICIO NO PUEDE CREAR MAS, PASE AL SIGUIENTE
		while(numeroCreaciones > 0 && edificioActual < edificiosCreadores.Count)
		{
			CrearUnidad accionCreadorUnidades = (CrearUnidad) edificiosCreadores [edificioActual].Acciones [CREAR_UNIDAD_INDEX];
			List<Node> nodosAlAlcance = accionCreadorUnidades.VerNodosAlAlcance (); // COJO LOS NODOS AL ALCANCE ACTUALIZADO DEL EDIFICIO DE CREACION
			if(nodosAlAlcance.Count == 0){ // SI EN ESTE EDIFICIO NO SE PUEDE CREAR MAS GUERREROS, PASO AL SIGUIENTE
				edificioActual++;
				if (edificioActual >= edificiosCreadores.Count) // SI HE RECORRIDO TODOS LOS EDIFICIOS, SIGNIFICA QUE ME QUEDAN PUNTOS POR GASTAR PERO NO ME CABE EN NINGUN EDIFICIO MAS GUERREROS
					print ("NO SE PUEDEN CREAR MAS UNIDADES  PORQUE NO CABEN MAS UNIDADES");				
			}
			else if(edificioActual < edificiosCreadores.Count) // EN CASO DE QUE TODO VAYA BIEN, LO CREO
			{
				bool haFuncionado = accionCreadorUnidades.Ejecutar (nodosAlAlcance [0], TipoUnidad.Warrior);
                if (haFuncionado)
                {
                    numeroCreaciones--;
                    //print(numeroCreaciones);
                    yield return new WaitForSeconds(0.5f); // DELAY QUE SE TARDA ENTRE CREAR UNA UNIDAD Y OTRA
                }
			}            
        }
        fin = true;
	}

	List<Unidad> GetCreadorDeUnidadesAdecuado()
	{
		List<Unidad> edificiosCreadoresDeunidades = StageData.currentInstance.GetPartidaActual().JugadorActual.edificios; // COJO TODOS LOS EDIFICIOS
        List<Unidad> edificiosAuxiliares = new List<Unidad>();
        for (int i = edificiosCreadoresDeunidades.Count - 1; i >= 0; i--) {
			if (edificiosCreadoresDeunidades [i].IdUnidad == TipoUnidad.Resource) // DISCRIMINO TODOS LOS QUE NO CREAN UNIDAD
                edificiosAuxiliares.Add (edificiosCreadoresDeunidades [i]);
		}
		edificiosAuxiliares.Add (StageData.currentInstance.GetPartidaActual().JugadorActual.Capital); // LA CAPITAL TAMBIEN PUEDE CREARLOS
		List<Unidad> edificiosOrdenadosPorPrioridad = new List<Unidad>(); // LA LISTA EN LA QUE VOY A METER LOS EDIFICIOS ORDENADOS POR PRIORIDAD
		Unidad objetivo = StageData.currentInstance.GetPartidaActual().Jugadores[StageData.currentInstance.GetPartidaActual().JugadorActual.IndexPlayerObjetivoActual].Capital; // MIRO CUAL ES EL OBJETIVO
		Unidad aux;
		while(edificiosAuxiliares.Count > 0) // ORDENACION
		{
			aux = edificiosAuxiliares[0];
			foreach(Unidad u in edificiosAuxiliares)  // LOS ORDENO POR DISTANCIA. CUANTA MENOR ES LA DISTANCIA, MAYOR ES LA PRIORIDAD
			{
				if (Vector3.Distance (u.transform.position, objetivo.transform.position) < Vector3.Distance (aux.transform.position, objetivo.transform.position))
					aux = u;
			}
			edificiosOrdenadosPorPrioridad.Add (aux);
            edificiosAuxiliares.Remove (aux);
		}

		return edificiosOrdenadosPorPrioridad;
	}

}
