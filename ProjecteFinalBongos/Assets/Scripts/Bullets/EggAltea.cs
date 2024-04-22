using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggAltea : Splash
{
    [SerializeField]
    private GameObject m_SerpientePrefab;

    private Transform m_Target;
    private Transform m_TransformSala;
    private bool m_Nacido;

    public void Init(Transform _Target, Transform parent)
    {
        m_TransformSala = parent;
        m_Size = new Vector2(m_SizeWideness, m_SizeLength);
        transform.localScale = m_Size;
        m_Target = _Target;
        m_Nacido = false;
        StartCoroutine(BornSNake());
    }
    protected virtual IEnumerator BornSNake()
    {
        yield return new WaitForSeconds(m_TimeUntilDestroyed);
        m_Nacido = true;
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        
    }

    private void OnDisable()
    {
        if (m_Nacido)
        {
            GameObject Serpiente = Instantiate(m_SerpientePrefab, m_TransformSala);
            Serpiente.transform.position = transform.position;
            Serpiente.GetComponent<EnemySnake>().Init(m_Target);
        }
        
        DisableBullet();
    }
}
