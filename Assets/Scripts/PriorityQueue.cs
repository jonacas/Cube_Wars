#define DEBUG

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Priority_Queue
{
    /// <summary>
    /// COLA DE PRIORIDAD
    /// - Permite encolar, desencolar, mirar la cima y cambiar prioridad
    /// - Ordena de menor a mayor prioridad (menor prioridad = cima ).
    /// - Si dos nodos tienen igual prioridad, se obtienen en el orden inverso al que se introducen (LIFO).
    /// </summary>

    public class PriorityQueue
    {
        private int _numNodos;
        private Node[] _nodos;

        //Instaniar cola de prioridad
        //Debe proporcionarse el tamano maximo posible

        /// <summary>
        /// Creacion de la cola
        /// </summary>
        /// <param name="maxNodos"> Numero de _nodos que podra haber en cualquier momento en la cola</param>
        public PriorityQueue(int maxNodos)
        {
#if DEBUG
            if (maxNodos <= 0)
                throw new InvalidOperationException("El tamano de la cola de prioridad debe ser mayor que 0");
#endif
            _numNodos = 0;
            _nodos = new Node[maxNodos + 1];
        }


        /// <summary>
        /// Devuelve el numero de _nodos que podra haber en cualquier momento en la cola
        /// O(1)
        /// </summary>
        /// <returns>Numero de _nodos que podra haber en cualquier momento en la cola</returns>
        public int TamanoMax()
        {
            return _nodos.Length - 1;
        }


        public int NumElementos()
        {
            return _numNodos;
        }



        /// <summary>
        /// Vacia la cola de prioridad SIN eliminar los datos
        /// La cola se comportara como una nueva
        /// O(1)
        /// </summary>
        public void Vaciar()
        {
            _numNodos = 0;
        }

        /// <summary>
        /// Vacia la cola de prioridad ELIMINANDO LOS DATOS
        /// O(n). Usese con moderación.
        /// </summary>
        public void Reset()
        {
            Array.Clear(_nodos, 1, _numNodos);
            _numNodos = 0;
        }

        /// <summary>
        /// Comprueba que el nodo proporcionado esta en la cola.
        /// O(1)
        /// </summary>
        /// <param name="nodo">Nodo que debe buscarse</param>
        /// <returns>True si el nodo es encontrado, false si no</returns>
        public bool Contiene(Node nodo)
        {
#if DEBUG
            if (nodo == null)
                throw new ArgumentNullException("El nodo proporcionado al metodo Contiene() de la clase PriorityQueue es nulo");
            //Esta excepcion debe desactivarse si en vez de usar Vaciar() se usa Reset()
            /* if(nodo.indiceCola < 0 || nodo.indiceCola > _nodos.Length)
                 throw new InvalidOperationException("El nodo proporcionado al metodo esta corrupto. ¿Ha sido modificado externamente?");*/
#endif
            return _nodos[nodo.indiceCola] == nodo;
        }


        /// <summary>
        /// Encola el nodo proporcionado con la prioridad dada.
        /// O(log n)
        /// </summary>
        /// <param name="nodo">Nodo que va a encolarse</param>
        /// <param name="prioridad">Prioridad del nodo en la cola</param>
        public void Encolar(Node nodo, float prioridad)
        {
#if DEBUG
            if (nodo == null)
            {
                throw new ArgumentNullException("El nodo proporcionado a Encolar() es nulo");
            }

            if (_numNodos >= _nodos.Length - 1)
                throw new InvalidOperationException("La cola estaba llena y se ha intentado encolar otro nodo:" + nodo);
            //Esta excepcion debe desactivarse si en vez de usar Vaciar() se usa Reset()
            /*if(Contiene(nodo))
                throw new InvalidOperationException("El nodo que ha intentado encolarse ya estaba en la cola " + nodo);*/
#endif
            nodo.prioridad = prioridad;
            _numNodos++;
            _nodos[_numNodos] = nodo;
            nodo.indiceCola = _numNodos;
            cascadaArriba(nodo);
        }


        /*
         * Sube un nodo hasta que esta en la posicion que le corresponde segun su prioridad
         */
        private void cascadaArriba(Node nodo)
        {
            int padre;

            //comprobamos que el nodo no es el primero
            if (nodo.indiceCola > 1)
            {
                padre = nodo.indiceCola / 2;
                Node nodoPadre = _nodos[padre];
                if (tienePirioridadMayorOIgual(nodoPadre, nodo))
                {
                    //se cambia la posicion del padre
                    _nodos[nodo.indiceCola] =nodoPadre;
                    //cambiamos posicion del padre
                    nodoPadre.indiceCola = nodo.indiceCola;
                    //actualizamos indice del nodo
                    nodo.indiceCola = padre;

                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }

                while (padre > 1) //mientras pueda seguir subiendo
                {
                    //scamos a su nuevo padre
                    padre = nodo.indiceCola / 2;
                    Node nodoPadre = _nodos[padre];

                    if (tienePirioridadMayorOIgual(nodoPadre, nodo))
                    {
                        //se cambia la posicion del padre
                        _nodos[nodo.indiceCola] = nodoPadre;
                        //se actualiza el indice
                        nodoPadre.indiceCola = nodo.indiceCola;
                        //actualizamos indice del nodo
                        nodo.indiceCola = padre;
                    }
                    else
                        break;
                }
                //ponemos al nodo en la posicion que le corresponde
                _nodos[nodo.indiceCola] = nodo;
            }


        /*
         * Baja un nodo hasta que esta en la posicion que le corresponde segun su prioridad
         */
        private void CascadaAbajo(Node nodo)
        {
            int posicionFinalNodo = nodo.indiceCola;
            int indiceHijoIzq = posicionFinalNodo * 2;

            //si es una hoja, hemos terminado
            if (indiceHijoIzq > _numNodos)
                return;

            int indiceHijoDer = indiceHijoIzq + 1;
            Node hijoIzq = _nodos[indiceHijoIzq];

            //comrpobamos si podemos cambiar con hijo izq
            if (tienePrioridadMayor(nodo, hijoIzq))
            {
                //comprobamos hijo derecha, si no lo hay hacmoes swap y terminamos (no hay mas niveles)
                if (indiceHijoDer > _numNodos)
                {
                    //modificamos posicionFinal del nodo
                    posicionFinalNodo = hijoIzq.indiceCola;
                    //cambiamos su posicion en el array
                    _nodos[posicionFinalNodo] = nodo;
                    //cambiamos el indice del hijo
                    hijoIzq.indiceCola = posicionFinalNodo;
                    //colocamos al hijo en su posicion
                    _nodos[indiceHijoIzq] = hijoIzq;
                    return;
                }

                //comprobamos su el hijo derecha tiene prioridad menor
                Node hijoDer = _nodos[indiceHijoDer];
                if (tienePrioridadMayor(hijoIzq, hijoDer))
                {
                    //derecho es el mas pequeno, se sube derecho
                    //cambiamos indice de derecho
                    hijoDer.indiceCola = posicionFinalNodo;
                    //cambiamos el hijo derecho de poicion
                    _nodos[posicionFinalNodo] = hijoDer;
                    //cambiamos posicion final
                    posicionFinalNodo = indiceHijoDer;

                }
                else
                {
                    //el izquierdo tiene la menor prioridad, hacemos swap
                    //cambiamos indice de izqierdo
                    hijoIzq.indiceCola = posicionFinalNodo;
                    //cambiamos el hijo derecho de poicion
                    _nodos[posicionFinalNodo] = hijoIzq;
                    //cambiamos indice del nodo
                    nodo.indiceCola = indiceHijoIzq;
                    
                }
            }

            //no podemos cambiar con hijo izq, pero ¿y con el derecho?
            else if (indiceHijoDer > _numNodos)
            {
                return; //no existe, hemos acabado
            }
            else
            {
                Node hijoDer = _nodos[indiceHijoDer];
                //existe pero, ¿su prioridad es menor?
                if (tienePrioridadMayor(nodo, hijoDer))
                {
                    //cambiamos con el hijo derecho
                    //cambiamos indice a hijo
                    hijoDer.indiceCola = posicionFinalNodo;                    
                    //cambiamos el padre de posicion
                    _nodos[posicionFinalNodo] = hijoDer;
                    //cambiamos indice al padre
                    nodo.indiceCola = indiceHijoDer;


                }
                //existe, pero no podemos bajar el nodo a ninguna posicion. Hemos acabado
                else
                    return;
            }


            //aqui repetimos el codigo en un bucle ya que de esta manera ahooramos la asignacion del nodo a bajar en cada iteracion
            while (true)
            {
                indiceHijoIzq = posicionFinalNodo * 2;

                //si es una hoja, hemos terminado
                if (indiceHijoIzq > _numNodos)
                {
                    nodo.indiceCola = posicionFinalNodo;
                    _nodos[nodo.indiceCola] = nodo;
                    break;
                }

                indiceHijoDer = indiceHijoIzq + 1;
                hijoIzq = _nodos[indiceHijoIzq];

                //comrpobamos si podemos cambiar con hijo izq
                if (tienePrioridadMayor(nodo, hijoIzq))
                {
                    //comprobamos hijo derecha, si no lo hay hacmoes swap y terminamos (no hay mas niveles)
                    if (indiceHijoDer > _numNodos)
                    {
                        //modificamos indice del nodo
                        nodo.indiceCola = indiceHijoIzq;                        
                        //cambiamos el indice del hijo
                        hijoIzq.indiceCola = posicionFinalNodo;
                        //cambiamos su posicion en el array
                        _nodos[indiceHijoIzq] = nodo;
                        //colocamos al hijo en su posicion
                        _nodos[posicionFinalNodo] = hijoIzq;
                        break;
                    }

                    //comprobamos su el hijo derecha tiene prioridad menor
                    Node hijoDer = _nodos[indiceHijoDer];
                    if (tienePrioridadMayor(hijoIzq, hijoDer))
                    {
                        //derecho es el mas pequeno, se sube derecho
                        //cambiamos indice de derecho
                        hijoDer.indiceCola = posicionFinalNodo;                        
                        //cambiamos el hijo derecho de poicion
                        _nodos[posicionFinalNodo] = hijoDer;
                        //cambiamos posicion final
                        posicionFinalNodo = indiceHijoDer;

                    }
                    else
                    {
                        //el izquierdo tiene la menor prioridad, hacemos swap
                        //cambiamos indice de derecho
                        hijoIzq.indiceCola = posicionFinalNodo; 
                        //cambiamos el hijo derecho de poicion
                        _nodos[posicionFinalNodo] = hijoIzq;
                        //cambiamos posicion final
                        posicionFinalNodo = indiceHijoIzq;
                       
                    }
                }

                //no podemos cambiar con hijo izq, pero ¿y con el derecho?
                else if (indiceHijoDer > _numNodos)
                {
                    nodo.indiceCola = posicionFinalNodo;
                    _nodos[posicionFinalNodo] = nodo;
                    break; //no existe, hemos acabado
                }
                else
                {
                    Node hijoDer = _nodos[indiceHijoDer];
                    //existe pero, ¿su prioridad es menor?
                    if (tienePrioridadMayor(nodo, hijoDer))
                    {
                        //cambiamos con el hijo derecho
                        //cambiamos indice a hijo
                        hijoDer.indiceCola = posicionFinalNodo;
                        //cambiamos el padre de posicion
                        _nodos[posicionFinalNodo] = hijoDer;
                        //cambiamos indice al padre
                        posicionFinalNodo = indiceHijoDer;


                    }
                    //existe, pero no podemos bajar el nodo a ninguna posicion. Hemos acabado
                    else
                    {
                        nodo.indiceCola = posicionFinalNodo;
                        _nodos[nodo.indiceCola] = nodo;
                        break;
                    }
                }
            }
        }



        /// <summary>
        /// Devuelve el nodo con menor prioridad y LO ELIMINA
        /// Para obtener el nodo SIN eliminarlo, usar .Primero
        /// O(log n)
        /// </summary>
        /// <returns>El nodo con menor prioridad</returns>
        public Node Desencolar()
        {
#if DEBUG
            if (_numNodos <= 0)
            {
                throw new InvalidOperationException("Se ha intentado desencolar en una cola vacia");
            }
#endif

            Node nodoADevolver = _nodos[1];

            //si solo quedaba un nodo, se puede devolver sin hacer nada mas
            if (_numNodos == 1)
            {
                _nodos[1] = null;
                _numNodos = 0;
                return nodoADevolver;

            }

            //cambiamos el nodo por el ultimo
            Node nodoQueEraElultimo = _nodos[_numNodos];
            _nodos[1] = nodoQueEraElultimo;
            nodoQueEraElultimo.indiceCola = 1;
            _nodos[_numNodos] = null;
            _numNodos--;

            //ahora el nodo que era el ultimo debe bajarse hasta la posicion que le corresponda
            //CascadaAbajo() organizara de nuevo la cola y garantizara que el nodo con menor prioridad este arriba
            CascadaAbajo(nodoQueEraElultimo);
            return nodoADevolver;
        }


        /// <summary>
        /// Este metodo debe llamarse cada vez que la prioridad de un nodo en la cola cambie
        /// <b>NO HACERLO CORROMPERA IRREMEDIAMBLEMENTE LA COLA</b>
        /// O(log n)
        /// </summary>
        /// <param name="nodo"></param>
        /// <param name="prioridad"></param>
        public void ActualizarPrioridad(Node nodo, float prioridad)
        {
#if DEBUG
            if (nodo == null)
                throw new ArgumentNullException("Se ha intentado actualizar la prioridad de n nodo nulo");
            if (!Contiene(nodo))
                throw new InvalidOperationException("Se ha intentado actualizar la prioridad de un nod que no esta en la cola");
#endif

            nodo.prioridad = prioridad;
            nodoActualizado(nodo);
        }




        private void nodoActualizado(Node nodo)
        {
            //lanzaremos una cascada hacia arriba o hacia abajo segun corresponda

            int indicePadre = nodo.indiceCola / 2;

            if (indicePadre > 0 && tienePrioridadMayor(_nodos[indicePadre], nodo))
                cascadaArriba(nodo);
            else
            {
                //Si no puede ir arriba, ira abajo ( si le corresponde)
                //Notese que si el nodo era la raiz, ira hacia abajo
                CascadaAbajo(nodo);
            }
        }


        /// <summary>
        /// Devuelve el elemento de menor prioridad SIN ELIMINARLO
        /// Para eliminarlo usar Desencolar()
        /// O(1)
        /// </summary>
        public Node Primero
        {
            get
            {
#if DEBUG
                if (_numNodos <= 0)
                {
                    throw new InvalidOperationException("No puede verse el primer elemento de una cola vacia");
                }
#endif
                return _nodos[1];
            }
        }


        /// <summary>
        /// Devuelve true si alto tienen una prioridad mayor que bajo.
        /// </summary>
        /// <param name="mayor">Nodo que deberia tener prioridad mayor</param>
        /// <param name="menor">Nodo que deberia tener prioridad menor</param>
        /// <returns></returns>
        private bool tienePrioridadMayor(Node mayor, Node menor)
        {
            return mayor.prioridad > menor.prioridad;
        }

        /// <summary>
        /// Devuelve true si alto tienen una prioridad mayor O IGUAL que bajo. Notese que devolvera true
        /// si ambos nodos son el mismo
        /// </summary>
        /// <param name="alto">Nodo que deberia tener prioridad mayor</param>
        /// <param name="bajo">Nodo que deberia tener prioridad menor</param>
        private bool tienePirioridadMayorOIgual(Node alto, Node bajo)
        {
            return alto.prioridad >= bajo.prioridad;
        }
    }
}
