using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalazingTentacleBehaviour : MonoBehaviour
{
    private FixedJoint2D m_Joint;
    void Start()
    {
        m_Joint = GetComponent<FixedJoint2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerHurtBox"))
        {
            if (collision.gameObject.GetComponent<SMBPlayerParryState>().parry) {
                return;
            }
                collision.gameObject.GetComponent<PlayerEstadosController>().AlternarEstado(EstadosAlterados.Atrapat);
        }
    }
}
