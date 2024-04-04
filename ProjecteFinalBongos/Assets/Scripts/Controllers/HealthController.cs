using m17;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour, IHealable, IDamageable
{
    private EstadosAlterados m_estado = EstadosAlterados.Normal;

    private const float MAXHP = 100f;
    [SerializeField]
    private float m_HP = MAXHP;
    private const int EnverinatNum = 4;
    private int EnverinatCount = EnverinatNum;

    public void Damage(float damageAmount)
    {
        if (m_estado.Equals(EstadosAlterados.Cremat))
        {
            m_HP -= damageAmount;
            if (m_HP < 0)
                m_HP = 0;
            m_HP -= (damageAmount * Random.Range(10, 31)) / 100;
            if (m_HP < 0)
                m_HP = 0;

        }
        else if (m_estado.Equals(EstadosAlterados.Paralitzat)) {
            m_HP -= (damageAmount * 1.5f);
            if (m_HP < 0)
                m_HP = 0;
        }
        else {
            m_HP -= damageAmount;
            if (m_HP < 0)
                m_HP = 0;
        }
      
    }

    public void Heal(float healAmount)
    {
        m_HP += healAmount;
        if (m_HP < 0)
            m_HP = 0;

    }
    public void CambiarEstado(EstadosAlterados estado) {
    
        switch (estado) {
            case EstadosAlterados.Cremat:
                    m_estado = EstadosAlterados.Cremat;
                    StartCoroutine(estadoRoutine());
                break;
            case EstadosAlterados.Enverinat:
                    m_estado = EstadosAlterados.Enverinat;
                    StartCoroutine(estadoRoutine());
                break;
            case EstadosAlterados.Paralitzat:
                    m_estado = EstadosAlterados.Paralitzat;
                break;
            default: 
                break;
        }
    }

    IEnumerator estadoRoutine() {
        if (m_estado.Equals(EstadosAlterados.Cremat))
        {
            yield return new WaitForSeconds(15f);
       
            PararRutina();
        }
        else if (m_estado.Equals(EstadosAlterados.Enverinat))
        {
            while (EnverinatCount > 0)
            {
             
                yield return new WaitForSeconds(4f);
                m_HP -= (m_HP * Random.Range(5, 8)) / 100;
                EnverinatCount--;
                if (m_HP < 0)
                    m_HP = 0;
            }
            EnverinatCount = EnverinatNum;
            PararRutina();
        }
    }

    private void PararRutina() { 
        StopCoroutine(estadoRoutine());
        EstadoNormal();
    }
    public void EstadoNormal() {
        m_estado = EstadosAlterados.Normal;

    }
}


