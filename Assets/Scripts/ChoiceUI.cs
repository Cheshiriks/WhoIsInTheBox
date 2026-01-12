using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text textYes;
    [SerializeField] private TMP_Text textNo;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    private Action<bool> onChosen;

    private void Awake()
    {
        if (!panel) panel = gameObject;
        
        textYes.text = SaveGame.Instance.lang == "ru" ? "ДА" : "YES";
        textNo.text = SaveGame.Instance.lang == "ru" ? "НЕТ" : "NO";

        yesButton.onClick.AddListener(() => Choose(true));
        noButton.onClick.AddListener(() => Choose(false));

        //panel.SetActive(false);
    }

    public void Show(Action<bool> onChosenCallback)
    {
        onChosen = onChosenCallback;
        panel.SetActive(true);
    }

    public void Hide()
    {
        panel.SetActive(false);
        onChosen = null;
    }

    private void Choose(bool yes)
    {
        // чтобы нельзя было нажать дважды
        panel.SetActive(false);

        var cb = onChosen;
        onChosen = null;

        cb?.Invoke(yes);
    }
}
