using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LighningSplash : Splash
{
    public override void Init()
    {
        base.Init();
        m_SplashEffectState = ObstacleStateEnum.ELECTRIFIED;
        Debug.Log("Soy el lighning y mi estado es: " + m_SplashEffectState);
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enabled)
            return;
        if (collision.CompareTag("Player"))
            Debug.Log("Player sufre da�o");
        //Podr�amos hacer que si el boss pasa por encima dispare balas tambi�n por que se electrifica o te hace da�o alrededor
    }
}
