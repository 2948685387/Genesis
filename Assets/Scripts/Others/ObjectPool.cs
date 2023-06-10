using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;
    public int poolSize;

    private List<GameObject> pool = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetObject(int index)
    {
        if (!pool[index].activeInHierarchy)
        {
            pool[index].SetActive(true);
            return pool[index];
        }
        else
        {
            return pool[index];
        }
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
    }
}
