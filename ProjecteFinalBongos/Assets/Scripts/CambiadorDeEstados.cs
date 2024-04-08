using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CambiadorDeEstados : MonoBehaviour
{
    [SerializeField]
    private EstadoEvent estadoEvent;
    [SerializeField]
    private Da�oEnemigoEvent da�o;
    [SerializeField]
    private EstadosAlterados estado;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        estadoEvent.Raise(estado);
        da�o.Raise(5);
    
    }
}
