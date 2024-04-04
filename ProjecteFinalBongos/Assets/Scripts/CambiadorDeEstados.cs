using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CambiadorDeEstados : MonoBehaviour
{
    [SerializeField]
    private EstadoEvent estado;
    [SerializeField]
    private Da�oEnemigoEvent da�o;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        estado.Raise(EstadosAlterados.Paralitzat);
        da�o.Raise(5);
    
    }
}
