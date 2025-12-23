using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private BoxEmotionFX boxFx;
    [SerializeField] private List<DialogueLine> lines;
    [SerializeField] private float charDelay = 0.04f;
    
    [SerializeField] private AudioSource typeWriterSource;

    private int index;
    private Coroutine typing;
    private bool isTyping;
    private string currentLine;
    public bool IsTyping => isTyping;
    
    [SerializeField] private GameObject _fair1;
    [SerializeField] private GameObject _fair2;
    [SerializeField] private GameObject _bg;
    [SerializeField] private GameObject _box;
    
    [SerializeField] private GameObject eyes;
    [SerializeField] private GameObject smile;
    
    private SpriteRenderer _bgImage;
    private Image _boxImage;
    
    private Image _eyesImage;
    private Image _smileImage;
    
    private float duration = 0.5f;
    private float durationIn = 2f;
    private byte rgbStep = 10;
    private byte rgbStep2 = 33;

    private void Start()
    {
        _bgImage = _bg.GetComponent<SpriteRenderer>();
        _boxImage = _box.GetComponent<Image>();
        
        _eyesImage = eyes.GetComponent<Image>();
        _smileImage = smile.GetComponent<Image>();
        
        index = SaveGame.Instance.dialogueIndex;
        
        var line = lines[index];
        boxFx.SetMood(line.mood);
        currentLine = SaveGame.Instance.lang == "ru" ? line.textRu : line.textEn;
        
        typing = StartCoroutine(Type());

        LoadGame(index);
        
        index++;
        if (index >= lines.Count)
            index = lines.Count - 1;
    }
    
    private void OnDisable()
    {
        StopTypeSound();
    }
    
    public void Next()
    {
        SaveGame.Instance.SaveDialogueIndex(index);

        var line = lines[index];
        index++;
        
        if (index >= lines.Count)
        {
            index = 0;
            NewGame();
        }
        
        if (index <= 168)
        {
            boxFx.SetMood(line.mood);
        }
        else if (index <= 177)
        {
            StartCoroutine(FadeCoroutine(_bgImage, rgbStep));
            StartCoroutine(FadeCoroutine(_boxImage, rgbStep));
        }
        else if (index <= 182)
        {
            StartCoroutine(FadeCoroutine(_bgImage, rgbStep2));
            StartCoroutine(FadeCoroutine(_boxImage, rgbStep2));
        }
        
        currentLine = SaveGame.Instance.lang == "ru" ? line.textRu : line.textEn;

        Debug.Log(index);
        
        CheckChangeScene(index);

        typing = StartCoroutine(Type());
    }

    private IEnumerator Type()
    {
        isTyping = true;
        text.text = "";

        StartTypeSound();
        
        foreach (char c in currentLine)
        {
            text.text += c;
            yield return new WaitForSeconds(charDelay);
        }
        
        StopTypeSound();

        isTyping = false;
        typing = null;
    }

    private void FinishInstant()
    {
        if (typing != null)
            StopCoroutine(typing);
        
        StopTypeSound();

        text.text = currentLine;
        isTyping = false;
        typing = null;
    }
    
    private void StartTypeSound()
    {
        if (!typeWriterSource) return;

        typeWriterSource.loop = true;

        // ▶ старт с 1-й секунды
        typeWriterSource.time = 1f;

        if (!typeWriterSource.isPlaying)
            typeWriterSource.Play();
    }

    private void StopTypeSound()
    {
        if (!typeWriterSource) return;

        if (typeWriterSource.isPlaying)
            typeWriterSource.Stop();

        typeWriterSource.loop = false;
    }

    private void CheckChangeScene(int index)
    {
        if (index == 169)
        {
            _fair2.SetActive(false);
        }
        if (index == 173) 
        {
            _fair1.SetActive(false);
        }
        if (index == 184) 
        {
            eyes.SetActive(true);
            StartCoroutine(FadeInCoroutine(_eyesImage));
        }
        if (index == 187) 
        {
            smile.SetActive(true);
            StartCoroutine(FadeInCoroutine(_smileImage));
        }
    }
    
    private IEnumerator FadeCoroutine(Image img, int step)
    {
        Color32 start = img.color;

        Color32 target = new Color32(
            (byte)Mathf.Max(0, start.r - step),
            (byte)Mathf.Max(0, start.g - step),
            (byte)Mathf.Max(0, start.b - step),
            start.a
        );

        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float p = t / duration;

            img.color = Color32.Lerp(start, target, p);
            yield return null;
        }

        img.color = target;
    }
    
    private IEnumerator FadeCoroutine(SpriteRenderer img, int step)
    {
        Color32 start = img.color;

        Color32 target = new Color32(
            (byte)Mathf.Max(0, start.r - step),
            (byte)Mathf.Max(0, start.g - step),
            (byte)Mathf.Max(0, start.b - step),
            start.a
        );

        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float p = t / duration;

            img.color = Color32.Lerp(start, target, p);
            yield return null;
        }

        img.color = target;
    }
    
    private IEnumerator FadeInCoroutine(Image image)
    {
        Color32 start = image.color;
        start.a = 0;

        Color32 target = image.color;
        target.a = 255;

        image.color = start;

        float t = 0f;

        while (t < durationIn)
        {
            t += Time.deltaTime;
            float p = t / durationIn;

            image.color = Color32.Lerp(start, target, p);
            yield return null;
        }

        image.color = target;
    }

    private void NewGame()
    {
        _bgImage.color = Color.white;
        _eyesImage.color = new Color32(255, 255, 255, 0);
        _smileImage.color = new Color32(255, 255, 255, 0);
        eyes.SetActive(false);
        smile.SetActive(false);
        _fair1.SetActive(true);
        _fair2.SetActive(true);
    }

    private void LoadGame(int index)
    {
        if (index >= 168)
        {
            _fair2.SetActive(false);
        }
        if (index >= 172) 
        {
            _fair1.SetActive(false);
        }
        if (index >= 177 && index <= 181)
        {
            int step = 90 + (index-176)*33;
            StartCoroutine(FadeCoroutine(_bgImage, step));
            StartCoroutine(FadeCoroutine(_boxImage, step));
        }
        else if (index >= 168 && index < 177)
        {
            StartCoroutine(FadeCoroutine(_bgImage, rgbStep*(index-167)));
            StartCoroutine(FadeCoroutine(_boxImage, rgbStep*(index-167)));
        }
        else if (index > 181)
        {
            StartCoroutine(FadeCoroutine(_bgImage, 255));
            StartCoroutine(FadeCoroutine(_boxImage, 255));
            if (index >= 183)
            {
                eyes.SetActive(true);
                StartCoroutine(FadeInCoroutine(_eyesImage));
            }
            if (index >= 186)
            {
                smile.SetActive(true);
                StartCoroutine(FadeInCoroutine(_smileImage));
            }
        }
    }
}