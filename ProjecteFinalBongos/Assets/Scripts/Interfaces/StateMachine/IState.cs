using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    /* All States will share all this functions */
    public void InitState();
    public void ExitState();
}
