using UnityEngine;

public class CircleMoverUI : MonoBehaviour
{
    private RectTransform rect;

    private Vector2 center;
    private float radius;
    private float angularSpeed; // рад/сек
    private float angle;        // текущий угол

    private void Awake()
    {
        if (!rect) rect = GetComponent<RectTransform>();
    }

    public void InitCircle(Vector2 centerAnchoredPos, float radiusPx, float angularSpeedRad, float startAngleRad)
    {
        center = centerAnchoredPos;
        radius = radiusPx;
        angularSpeed = angularSpeedRad;
        angle = startAngleRad;

        // стартовая позиция
        rect.anchoredPosition = center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
    }

    private void Update()
    {
        angle += angularSpeed * Time.deltaTime;

        rect.anchoredPosition = center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
    }
}
