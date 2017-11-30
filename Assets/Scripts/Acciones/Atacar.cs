using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Atacar : Accion {

    public bool Ejecutar(GameObject objetivo, int danyo)
    {
        Unidad unidadActual = GetComponent<Unidad>();
        if (Partida.GetPartidaActual().Jugadores[unidadActual.IdJugador].RestarPuntosDeAccion(costeAccion))
        {
            //Hay que controlar si el objetivo está al alcance de la unidad que ataca cuando se llama a esta funcion.
            try
            {
                Unidad atacante = gameObject.GetComponent<Unidad>();
                Unidad victima = objetivo.GetComponent<Unidad>();

                victima.RecibirAtaque(danyo);

                // PA CUANDO SE IMPLEMENTEN LAS UNIDADES QUE ATACAN, PODER CONTRATACAR

                /*
                try
                {
                    Guerrero unidadQueContrataca = (Guerrero)victima;
                }
                catch (Exception e)
                {
                    try
                    {
                        Arquero unidadQueContrataca = (Arquero)victima;
                    }
                    catch (Exception e)
                    {
                        try
                        {
                            Torre unidadQueContrataca = (Torre)victima;
                        }
                        catch (Exception e)
                        {
                            print("La unidad atacada no puede contratacar");
                        }
                    }

                }
                try
                {
                    atacante.RecibirAtaque(victima.GetDanyoContraataque());
                }
                catch (Exception e)
                {

                }*/

                //Ejecutar alguna animacion en caso de que se hiciera, para ver que se está atacando y no que haya solo dos cubos quietos.
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError("Se está intentando atacar a algo sin el componente Unidad");
                return false;
            }
        }
        return false;
    }


    public override void CancelarAccion()
    {
        throw new NotImplementedException();
    }
}
