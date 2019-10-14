using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem
{
    public int AmountToPool;
    public GameObject objectToPool;
    public bool ShouldExpand;
}

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool SharedInstance;

    public List<ObjectPoolItem> itemsToPool;
    List<GameObject> pooledObjects;

    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        pooledObjects = new List<GameObject>();

        foreach(ObjectPoolItem item in itemsToPool)
        {
            for (int i = 0; i < item.AmountToPool; i++)
            {
                GameObject obj = (GameObject)Instantiate(item.objectToPool);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }

        
    }

    public GameObject GetPooledObject(string tag)
    {
        for (int i = 0; i< pooledObjects.Count; i++)
        {
            if((!pooledObjects[i].activeInHierarchy)&&(pooledObjects[i].tag == tag))
            {
                return pooledObjects[i];
            }
        }
        foreach(ObjectPoolItem item in itemsToPool)
        {
            if (item.objectToPool.tag == tag)
            {
                if (item.ShouldExpand)
                {
                    GameObject obj = (GameObject)Instantiate(item.objectToPool);
                    obj.SetActive(false);
                    pooledObjects.Add(obj);
                    return obj;
                }
            }
        }
        return null;
    }
}
