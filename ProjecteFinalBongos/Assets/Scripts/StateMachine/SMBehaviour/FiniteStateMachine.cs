using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class FiniteStateMachine : MonoBehaviour
{
    /* Implementation of the SM Behaviour */
    IState[] m_States;

    IState m_CurrentState = null;

    private void Awake()
    {
        m_States = GetComponents<IState>();

        foreach (IState state in m_States)
            state.ExitState();
    }

    public T GetState<T>() where T : IState
    {
        return (T) m_States.First(state => state.GetType() == typeof(T));
    }

    public void ChangeState<T>() where T : IState
    {
        T state = GetState<T>();
        Assert.IsFalse(state == null);

        if(m_CurrentState != null) 
        {
            m_CurrentState.ExitState();
        }

        m_CurrentState = state;
        m_CurrentState.InitState();
    }

    
}
