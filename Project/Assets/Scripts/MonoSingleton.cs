using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    private static readonly object balanceLock = new object();
    public static T Instance
    {
        get
        {
            if(instance == null)  //双重检查锁
            {
                lock (balanceLock)
                {   
                    if (instance == null)
                    {
                        instance = FindObjectOfType<T>();
                    }
                }
            }
            return instance;
        }

    }

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
    }
    
    
}
