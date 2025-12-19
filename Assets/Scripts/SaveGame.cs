using UnityEngine;
using YG;

public class SaveGame : MonoBehaviour
{
    public static SaveGame Instance;

    public int dialogueIndex = 0;
    public string lang = "ru";
    public string domain = "ru";
    
    private void Awake()
    {
        if (Instance == null)
        {
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
            Instance = this;
            //LoadDate();
            lang = YG2.envir.language;
            domain = YG2.envir.domain;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveDialogueIndex(int index)
    {
        dialogueIndex = index;
        PlayerPrefs.SetInt(SaveKeys.DialogueIndex, dialogueIndex);
        PlayerPrefs.Save();
    }

    public void LoadDate()
    {
        if (PlayerPrefs.HasKey(SaveKeys.DialogueIndex))
        {
            dialogueIndex = PlayerPrefs.GetInt(SaveKeys.DialogueIndex, 0);
        }
    }
}

public static class SaveKeys
{
    public const string DialogueIndex = "DialogueIndex";
}
