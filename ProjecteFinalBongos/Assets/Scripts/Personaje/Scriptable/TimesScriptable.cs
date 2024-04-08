using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Time", menuName ="Time/Times")]
public class TimesScriptable : ScriptableObject
{
    public float m_WetTime;
    public float m_StrengthTime;
    public float m_VelocityTime;
    public float m_WrathTime;
    public float m_BurnTime;
    public float m_PoisonTime;
    public float m_InvencibleTime;
    public float m_ParalizedTime;
    public float m_StunTime;
    public float m_SleepTime;
    public float m_StuckTime;
}
