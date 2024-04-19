using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private GameObject m_DialogueMark;
    private GameObject dialoguePanel;
    private TextMeshProUGUI dialogueText;
    [SerializeField, TextArea(4, 6)] private string[] m_DialogueLines;

    [SerializeField] private float typingTime;

    private bool isPlayerInRange;
    private bool didDialogueStart;

    private Coroutine m_TypeCoroutine;

    private int lineIndex;


    private void Awake()
    {
        dialoguePanel = LevelManager.Instance.DialoguePanel;
        dialogueText = LevelManager.Instance.DialogueText;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.Space))
        {
            if (!didDialogueStart)
            {
                StartDialogue();
            }
            else if (dialogueText.text == m_DialogueLines[lineIndex])
            {
                CloseDialogue();
            }
            else
            {
                BreakCoroutine();
            }
        }
    }

    private void CloseDialogue()
    {
        dialogueText.text = string.Empty;
        m_DialogueMark.SetActive(true);
        didDialogueStart = false;
        dialoguePanel.SetActive(false);
    }

    private void BreakCoroutine()
    {
        StopCoroutine(m_TypeCoroutine);
        dialogueText.text = m_DialogueLines[lineIndex];
    }

    private void StartDialogue()
    {
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
