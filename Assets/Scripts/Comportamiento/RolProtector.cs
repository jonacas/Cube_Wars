using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RolProtector : MonoBehaviour {

    const float UMBRAL_CREACION_TORRES = 5.5f;

    List<Vector3> caminoANodoDestino;
    public bool fin;
    Partida partidaActual;
    int numeroCreaciones;
    int puntosDispo;
    bool numeroAldeanosMINIMOS;

    void Start()
    {
        partidaActual = StageData.currentInstance.GetPartidaActual();
        caminoANodoDestino = new List<Vector3>();
    }

    public bool ComenzarTurno(ref int puntosAsig)
    {
        partidaActual = StageData.currentInstance.GetPartidaActual();
        // Buscamos ahora a los aldeanos disponibles.
        int totalAldeanos = 0;
        puntosDispo = puntosAsig;
        foreach (Unidad un in StageData.currentInstance.GetPartidaActual().JugadorActual.unidadesDisponibles)
        {
            if (un.IdUnidad == TipoUnidad.Worker)
                totalAldeanos++;
        }

        if (totalAldeanos < 3)
            numeroAldeanosMINIMOS = true;
        else
            numeroAldeanosMINIMOS = false;
          
        print("COMIENZA ROL PROTECTOR");

        if (numeroAldeanosMINIMOS)
        {
            numeroCreaciones = puntosAsig / StageData.COSTE_PA_CREAR_ALDEANO;

            if (numeroCreaciones > 3)
                numeroCreaciones = 3;

            if (numeroCreaciones > 0)
            {
                StartCoroutine("CrearAldeano");
                return true;
            }
            else
            {
                return false;
                fin = true;
            }
            //Empieza la corutina de crear los aldeanos.
        }
        else
        {
            int movimientosDisponibles = puntosDispo / StageData.COSTE_PA_MOVER_UNIDAD;

            if (movimientosDisponibles > 0)
            {
                StartCoroutine("PrepararOrdenesAldeanos");
                return true;
            }
            else
            {
                return false;
                fin = true;
            }

        }

        //Comprobamos si necesitamos recursos mínimos, para cumplir necesidades mínimas.
    }

    IEnumerator PrepararOrdenesAldeanos()
    {
        print("PrepararOrdenesAldeanos");
        ///asignamos a cada aldeano un recurso que deb ir a explotar
		List<Unidad> aldeanos = new List<Unidad>();
        List<Unidad> unidades = StageData.currentInstance.GetPartidaActual().JugadorActual.unidadesDisponibles;
        

        //se obtienen los aldeanos disponibles
        for (int i = unidades.Count - 1; i >= 0; i--)
        {
            if (unidades[i].IdUnidad == TipoUnidad.Worker)
            {
                print("borr");
                aldeanos.Add(unidades[i]);
            }
        }

        //se obtienen los edificios que se deben proteger
        List<Unidad> unidadesParaEdificios = StageData.currentInstance.GetPartidaActual().JugadorActual.edificios;
        List<Unidad> edificios = new List<Unidad>();
        //int torres = 0;
        foreach (Unidad u in unidadesParaEdificios)
        {
            if (u.IdUnidad == TipoUnidad.Resource || u.IdUnidad == TipoUnidad.Capital)
            {
                edificios.Add(u);
            }

            /*if(u.IdUnidad == TipoUnidad.DefensiveBuilding)
            {
                torres++;
            }*/
        }


        /*if (torres > 5)
            StopCoroutine("PrepararOrdenesAldeanos");*/

        IA_Aldeano aldIA;
        int edificioActual = 0;
        foreach (Unidad al in aldeanos)
        {
            print("preparandoAldeano PROTEC");
            aldIA = (IA_Aldeano)al;
            print("set destino PROTEC");


            if (edificioActual >= unidadesParaEdificios.Count)
                break;

                List<Node> cercanias;
            cercanias = Control.GetNodosAlAlcance(edificios[edificioActual].Nodo, 5);
            int suma = 0;
            foreach (Node n in cercanias)
            {
                suma += n.GetPlayerInfluence(StageData.currentInstance.GetPartidaActual().JugadorActual.idJugador);
            }

            float media = suma / cercanias.Count;

            //si la media no llega al minimo, se envia un aldeano a una posicion cercana para que construya una torre
            if (media < UMBRAL_CREACION_TORRES)
            {
                cercanias = Control.GetNodosAlAlcance(edificios[edificioActual].Nodo, 3);
                aldIA.SetDestino(StageData.currentInstance.GetNodeFromPosition(cercanias[Random.Range(0, cercanias.Count - 1)].position));
                while (!aldIA.caminoListo)
                    yield return null;
                continue;
            }


            //si llega, se elimina el edificio y se sigue con el resto
            else
            {
                edificios.Remove(edificios[edificioActual]);
                continue;
            }
            edificioActual++;
            //enviamos un aldeano por edificio
        }
        yield return null;
        StartCoroutine("MoverAldeanos");
    }

    IEnumerator MoverAldeanos()
    {
        //AQUI MIRO SI TENGO ALDEANOS CERCA.
        yield return new WaitForSeconds(1);

        List<Unidad> aux1 = StageData.currentInstance.GetPartidaActual().JugadorActual.unidadesDisponibles;
        List<Unidad> aldeanos = new List<Unidad>();

        for (int i = aux1.Count - 1; i >= 0; i--)
        {
            if (aux1[i].IdUnidad == TipoUnidad.Worker)
            {
                aldeanos.Add(aux1[i]);
            }
        }

        if (aldeanos.Count != 0)
        {
            IA_Aldeano aldIA;
            foreach (Unidad un in aldeanos)
            {
                //print("Moviendo aldeanos recoleccion");
                aldIA = (IA_Aldeano)un;
                aldIA.AvanzarHaciaDestino(true);
            }
        }

        fin = true;
    }

        IEnumerator CrearAldeano()
    {
        print("CREANDO ALDEANOS");
        List<Unidad> edificiosCreadores = GetCreadoresDeTorres();
        int edificioActual = 0;
        int totalCreados = 0;
        while (numeroCreaciones > 0 && edificioActual < edificiosCreadores.Count)
        {
            CrearUnidad accionCreadorUnidades = (CrearUnidad)edificiosCreadores[edificioActual].Acciones[0];
            List<Node> nodosAlAlcance = accionCreadorUnidades.VerNodosAlAlcance();
            if (nodosAlAlcance.Count == 0)
            {
                edificioActual++;
                if (edificioActual >= edificiosCreadores.Count)
                {
                    print("No se pueden crear más en este edificio");
                }
            }
            else if (edificioActual < edificiosCreadores.Count)
            {
                bool haFuncionado = accionCreadorUnidades.Ejecutar(nodosAlAlcance[0], TipoUnidad.Worker);
                if (haFuncionado)
                {
                    totalCreados++;
                    numeroCreaciones--;
                    //print (numeroCreaciones);
                    yield return new WaitForSeconds(1);
                }
            }
        }

        StageData.currentInstance.GetPartidaActual().JugadorActual.Aldeanos += totalCreados;
        StartCoroutine("PrepararOrdenesAldeanos");

    }


    void CrearTorres() {

        List<Unidad> aldeanosCreadores = GetCreadoresDeTorres();
        int aldeanoActual = 0;
        Construir contructor = new Construir();
        while (numeroCreaciones > 0 && aldeanoActual < aldeanosCreadores.Count) { //Proceso para crear
            List<Node> NodosAlAlcance = OrdenarNodos(Control.GetNodosAlAlcance(StageData.currentInstance.GetNodeFromPosition(aldeanosCreadores[aldeanoActual].transform.position), 2));//Obtenemos los nodos en los que podemos crear
            foreach (Node n in NodosAlAlcance) { //RecorremosEsos nodos ahora que estan ordenados por distancia
                if (contructor.Ejecutar(n)) {//Si hemos conseguido crear pasamos al siguiente nodo
                    numeroCreaciones--;
                    break;

                }

            }
            aldeanoActual++;
            if (aldeanoActual >= aldeanosCreadores.Count) {
                print("NO SE PUEDEN CREAR MAS TORRES  PORQUE NO CABEN MAS UNIDADES");

            }
        }


    }

    List<Node> OrdenarNodos(List<Node> Nodos) {

        List<Node> NodosOrdenados = new List<Node>();
        Unidad objetivo = partidaActual.Jugadores[partidaActual.JugadorActual.idJugador].Capital;
        Node aux;
        while (Nodos.Count > 0) {
            aux = Nodos[0];
            foreach (Node n in Nodos)
            {
                if(Vector3.Distance(n.position,objetivo.transform.position) < Vector3.Distance(aux.position,objetivo.transform.position)){

                    aux = n;
                }
            }
            NodosOrdenados.Add(aux);
            Nodos.Remove(aux);
        }
        return NodosOrdenados;
    }

    List<Unidad> GetCreadoresDeTorres() {

        print("ATENCION, SE ESTAN BORRANDO UNIDADES DONDE NO SE DEBE");
        List<Unidad> creadoresDeTorres = partidaActual.JugadorActual.unidadesDisponibles; //COJO TODAS LAS UNIDADES
        for (int i = creadoresDeTorres.Count - 1; i >= 0; i--) {
            if (creadoresDeTorres[i].IdUnidad != TipoUnidad.Worker) {  //ME DESHAGO DE AQUELLAS QUE NO SEAN ALDEANOS
                creadoresDeTorres.Remove(creadoresDeTorres[i]);
            }
        }
        List<Unidad> creadoresOrdenados = new List<Unidad>();
        Unidad objetivo = partidaActual.Jugadores[partidaActual.JugadorActual.idJugador].Capital;
        Unidad aux;
        while (creadoresDeTorres.Count > 0) { //ORDENACION
            aux = creadoresDeTorres[0];
            foreach (Unidad u in creadoresDeTorres) {

                if (Vector3.Distance(u.transform.position, objetivo.transform.position) < Vector3.Distance(aux.transform.position, objetivo.transform.position)) {
                    aux = u;
                }
            }

            creadoresOrdenados.Add(aux);
            creadoresDeTorres.Remove(aux);

        }

        return creadoresOrdenados;
    }

}
