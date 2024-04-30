using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesGUIController : MonoBehaviour
{
    [Header("Ability GUI components")]
    [SerializeField]
    private GameObject m_AbilityHUD;


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
}
