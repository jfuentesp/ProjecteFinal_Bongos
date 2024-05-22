using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;

public class MedusitaBehaviour : BossBehaviour
{
    private int m_KindOfMedusita;
    private SpriteRenderer m_SpriteRenderer;
    [SerializeField] private float m_Speed;
    [SerializeField] private float m_UpdateDirectonTime;
    private Rigidbody2D m_RigidBody;
    private bool m_Inmolando;

    private new void Awake()
    {
        base.Awake();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_RigidBody = GetComponent<Rigidbody2D>();
        m_Inmolando = false;
    }
    public override void Init(Transform _Target)
    {
        base.Init(_Target);
        m_Target = _Target;
        m_KindOfMedusita = Random.Range(1, 4); 
        switch (m_KindOfMedusita)
        {
            case 1:
                m_SpriteRenderer.color = Color.green;
                m_Speed *= 1;
                m_UpdateDirectonTime = 1;
                break;
            case 2:
                m_SpriteRenderer.color = Color.yellow;
                m_Speed *= 2;
                m_UpdateDirectonTime = 2;
                break;
            case 3:
                m_SpriteRenderer.color = Color.red;
                m_Speed *= 3;
                m_UpdateDirectonTime = 3;
                break;
            default:
                break;
        }
        OnPlayerInSala?.Invoke();
    }
    public void PlayerHoming()
    {
        StartCoroutine(Homing());
    }

    private IEnumerator Homing()
    {
        m_Inmolando = true;
        //UpdateRotacion(1);
        while (true)
        {
            /*m_RigidBody.velocity = transform.up * m_Speed;
            UpdateRotacion(0.87f);*/
            // Dirección hacia el objetivo
            Vector3 direction = (m_Target.position - transform.position).normalized;

            // Rotación gradual hacia la dirección del objetivo solo en el eje Z
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, m_UpdateDirectonTime * Time.deltaTime);

            // Movimiento hacia adelante en la dirección local del misil (su up)
            transform.Translate(Vector3.up * m_Speed * 2 * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
    }

    private void UpdateRotacion(float max)
    {
        Vector2 posicionPlayer = m_Target.position - transform.position;
        float angulo = Mathf.Atan2(posicionPlayer.y, posicionPlayer.x);
        angulo = Mathf.Rad2Deg * angulo - 90;
        transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, new Vector3(0, 0, angulo), max); ;
    }


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (m_Inmolando)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerHurtBox") || collision.gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                VidaCero();
            }
        }
    }

    private void MatarBoss()
    {
        Destroy(gameObject);
    }
    protected override void VidaCero()
    {
        base.VidaCero();
        StopAllCoroutines();
        GetComponentInParent<SalaBoss>().OnPlayerIn -= Init;
        m_StateMachine.ChangeState<DeathState>();
        m_IsAlive = false;
        OnBossDeath?.Invoke();
        m_BossMuertoEvent.Raise();

    }
}
