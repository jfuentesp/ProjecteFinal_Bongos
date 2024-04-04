using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CambiadorDeEstados : MonoBehaviour
{
    [SerializeField]
    private EstadoEvent estado;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        estado.Raise(EstadosAlterados.Enverinat);
    }
}
