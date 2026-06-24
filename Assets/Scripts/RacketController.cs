using UnityEngine;

public class RacketController : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other)
    {
        if (!gameObject.activeSelf) return;

        MosquitoDeath md = other.GetComponent<MosquitoDeath>();
        if (md != null)
            md.Die(true);
    }
}
