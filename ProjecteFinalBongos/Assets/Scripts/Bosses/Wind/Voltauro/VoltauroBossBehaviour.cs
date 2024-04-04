using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SMBIdleState))]
[RequireComponent(typeof(SMBParriedState))]
[RequireComponent(typeof(SMBChaseState))]
[RequireComponent(typeof(SMBSingleAttackState))]
[RequireComponent(typeof(SMBVoltauroTripleAttackState))]
[RequireComponent(typeof(SMBChargeState))]
[RequireComponent(typeof(SMBLightningSummonState))]
public class VoltauroBossBehaviour : BossBehaviour
{


    private new void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        //m_StateMachine.ChangeState<SMBChaseState>();
        m_StateMachine.ChangeState<SMBLightningSummonState>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            float rng = Random.value;
            if (rng < 0.7f)
            {
                m_StateMachine.ChangeState<SMBSingleAttackState>();
            } else
            {
                m_StateMachine.ChangeState<SMBVoltauroTripleAttackState>();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            m_StateMachine.ChangeState<SMBChaseState>();
        }
    }

}
