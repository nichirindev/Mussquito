using UnityEngine;
using TMPro;

public class KillCounterUI : MonoBehaviour
{
    public TextMeshProUGUI killText;

    void Update()
    {
        if (GameManager.Instance == null || killText == null) return;

        int remaining = GameManager.Instance.GetKillsUntilSpin();
        killText.text = remaining > 0 ? remaining + " : kills to spin" : "Spin available!";
    }
}
