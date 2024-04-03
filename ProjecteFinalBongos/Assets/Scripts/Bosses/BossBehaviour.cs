using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

[RequireComponent(typeof(FiniteStateMachine))]
[RequireComponent(typeof(SMBIdleState))]
[RequireComponent(typeof(SMBWalkState))]
[RequireComponent(typeof(SMBChaseState))]
[RequireComponent(typeof(SMBSingleAttackState))]
[RequireComponent(typeof(Rigidbody2D))]
public class BossBehaviour : MonoBehaviour
{
    private FiniteStateMachine m_StateMachine;

    //Shared parameters
    [SerializeField]
    private string m_BossName;
    [SerializeField]
    private string m_Description;
    [SerializeField]
    private Sprite m_Sprite;

    
    //en el meu estat--
    public delegate void OnPlayerEnter(GameObject obj);
    private OnPlayerEnter onPlayerEnter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //onPlayerEnter(collision.gameObject);
        if(collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Detecto al Player");
            m_StateMachine.ChangeState<SMBSingleAttackState>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            m_StateMachine.ChangeState<SMBChaseState>();
    }

    private void Awake()
    {
        m_StateMachine = GetComponent<FiniteStateMachine>();
        /* GetComponent<SMBPatrol>().OnPlayerEnter = (GameObject obj) =>
         {
             m_StateMachine.ChangeState<SMBAttack>();
         };*/
    }


    // Start is called before the first frame update
    void Start()
    {
        //m_StateMachine.ChangeState<SMBChaseState>();
        m_StateMachine.ChangeState<SMBWalkState>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    


}
