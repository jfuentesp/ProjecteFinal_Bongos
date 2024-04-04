using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [Header("Object to pool")]
    [SerializeField]
    private GameObject m_ObjectToPool;

    [Header("Number of instantiated objects")]
    [SerializeField]
    private int m_AmountToPool;

    private List<GameObject> m_PooledObjects = new List<GameObject>();

    private void Awake()
    {
        
    }

    private void Start()
    {
        m_PooledObjects = new List<GameObject>();
        GameObject tempInstance;
        for(int i = 0; i < m_AmountToPool; i++)
        {
            tempInstance = Instantiate(m_ObjectToPool, transform);
            tempInstance.SetActive(false);
            m_PooledObjects.Add(tempInstance);
        }
    }

    public GameObject GetElement()
    {
        foreach(GameObject obj in m_PooledObjects)
        {
            if (!obj.activeInHierarchy)
                return obj;
        }
        return null;
    }

    public bool ReturnElement(GameObject item)
    {
        if(m_PooledObjects.Contains(item))
        {
            if(item.activeInHierarchy)
            {
                item.SetActive(false);
                return true;
            }
        }
        return false;
    }
}
