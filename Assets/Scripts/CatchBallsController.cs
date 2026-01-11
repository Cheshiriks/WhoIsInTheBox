using UnityEngine;

public class CatchBallsController : MonoBehaviour
{
    [SerializeField] private YellowBall ballPrefab;
    [SerializeField] private int ballsCount = 5;
    [SerializeField] private RectTransform spawnArea;

    private int remaining;

    public void StartInteraction()
    {
        remaining = ballsCount;
        gameObject.SetActive(true);

        for (int i = 0; i < ballsCount; i++)
            SpawnBall();
    }

    private void SpawnBall()
    {
        YellowBall ball = Instantiate(ballPrefab, spawnArea);
        RectTransform ballRect = ball.GetComponent<RectTransform>();

        // 1) Случайный центр внутри spawnArea
        float cx = Random.Range(spawnArea.rect.xMin, spawnArea.rect.xMax);
        float cy = Random.Range(spawnArea.rect.yMin, spawnArea.rect.yMax);
        Vector2 center = new Vector2(cx, cy);

        // 2) Ограничим радиус, чтобы окружность не выходила за границы
        // Учтём размер шара (половина ширины/высоты)
        float halfW = ballRect.rect.width * 0.5f;
        float halfH = ballRect.rect.height * 0.5f;
        float margin = Mathf.Max(halfW, halfH);

        float maxRadiusX = Mathf.Min(center.x - spawnArea.rect.xMin, spawnArea.rect.xMax - center.x) - margin;
        float maxRadiusY = Mathf.Min(center.y - spawnArea.rect.yMin, spawnArea.rect.yMax - center.y) - margin;
        float maxRadius = Mathf.Min(maxRadiusX, maxRadiusY);

        // если центр слишком близко к краю — maxRadius может быть <= 0
        maxRadius = Mathf.Max(5f, maxRadius);

        float radius = Random.Range(10f, Mathf.Min(80f, maxRadius)); // подбери цифры под свою игру

        // 3) Скорость и стартовый угол
        float speed = Random.Range(1.5f, 3.5f);     // радиан/сек
        if (Random.value < 0.5f) speed = -speed;    // половина в другую сторону
        float startAngle = Random.Range(0f, Mathf.PI * 2f);

        // 4) Запускаем движение
        var mover = ball.GetComponent<CircleMoverUI>();
        mover.InitCircle(center, radius, speed, startAngle);

        // 5) Клик
        ball.Init(OnBallClicked);
    }

    private void OnBallClicked(YellowBall ball)
    {
        Destroy(ball.gameObject);
        remaining--;

        if (remaining <= 0)
            FinishInteraction();
    }

    private void FinishInteraction()
    {
        gameObject.SetActive(false);
        GameFlow.State = GameState.Dialogue;

        // возвращаемся к диалогу
        FindObjectOfType<DialogueManager>().Next();
    }
}
