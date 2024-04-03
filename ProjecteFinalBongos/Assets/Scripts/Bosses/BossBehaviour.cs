using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

[RequireComponent(typeof(FiniteStateMachine))]
[RequireComponent(typeof(Rigidbody2D))]
public class BossBehaviour : MonoBehaviour
{
    [Header("Boss parameters")]
    [SerializeField]
    private string m_BossName;
    public string BossName => m_BossName;
    [SerializeField]
    private string m_Description;
    public string Description => m_Description;
    [SerializeField]
    private float m_MaxHP;
    public float MaxHP => m_MaxHP;
    [SerializeField]
    private Sprite m_Sprite;

    
    //en el meu estat--
    public delegate void OnPlayerEnter(GameObject obj);
    private OnPlayerEnter onPlayerEnter;

    private void Awake()
    {
        /* GetComponent<SMBPatrol>().OnPlayerEnter = (GameObject obj) =>
         {
             m_StateMachine.ChangeState<SMBAttack>();
         };*/
    }
}
