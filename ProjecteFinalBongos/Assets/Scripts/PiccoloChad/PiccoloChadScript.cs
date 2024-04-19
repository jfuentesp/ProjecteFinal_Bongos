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

    [SerializeField] private float typingTime;

    private bool isPlayerInRange;
    private bool didDialogueStart;
    private bool isInFirstDialogues;

    private Coroutine m_TypeCoroutine;
    private Coroutine m_FinalTypeCoroutine;

    private int lineIndex;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        dialoguePanel = LevelManager.Instance.DialoguePanel;
        dialogueText = LevelManager.Instance.DialogueText;
    }

    public void Init(List<LevelManager.ObjetosDisponibles> _ObjetosDisponibles)
    {
        m_ObjetosDisponibles = _ObjetosDisponibles;
        m_Animator.Play("Idle");
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.Space))
        {
            if (!didDialogueStart)
            {
                m_Animator.Play("Treballar");
            }
            else if (isInFirstDialogues)
            {
                if (dialogueText.text == m_DialogueLines[lineIndex])
                {
                    CloseDialogue();
                    FinalPhrase();
                }
                else
                {
                    BreakCoroutineDialogueInicial();
                }
            }
            else if (!isInFirstDialogues)
            {
                if (lineIndex == m_FinalLine.Length - 1)
                {
                    if (dialogueText.text == m_FinalLine[lineIndex])
                    {
                        CloseDialogue();
                    }
                    else
                    {

                    }
                }
                else
                {

                }

            }
        }
    }

    private void FinalPhrase()
    {
        throw new System.NotImplementedException();
    }

    private void CloseDialogue()
    {
        dialogueText.text = string.Empty;
        if (isInFirstDialogues)
        {
            isInFirstDialogues = false;
        }
        else
        {
            m_DialogueMark.SetActive(true);
            didDialogueStart = false;
            dialoguePanel.SetActive(false);
        }
    }

    private void BreakCoroutineDialogueInicial()
    {
        StopCoroutine(m_TypeCoroutine);
        dialogueText.text = m_DialogueLines[lineIndex];
    }
    private void BreakCoroutineDialogueFinall()
    {
        StopCoroutine(m_FinalTypeCoroutine);
        dialogueText.text = m_FinalLine[lineIndex];
    }

    private void StartDialogue()
    {
        isInFirstDialogues = true;
        didDialogueStart = true;
        dialoguePanel.SetActive(true);
        m_DialogueMark.SetActive(false);
        lineIndex = Random.Range(0, m_DialogueLines.Length);
        m_TypeCoroutine = StartCoroutine(ShowLine());
    }

    private IEnumerator ShowLine()
    {
        dialogueText.text = string.Empty;

        foreach (char ch in m_DialogueLines[lineIndex])
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
