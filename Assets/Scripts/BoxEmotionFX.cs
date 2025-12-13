using UnityEngine;
using UnityEngine.UI;

public class BoxEmotionFX : MonoBehaviour
{
    private RectTransform _rect;
    private Image _image;

    private Vector3 baseScale;
    private Vector2 basePos;
    private BoxMood mood = BoxMood.Neutral;

    private float t;

    private void Awake()
    {
        if (!_rect) _rect = GetComponent<RectTransform>();
        if (!_image) _image = GetComponent<Image>();

        baseScale = _rect.localScale;
        basePos = _rect.anchoredPosition;
    }

    public void SetMood(BoxMood newMood)
    {
        mood = newMood;
        t = 0f;

        // сброс к базовому, чтобы не копились смещения
        _rect.localScale = baseScale;
        _rect.anchoredPosition = basePos;
        _image.color = Color.white;
    }

    private void Update()
    {
        t += Time.deltaTime;

        switch (mood)
        {
            case BoxMood.Neutral:
                // ничего
                break;

            case BoxMood.Anxious:
                // дрожание + лёгкое дыхание
                _rect.anchoredPosition = basePos + Random.insideUnitCircle * 1.5f;
                _rect.localScale = baseScale * (1f + Mathf.Sin(t * 6f) * 0.01f);
                break;

            case BoxMood.Angry:
                // более сильное дрожание + небольшая "красная" вспышка
                _rect.anchoredPosition = basePos + Random.insideUnitCircle * 3.5f;
                _image.color = Color.Lerp(Color.white, new Color(1f, 0.75f, 0.75f), 0.6f);
                break;

            case BoxMood.Happy:
                // мягкий bounce
                _rect.anchoredPosition = basePos + new Vector2(0, Mathf.Sin(t * 4f) * 6f);
                _rect.localScale = baseScale * (1f + Mathf.Sin(t * 4f) * 0.02f);
                break;

            case BoxMood.Tired:
                // медленное "проседание"
                _rect.anchoredPosition = Vector2.Lerp(_rect.anchoredPosition, basePos + new Vector2(0, -30f), Time.deltaTime * 0.6f);
                _image.color = Color.Lerp(_image.color, new Color(0.85f, 0.85f, 0.85f), Time.deltaTime * 1f);
                break;

            case BoxMood.Whisper:
                // еле заметное дыхание + мерцание
                _rect.localScale = baseScale * (1f + Mathf.Sin(t * 2.5f) * 0.008f);
                var c = _image.color;
                c.a = 1f - (Mathf.Sin(t * 3f) * 0.05f + 0.05f);
                _image.color = c;
                break;
        }
    }
}
