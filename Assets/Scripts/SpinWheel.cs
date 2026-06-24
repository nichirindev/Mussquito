using System.Collections;
using UnityEngine;

public class SpinWheel : MonoBehaviour
{
    public Transform pointer;
    public BoxCollider2D greenZone;
    public AudioClip spinSound;
    public float spinSpeed = 360f;
    public float spinDuration = 2f;

    private bool isSpinning = false;

    void Start()
    {
        if (greenZone == null)
            greenZone = GetComponent<BoxCollider2D>();
    }

    void OnMouseDown()
    {
        if (isSpinning) return;
        if (GameManager.Instance == null || !GameManager.Instance.CanSpin()) return;

        GameManager.Instance.UseSpin();
        StartCoroutine(Spin());
    }

    IEnumerator Spin()
    {
        isSpinning = true;
        float elapsed = 0f;

        if (spinSound != null)
            AudioSource.PlayClipAtPoint(spinSound, transform.position);

        while (elapsed < spinDuration)
        {
            float t = elapsed / spinDuration;
            float currentSpeed = spinSpeed * (1f - t);
            transform.Rotate(0f, 0f, -currentSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (greenZone != null && greenZone.OverlapPoint(pointer.position))
        {
            if (GameManager.Instance != null)
                GameManager.Instance.ActivateRacket();
        }

        isSpinning = false;
    }
}
