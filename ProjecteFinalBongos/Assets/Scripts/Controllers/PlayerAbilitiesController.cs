using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilitiesController : MonoBehaviour
{
    public Action OnLearnAbility;


    [Header("Habilidades")]
    private List<Ability> m_ParriesDisponibles = new List<Ability>();
    private Ability m_actualParry;
    public Ability Parry => m_actualParry;
    private List<Ability> m_MovementActionsDisponibles = new List<Ability>();
    public List<Ability> AbilityList => m_MovementActionsDisponibles;
    private Ability m_actualMovement;
    public Ability Movement => m_actualMovement;
    private bool m_canMove = true;
    public bool CanMove => m_canMove;
    private List<Ability> m_AtaquesMejorados = new List<Ability>();
    public List<Ability> AtaquesMejoradosDisponibles => m_AtaquesMejorados;
    [SerializeField]
    private float m_CoolDown;
    private  void Awake()
    {
        initMovementAbility();
        initParryAbility();
    }

    private void initMovementAbility()
    {
        if(m_MovementActionsDisponibles.Count > 0)
            m_actualMovement = m_MovementActionsDisponibles[0];
    }
    private void initParryAbility()
    {
        if(m_ParriesDisponibles.Count > 0)
            m_actualParry = m_ParriesDisponibles[0];
    }
    public void initCoolDown()
    {
        StartCoroutine(MovementCooldown());
    }

    public void changeMovement(Ability movementAction)
    {
        if (m_MovementActionsDisponibles.Contains(movementAction))
        {
            m_actualMovement = m_MovementActionsDisponibles[m_MovementActionsDisponibles.IndexOf(movementAction)];
        }

    }
    public void changeParry(Ability parryAction)
    {
        if (m_ParriesDisponibles.Contains(parryAction))
        {
            m_actualParry = m_ParriesDisponibles[m_ParriesDisponibles.IndexOf(parryAction)];
        }
    }

    public void learnMovement(Ability movementAction)
    {
        m_MovementActionsDisponibles.Add(movementAction);
        changeMovement(movementAction);
        OnLearnAbility?.Invoke();
        Debug.Log("Aprendido el movimiento " + movementAction.abilityName);
    }

    public void learnParry(Ability parryAction)
    {
        m_ParriesDisponibles.Add(parryAction);
        changeParry(parryAction);
        Debug.Log("Aprendido el bloqueo " + parryAction.abilityName);
    }
    public void learnAttack(Ability attack)
    {
        m_AtaquesMejorados.Add(attack);
        Debug.Log("Aprendido el ataque " + attack.abilityName);
    }

    public Ability GetNextAbility()
    {
        int currentIndex = m_MovementActionsDisponibles.IndexOf(m_actualMovement);
        if (currentIndex < m_MovementActionsDisponibles.Count - 1)
            return m_MovementActionsDisponibles[currentIndex + 1];
        else
            return null;
    }

    public Ability GetPreviousAbility()
    {
        int currentIndex = m_MovementActionsDisponibles.IndexOf(m_actualMovement);
        if (currentIndex > 0)
            return m_MovementActionsDisponibles[currentIndex-1];
        else
            return null;
    }

    public void SelectNextAbility()
    {
        Ability abilityToSet = GetNextAbility();
        if(abilityToSet != null)
            m_actualMovement = abilityToSet;
        OnLearnAbility.Invoke();
    }

    public void SelectPreviousAbility()
    {
        Ability abilityToSet = GetPreviousAbility();
        if(abilityToSet != null)
            m_actualMovement = abilityToSet;
        OnLearnAbility.Invoke();
    }

    IEnumerator MovementCooldown()
    {
        m_canMove = false;
        yield return new WaitForSeconds(m_CoolDown);
        m_canMove = true;
        Exit();
    }
    private void Exit() {
        StopCoroutine(MovementCooldown());
    }
}
