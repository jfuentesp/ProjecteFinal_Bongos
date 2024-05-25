using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TornadoBehaviour : MonoBehaviour
{
    [SerializeField]
    private float m_Speed;
    [SerializeField]
    private float m_FinalSpeed;

    private bool m_PlayerTucat;

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
        m_PlayerTucat = false;
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
        if (!m_PlayerTucat)
        {
            /*Vector3 direction = (m_Target.position - transform.position).normalized;

             Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
             Quaternion rotacion = Quaternion.Lerp(transform.rotation, targetRotation, m_UpdateDirectonTime * Time.deltaTime);
             m_Rigidbody.velocity = rotacion * transform.up * m_Speed;*/
             Vector3 nuevaPosicion = Vector3.Lerp(transform.position, m_Target.position, m_Speed * Time.deltaTime);

             // Actualizamos la posición del objeto que tiene este script
             transform.position = nuevaPosicion;
        }

    }
    // Update is called once per frame
    void Update()
    {
        m_Duration += Time.deltaTime;
        if (m_Duration >= m_TornadoActiveDuration)
        {
            m_Rigidbody.velocity = Vector3.zero;
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!m_PlayerTucat)
            {
                m_PlayerTucat = true;
                m_Rigidbody.velocity = (m_Target.position - transform.position).normalized * m_FinalSpeed;
            }
        }
        if (collision.gameObject.CompareTag("Splash"))
        {
            print("Toque splash");
            if (collision.GetComponent<Splash>().SplashEffectState == ObstacleStateEnum.ELECTRIFIED)
            {
                m_SpriteRenderer.material.SetColor("_Color", m_CoorquemadoSader);
            }
        }


    }
}
