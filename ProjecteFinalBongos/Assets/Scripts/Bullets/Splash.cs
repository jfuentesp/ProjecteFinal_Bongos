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

    [SerializeField]
    protected ObstacleStateEnum m_SplashEffectState;
    public ObstacleStateEnum SplashEffectState => m_SplashEffectState;

    public virtual void Init()
    {
        m_Size = new Vector2(m_SizeWideness, m_SizeLength);
        transform.localScale = m_Size;
        StartCoroutine(ReturnToPoolCoroutine());
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
        this.gameObject.SetActive(false);
    }
}
