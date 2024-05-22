using NavMeshPlus.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AgentOverride2d))]
[RequireComponent(typeof(SMBIdleState))]
[RequireComponent(typeof(SMBRangedAttack))]
[RequireComponent(typeof(SMBChaseState))]
[RequireComponent(typeof(SMBRunAwayState))]
[RequireComponent(typeof(HealthController))]
public class MedusaBossBehaviour : BossBehaviour
{
    [SerializeField] private int m_RangoHuirPerseguir;
    private Coroutine m_PlayerDetectionCoroutine;
    [Header("Variables medusita")]
    [SerializeField] private GameObject m_Medusita;
    [SerializeField] private int m_NumeroMinimoMedusas;
    [SerializeField] private LayerMask m_MedusitaLayerMask;
    [SerializeField] private float m_TiempoEntreMedusas;
    private new void Awake()
    {
        base.Awake();
        m_SalaPadre = GetComponentInParent<SalaBoss>();
        GetComponent<SMBIdleState>().OnPlayerEnter = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        };
        GetComponent<SMBRangedAttack>().onAttackStopped = (GameObject obj) =>
        {
            PerseguirHuir();
        };
        GetComponent<SMBRunAwayState>().onStoppedRunningAway = (GameObject obj) =>
        {
            m_StateMachine.ChangeState<SMBRangedAttack>();
        };

        m_StateMachine.ChangeState<SMBIdleState>();
        GetComponent<SMBIdleState>().OnPlayerEnter += EmpezarCorutina;
    }
    private void EmpezarCorutina(GameObject obj)
    {
        m_PlayerDetectionCoroutine = StartCoroutine(PlayerDetectionCoroutine());
        GenerarMedusitas();
        StartCoroutine(IniciaMedusas());
    }

    private IEnumerator IniciaMedusas()
    {
        while (m_IsAlive)
        {
            yield return new WaitForSeconds(m_TiempoEntreMedusas);
            float numero = m_HealthController.HP / m_HealthController.HPMAX * 100;
            int numerinSuma;
            if (numero >= 0 && numero < 25)
                numerinSuma = 1;
            else if (numero >= 25 && numero < 50)
                numerinSuma = 2;
            else if (numero >= 50 && numero < 75)
                numerinSuma = 3;
            else
                numerinSuma = 4;

            int numerinPlusMedusas = 1 + numerinSuma;

            for (int i = 0; i < numerinPlusMedusas; i++)
            {
                int random = Random.Range(0, transform.childCount);
                GameObject medusita = transform.GetChild(random).gameObject;
                medusita.transform.localPosition = new Vector2(1,1);
                medusita.transform.parent = transform.parent;
                medusita.GetComponent<MedusitaBehaviour>().PlayerHoming();
            }
            GenerarMedusitas();
        }
    }
    private void GenerarMedusitas()
    {
        int numerinPlusMedusas = 10 - (int)(m_HealthController.HP / m_HealthController.HPMAX * 10);
        int numeroMaximoMedusitas = m_NumeroMinimoMedusas + numerinPlusMedusas;
        print(numeroMaximoMedusitas);
        while (transform.childCount < numeroMaximoMedusitas)
        {
            GameObject medusa = Instantiate(m_Medusita, transform);
            //medusa.GetComponent<CircleCollider2D>().enabled = false;
            
            medusa.transform.localPosition = GetRandomPosition();
            medusa.GetComponent<MedusitaBehaviour>().Init(Random.Range(1,4),m_Target);
        }
    }
    private Vector2 GetRandomPosition()
    {
        Vector2 posicion = Random.insideUnitCircle.normalized* Random.Range(0.65f, 1f);
        if (posicion.x > -0.6 && posicion.x < 0.6 && posicion.y > 0.6)
        {
            return GetRandomPosition();
        }
        else
        {
            RaycastHit2D hit = Physics2D.CircleCast(posicion,0.25f, transform.position, 0.25f, m_MedusitaLayerMask);
            if (hit.collider != null)
            {
                return GetRandomPosition();
            }
            else
            {
                return posicion;
            }
            
        }
    }
    private IEnumerator PlayerDetectionCoroutine()
    {
        while (m_IsAlive)
        {

            if (m_PlayerAttackDetectionAreaType == CollisionType.CIRCLE)
            {
                RaycastHit2D hitInfo = Physics2D.CircleCast(transform.position, m_AreaRadius, transform.position, m_AreaRadius, m_LayersToCheck);
                if (hitInfo.collider != null && hitInfo.collider.CompareTag("Player") && !m_IsBusy)
                {
                    m_IsPlayerDetected = true;
                    m_StateMachine.ChangeState<SMBRangedAttack>();
                }
                else
                {
                    m_IsPlayerDetected = false;
                }
            }
            else
            {
                RaycastHit2D hitInfo = Physics2D.BoxCast(transform.position, m_BoxArea, transform.rotation.z, transform.position);
                if (hitInfo.collider != null && hitInfo.collider.CompareTag("Player") && !m_IsBusy)
                {
                    m_IsPlayerDetected = true;
                    m_StateMachine.ChangeState<SMBRangedAttack>();
                }
                else
                {
                    m_IsPlayerDetected = false;
                }
            }
            yield return new WaitForSeconds(m_CheckingPlayerTimelapse);
        }
    }
    private void PerseguirHuir()
    {

        if (Vector2.Distance(transform.position, m_Target.position) > m_RangoHuirPerseguir)
        {
            print("Me acerco " + Vector2.Distance(transform.position, m_Target.position));
            m_StateMachine.ChangeState<SMBChaseState>();
        }
        else
        {
            print("Me alejo " + Vector2.Distance(transform.position, m_Target.position));
            m_StateMachine.ChangeState<SMBRunAwayState>();
        }
            
    }

    private void MatarBoss() {
        Destroy(gameObject);
    }
    public override void Init(Transform _Target)
    {
        base.Init(_Target);
        OnPlayerInSala?.Invoke();
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
