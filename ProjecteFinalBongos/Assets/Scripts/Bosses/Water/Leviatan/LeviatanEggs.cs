using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LeviatanEggs : Splash
{
    [SerializeField] private GameObject m_piranha;
    [SerializeField] private GameObject m_anguila;
    private int m_NumBicho;
    private Transform m_Target;
    private Transform m_TransformSala;
    private bool m_Nacido;

    public void Init(Transform _Target, Transform parent, int _BichoPrefab)
    {
        m_NumBicho = _BichoPrefab;
        m_TransformSala = parent;
        m_Size = new Vector2(m_SizeWideness, m_SizeLength);
        transform.localScale = m_Size;
        m_Target = _Target;
        m_Nacido = false;
        StartCoroutine(BornBicho());
    }

    protected virtual IEnumerator BornBicho()
    {
        yield return new WaitForSeconds(m_TimeUntilDestroyed);
        m_Nacido = true;
        this.gameObject.SetActive(false);
    }
    private void OnDisable()
    {

        if (m_Nacido)
        {
            if (m_NumBicho == 0)
            {
                GameObject bicho = Instantiate(m_piranha, m_TransformSala);
                if (NavMesh.SamplePosition(transform.position, out NavMeshHit closestHit, 500, 1))
                {
                    bicho.transform.position = closestHit.position;
                    bicho.GetComponent<PiranaBehaviour>().Init(m_Target);
                }
            }
            else
            {
                GameObject bicho = Instantiate(m_piranha, m_TransformSala);
                if (NavMesh.SamplePosition(transform.position, out NavMeshHit closestHit, 500, 1))
                {
                    bicho.transform.position = closestHit.position;
                    bicho.GetComponent<PiranaBehaviour>().Init(m_Target);
                }
            }
        }
        DisableBullet();
    }
}