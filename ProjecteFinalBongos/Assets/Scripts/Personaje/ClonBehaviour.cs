using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClonBehaviour : MonoBehaviour
{

    private Animator m_Animator;
    private Rigidbody2D m_rb;
    [SerializeField] private float m_ClonSpeed = 10f;
    [SerializeField] private LayerMask m_LayerMask;
    [SerializeField] private float m_DirectionTime;
    [SerializeField] private int Steps = 5;
    [SerializeField] private GameEvent ClonEvent;
    private bool m_Alive;
    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_Alive = true;
    }

    public void Init(Vector2 direction)
    {
        ClonEvent.Raise();
        if (direction == Vector2.up) {
            m_Animator.Play("walkUp");
        }
        else if (direction == Vector2.down)
        {
            m_Animator.Play("walkDown");
        }
        else if(direction == Vector2.left ||  direction == new Vector2(-1, -1) || direction == new Vector2(-1, 1)) {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            m_Animator.Play("walkPlayer");
        }
        else if (direction == Vector2.right || direction == new Vector2(1, 1) ||  direction == new Vector2(1, -1))
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            m_Animator.Play("walkPlayer");
        }
        m_rb.velocity = direction.normalized * m_ClonSpeed;
        StartCoroutine(Merge());
        
    }
    private float m_TimeChangeDirection = 0;
    private void FixedUpdate()
    {
        m_TimeChangeDirection += Time.fixedDeltaTime;
        if (m_TimeChangeDirection > m_DirectionTime)
        {
            RandomWalk();
            m_TimeChangeDirection = 0;
        }
    }
    private void RandomWalk()
    {
        Vector2 direccion = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direccion, 7, m_LayerMask);
        if (hit.collider != null)
        {
            RandomWalk();
        }
        else
        {
            if (direccion.y > 0 && direccion.x == 0)
            {
                m_Animator.Play("walkUp");
            }
            else if (direccion.y < 0 && direccion.x == 0)
            {
                m_Animator.Play("walkDown");
            }
            else if (direccion.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                m_Animator.Play("walkPlayer");
            }
            else if (direccion.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                m_Animator.Play("walkPlayer");
            }
            m_rb.velocity = direccion.normalized * m_ClonSpeed;


        }
    }

    private IEnumerator Merge()
    {
        while (Steps > 0)
        {
            Steps--;
            yield return new WaitForSeconds(1f);
            if (Steps <= 0)
            {
                Finish();
            }
        }

    }

    public void Finish() {
        ClonEvent.Raise();
        StopAllCoroutines();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_Alive)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("BossHitBox"))
            {
                m_Alive = false;
                Finish();
            }
        }
    }
}
