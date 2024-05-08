using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Interactuable : MonoBehaviour
{

    [SerializeField] private LayerMask layersToCheck;
    [SerializeField] private float checkTime;
    [SerializeField] private GameObject checkMark;
    protected bool inRange = false;
    private void Start()
    {
        StartCoroutine(check());
        PJSMB.Instance.Input.FindActionMap("PlayerActions").FindAction("Interact").performed += Interact;
    }
    protected IEnumerator check() { 
        while (true)
        {
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, 1, transform.position, 1, layersToCheck);
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Player"))
            {
                checkMark.SetActive(true);
                inRange = true;
            }
            else {
                checkMark.SetActive(false);
                inRange = false;
            }
            yield return new WaitForSeconds(checkTime);
        }
    }
    public abstract void Interact(InputAction.CallbackContext context);
}
