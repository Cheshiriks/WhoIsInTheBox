using UnityEngine;

public class SaveGame : MonoBehaviour
{
    public static SaveGame Instance;

    public int dialogueIndex = 0;
    
    private void Awake()
    {
        if (Instance == null)
        {
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
            Instance = this;
            //LoadDate();
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
