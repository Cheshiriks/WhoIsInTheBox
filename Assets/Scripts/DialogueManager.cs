using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private BoxEmotionFX boxFx;
    [SerializeField] private List<DialogueLine> lines;
    [SerializeField] private float charDelay = 0.04f;

    private int index;
    private Coroutine typing;
    private bool isTyping;
    private string currentLine;
    public bool IsTyping => isTyping;

    private void Start()
    {
        index = SaveGame.Instance.dialogueIndex;
        
        var line = lines[index];
        boxFx.SetMood(line.mood);
        currentLine = line.text;
        
        typing = StartCoroutine(Type());
        
        index++;
        if (index >= lines.Count)
            index = lines.Count - 1;
    }
    
    public void Next()
    {
        SaveGame.Instance.SaveDialogueIndex(index);

        var line = lines[index];
        index++;

        if (index >= lines.Count)
            index = lines.Count - 1;
        
        boxFx.SetMood(line.mood);
        currentLine = line.text;

        typing = StartCoroutine(Type());
    }

    private IEnumerator Type()
    {
        isTyping = true;
        text.text = "";

        foreach (char c in currentLine)
        {
            text.text += c;
            yield return new WaitForSeconds(charDelay);
        }

        isTyping = false;
        typing = null;
    }

    private void FinishInstant()
    {
        if (typing != null)
            StopCoroutine(typing);

        text.text = currentLine;
        isTyping = false;
        typing = null;
    }
}