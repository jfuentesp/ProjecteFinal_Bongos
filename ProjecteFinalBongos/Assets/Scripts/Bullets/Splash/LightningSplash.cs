using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSplash : Splash
{
    public override void Init()
    {
        base.Init();
        m_SplashEffectState = ObstacleStateEnum.ELECTRIFIED;
        SetObstacleEffect(ObstacleStateEnum.ELECTRIFIED);
        GetComponent<BossAttackDamage>().SetEstado(EstadosAlterados.Paralitzat);
        GetComponent<BossAttackDamage>().SetTime(0.5f);
        GetComponent<BossAttackDamage>().SetDamage(15);
    }

    private void Update()
    {
        
    }
    private void OnDisable()
    {
        GetComponent<BossAttackDamage>().SetEstado(EstadosAlterados.Normal);
        GetComponent<BossAttackDamage>().SetTime(0);
        GetComponent<BossAttackDamage>().SetDamage(0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enabled)
            return;
        print(collision.gameObject.name);
        //Podríamos hacer que si el boss pasa por encima dispare balas también por que se electrifica o te hace daño alrededor
    }
}
