using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalazingTentacleBehaviour : MonoBehaviour
{
    private int attackCount = 5;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerHitBox")) {
            attackCount--;
            if (attackCount <= 0)
            {
                GetComponentInParent<KrakenParalizingAttack>().Destroyed();
            }
        }
    }

 

}
