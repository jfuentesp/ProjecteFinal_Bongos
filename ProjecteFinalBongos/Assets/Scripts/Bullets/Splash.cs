using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Splash : MonoBehaviour
{
    [SerializeField]
    protected float m_TimeUntilDestroyed;
    [SerializeField]
    protected float m_SizeLength;
    [SerializeField]
    protected float m_SizeWideness;
    protected Vector2 m_Size;

    public virtual void Init()
    {
        m_Size = new Vector2(m_SizeWideness, m_SizeLength);
        transform.localScale = m_Size;
        StartCoroutine(ReturnToPoolCoroutine());
    }
    private void Update()
    {

    }

    protected virtual IEnumerator ReturnToPoolCoroutine()
    {
        yield return new WaitForSeconds(m_TimeUntilDestroyed);
        this.gameObject.SetActive(false);
    }
    protected void DisableBullet()
    {
        MonoBehaviour[] comps = gameObject.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour comp in comps)
        {
            comp.enabled = false;
        }
        transform.localScale = m_Size;
        GetComponent<Splash>().enabled = true;
        this.gameObject.SetActive(false);
    }
}
