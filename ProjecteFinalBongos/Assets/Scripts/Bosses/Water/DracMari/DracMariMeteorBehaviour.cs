using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DracMariMeteorBehaviour : MonoBehaviour
{
    private Pool m_Pool;

    private void Awake()
    {
        m_Pool = LevelManager.Instance._SplashPool;
    }

    public void Fall() {
        print("aaaa");
        GameObject vaporCrash = m_Pool.GetElement();
        vaporCrash.transform.position = transform.position;
        vaporCrash.SetActive(true);
        DracMVaporSplash vaporsplash = vaporCrash.GetComponent<DracMVaporSplash>();
        vaporsplash.enabled = true;
        vaporCrash.GetComponent<DracMVaporSplash>().Init();
        Destroy(gameObject);
    }

}
