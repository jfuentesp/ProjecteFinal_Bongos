using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CambiadorDeEstados : MonoBehaviour
{
    [SerializeField]
    private EstadoEvent estadoEvent;
    [SerializeField]
    private DañoEnemigoEvent daño;
    [SerializeField]
    private EstadosAlterados estado;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        estadoEvent.Raise(estado);
        daño.Raise(5);
    
    }
}
