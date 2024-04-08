using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace m17
{
    public class FiniteStateMachine2 : MonoBehaviour
    {
        IState[] m_States;

        IState m_CurrentState = null;

        private void Awake()
        {
            m_States = GetComponents<IState>();

            foreach (IState state in m_States)
                state.Exit();
        }

        public T GetState<T>() where T : IState
        {
            return (T) m_States.First(state => state.GetType() == typeof(T));
        }

        public void ChangeState<T>() where T : IState
        {
            T state = GetState<T>();
            Assert.IsFalse(state == null);

            if (m_CurrentState != null)
            {
                m_CurrentState.Exit();
            }

            m_CurrentState = state;
            m_CurrentState.Init();
        }
    }
}
