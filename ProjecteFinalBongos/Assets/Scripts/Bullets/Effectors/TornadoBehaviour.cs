using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TornadoBehaviour : MonoBehaviour
{
    [SerializeField]
    private int m_Speed;

    private Transform m_Target;
    private Rigidbody2D m_Rigidbody;

    [SerializeField]
    private float m_TornadoActiveDuration;
    [SerializeField, ColorUsage(hdr: true, showAlpha: true)] private Color m_ColorShader;
    [SerializeField, ColorUsage(hdr: true, showAlpha: true)] private Color m_CoorquemadoSader;
    private SpriteRenderer m_SpriteRenderer;
    [SerializeField] private float m_UpdateDirectonTime;
    private float m_Duration;

    public void Init(Transform target)
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_SpriteRenderer.material.SetColor("_Color", m_ColorShader);
        m_Target = target;
        m_Duration = 0;
        m_Rigidbody.velocity = (m_Target.position - transform.position).normalized * m_Speed;
    }
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        Vector3 direction = (m_Target.position - transform.position).normalized;

        // Rotación gradual hacia la dirección del objetivo solo en el eje Z
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, m_UpdateDirectonTime * Time.deltaTime);

        // Movimiento hacia adelante en la dirección local del misil (su up)
        m_Rigidbody.velocity = transform.up * m_Speed;
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
