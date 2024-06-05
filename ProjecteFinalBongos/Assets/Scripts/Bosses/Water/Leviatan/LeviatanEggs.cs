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
    private Transform m_TransformBoss;
    private bool m_Nacido;

    public void Init(Transform _Target, Transform parent, int _BichoPrefab, Transform leviatanTransform)
    {
        m_NumBicho = _BichoPrefab;
        m_TransformSala = parent;
        m_TransformBoss = leviatanTransform;
        if (m_TransformBoss.TryGetComponent<LeviatanBossBehaviour>(out LeviatanBossBehaviour altea))
            altea.OnBossDeath += BossMuerto;
        m_Size = new Vector2(m_SizeWideness, m_SizeLength);
        transform.localScale = m_Size;
        m_Target = _Target;
        m_Nacido = false;
        StartCoroutine(BornBicho());
    }
    private void BossMuerto()
    {
        StopAllCoroutines();
        if (m_TransformBoss.TryGetComponent<LeviatanBossBehaviour>(out LeviatanBossBehaviour leviatan))
            leviatan.CreepMuerto();
        DisableBullet();
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
                    bicho.GetComponent<BossBehaviour>().BossFinalSalaSpawn(m_Target);
                    bicho.GetComponent<PiranaBehaviour>().SetLeviatanTransform(m_TransformBoss);
                }
            }
            else
            {
                GameObject bicho = Instantiate(m_anguila, m_TransformSala);
                if (NavMesh.SamplePosition(transform.position, out NavMeshHit closestHit, 500, 1))
                {
                    bicho.transform.position = closestHit.position;
                    bicho.GetComponent<BossBehaviour>().BossFinalSalaSpawn(m_Target);
                    bicho.GetComponent<AnguilaBehaviour>().SetLeviatanTransform(m_TransformBoss);
                }
            }
        }
        if (m_TransformBoss != null)
        {
            if (m_TransformBoss.TryGetComponent<LeviatanBossBehaviour>(out LeviatanBossBehaviour leviatan))
                leviatan.OnBossDeath -= BossMuerto;
        }
        DisableBullet();
    }
}