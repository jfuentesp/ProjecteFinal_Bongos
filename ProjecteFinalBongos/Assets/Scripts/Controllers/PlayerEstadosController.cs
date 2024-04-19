using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEstadosController : MonoBehaviour
{
    private FiniteStateMachine m_StateMachine;
    private HealthController m_HealthController;
    private PlayerStatsController m_Stats;
    private PJSMB m_PJ;
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
        m_Stats = GetComponent<PlayerStatsController>();
        m_PJ = GetComponent<PJSMB>();
        Stun = false;
    }
    public void AlternarEstado(EstadosAlterados estado)
    {
        switch (estado)
        {
            case EstadosAlterados.Adormit:
                if (!Stun)
                {
                    Stun = true;
                    m_StateMachine.ChangeState<SMBAdormitState>();
                }
                break;
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
            case EstadosAlterados.Invencible:
                if (!Invencible)
                    StartCoroutine(InvencibleRoutine());
                break;
            case EstadosAlterados.Ira:
                if (!Wrath)
                    StartCoroutine(WrathRoutine());
                break;
            case EstadosAlterados.Atrapat:
                if (!Stuck)
                    StartCoroutine(StuckRoutine());
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
        yield return new WaitForSeconds(m_Stats.m_playerTimes.m_WetTime);
        Wet = false;
        m_Stats.m_Velocity = velocityBefore;
        PararCorrutina("WetRoutine");
    }
    IEnumerator SpeedRoutine()
    {
        Speedy = true;
        velocityBefore = m_Stats.m_Velocity;
        m_Stats.m_Velocity += (m_Stats.m_Velocity * m_Stats.getModifier("Fast")) / 100;
        yield return new WaitForSeconds(m_Stats.m_playerTimes.m_VelocityTime);
        Speedy = false;
        m_Stats.m_Velocity = velocityBefore;
        PararCorrutina("SpeedRoutine");
    }
    IEnumerator StrongRoutine()
    {
        StrongMan = true;
        strengthBefore = m_Stats.m_Strength;
        m_Stats.m_Strength += (m_Stats.m_Strength * m_Stats.getModifier("Strength")) / 100;
        yield return new WaitForSeconds(m_Stats.m_playerTimes.m_StrengthTime);
        StrongMan = false;
        m_Stats.m_Strength = strengthBefore;
        PararCorrutina("StrongRoutine");
    }
    IEnumerator BurntRoutine()
    {
        Burn = true;
        yield return new WaitForSeconds(m_Stats.m_playerTimes.m_BurnTime);
        Burn = false;
        PararCorrutina("BurntRoutine");
    }
    IEnumerator PoisonRoutine()
    {
        Poison = true;
        while (poisonCount > 0)
        {
            yield return new WaitForSeconds(m_Stats.m_playerTimes.m_PoisonTime);
            poisonDamage = (m_HealthController.HP * m_Stats.getModifier("Poison")) / 100;
            m_PJ.recibirDaño(poisonDamage);
            poisonCount--;
        }
        Poison = false;
        poisonCount = poisonNum;
        PararCorrutina("PoisonRoutine");
    }
    IEnumerator InvencibleRoutine()
    {
        Invencible = true;
        yield return new WaitForSeconds(m_Stats.m_playerTimes.m_InvencibleTime);
        Invencible = false;
        PararCorrutina("InvencibleRoutine");
    }
    IEnumerator WrathRoutine()
    {
        Wrath = true;
        velocityBefore = m_Stats.m_Velocity;
        m_Stats.m_Velocity += (m_Stats.m_Velocity * m_Stats.getModifier("WrathSpeed")) / 100;
        strengthBefore = m_Stats.m_Strength;
        m_Stats.m_Strength += (m_Stats.m_Strength * m_Stats.getModifier("WrathStrength")) / 100;
        yield return new WaitForSeconds(m_Stats.m_playerTimes.m_WrathTime);
        Wrath = false;
        m_Stats.m_Strength = strengthBefore;
        m_Stats.m_Velocity = velocityBefore;
        PararCorrutina("WrathRoutine");

    }
    IEnumerator StuckRoutine()
    {
        Stuck = true;
        velocityBefore = m_Stats.m_Velocity;
        m_Stats.m_Velocity = 0;
        yield return new WaitForSeconds(m_Stats.m_playerTimes.m_StuckTime);
        Stuck = false;
        m_Stats.m_Velocity = velocityBefore;
        PararCorrutina("StuckRoutine");
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
