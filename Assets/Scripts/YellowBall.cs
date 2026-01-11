using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class YellowBall : MonoBehaviour, IPointerClickHandler
{
    private Action<YellowBall> onClick;

    public void Init(Action<YellowBall> callback)
    {
        onClick = callback;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke(this);
    }
}
