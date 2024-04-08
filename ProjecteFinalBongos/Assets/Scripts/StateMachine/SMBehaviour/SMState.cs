using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SMState : MonoBehaviour, IState
{
    /* An external State for a Finite State Machine */
    public virtual void ExitState()
    {
        enabled = false;
    }

    public virtual void InitState()
    {
        enabled = true;
    }
}