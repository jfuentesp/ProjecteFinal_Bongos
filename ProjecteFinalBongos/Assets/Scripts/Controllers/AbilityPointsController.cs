using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPointsController : MonoBehaviour
{
    public Action onAbilityPointGained;
    private const int ABILITYPOINTSINICIAL = 0;

    [SerializeField]
    private int m_AbilityPoints = ABILITYPOINTSINICIAL;
    public int AbilityPoints => m_AbilityPoints;

    public void SetHabilityPoints(int _AbilityPoints)
    {
        m_AbilityPoints = _AbilityPoints;
    }

    public void ConsumeAbilityPoints(int _AbilityPoints)
    {
        m_AbilityPoints -= _AbilityPoints;
    }

    public void AddAbilityPoints(int _AbilityPoints)
    {
        m_AbilityPoints += _AbilityPoints;
        onAbilityPointGained?.Invoke();
    }
}
