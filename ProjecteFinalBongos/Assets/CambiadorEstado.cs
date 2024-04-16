using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CambiadorEstado : MonoBehaviour
{
    [SerializeField]
    private EstadoEvent a;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        a.Raise(EstadosAlterados.Adormit);
    }

}
