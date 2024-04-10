using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggAltea : Splash
{
    [SerializeField]
    private GameObject m_SerpientePrefab;

    public override void Init()
    {
        base.Init();
      
    }
    private void Update()
    {
        
    }

    private void OnDisable()
    {
        //print("Spawn Serpiente" + transform.localPosition);
    }
}
