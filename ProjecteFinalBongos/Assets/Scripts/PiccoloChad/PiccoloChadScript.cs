using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static SaveLoadGame.SaveGame;

public class PiccoloChadScript : Interactuable
{
    private Animator m_Animator;
    private int id;
    [SerializeField] private List<Consumable> m_ObjetosDisponibles = new();
    [Header("Variables Dialogo")]
    [SerializeField] private GameObject m_DialogueMark;
    private GameObject dialoguePanel;
    private TextMeshProUGUI dialogueText;
    [SerializeField, TextArea(4, 6)] private string[] m_DialogueLines;
    [SerializeField, TextArea(4, 6)] private string[] m_FinalLine;
    private multilanguaje.Multilanguage.PiccoloChanFrases m_PiccoloChanFrases;

    private Queue<string> m_FrasesParaDecir = new Queue<string>();

    [SerializeField] private float typingTime;

    private bool didDialogueStart;
    private bool isInFirstMessage;

    private Coroutine m_FirstCoroutine;
    private Coroutine m_FinalTypeCoroutine;

    private string m_FraseActual;

    private int lineIndex;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        dialoguePanel = LevelManager.Instance.DialoguePanel;
        dialogueText = LevelManager.Instance.DialogueText;
        LevelManager.Instance.onCloseShopOfPiccolo += SegundoDialogo;
    }
    protected override void Start()
    {
        base.Start();
        GetFrases();
    }

    public void GetFrases()
    {
        m_PiccoloChanFrases = GameManager.Instance.LanguageManager.GetPiccoloChadFrases();
        m_DialogueLines = m_PiccoloChanFrases.frasesIniciales;
        m_FinalLine = m_PiccoloChanFrases.frasesFinales;
    }

    public void Init()
    {
        m_ObjetosDisponibles = GetComponentInParent<PasilloTienda>().ObjetosDisponibles;
        id = GetComponentInParent<PasilloTienda>().PiccoloId;
        m_Animator.Play("Idle");
    }

    protected override void Interact(InputAction.CallbackContext context)
    {
        if (inRange)
        {
            if (!didDialogueStart)
            {
                m_Animator.Play("Treballar");
            }
            else if (isInFirstMessage && dialogueText.text == m_FraseActual)
            {
                LevelManager.Instance.OpenShop(id);
                inRange = false;
            }
            else if (isInFirstMessage && dialogueText.text != m_FraseActual)
            {
                BreakCoroutineDialogueInicial();
            }
            else if (!isInFirstMessage && dialogueText.text != m_FraseActual)
            {
                BreakCoroutineDialogueFinall();
            }
            else
            {
                if (m_FrasesParaDecir.Count == 0)
                    CloseDialogue();
                else
                    SegundoDialogo(id);
            }
        }
    }

    private void SegundoDialogo(int obj)
    {
        if (obj == id)
        {
            inRange = true;
            m_FinalTypeCoroutine = StartCoroutine(ShowLastLine());
        }
    }
    private void CloseDialogue()
    {
        dialogueText.text = string.Empty;
        m_Animator.Play("Idle");
        didDialogueStart = false;
        m_DialogueMark.SetActive(true);
        dialoguePanel.SetActive(false);
    }

    private void BreakCoroutineDialogueInicial()
    {
        StopCoroutine(m_FirstCoroutine);
        dialogueText.text = m_FraseActual;
    }
    private void BreakCoroutineDialogueFinall()
    {
        StopCoroutine(m_FinalTypeCoroutine);
        dialogueText.text = m_FraseActual;
    }

    private void StartDialogue()
    {
        inRange = true;
        isInFirstMessage = true;
        dialoguePanel.SetActive(true);
        m_DialogueMark.SetActive(false);
        didDialogueStart = true;
        lineIndex = Random.Range(0, m_DialogueLines.Length);
        m_FrasesParaDecir.Enqueue(m_DialogueLines[lineIndex]);
        foreach (string frase in m_FinalLine)
            m_FrasesParaDecir.Enqueue(frase);

        m_FirstCoroutine = StartCoroutine(ShowFirstLine());
    }

    private IEnumerator ShowLastLine()
    {
        isInFirstMessage = false;
        dialogueText.text = string.Empty;
        m_FraseActual = m_FrasesParaDecir.Dequeue();
        foreach (char ch in m_FraseActual)
        {
            dialogueText.text += ch;
            yield return new WaitForSeconds(typingTime);
        }
    }
    private IEnumerator ShowFirstLine()
    {
        dialogueText.text = string.Empty;
        m_FraseActual = m_FrasesParaDecir.Dequeue();
        foreach (char ch in m_FraseActual)
        {
            dialogueText.text += ch;
            yield return new WaitForSeconds(typingTime);
        }
    }




}
