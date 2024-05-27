using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEstadosController : MonoBehaviour
{
    private FiniteStateMachine m_StateMachine;
    private HealthController m_HealthController;
    private PlayerStatsController m_Stats;
    private PJSMB m_PJ;
    [SerializeField] private GameObject PoisonParticles;
    public bool Invencible, Stun, Poison, Wet, Burn, Wrath, Speedy, StrongMan, Stuck, Paralized;
    public float velocityBefore;
    public float strengthBefore;
    public const int poisonNum = 4;
    public int poisonCount = poisonNum;
    public float poisonDamage;
    public float burntDamage;
    public Action<EstadosAlterados, float> OnApplyEstadoAlterado;
    private void Awake()
    {
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_HealthController = GetComponent<HealthController>();
        m_Stats = GetComponent<PlayerStatsController>();
        m_PJ = GetComponent<PJSMB>();
        Stun = false;
    }
    public void AlternarEstado(EstadosAlterados estado, float time)
    {
        if (!GetComponent<SMBPlayerParryState>().parry && !Invencible) {
            switch (estado)
            {
                case EstadosAlterados.Adormit:
                    if (!Stun)
                    {
                        Stun = true;
                        m_StateMachine.ChangeState<SMBAdormitState>();
                        OnApplyEstadoAlterado.Invoke(EstadosAlterados.Adormit, time);
                    }
                    break;
                case EstadosAlterados.Atordit:
                    if (!Stun)
                    {
                        Stun = true;
                        m_PJ.GetComponent<SMBStunState>().ChangeTime(time);
                        m_StateMachine.ChangeState<SMBStunState>();
                        OnApplyEstadoAlterado.Invoke(EstadosAlterados.Atordit, time);
                    }
                    break;
                case EstadosAlterados.Mullat:
                    if (!Wet)
                    {
                        StartCoroutine(WetRoutine(time));
                        OnApplyEstadoAlterado.Invoke(EstadosAlterados.Mullat, time);
                    }
                    break;
                case EstadosAlterados.Peus_Lleugers:
                    if (!Speedy)
                    {
                        StartCoroutine(SpeedRoutine(time));
                        OnApplyEstadoAlterado.Invoke(EstadosAlterados.Peus_Lleugers, time);
                    }
                    break;
                case EstadosAlterados.Forçut:
                    if (!StrongMan)
                    {
                        StartCoroutine(StrongRoutine(time));
                        OnApplyEstadoAlterado.Invoke(EstadosAlterados.Forçut, time);
                    }
                    break;
                case EstadosAlterados.Paralitzat:
                    if (!Stun)
                    {
                        Stun = true;
                        Paralized = true;
                        m_PJ.GetComponent<SMBParalitzatState>().ChangeTime(time);
                        OnApplyEstadoAlterado.Invoke(EstadosAlterados.Paralitzat, time);
                        m_StateMachine.ChangeState<SMBParalitzatState>();
                    }
                    break;
                case EstadosAlterados.Cremat:
                    if (!Burn)
                    {
                        StartCoroutine(BurntRoutine(time));
                        OnApplyEstadoAlterado.Invoke(EstadosAlterados.Cremat, time);
                    }
                    break;
                case EstadosAlterados.Enverinat:
                    if (!Poison)
                    {
                        StartCoroutine(PoisonRoutine(time));
                        OnApplyEstadoAlterado.Invoke(EstadosAlterados.Enverinat, time);
                    }
                    break;
                case EstadosAlterados.Invencible:
                    if (!Invencible)
                    {
                        StartCoroutine(InvencibleRoutine(time));
                        OnApplyEstadoAlterado.Invoke(EstadosAlterados.Invencible, time);
                    }
                    break;
                case EstadosAlterados.Ira:
                    if (!Wrath)
                    {
                        StartCoroutine(WrathRoutine(time));
                        OnApplyEstadoAlterado.Invoke(EstadosAlterados.Invencible, time);
                    }
                    break;
                case EstadosAlterados.Atrapat:
                    if (!Stuck)
                    {
                        StuckFunction();
                        OnApplyEstadoAlterado.Invoke(EstadosAlterados.Atrapat, time);
                    }
                    break;
                case EstadosAlterados.Escapar:
                    UnStuckFunction();
                    OnApplyEstadoAlterado.Invoke(EstadosAlterados.Escapar, time);
                    break;
                default:
                    break;

            }
        }
       
    }

    IEnumerator WetRoutine(float time)
    {
        Wet = true;
        velocityBefore = m_Stats.m_Velocity;
        m_Stats.m_Velocity -= (m_Stats.m_Velocity * m_Stats.getModifier("Wet")) / 100; 
        yield return new WaitForSeconds(time);
        Wet = false;
        m_Stats.m_Velocity = velocityBefore;
        PararCorrutina("WetRoutine");
    }
    IEnumerator SpeedRoutine(float time)
    {
        Speedy = true;
        velocityBefore = m_Stats.m_Velocity;
        m_Stats.m_Velocity += (m_Stats.m_Velocity * m_Stats.getModifier("Fast")) / 100;
        yield return new WaitForSeconds(time);
        Speedy = false;
        m_Stats.m_Velocity = velocityBefore;
        PararCorrutina("SpeedRoutine");
    }
    IEnumerator StrongRoutine(float time)
    {
        StrongMan = true;
        strengthBefore = m_Stats.m_Strength;
        m_Stats.m_Strength += (m_Stats.m_Strength * m_Stats.getModifier("Strength")) / 100;
        yield return new WaitForSeconds(time);
        StrongMan = false;
        m_Stats.m_Strength = strengthBefore;
        PararCorrutina("StrongRoutine");
    }
    IEnumerator BurntRoutine(float time)
    {
        Burn = true;
        yield return new WaitForSeconds(time);
        Burn = false;
        PararCorrutina("BurntRoutine");
    }
    IEnumerator PoisonRoutine(float time)
    {
        Poison = true;
        PoisonParticles.SetActive(true);
        while (poisonCount > 0)
        {
            yield return new WaitForSeconds(time);
            poisonDamage = (m_HealthController.HP * m_Stats.getModifier("Poison")) / 100;
            m_PJ.recibirDamage(poisonDamage);
            poisonCount--;
        }
        Poison = false;
        PoisonParticles.SetActive(false);
        poisonCount = poisonNum;
        PararCorrutina("PoisonRoutine");
    }
    IEnumerator InvencibleRoutine(float time)
    {
        Invencible = true;
        yield return new WaitForSeconds(time);
        Invencible = false;
        PararCorrutina("InvencibleRoutine");
    }
    IEnumerator WrathRoutine(float time)
    {
        Wrath = true;
        velocityBefore = m_Stats.m_Velocity;
        m_Stats.m_Velocity += (m_Stats.m_Velocity * m_Stats.getModifier("WrathSpeed")) / 100;
        strengthBefore = m_Stats.m_Strength;
        m_Stats.m_Strength += (m_Stats.m_Strength * m_Stats.getModifier("WrathStrength")) / 100;
        yield return new WaitForSeconds(time);
        Wrath = false;
        m_Stats.m_Strength = strengthBefore;
        m_Stats.m_Velocity = velocityBefore;
        PararCorrutina("WrathRoutine");

    }
    /*IEnumerator StuckRoutine()
    {
        Stuck = true;
        velocityBefore = m_Stats.m_Velocity;
        m_Stats.m_Velocity = 0;
        yield return new WaitForSeconds(m_Stats.m_playerTimes.m_StuckTime);
        Stuck = false;
        m_Stats.m_Velocity = velocityBefore;
        PararCorrutina("StuckRoutine");
    }*/
    public void StuckFunction() {
        velocityBefore = m_Stats.m_Velocity;
        m_Stats.m_Velocity = 0;
    }
    public void UnStuckFunction() {
        if (m_Stats.m_Velocity == 0)
            m_Stats.m_Velocity = velocityBefore;
    }
    private void PararCorrutina(string rutina)
    {
        StopCoroutine(rutina);
    }
    public void StopStun()
    {
        Stun = false;
        Paralized = false;
    }
    public void StopStuck()
    {
        if(m_Stats.m_Velocity == 0)
            m_Stats.m_Velocity = velocityBefore;
    }

}
