using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaryaParedScript : MonoBehaviour
{
    [SerializeField] private float m_TimeAlive;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TimeToDie()); ;
    }

    private IEnumerator TimeToDie()
    {
        yield return new WaitForSeconds(m_TimeAlive);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
