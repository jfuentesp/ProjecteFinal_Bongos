using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HabilityPointsController : MonoBehaviour
{
    private const int HABILITYPOINTSINICIAL = 0;

    [SerializeField]
    private int m_HabilityPoints = HABILITYPOINTSINICIAL;
    public int HabilityPoints => m_HabilityPoints;

    public void SetHabilityPoints(int _HabilityPoints)
    {
        m_HabilityPoints = _HabilityPoints;
    }

    public void ConsumeAbilityPoints(int _AbilityPoints)
    {
        m_HabilityPoints -= _AbilityPoints;
    }

    public void AddAbilityPoints(int _AbilityPoints)
    {
        m_HabilityPoints += _AbilityPoints;
    }
}
