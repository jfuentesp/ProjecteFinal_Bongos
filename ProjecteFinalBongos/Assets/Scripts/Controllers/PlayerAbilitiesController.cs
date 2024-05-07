using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilitiesController : MonoBehaviour
{
    [Header("Habilidades")]
    private List<string> m_ParriesDisponibles = new List<string>();
    private string m_actualParry;
    public string Parry => m_actualParry;
    private List<string> m_MovementActionsDisponibles = new List<string>();
    private string m_actualMovement;
    public string Movement => m_actualMovement;
    private bool m_canMove = true;
    public bool CanMove => m_canMove;
    private List<string> m_AtaquesMejorados = new List<string>();
    public List<string> AtaquesMejoradosDisponibles => m_AtaquesMejorados;
    [SerializeField]
    private float m_CoolDown;
    private  void Awake()
    {
        
        initMovementAbility();
        initParryAbility();
        m_AtaquesMejorados.Add("1x4better");
        m_AtaquesMejorados.Add("2x1better");
        m_AtaquesMejorados.Add("2x2better");
    }

    private void initMovementAbility()
    {
        m_MovementActionsDisponibles.Add("Clon");
        m_actualMovement = m_MovementActionsDisponibles[0];

    }
    private void initParryAbility()
    {
        m_ParriesDisponibles.Add("Invincible");
        m_actualParry = m_ParriesDisponibles[0];

    }
    public void initCoolDown()
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
    public void learnMovement(string movementAction)
    {
        m_MovementActionsDisponibles.Add(movementAction);
    }
    public void learnParry(string parry)
    {
        m_ParriesDisponibles.Add(parry);
    }
    public void learnAttack(string attack)
    {
        m_AtaquesMejorados.Add(attack);
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
