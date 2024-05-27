using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Interactuable : MonoBehaviour
{

    [SerializeField] protected LayerMask layersToCheck;
    [SerializeField] protected float checkTime;
    [SerializeField] protected GameObject checkMark;
    [SerializeField] protected Material m_DefaultMaterial;
    [SerializeField] protected Material m_OutlineMaterial;
    [SerializeField] protected float m_CircleCastRange = 1;
    protected SpriteRenderer m_SpriteRenderer;
    protected bool inRange = false;
    protected bool canInteract = true;
    protected virtual void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(check());
        PJSMB.Instance.Input.FindActionMap("PlayerActions").FindAction("Interact").performed += Interact;
    }
    protected IEnumerator check() { 
        while (canInteract)
        {
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, m_CircleCastRange, transform.position, m_CircleCastRange, layersToCheck);
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Player"))
            {
                if (!inRange)
                {
                    if(checkMark)
                        checkMark.SetActive(true);
                    m_SpriteRenderer.material = m_OutlineMaterial;
                    inRange = true;
                }
            }
            else {
                if (inRange)
                {
                    if (checkMark)
                        checkMark.SetActive(false);
                    m_SpriteRenderer.material = m_DefaultMaterial;
                    inRange = false;
                }
            }
            yield return new WaitForSeconds(checkTime);
        }
        if (checkMark)
            checkMark.SetActive(false);
        m_SpriteRenderer.material = m_DefaultMaterial;
        inRange = false;
    }
    protected abstract void Interact(InputAction.CallbackContext context);

    private void OnDestroy()
    {
        StopCoroutine(check());
        if(PJSMB.Instance != null)
        {
            PJSMB.Instance.Input.FindActionMap("PlayerActions").FindAction("Interact").performed -= Interact;
        }
    }
}