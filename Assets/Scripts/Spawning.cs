using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawning : MonoBehaviour
{
    public ObjectPool pool;
    public Transform spawnPoint;
    public float spawnInterval = 2f;

    private float timer;

    void Start()
    {
        timer = spawnInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Spawn();
            timer = spawnInterval;
        }
    }

    void Spawn()
    {
        GameObject obj = pool.GetObject();
        obj.transform.position = spawnPoint.position;
        obj.transform.rotation = spawnPoint.rotation;
    }
}
