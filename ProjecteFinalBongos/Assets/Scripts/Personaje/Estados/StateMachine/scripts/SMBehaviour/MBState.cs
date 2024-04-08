using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public abstract class MBState : MonoBehaviour, IState
    {
        public virtual void InitState()
        {
            enabled = true;
        }

        public virtual void ExitState()
        {
            enabled = false;
        }
    }


