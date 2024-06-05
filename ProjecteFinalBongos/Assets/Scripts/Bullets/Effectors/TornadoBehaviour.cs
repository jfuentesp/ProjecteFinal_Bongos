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
    private bool m_PlayerCremat;

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
        m_PlayerCremat = false;
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
            Destroy(gameObject);
        }
    }

    private Coroutine m_CremarCoroutine;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!m_PlayerTucat)
            {
                m_PlayerTucat = true;
                m_Rigidbody.velocity = (m_Target.position - transform.position).normalized * m_FinalSpeed;
            }
            if (m_PlayerCremat)
            {
                collision.gameObject.TryGetComponent(out PlayerEstadosController estados);
                if (estados != null)
                    estados.AlternarEstado(EstadosAlterados.Cremat, 20f);
                collision.gameObject.TryGetComponent(out HealthController health);
                if (health != null)
                    m_CremarCoroutine = StartCoroutine(DamageCoroutine(health));
            }

        }
        if (collision.gameObject.CompareTag("Splash"))
        {
            collision.gameObject.TryGetComponent(out LightningSplash splash);
            if (splash?.SplashEffectState == ObstacleStateEnum.ELECTRIFIED)
            {
                m_PlayerCremat = true;
                m_SpriteRenderer.material.SetColor("_Color", m_CoorquemadoSader);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if (!m_PlayerCremat)
                return;
            if (m_CremarCoroutine != null)
                StopCoroutine(m_CremarCoroutine);
        }
    }

    private IEnumerator DamageCoroutine(HealthController health)
    {
        while(m_PlayerCremat)
        {
            yield return new WaitForSeconds(1f);
            health.Damage(5f);
        }
    }
}
