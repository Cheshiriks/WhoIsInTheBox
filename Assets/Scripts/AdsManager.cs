using UnityEngine;
using YG;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance;

    [SerializeField] private float intervalSeconds = 70f;
    private float lastShownTime = -99999f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void TryShowInterstitial()
    {
        if (Time.unscaledTime - lastShownTime < intervalSeconds)
            return;

        Debug.Log("Реклама: " + (Time.unscaledTime - lastShownTime));
        YG2.InterstitialAdvShow();
        lastShownTime = Time.unscaledTime;
    }
}
