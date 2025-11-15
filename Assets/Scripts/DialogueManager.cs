using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [TextArea(2, 6)]
    public List<string> dialogueLines;  // Добавь реплики в инспекторе
    public TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.03f;

    private int currentLineIndex = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    void Start()
    {
        dialogueText.text = "123";
        StartDialogue();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                // Прерываем печать и сразу показываем всю строку
                StopCoroutine(typingCoroutine);
                dialogueText.text = dialogueLines[currentLineIndex];
                isTyping = false;
            }
            else
            {
                ShowNextLine();
            }
        }
    }

    public void StartDialogue()
    {
        currentLineIndex = 0;
        ShowNextLine();
    }

    void ShowNextLine()
    {
        if (currentLineIndex < dialogueLines.Count)
        {
            typingCoroutine = StartCoroutine(TypeLine(dialogueLines[currentLineIndex]));
            currentLineIndex++;
        }
        else
        {
            dialogueText.text = "";
            // Диалог окончен — можно вызвать событие, скрыть UI и т.п.
        }
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char c in line.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }
}