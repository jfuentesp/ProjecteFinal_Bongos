using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEstadosController : MonoBehaviour
{
    private FiniteStateMachine m_StateMachine;
    private HealthController m_HealthController;
    private BossStatsController m_Stats;
    public bool Invencible, Stun, Poison, Wet, Burn, Wrath, Speedy, StrongMan, Stuck, Paralized;
    public float velocityBefore;
    public float strengthBefore;
    public const int poisonNum = 4;
    public int poisonCount = poisonNum;
    public float poisonDamage;
    public float burntDamage;
    private void Awake()
    {
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_HealthController = GetComponent<HealthController>();
        m_Stats = GetComponent<BossStatsController>();
        Stun = false;
    }
    public void AlternarEstado(EstadosAlterados estado)
    {
      
        switch (estado)
        {
            case EstadosAlterados.Atordit:
                if (!Stun)
                {
                    Stun = true;
                    m_StateMachine.ChangeState<SMBStunState>();
                }
                break;
            case EstadosAlterados.Mullat:
                if (!Wet)
                    StartCoroutine(WetRoutine());
                break;
            case EstadosAlterados.Peus_Lleugers:
                if (!Speedy)
                    StartCoroutine(SpeedRoutine());
                break;
            case EstadosAlterados.Forçut:
                if (!StrongMan)
                    StartCoroutine(StrongRoutine());
                break;
            case EstadosAlterados.Paralitzat:
                if (!Stun)
                {
                    Stun = true;
                    m_StateMachine.ChangeState<SMBParalitzatState>();
                }
                break;
            case EstadosAlterados.Cremat:
                if (!Burn)
                    StartCoroutine(BurntRoutine());
                break;
            case EstadosAlterados.Enverinat:
                if (!Poison)
                {
                    StartCoroutine(PoisonRoutine());
                }

       
                break;
            case EstadosAlterados.Ira:
                if (!Wrath)
                    StartCoroutine(WrathRoutine());
                break;
            default:
                break;

        }
    }

    IEnumerator WetRoutine()
    {
        Wet = true;
        velocityBefore = m_Stats.m_Velocity;
        m_Stats.m_Velocity -= (m_Stats.m_Velocity * m_Stats.getModifier("Wet")) / 100;
        yield return new WaitForSeconds(m_Stats.m_bossTimes.m_WetTime);
        Wet = false;
        m_Stats.m_Velocity = velocityBefore;
        PararCorrutina("WetRoutine");
    }
    IEnumerator SpeedRoutine()
    {
        Speedy = true;
        velocityBefore = m_Stats.m_Velocity;
        m_Stats.m_Velocity += (m_Stats.m_Velocity * m_Stats.getModifier("Fast")) / 100;
        yield return new WaitForSeconds(m_Stats.m_bossTimes.m_VelocityTime);
        Speedy = false;
        m_Stats.m_Velocity = velocityBefore;
        PararCorrutina("SpeedRoutine");
    }
    IEnumerator StrongRoutine()
    {
        StrongMan = true;
        strengthBefore = m_Stats.m_Strength;
        m_Stats.m_Strength += (m_Stats.m_Strength * m_Stats.getModifier("Strength")) / 100;
        yield return new WaitForSeconds(m_Stats.m_bossTimes.m_StrengthTime);
        StrongMan = false;
        m_Stats.m_Strength = strengthBefore;
        PararCorrutina("StrongRoutine");
    }
    IEnumerator BurntRoutine()
    {
        print("CuloQuemado");
        Burn = true;
        yield return new WaitForSeconds(m_Stats.m_bossTimes.m_BurnTime);
        Burn = false;
        PararCorrutina("BurntRoutine");
    }
    IEnumerator PoisonRoutine()
    {
        Poison = true;
        while (poisonCount > 0)
        {
            yield return new WaitForSeconds(m_Stats.m_bossTimes.m_PoisonTime);
            poisonDamage = (m_HealthController.HP * m_Stats.getModifier("Poison")) / 100;
            m_HealthController.Damage(poisonDamage);
            poisonCount--;
        }
        Poison = false;
        poisonCount = poisonNum;
        PararCorrutina("PoisonRoutine");
    }
    IEnumerator WrathRoutine()
    {
        Wrath = true;
        velocityBefore = m_Stats.m_Velocity;
        m_Stats.m_Velocity += (m_Stats.m_Velocity * m_Stats.getModifier("WrathSpeed")) / 100;
        strengthBefore = m_Stats.m_Strength;
        m_Stats.m_Strength += (m_Stats.m_Strength * m_Stats.getModifier("WrathStrength")) / 100;
        yield return new WaitForSeconds(m_Stats.m_bossTimes.m_WrathTime);
        Wrath = false;
        m_Stats.m_Strength = strengthBefore;
        m_Stats.m_Velocity = velocityBefore;
        PararCorrutina("WrathRoutine");

    }

    private void PararCorrutina(string rutina)
    {
        StopCoroutine(rutina);
    }
    public void StopStun()
    {
        Stun = false;
    }
}
