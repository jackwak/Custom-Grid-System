using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;


    [System.Serializable]
    public struct Pool
    {
        public Queue<GameObject> _pooledObjects;
        public GameObject objectPrefab;
        public int poolSize;
    }

    [SerializeField] private Pool[] pools = null;

    private void Awake()
    {
        //Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        //Create pooled objects
        for (int j = 0; j < pools.Length; j++)
        {
            pools[j]._pooledObjects = new Queue<GameObject>();

            for (int i = 0; i < pools[j].poolSize; i++)
            {
                GameObject obj = Instantiate(pools[j].objectPrefab, transform);
                obj.SetActive(false);

                pools[j]._pooledObjects.Enqueue(obj);
            }
        }
    }

    public GameObject GetPooledObject(int objectType)
    {
        if (objectType >= pools.Length)
        {
            Debug.Log("Obje Bulunamadý");
            return null;
        }

        GameObject obj = pools[objectType]._pooledObjects.Dequeue();
        obj.SetActive(true);

        pools[objectType]._pooledObjects.Enqueue(obj);

        return obj;
    }
}
