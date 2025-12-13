using UnityEngine;
using UnityEngine.EventSystems;

public class BoxButton : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler,
    IPointerClickHandler
{
    private RectTransform _rect;
    [SerializeField] private float pressedScale = 0.95f;
    [SerializeField] private float scaleSpeed = 20f;
    
    [SerializeField] private DialogueManager dialogue;

    private Vector3 normalScale;
    private Vector3 targetScale;

    public System.Action OnBoxClicked;

    private void Awake()
    {
        if (!_rect) _rect = GetComponent<RectTransform>();
        normalScale = _rect.localScale;
        targetScale = normalScale;
    }

    private void Update()
    {
        _rect.localScale = Vector3.Lerp(
            _rect.localScale,
            targetScale,
            Time.deltaTime * scaleSpeed
        );
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (dialogue != null && dialogue.IsTyping) return;
        
        targetScale = normalScale * pressedScale;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (dialogue != null && dialogue.IsTyping) return;
        
        targetScale = normalScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (dialogue != null && dialogue.IsTyping)
            return; // <-- клики не фиксируются
        
        OnBoxClicked?.Invoke();
    }
}