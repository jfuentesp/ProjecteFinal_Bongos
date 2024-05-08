using System.Collections;
using System.Collections.Generic;
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
    }

    private Coroutine m_BlinkCoroutine;
    private void OnHPBarGUIUpdate()
    {
        //m_HPBar.fillAmount = m_PlayerStats.Health.HP / m_PlayerStats.Health.HPMAX;
        m_HPBar.fillAmount = Mathf.Lerp(m_HPBar.fillAmount, m_PlayerStats.Health.HP / m_PlayerStats.Health.HPMAX, lerpSpeed);
        if (m_HPBar.fillAmount <= 0.3f)
            if(m_BlinkCoroutine == null)
                m_BlinkCoroutine = StartCoroutine(BlinkCoroutine());
    }

    private IEnumerator BlinkCoroutine()
    {
        while(m_HPBar.fillAmount <= 0.3f)
        {
            m_BackgroundHP.color = Color.Lerp(m_BackgroundHP.color, Color.red, lerpSpeed);
            yield return new WaitForSeconds(0.8f);
            m_BackgroundHP.color = Color.Lerp(m_BackgroundHP.color, Color.white, lerpSpeed);
            yield return new WaitForSeconds(0.8f);
        }
        m_BackgroundHP.color = Color.white;
    }
}
