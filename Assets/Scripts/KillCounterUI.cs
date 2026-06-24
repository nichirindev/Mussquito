using System.Collections;
using UnityEngine;
using TMPro;

public class KillCounterUI : MonoBehaviour
{
    public TextMeshProUGUI killText;
    public float squeezeAmount = 1.3f;
    public float squeezeDuration = 0.15f;

    private int lastKillCount;
    private RectTransform rect;
    private Coroutine squeezeCoroutine;

    void Start()
    {
        rect = killText.GetComponent<RectTransform>();
        if (GameManager.Instance != null)
            lastKillCount = GameManager.Instance.GetKillCount();
    }

    void Update()
    {
        if (GameManager.Instance == null || killText == null) return;

        int currentKills = GameManager.Instance.GetKillCount();
        if (currentKills != lastKillCount)
        {
            lastKillCount = currentKills;
            if (squeezeCoroutine != null)
                StopCoroutine(squeezeCoroutine);
            squeezeCoroutine = StartCoroutine(Squeeze());
        }

        int remaining = GameManager.Instance.GetKillsUntilSpin();
        killText.text = remaining > 0 ? remaining + " : kills to spin" : "Spin available!";
    }

    IEnumerator Squeeze()
    {
        rect.localScale = new Vector3(squeezeAmount, 1f / squeezeAmount, 1f);
        float elapsed = 0f;

        while (elapsed < squeezeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / squeezeDuration;
            float x = Mathf.Lerp(squeezeAmount, 1f, t);
            float y = Mathf.Lerp(1f / squeezeAmount, 1f, t);
            rect.localScale = new Vector3(x, y, 1f);
            yield return null;
        }

        rect.localScale = Vector3.one;
    }
}
