using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonComponent<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    Debug.LogError($"Singleton {typeof(T)} was not found!");

                }
            }

            return _instance;
        }
    }

    // make dont destroy on load
    protected void DoNotDestroyOnLoad()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        var curObjectScripts = FindObjectsOfType<T>();

        if (curObjectScripts.Length > 1)
        {
            Debug.LogWarning($"Singleton {typeof(T)} should be the only instance!");

            Destroy(gameObject);
            return;
        }
    }
}