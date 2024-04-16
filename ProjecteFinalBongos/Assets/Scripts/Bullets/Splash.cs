using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Splash : MonoBehaviour
{

    /*
     * Según lo hablado con el Héctor, en caso de querer usar dos efectos a la vez habría que cambiar el sistema del Splash.
     * Ahora mismo está funcionando con Linq, pero podemos rehacerlo para que use un Manager (Splash) y que use clases normales o
     * interficies (los hijos) no monobehaviour. El manager comprueba qué efectos inicializa y los hijos se encargan de darle los
     * comporotamientos.
     */

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

    public void SetObstacleEffect(ObstacleStateEnum state)
    {
        m_SplashEffectState = state;
    }
}
