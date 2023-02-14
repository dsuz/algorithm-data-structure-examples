using UnityEngine;

/// <summary>
/// 
/// </summary>
public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    /// <summary>シングルトンのインスタンスを保存しておく static 変数</summary>
    static T instance;

    public static T Instance
    {
        get
        {
            if (!instance)
            {
                SetupInstance();
            }

            return instance;
        }
    }

    void Awake()
    {
        if (!instance)
        {
            instance = this.GetComponent<T>();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    static void SetupInstance()
    {
        instance = FindObjectOfType<T>();

        if (!instance)
        {
            GameObject go = new GameObject();
            instance = go.AddComponent<T>();
            go.name = instance.GetType().Name;
            DontDestroyOnLoad(go);
        }
    }
}
