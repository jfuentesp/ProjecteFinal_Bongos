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
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enabled)
            return;
        if (collision.CompareTag("Player"))
            Debug.Log("Me cago en to");
        //Podr�amos hacer que si el boss pasa por encima dispare balas tambi�n por que se electrifica o te hace da�o alrededor
    }
}
