using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateSword : Sword, IStateChanger
{
    public float maxRatio;
    public float minRatio;
    public float minPosibilty;
    public float maxPosibilty;
    public void ChangeState(GameObject go)
    {
        if (!go.TryGetComponent(out BossEstadosController bec))
        { return; }
        float random = Random.Range(minRatio, maxRatio);
        if (random >= minPosibilty && random <= maxPosibilty)
        {
            bec.AlternarEstado(Estado);
        }
        Debug.Log("pingo");

    }
}
