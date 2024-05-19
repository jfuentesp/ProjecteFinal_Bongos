using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Interactuable : MonoBehaviour
{

    [SerializeField] private LayerMask layersToCheck;
    [SerializeField] private float checkTime;
    [SerializeField] private GameObject checkMark;
    [SerializeField] private Material m_DefaultMaterial;
    [SerializeField] private Material m_OutlineMaterial;
    protected SpriteRenderer m_SpriteRenderer;
    protected bool inRange = false;
    protected virtual void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(check());
        PJSMB.Instance.Input.FindActionMap("PlayerActions").FindAction("Interact").performed += Interact;
    }
    protected IEnumerator check() { 
        while (true)
        {
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, 1, transform.position, 1, layersToCheck);
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Player"))
            {
                if (!inRange)
                {
                    checkMark.SetActive(true);
                    m_SpriteRenderer.material = m_OutlineMaterial;
                    inRange = true;
                }
            }
            else {
                if (inRange)
                {
                    checkMark.SetActive(false);
                    m_SpriteRenderer.material = m_DefaultMaterial;
                    inRange = false;
                }
            }
            yield return new WaitForSeconds(checkTime);
        }
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