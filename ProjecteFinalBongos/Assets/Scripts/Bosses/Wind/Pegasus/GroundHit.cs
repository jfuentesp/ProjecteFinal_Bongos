using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundHit : MonoBehaviour
{
    [SerializeField]
    float m_GrowingDuration;
    [SerializeField]
    private int m_FuerzaRepulsion;
    float m_CurrentDuration;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        m_CurrentDuration += Time.deltaTime;
        if (m_CurrentDuration >= m_GrowingDuration)
        {
            StopGrowing();
        }
        if (transform.localScale.x < 10)
        {
            transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
        }
    }

    private void StopGrowing()
    {
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<Rigidbody2D>().AddForce(collision.transform.position - transform.position.normalized * m_FuerzaRepulsion, ForceMode2D.Impulse);
        }
    }
}
