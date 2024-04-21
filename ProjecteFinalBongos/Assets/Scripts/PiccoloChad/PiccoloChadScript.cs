using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PiccoloChadScript : MonoBehaviour
{
    [Header("Variables tienda")]
    [SerializeField] private List<LevelManager.ObjetosDisponibles> m_ObjetosDisponibles = new();
    private Animator m_Animator;

    [Header("Variables Dialogo")]
    [SerializeField] private GameObject m_DialogueMark;
    private GameObject dialoguePanel;
    private TextMeshProUGUI dialogueText;
    [SerializeField, TextArea(4, 6)] private string[] m_DialogueLines;
    [SerializeField, TextArea(4, 6)] private string[] m_FinalLine;

    private Queue<string> m_FrasesParaDecir = new Queue<string>();

    [SerializeField] private float typingTime;

    private bool isPlayerInRange;
    private bool didDialogueStart;
    private bool isInFirstMessage;
    private bool canInteract;

    private Coroutine m_FirstCoroutine;
    private Coroutine m_FinalTypeCoroutine;

    private string m_FraseActual;

    private int lineIndex;

    private int id;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        dialoguePanel = LevelManager.Instance.DialoguePanel;
        dialogueText = LevelManager.Instance.DialogueText;
        LevelManager.Instance.onCloseShopOfPiccolo += SegundoDialogo;
    }


    public void Init(List<LevelManager.ObjetosDisponibles> _ObjetosDisponibles)
    {
        m_ObjetosDisponibles = _ObjetosDisponibles;
        m_Animator.Play("Idle");
        id = LevelManager.Instance.GiveIdToPiccoloChad();
        canInteract = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canInteract)
        {
            if (isPlayerInRange && Input.GetKeyDown(KeyCode.Space))
            {
                if (!didDialogueStart)
                {
                    m_Animator.Play("Treballar");
                }
                else if (isInFirstMessage && dialogueText.text == m_FraseActual)
                {
                    LevelManager.Instance.OpenShop(id);
                    canInteract = false;
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
    }
    private void SegundoDialogo(int obj)
    {
        if(obj == id)
        {
            canInteract = true;
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
        canInteract = true;
        isInFirstMessage = true;
        dialoguePanel.SetActive(true);
        m_DialogueMark.SetActive(false);
        didDialogueStart = true;
        lineIndex = Random.Range(0, m_DialogueLines.Length);
        m_FrasesParaDecir.Enqueue(m_DialogueLines[lineIndex]);
        m_FrasesParaDecir.Enqueue(m_FinalLine[0]);
        m_FrasesParaDecir.Enqueue(m_FinalLine[1]);

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            m_DialogueMark.SetActive(true);
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            m_DialogueMark.SetActive(false);
            isPlayerInRange = false;
        }
    }
}
