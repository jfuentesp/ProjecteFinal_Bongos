using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BuffsPanelController : MonoBehaviour
{
    [SerializeField]
    private EstadosScriptable[] m_Estados;
    [SerializeField]
    private StatScriptables[] m_Stats;
    [SerializeField]
    private Image m_Image;

    private EstadosAlterados m_EstadoActual;
    private StatType m_StatTypeBuff;
    private float m_Cooldown;

    private void Awake()
    {
        m_Image = GetComponent<Image>();
    }

    public void InitEstado(EstadosAlterados estado, float time)
    {
        m_Image.fillAmount = 1;
        m_EstadoActual = estado;
        if (estado == EstadosAlterados.Enverinat)
            m_Cooldown = time * 4;
        else
            m_Cooldown = time;
        m_TimeElapsed = m_Cooldown;
        m_Image.sprite = m_Estados.FirstOrDefault(estadoAlterado => estadoAlterado.Estado == estado).Sprite;
    }

    public void InitStat(StatType type, float time)
    {
        m_Image.fillAmount = 1;
        m_StatTypeBuff = type;
        m_Cooldown = time;
        m_TimeElapsed = m_Cooldown;
        m_Image.sprite = m_Stats.FirstOrDefault(estadistica => estadistica.TipoStat == type).Sprite;
    }

    private float m_TimeElapsed;
    // Update is called once per frame
    void Update()
    {
        m_TimeElapsed -= Time.deltaTime;
        m_Image.fillAmount = m_TimeElapsed / m_Cooldown;
        if (m_TimeElapsed <= 0)
            transform.parent.gameObject.SetActive(false);
    }
}
