using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDController : MonoBehaviour
{
    [Header("Player HUD Components")]
    [SerializeField]
    private PlayerStatsController m_PlayerStats;
    [SerializeField]
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
    [SerializeField]
    private AbilityPanelController m_AbilityPanel;
    [SerializeField]
    private TimerPanelController m_TimerPanel;
    [SerializeField]
    private QuickItemsController m_QuickItemsPanel;

    [Header("HP Panel settings")]
    [SerializeField]
    private float m_SmoothSpeed;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    float lerpSpeed;
    // Update is called once per frame
    void Update()
    {
        lerpSpeed = m_SmoothSpeed * Time.deltaTime;
        OnHPBarGUIUpdate();
        if (Input.GetKeyDown(KeyCode.F))
            m_PlayerStats.Health.Damage(10);
        if (Input.GetKeyDown(KeyCode.R))
            m_PlayerStats.Health.Heal(10);

        if (m_HPBar.fillAmount <= 0.3f)
            m_BackgroundHP.color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time * 1f, 1f));
        else
            m_BackgroundHP.color = Color.white;
    }

    private Coroutine m_BlinkCoroutine;
    private void OnHPBarGUIUpdate()
    {
        m_HPBar.fillAmount = Mathf.Lerp(m_HPBar.fillAmount, m_PlayerStats.Health.HP / m_PlayerStats.Health.HPMAX, lerpSpeed);
    }
}
