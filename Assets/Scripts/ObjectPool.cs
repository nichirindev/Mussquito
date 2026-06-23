using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject[] prefabs;
    public int poolSize = 11;

    private List<GameObject> pool = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            int index = i % prefabs.Length;
            GameObject obj = Instantiate(prefabs[index]);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetObject()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        // Expand pool with random prefab
        int randomIndex = Random.Range(0, prefabs.Length);
        GameObject newObj = Instantiate(prefabs[randomIndex]);
        pool.Add(newObj);
        return newObj;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
    }
}
