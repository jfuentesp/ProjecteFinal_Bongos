using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public GameEvent Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public UnityEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        Response.Invoke();
    }
}

public abstract class GameEventListener<T> : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public GameEvent<T> Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public UnityEvent<T> Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(T parameter)
    {
        Response.Invoke(parameter);
    }
}

public abstract class GameEventListener<T0, T1> : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public GameEvent<T0, T1> Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public UnityEvent<T0, T1> Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(T0 parameter0, T1 parameter1)
    {
        Response.Invoke(parameter0, parameter1);
    }
}

public abstract class GameEventListener<T0, T1, T2> : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public GameEvent<T0, T1, T2> Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public UnityEvent<T0, T1, T2> Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(T0 parameter0, T1 parameter1, T2 parameter2)
    {
        Response.Invoke(parameter0, parameter1, parameter2);
    }
}

public abstract class GameEventListener<T0, T1, T2, T3> : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public GameEvent<T0, T1, T2, T3> Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public UnityEvent<T0, T1, T2, T3> Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(T0 parameter0, T1 parameter1, T2 parameter2, T3 parameter3)
    {
        Response.Invoke(parameter0, parameter1, parameter2, parameter3);
    }
}
