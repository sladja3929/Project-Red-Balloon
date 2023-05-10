using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance != null) return _instance;
            
            //없으면 동일 타입의 오브젝트 탐색
            _instance = (T)FindObjectOfType(typeof(T));

            if (_instance != null) return _instance;
            
            //찾아도 없으면 오브젝트 생성
            var obj = new GameObject(typeof(T).Name, typeof(T));
            _instance = obj.GetComponent<T>();

            return _instance;
        }
    }

    protected void Awake()
    {
        if(_instance != null)
        {
            Destroy(this.gameObject);
        }

        if (transform.parent != null && transform.root != null)
        {
            DontDestroyOnLoad(transform.root.gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
