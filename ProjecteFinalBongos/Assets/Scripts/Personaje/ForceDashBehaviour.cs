using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceDashBehaviour : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
      
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("BossHurtBox"))
        {
            collision.gameObject.GetComponent<BossEstadosController>().AlternarEstado(EstadosAlterados.Paralitzat);
        }
    }
}
