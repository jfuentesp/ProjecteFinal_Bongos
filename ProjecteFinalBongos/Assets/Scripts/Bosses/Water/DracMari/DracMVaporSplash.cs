using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DracMVaporSplash : Splash
{
    public override void Init()
    {
        base.Init();
        gameObject.tag = "Untagged";
    }

    private void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!enabled)
            return;
        if (collision.CompareTag("Player")) {
            Debug.Log("Player");
        } else if (collision.gameObject.layer == LayerMask.NameToLayer("BossHurtBox")) {
            Debug.Log("Boss");        
        }
                
        //Podríamos hacer que si el boss pasa por encima dispare balas también por que se electrifica o te hace daño alrededor
    }
}
