using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilitiesController : MonoBehaviour
{
    [Header("Habilidades")]
    private List<AbilityEnum> m_ParriesDisponibles = new List<AbilityEnum>();
    private AbilityEnum m_actualParry;
    public AbilityEnum Parry => m_actualParry;
    private List<AbilityEnum> m_MovementActionsDisponibles = new List<AbilityEnum>();
    private AbilityEnum m_actualMovement;
    public AbilityEnum Movement => m_actualMovement;
    private bool m_canMove = true;
    public bool CanMove => m_canMove;
    private List<AbilityEnum> m_AtaquesMejorados = new List<AbilityEnum>();
    public List<AbilityEnum> AtaquesMejoradosDisponibles => m_AtaquesMejorados;
    [SerializeField]
    private float m_CoolDown;
    private  void Awake()
    {
        
        initMovementAbility();
        initParryAbility();
        m_AtaquesMejorados.Add(AbilityEnum.WHIRLWINDATTACK);
        m_AtaquesMejorados.Add(AbilityEnum.STABATTACK);
        m_AtaquesMejorados.Add(AbilityEnum.WAVEATTACK);
    }

    private void initMovementAbility()
    {
        m_MovementActionsDisponibles.Add(AbilityEnum.RECALL);
        m_actualMovement = m_MovementActionsDisponibles[0];

    }
    private void initParryAbility()
    {
        m_ParriesDisponibles.Add(AbilityEnum.INVULNERABLEPARRY);
        m_actualParry = m_ParriesDisponibles[0];

    }
    public void initCoolDown(float cooldown)
    {
        StartCoroutine(MovementCooldown());
    }
    public void changeMovement(int movementAction)
    {
        if (m_MovementActionsDisponibles.Contains(m_MovementActionsDisponibles[movementAction]))
        {
            m_actualMovement = m_MovementActionsDisponibles[movementAction];
        }

    }
    public void changeParry(int parry)
    {
        if (m_ParriesDisponibles.Contains(m_ParriesDisponibles[parry]))
        {
            m_actualParry = m_ParriesDisponibles[parry];
        }

    }
    public void learnMovement(AbilityEnum movementAction)
    {
        m_MovementActionsDisponibles.Add(movementAction);
        Debug.Log("Aprendido el movimiento " + movementAction);
    }
    public void learnParry(AbilityEnum parry)
    {
        m_ParriesDisponibles.Add(parry);
        Debug.Log("Aprendido el bloqueo " + parry);
    }
    public void learnAttack(AbilityEnum attack)
    {
        m_AtaquesMejorados.Add(attack);
        Debug.Log("Aprendido el ataque " + attack);
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
