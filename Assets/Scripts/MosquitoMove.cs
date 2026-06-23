using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MosquitoMove : MonoBehaviour
{
    public Transform centerPoint;
    public float roamRadius = 5f;
    public float moveSpeed = 2f;
    public float directionChangeInterval = 0.5f;
    public float hoverStrength = 0.3f;

    private Vector2 targetPosition;
    private float timer;
    private Vector2 velocity;

    void Start()
    {
        PickNewTarget();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            PickNewTarget();
            timer = directionChangeInterval + Random.Range(-0.2f, 0.2f);
        }

        Vector2 toTarget = (targetPosition - (Vector2)transform.position);
        Vector2 desired = toTarget.normalized * moveSpeed;

        velocity = Vector2.Lerp(velocity, desired, Time.deltaTime * 5f);

        // Add jitter for erratic mosquito movement
        velocity += (Vector2)(Random.insideUnitSphere * hoverStrength);

        transform.position += (Vector3)(velocity * Time.deltaTime);

        // Flip sprite based on direction
        if (velocity.x > 0.01f)
            transform.localScale = new Vector3(1, 1, 1);
        else if (velocity.x < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void PickNewTarget()
    {
        Vector2 randomPoint = Random.insideUnitCircle * roamRadius;
        targetPosition = (Vector2)centerPoint.position + randomPoint;
    }
}
