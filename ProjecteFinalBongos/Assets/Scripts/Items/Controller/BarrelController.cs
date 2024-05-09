using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BarrelController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_ItemPrefab;
    [SerializeField]
    private Consumable m_Consumable;
    [SerializeField]
    private Equipable m_Equipable;
    private Animator m_Animator;
    private CircleCollider2D m_CircleCollider;

    private void Awake()
    {
        m_CircleCollider = GetComponent<CircleCollider2D>();
        m_Animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Animator.Play("Spawn");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerHitBox"))
        {
            StartCoroutine(DestroyingMyself());
        }
    }

    public void SetConsumable(Consumable _Consumable)
    {
        m_Consumable = _Consumable;
    }

    public void SetEquipable(Equipable _Equipable)
    {
        m_Equipable = _Equipable;
    }

    private IEnumerator DestroyingMyself()
    {
        m_Animator.Play("Destroy");
        m_CircleCollider.enabled = false;
        yield return new WaitForSeconds(0.5f);
        if (m_Consumable)
        {
            GameObject objeto = Instantiate(m_ItemPrefab, transform.parent);
            objeto.transform.position = transform.position;
            objeto.GetComponent<ProximityItemBehaviour>().SetConsumable(m_Consumable);
            objeto.GetComponent<ProximityItemBehaviour>().SetSprite(m_Consumable.Sprite);
        }
        if (m_Equipable)
        {
            GameObject objeto = Instantiate(m_ItemPrefab, transform.parent);
            objeto.transform.position = transform.position;
            objeto.GetComponent<ProximityItemBehaviour>().SetEquipable(m_Equipable);
            objeto.GetComponent<ProximityItemBehaviour>().SetSprite(m_Equipable.Sprite);
        }
    }
}
