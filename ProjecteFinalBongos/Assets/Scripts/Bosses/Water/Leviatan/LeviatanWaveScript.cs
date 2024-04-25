using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeviatanWaveScript : MonoBehaviour
{
    [SerializeField] private float m_minimumDistance;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {
        float distance = Vector3.Distance(other.transform.position, transform.position);

        if (distance >= m_minimumDistance)
        {
            //Do stuff here
        }


    }
