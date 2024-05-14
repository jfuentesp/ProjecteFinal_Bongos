using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalazingTentacleBehaviour : MonoBehaviour
{
    private int attackCount = 5;
    public float Damage = 1f;
    private GameObject m_Player;

    private void Start()
    {
        
    }
  /*  private void OnTriggerEnter2D(Collider2D collision)
    {
       
            if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerHurtBox"))
            {
                if (collision.gameObject.GetComponent<SMBPlayerParryState>().parry)
                    GetComponentInParent<KrakenParalizingAttack>().Finish();
               
                m_Player = collision.gameObject;
                collision.gameObject.GetComponent<PlayerEstadosController>().AlternarEstado(EstadosAlterados.Atrapat);

            }

            if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerHitBox"))
            {
                attackCount--;
                if (attackCount == 0)
                {
                    collision.gameObject.GetComponentInParent<PlayerEstadosController>().AlternarEstado(EstadosAlterados.Escapar);
                    attackCount = 5;
                    GetComponentInParent<KrakenParalizingAttack>().Finish();
                }

            }
   
    }

    public void Finish() {
        if (m_Player != null)
            m_Player.GetComponent<PlayerEstadosController>().AlternarEstado(EstadosAlterados.Escapar);
    }*/

}
