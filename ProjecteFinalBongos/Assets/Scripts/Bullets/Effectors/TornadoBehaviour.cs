using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoBehaviour : MonoBehaviour
{
    [SerializeField]
    private int m_Speed;

    private Transform m_Target;
    private Rigidbody2D m_Rigidbody;

    [SerializeField]
    private float m_TornadoActiveDuration;
    private float m_Duration;

    public void Init(Transform target)
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Target = target;
        m_Duration = 0;
        m_Rigidbody.velocity = (m_Target.position - transform.position).normalized * m_Speed;
    }
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        m_Duration += Time.deltaTime;
        if(m_Duration >= m_TornadoActiveDuration)
        {
            m_Rigidbody.velocity = Vector3.zero;
            gameObject.SetActive(false);
        }
    }
}
