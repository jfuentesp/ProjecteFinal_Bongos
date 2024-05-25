using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FountainScript : Interactuable
{
    [SerializeField] private Color m_ColorOutline;
    [SerializeField] private float m_Thickness;
    [SerializeField] private float m_Heal;
    private bool m_AlreadyActivated;

    protected override void Start()
    {
        base.Start();
        m_AlreadyActivated = false;
        m_OutlineMaterial.SetFloat("_Thickness", m_Thickness);
        m_OutlineMaterial.SetColor("_Color", m_ColorOutline);
        m_SpriteRenderer.material = m_DefaultMaterial;
    }

    protected override void Interact(InputAction.CallbackContext context)
    {
        if (inRange && !m_AlreadyActivated)
        {
            if(PJSMB.Instance.GetComponent<HealthController>().HP < PJSMB.Instance.GetComponent<HealthController>().HPMAX)
            {
                m_AlreadyActivated = true;
                PJSMB.Instance.Heal(m_Heal);
                canInteract = false;
            }
        }
    }
}
