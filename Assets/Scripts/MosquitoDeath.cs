using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MosquitoDeath : MonoBehaviour
{
    public Sprite deadSprite;
    public Sprite hitMarker;
    public AudioClip hitMarkerSound;
    public float hitMarkerDuration = 0.2f;
    public float lifetime = 10f;
    public float fadeDelay = 5f;
    public float fadeDuration = 1f;

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private MosquitoMove moveScript;
    private ObjectPool pool;
    private bool isDead = false;
    private bool diedFromLifetime = false;
    private Sprite originalSprite;
    private float lifeTimer;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        moveScript = GetComponent<MosquitoMove>();
        pool = FindObjectOfType<ObjectPool>();
        originalSprite = sr.sprite;

        ResetState();
    }

    void OnEnable()
    {
        if (sr != null)
            ResetState();
    }

    void ResetState()
    {
        isDead = false;
        diedFromLifetime = false;
        lifeTimer = lifetime;
        rb.isKinematic = true;
        rb.gravityScale = 0f;
        sr.sprite = originalSprite;
        sr.color = Color.white;
        if (moveScript != null)
            moveScript.enabled = true;
    }

    void Update()
    {
        if (isDead) return;

        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
            FadeAway();
    }

    void OnMouseDown()
    {
        if (isDead) return;
        if (GameManager.Instance != null && GameManager.Instance.IsRacketActive()) return;
        Die();
    }

    IEnumerator ShowHitMarker()
    {
        GameObject marker = new GameObject("HitMarker");
        marker.transform.position = transform.position + Vector3.up * 0.5f;
        SpriteRenderer markerSr = marker.AddComponent<SpriteRenderer>();
        markerSr.sprite = hitMarker;
        markerSr.sortingOrder = 10;

        if (hitMarkerSound != null)
            AudioSource.PlayClipAtPoint(hitMarkerSound, transform.position);

        yield return new WaitForSeconds(hitMarkerDuration);
        Destroy(marker);
    }

    public void Die(bool isRacketKill = false)
    {
        if (isDead) return;
        isDead = true;

        StartCoroutine(ShowHitMarker());

        if (GameManager.Instance != null && !isRacketKill && !diedFromLifetime)
            GameManager.Instance.AddKill();

        if (moveScript != null)
            moveScript.enabled = false;

        rb.isKinematic = false;
        rb.gravityScale = 3f;

        sr.sprite = deadSprite;

        StartCoroutine(FadeAndDestroy());
    }

    void FadeAway()
    {
        isDead = true;
        diedFromLifetime = true;

        if (moveScript != null)
            moveScript.enabled = false;

        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float elapsed = 0f;
        Color originalColor = sr.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        pool.ReturnObject(gameObject);
    }

    IEnumerator FadeAndDestroy()
    {
        yield return new WaitForSeconds(fadeDelay);

        float elapsed = 0f;
        Color originalColor = sr.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        pool.ReturnObject(gameObject);
    }
}
