using UnityEngine;
using UnityEngine.UI;

public class BoxEmotionFX : MonoBehaviour
{
    private RectTransform _rect;
    private Image _image;

    private Vector3 _baseScale;
    private Vector2 _basePos;
    private BoxMood _mood = BoxMood.Neutral;

    private float _time;

    private void Awake()
    {
        if (!_rect) _rect = GetComponent<RectTransform>();
        if (!_image) _image = GetComponent<Image>();

        _baseScale = _rect.localScale;
        _basePos = _rect.anchoredPosition;
    }

    public void SetMood(BoxMood newMood)
    {
        _mood = newMood;
        _time = 0f;

        // сброс к базовому, чтобы не копились смещения
        _rect.localScale = _baseScale;
        _rect.anchoredPosition = _basePos;
        _image.color = Color.white;
    }

    private void Update()
    {
        _time += Time.deltaTime;

        switch (_mood)
        {
            case BoxMood.Neutral:
                // ничего
                break;

            case BoxMood.Anxious:
                // дрожание + лёгкое дыхание
                _rect.anchoredPosition = _basePos + Random.insideUnitCircle * 1.5f;
                _rect.localScale = _baseScale * (1f + Mathf.Sin(_time * 6f) * 0.01f);
                break;

            case BoxMood.Angry:
                // более сильное дрожание + небольшая "красная" вспышка
                _rect.anchoredPosition = _basePos + Random.insideUnitCircle * 3.5f;
                _image.color = Color.Lerp(Color.white, new Color(1f, 0.75f, 0.75f), 0.6f);
                break;

            case BoxMood.Happy:
                // мягкий bounce
                _rect.anchoredPosition = _basePos + new Vector2(0, Mathf.Sin(_time * 4f) * 6f);
                _rect.localScale = _baseScale * (1f + Mathf.Sin(_time * 4f) * 0.02f);
                break;

            case BoxMood.Tired:
                // медленное "проседание"
                _rect.anchoredPosition = Vector2.Lerp(_rect.anchoredPosition, _basePos + new Vector2(0, -30f), Time.deltaTime * 0.6f);
                _image.color = Color.Lerp(_image.color, new Color(0.85f, 0.85f, 0.85f), Time.deltaTime * 1f);
                break;

            case BoxMood.Whisper:
                // еле заметное дыхание + мерцание
                _rect.localScale = _baseScale * (1f + Mathf.Sin(_time * 2.5f) * 0.008f);
                var c = _image.color;
                c.a = 1f - (Mathf.Sin(_time * 3f) * 0.05f + 0.05f);
                _image.color = c;
                break;
        }
    }
}
