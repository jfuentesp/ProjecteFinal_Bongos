using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class TelegraphShaderController : MonoBehaviour
{
    [SerializeField, Range(-0.5f, 1.5f)]
    private float m_ShaderPuntoFinal = 1.2f;
    private float m_ShaderPuntoInicial = .8f;
    private Material m_MaterialPadre;
    private float m_Tiempo;
    private float currentValue;      

    private float elapsedTime = 0.0f; // Tiempo transcurrido

    Coroutine m_TelegraphAttackCoroutine;

    private void Awake()
    {
        m_MaterialPadre = GetComponent<SpriteRenderer>().material;
    }

    public void Init(float temps)
    {
        print("Dale");
        m_Tiempo = temps;
        currentValue = m_ShaderPuntoInicial;
        elapsedTime = 0.0f;
        if (m_TelegraphAttackCoroutine != null)
        {
            StopCoroutine(m_TelegraphAttackCoroutine);
        }

        m_TelegraphAttackCoroutine = StartCoroutine(Fundido());
    }

    private IEnumerator Fundido()
    {
        bool pepino = false;
        while (!pepino)
        {
            elapsedTime += m_Tiempo/100;

            currentValue = Mathf.MoveTowards(currentValue, m_ShaderPuntoFinal, .4f / 100f);
            print(currentValue);

            m_MaterialPadre.SetFloat("_Dissolve", currentValue);

            if (elapsedTime >= m_Tiempo)
            {
                pepino= true;
            }

            yield return new WaitForSeconds(m_Tiempo/100);
        }
    }
}
