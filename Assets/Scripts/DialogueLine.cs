using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    [TextArea] public string textRu;
    [TextArea] public string textEn;
    public BoxMood mood;
}
