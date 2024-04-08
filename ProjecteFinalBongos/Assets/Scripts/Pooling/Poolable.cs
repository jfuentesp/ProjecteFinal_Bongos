using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour
{
    private Pool m_Owner;

    public void SetPool(Pool ownerPool)
    {
        m_Owner = ownerPool;
    }

    private void OnDisable()
    {
        /*if (!m_Owner.ReturnElement(gameObject))
            Debug.Log(gameObject + ": Pool return error.");*/
    }
}
