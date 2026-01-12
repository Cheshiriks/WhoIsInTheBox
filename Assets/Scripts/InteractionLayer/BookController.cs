using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BookController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image img;
    [SerializeField] private float fadeInDuration = 1f;
    [SerializeField] private float fadeOutDuration = 0.2f;

    private Action onClicked;
    private Coroutine routine;
    private bool canClick;

    private void Awake()
    {
        if (!img) img = GetComponent<Image>();
        
        SetAlpha(0f);
        canClick = false;
    }

    public void Show(Action onClickAfterFadeIn)
    {
        onClicked = onClickAfterFadeIn;
        canClick = false;

        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(Fade(0f, 1f, fadeInDuration, () => canClick = true));
    }

    public void Hide(Action onHidden = null)
    {
        canClick = false;

        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(Fade(GetAlpha(), 0f, fadeOutDuration, onHidden));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!canClick) return;

        // чтобы не было двойных кликов
        canClick = false;
        
        Hide(() => onClicked?.Invoke());
    }

    private IEnumerator Fade(float from, float to, float dur, Action onDone)
    {
        float t = 0f;
        while (t < dur)
        {
            t += Time.deltaTime;
            float p = dur <= 0f ? 1f : Mathf.Clamp01(t / dur);
            SetAlpha(Mathf.Lerp(from, to, p));
            yield return null;
        }

        SetAlpha(to);
        routine = null;
        onDone?.Invoke();
    }

    private void SetAlpha(float a)
    {
        var c = img.color;
        c.a = a;
        img.color = c;
    }

    private float GetAlpha() => img.color.a;
}
