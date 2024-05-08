using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HabilidadDeMovimientoState : SMState
{
    [SerializeField]
    private GameEvent coolDownMovement;
    private PJSMB m_PJ;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private FiniteStateMachine m_StateMachine;
    private AbilityEnum m_habilidad;
    private float dashSpeed = 15f;
    private float dashSpeedInvicible = 10f;
    [SerializeField]
    private GameObject m_SlowDownZone;
    [SerializeField]
    private EstadoEvent changeEstado;
    [SerializeField] private GameEvent invencibleTitleCard;
    private Vector2 m_RecallPosition = Vector2.zero;
    [SerializeField] private GameObject m_RecallZone;
    private GameObject RecallZone = null;
    [SerializeField] private GameObject m_Clon;
    private GameObject Clon = null;
    private Vector2 m_Movement;
    [SerializeField] private GameEvent ClonEvent;
    private new void Awake()
    {
        base.Awake();
        m_PJ = GetComponent<PJSMB>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_Movement = m_PJ.MovementAction.ReadValue<Vector2>();


    }

    public override void InitState()
    {
        base.InitState();
        m_habilidad = m_PJ.PlayerAbilitiesController.Movement;
        StartCoroutine(habilidad());
    }

    IEnumerator habilidad()
    {
        switch (m_habilidad)
        {
            case AbilityEnum.DASH:
                coolDownMovement.Raise();
                if (m_PJ.MovementAction.ReadValue<Vector2>() == Vector2.zero)
                {
                    if (m_PJ.direccion == 0)
                    {
                        m_Animator.Play("Dash");
                        m_Rigidbody.velocity = transform.right * dashSpeed;
                    }
                    else if (m_PJ.direccion == 1)
                    {
                        m_Animator.Play("DashDown");
                        m_Rigidbody.velocity = -transform.up * dashSpeed;
                    }
                    else if (m_PJ.direccion == 2)
                    {
                        m_Animator.Play("DashUp");
                        m_Rigidbody.velocity = transform.up * dashSpeed;
                    }
                }
                else {
                    if (m_PJ.direccion == 0)
                    {
                        m_Animator.Play("Dash");
                    }
                    else if (m_PJ.direccion == 1)
                    {
                        m_Animator.Play("DashDown");
                    }
                    else if (m_PJ.direccion == 2)
                    {
                        m_Animator.Play("DashUp");
                    }
                    m_Rigidbody.velocity = m_PJ.MovementAction.ReadValue<Vector2>() * dashSpeed;
                }
                yield return new WaitForSeconds(0.4f);
                Exit();
                break;
                case AbilityEnum.INVULNERABLEDASH:
                coolDownMovement.Raise();
                invencibleTitleCard.Raise();
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("PlayerHurtBox"), LayerMask.NameToLayer("BossHurtBox"), true);
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("PlayerHurtBox"), LayerMask.NameToLayer("BossHitBox"), true);
                if (m_PJ.MovementAction.ReadValue<Vector2>() == Vector2.zero)
                {
                    if (m_PJ.direccion == 0)
                    {
                        m_Animator.Play("Dash");
                        m_Rigidbody.velocity = transform.right * dashSpeedInvicible;
                    }
                    else if (m_PJ.direccion == 1)
                    {
                        m_Animator.Play("DashDown");
                        m_Rigidbody.velocity = -transform.up * dashSpeedInvicible;
                    }
                    else if (m_PJ.direccion == 2)
                    {
                        m_Animator.Play("DashUp");
                        m_Rigidbody.velocity = transform.up * dashSpeedInvicible;
                    }
                }
                else
                {
                    if (m_PJ.direccion == 0)
                    {
                        m_Animator.Play("Dash");
                    }
                    else if (m_PJ.direccion == 1)
                    {
                        m_Animator.Play("DashDown");
                    }
                    else if (m_PJ.direccion == 2)
                    {
                        m_Animator.Play("DashUp");
                    }
                    m_Rigidbody.velocity = m_PJ.MovementAction.ReadValue<Vector2>() * dashSpeedInvicible;
                }
                yield return new WaitForSeconds(0.4f);
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("PlayerHurtBox"), LayerMask.NameToLayer("BossHurtBox"), false);
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("PlayerHurtBox"), LayerMask.NameToLayer("BossHitBox"), false);
                Exit();
                break;
            case AbilityEnum.SLOW:
                coolDownMovement.Raise();
                if (m_PJ.direccion == 0)
                {
                    m_Animator.Play("Dash");
                }
                else if (m_PJ.direccion == 1)
                {
                    m_Animator.Play("DashDown");
                }
                else if (m_PJ.direccion == 2)
                {
                    m_Animator.Play("DashUp");
                }
                GameObject slowDown = Instantiate(m_SlowDownZone);
                slowDown.transform.position = transform.position;
                Exit();
                break;
            case AbilityEnum.SPEED:
                changeEstado.Raise(EstadosAlterados.Peus_Lleugers);
                Exit(); 
                break;
            case AbilityEnum.RECALL:
                if (m_RecallPosition == Vector2.zero)
                {
                    RecallZone = Instantiate(m_RecallZone);
                    RecallZone.transform.position = transform.position;
                    m_RecallPosition = transform.position;
                    Exit();
                }
                else
                {
                    invencibleTitleCard.Raise();
                    coolDownMovement.Raise();
                    transform.position = m_RecallPosition;
                    Destroy(RecallZone.gameObject);
                    m_RecallPosition = Vector2.zero;
                    Exit(); 
                }
                break;
            case AbilityEnum.CLONE:
                coolDownMovement.Raise();
                if (m_Movement == Vector2.zero)
                {
                    if (m_PJ.direccion == 0)
                    {
                        Clon = Instantiate(m_Clon);
                        Clon.transform.position = new Vector2(transform.position.x + (1f*transform.right.x), transform.position.y);
                        Clon.GetComponent<ClonBehaviour>().Init(transform.right);
                    }
                    else if (m_PJ.direccion == 1)
                    {
                        Clon = Instantiate(m_Clon);
                        Clon.transform.position = new Vector2(transform.position.x, transform.position.y - 0.5f);
                        Clon.GetComponent<ClonBehaviour>().Init(Vector2.down);
                    }
                    else if (m_PJ.direccion == 2)
                    {
                        Clon = Instantiate(m_Clon);
                        Clon.transform.position = new Vector2(transform.position.x, transform.position.y + 0.5f);
                        Clon.GetComponent<ClonBehaviour>().Init(Vector2.up);
                    }
                }
                else {
                    if (m_Movement.x > 0)
                    {
                        Clon = Instantiate(m_Clon);
                        Clon.transform.position = new Vector2(transform.position.x + 0.5f, transform.position.y);
                        Clon.GetComponent<ClonBehaviour>().Init(Vector2.right);

                    }
                    else if (m_Movement.x < 0)
                    {
                        Clon = Instantiate(m_Clon);
                        Clon.transform.position = new Vector2(transform.position.x + 0.5f, transform.position.y);
                        Clon.GetComponent<ClonBehaviour>().Init(Vector2.left);
                    }
                    if (m_Movement.y < 0 && m_Movement.x == 0)
                    {
                        Clon = Instantiate(m_Clon);
                        Clon.transform.position = new Vector2(transform.position.x, transform.position.y - 0.5f);
                        Clon.GetComponent<ClonBehaviour>().Init(Vector2.down);
                    }
                    else if (m_Movement.y > 0 && m_Movement.x == 0)
                    {
                        Clon = Instantiate(m_Clon);
                        Clon.transform.position = new Vector2(transform.position.x, transform.position.y + 0.5f);
                        Clon.GetComponent<ClonBehaviour>().Init(Vector2.up);
                    }
                    else if (m_Movement.y > 0 && m_Movement.x > 0)
                    {
                        Clon = Instantiate(m_Clon);
                        Clon.transform.position = new Vector2(transform.position.x + 0.5f, transform.position.y);
                        Clon.GetComponent<ClonBehaviour>().Init(new Vector2(1, 1));
                    }
                    else if (m_Movement.y < 0 && m_Movement.x > 0)
                    {
                        Clon = Instantiate(m_Clon);
                        Clon.transform.position = new Vector2(transform.position.x + 0.5f, transform.position.y);
                        Clon.GetComponent<ClonBehaviour>().Init(new Vector2(1, -1));
                    }
                    else if (m_Movement.y > 0 && m_Movement.x < 0)
                    {
                        Clon = Instantiate(m_Clon);
                        Clon.transform.position = new Vector2(transform.position.x + 0.5f, transform.position.y);
                        Clon.GetComponent<ClonBehaviour>().Init(new Vector2(-1, 1));
                    }
                    else if (m_Movement.y < 0 && m_Movement.x < 0)
                    {
                        Clon = Instantiate(m_Clon);
                        Clon.transform.position = new Vector2(transform.position.x + 0.5f, transform.position.y);
                        Clon.GetComponent<ClonBehaviour>().Init(new Vector2(-1, -1));
                    }
                }
                
                Exit();
                break;


        }
    }
    public void DestroyMovementElements() {
        if (Clon != null) {
            Destroy(Clon);
        }
        if (RecallZone != null) {
            Destroy(RecallZone);
        }
        
        m_RecallPosition = Vector2.zero;
    }
    private void Exit() { 
        m_StateMachine.ChangeState<SMBPlayerIdleState>();
    }
    public override void ExitState()
    {
        base.ExitState();
        StopAllCoroutines();
    }
}
