using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace m17
{
    public abstract class MBState : MonoBehaviour, IState
    {
        public virtual void Init()
        {
            enabled = true;
        }

        public virtual void Exit()
        {
            enabled = false;
        }
    }
}

