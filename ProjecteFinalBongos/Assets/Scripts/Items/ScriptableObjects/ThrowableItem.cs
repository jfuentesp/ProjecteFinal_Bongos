using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableItem : Consumable
{
    [SerializeField]
    private GameObject m_PrefabToInstantiateAndThrow;
    [SerializeField]
    private float m_Speed;
    [SerializeField]
    private Vector2 m_Direction;
    [SerializeField]
    private bool m_MoveByForce;
    public override void OnUse(GameObject usedBy)
    {
        GameObject objectToThrow = Instantiate(m_PrefabToInstantiateAndThrow);
        objectToThrow.transform.position = usedBy.transform.position;
        Rigidbody2D rigidbody = objectToThrow.GetComponent<Rigidbody2D>();
        if(m_MoveByForce)
        {
            rigidbody.AddForce(m_Direction * m_Speed, ForceMode2D.Impulse);
        }
        else
        {
            rigidbody.velocity = m_Direction * m_Speed;
        }
    }
}
