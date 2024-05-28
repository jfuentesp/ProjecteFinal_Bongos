using System.Collections;
using UnityEngine;

public class PlaceableController : MonoBehaviour
{
    private SpriteRenderer m_SpriteRenderer;
    private Animator m_Animator;
    private PlaceableEnum m_PlaceableType;
    private Consumable m_Consumable;
    private Rigidbody2D m_Rigidbody;
    [SerializeField]
    private ExplosionBehaviour m_ExplosionBehaviour;

    private bool m_FirstPush;

    private void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_FirstPush = false;
    }

    private Coroutine m_Coroutine;
    public void Initialize(PlaceableEnum type, Consumable consumable)
    {
        m_PlaceableType = type;
        m_Consumable = consumable;
        m_SpriteRenderer.sprite = consumable.Sprite;
        switch(type)
        {
            case PlaceableEnum.BOMB:
                m_Coroutine = StartCoroutine(Bomb());
                m_ExplosionBehaviour.InitializeExplosion(ExplosionType.EXPLOSION);
                break;
            case PlaceableEnum.TRAPEXPLOSION:
                m_Coroutine = StartCoroutine(ExplosiveTrap());
                m_ExplosionBehaviour.InitializeExplosion(ExplosionType.EXPLOSION);
                break;
            case PlaceableEnum.TRAPPOISON:
                m_Coroutine = StartCoroutine(PoisionTrap());
                m_ExplosionBehaviour.InitializeExplosion(ExplosionType.POISON);
                break;
            case PlaceableEnum.TRAPWATER:
                m_Coroutine = StartCoroutine(WaterTrap());
                m_ExplosionBehaviour.InitializeExplosion(ExplosionType.SLOW);
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !m_FirstPush)
            return;

        switch (m_PlaceableType) 
        {
            case PlaceableEnum.BOMB:
                if (m_FirstPush)
                    m_Animator.Play("Explosion");
                m_FirstPush = true;
                break;
            case PlaceableEnum.TRAPEXPLOSION:
                m_Animator.Play("Explosion");
                break;
            case PlaceableEnum.TRAPPOISON:
                m_Animator.Play("PoisonCloud");
                break;
            case PlaceableEnum.TRAPWATER:
                m_Animator.Play("WaterWave");
                break;
        }   
    }

    private IEnumerator Bomb()
    {
        m_Animator.Play("Bomb");
        yield return new WaitForSeconds(3f);
        m_Animator.Play("Explosion");
    }

    private IEnumerator ExplosiveTrap()
    {
        m_Animator.Play("ExplosionTrap");
        m_Rigidbody.simulated = false;
        yield return new WaitForSeconds(20f);
        m_Animator.Play("Explosion");
    }

    private IEnumerator PoisionTrap()
    {
        m_Animator.Play("PoisonTrap");
        m_Rigidbody.simulated = false;
        yield return new WaitForSeconds(20f);
        m_Animator.Play("PoisonCloud");
    }

    private IEnumerator WaterTrap()
    {
        m_Animator.Play("WaterTrap");
        m_Rigidbody.simulated = false;
        yield return new WaitForSeconds(20f);
        m_Animator.Play("WaterWave");
    }

    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
