using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BoxButton box;
    [SerializeField] private DialogueManager dialogue;

    private void Awake()
    {
        box.OnBoxClicked += dialogue.Next;
    }
}
