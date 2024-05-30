using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerHUDController : MonoBehaviour
{
    [Header("Player HUD Components")]
    private PlayerStatsController m_PlayerStats;
    private PlayerAbilitiesController m_PlayerAbilities;

    [Header("HUD Objects to Refresh")]
    [SerializeField]
    private Image m_HPBar;
    [SerializeField]
    private Image m_BackgroundHP;
    [SerializeField]
    private Image m_PlayerMiniature;

    [SerializeField]
    private TextMeshProUGUI m_Timer;
    [SerializeField]
    private BuffsPanelController m_BuffsPanel;

    [Header("HP Panel settings")]
    [SerializeField]
    private float m_SmoothSpeed;

    [Header("Buffs and debuffs panel settings")]
    [SerializeField]
    private GridLayoutGroup m_BuffsGrid;

    private List<GameObject> m_Buffs = new List<GameObject>(); 

    // Start is called before the first frame update
    void Start()
    {
        m_PlayerStats = PJSMB.Instance.PlayerStatsController;
        m_PlayerAbilities = PJSMB.Instance.PlayerAbilitiesController;
        PJSMB.Instance.PlayerEstadosController.OnApplyEstadoAlterado += UpdateEstados;
        PJSMB.Instance.PlayerStatsController.OnApplyBuff += UpdateBuff;
        GameManager.Instance.OnTimerUpdate += UpdateTimerGUI;
        PJSMB.Instance.Input.FindActionMap("PlayerActions").FindAction("LeftAbility").performed += LeftAbility;
        PJSMB.Instance.Input.FindActionMap("PlayerActions").FindAction("RightAbility").performed += RightAbility;
        FillBuffs();
    }

    private void OnDestroy()
    {
        PJSMB.Instance.PlayerEstadosController.OnApplyEstadoAlterado -= UpdateEstados;
        PJSMB.Instance.PlayerStatsController.OnApplyBuff -= UpdateBuff;
        GameManager.Instance.OnTimerUpdate -= UpdateTimerGUI;
        PJSMB.Instance.Input.FindActionMap("PlayerActions").FindAction("LeftAbility").performed -= LeftAbility;
        PJSMB.Instance.Input.FindActionMap("PlayerActions").FindAction("RightAbility").performed -= RightAbility;
    }

    private void LeftAbility(InputAction.CallbackContext context)
    {
        m_PlayerAbilities.SelectPreviousAbility();
    }
    private void RightAbility(InputAction.CallbackContext context)
    {
        m_PlayerAbilities.SelectNextAbility();
    }


    float lerpSpeed;
    void Update()
    {
        lerpSpeed = m_SmoothSpeed * Time.deltaTime;
        OnHPBarGUIUpdate();
        if (m_HPBar.fillAmount <= 0.3f)
            m_BackgroundHP.color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time * 1f, 1f));
        else
            m_BackgroundHP.color = Color.white;
        foreach(Ability ability in m_PlayerAbilities.AbilityList)
        {
            ability.UpdateRemainingCooldown(Time.deltaTime);
        }
    }

    private Coroutine m_BlinkCoroutine;
    private void OnHPBarGUIUpdate()
    {
        m_HPBar.fillAmount = Mathf.Lerp(m_HPBar.fillAmount, m_PlayerStats.Health.HP / m_PlayerStats.Health.HPMAX, lerpSpeed);
    }

    private void UpdateTimerGUI(float timer)
    {
        m_Timer.text = TimeSpan.FromSeconds(timer).ToString(@"mm\:ss");
    }

    private void FillBuffs()
    {
        for(int i = 0; i < m_BuffsGrid.transform.childCount; i++) 
        { 
            GameObject slot = m_BuffsGrid.transform.GetChild(i).gameObject;
            m_Buffs.Add(slot);
        }
    }

    private void UpdateBuff(StatType stat, float time)
    {
        GameObject slotObject = m_Buffs.FirstOrDefault(slot => slot.activeInHierarchy == false);
        if (slotObject != null)
        {
            slotObject.SetActive(true);
            slotObject.transform.GetChild(0).TryGetComponent(out BuffsPanelController buffsController);
            buffsController?.InitStat(stat, time);
        }
    }

    private void UpdateEstados(EstadosAlterados estado, float time)
    {
        GameObject slotObject = m_Buffs.FirstOrDefault(slot => slot.activeInHierarchy == false);
        if (slotObject != null)
        {
            slotObject.SetActive(true);
            slotObject.transform.GetChild(0).TryGetComponent(out BuffsPanelController buffsController);
            buffsController?.InitEstado(estado, time);
        }
    }
}
