using UnityEngine;

public class CursorWoosh : MonoBehaviour
{
    public float moveThreshold = 0.1f;
    public float fadeSpeed = 5f;
    public float maxPitch = 1.2f;
    public float minPitch = 0.8f;
    public float maxVolume = 1f;

    private AudioSource audioSrc;
    private float targetVolume;

    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        audioSrc.volume = 0f;
        audioSrc.loop = true;
        audioSrc.Play();
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        float moveDelta = new Vector2(mouseX, mouseY).sqrMagnitude;

        if (moveDelta > moveThreshold)
        {
            float speed = Mathf.Sqrt(moveDelta);
            targetVolume = maxVolume;
            audioSrc.pitch = Mathf.Lerp(minPitch, maxPitch, speed / 5f);
        }
        else
        {
            targetVolume = 0f;
        }

        audioSrc.volume = Mathf.Lerp(audioSrc.volume, targetVolume, Time.deltaTime * fadeSpeed);
    }
}
