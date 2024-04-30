using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesGUIController : MonoBehaviour
{
    [Header("Ability GUI components")]
    [SerializeField]
    private GameObject m_AbilityHUD;

    [Header("Listed Abilities")]
    [SerializeField]
    private List<string> m_Tier1Abilities;
    [SerializeField]
    private List<string> m_Tier2Abilities;
    [SerializeField]
    private List<string> m_Tier3Abilities;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            m_AbilityHUD.SetActive(!m_AbilityHUD.activeSelf);
        }
    }

    public string GetRandomAbilityByTier(AbilityTierEnum tier)
    {
        int random = 0;
        string ability = null;
        switch (tier)
        {
            case AbilityTierEnum.TIER1:
                random = Random.Range(0, m_Tier1Abilities.Count);
                ability = m_Tier1Abilities[random];
                return ability;
            case AbilityTierEnum.TIER2:
                random = Random.Range(0, m_Tier1Abilities.Count);
                ability = m_Tier2Abilities[random];
                return ability;
            case AbilityTierEnum.TIER3:
                random = Random.Range(0, m_Tier1Abilities.Count);
                ability = m_Tier3Abilities[random];
                return ability;
        }
        return null;
    }
}
