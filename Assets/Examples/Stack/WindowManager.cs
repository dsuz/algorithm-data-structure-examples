using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スタックでウインドウを管理するコンポーネント
/// シングルトンパターンである
/// </summary>
public class WindowManager : MonoBehaviour
{
    /// <summary>ウインドウのプレハブ</summary>
    [SerializeField] WindowController _windowPrefab;
    /// <summary>ウインドウのルートとなるオブジェクト</summary>
    [SerializeField] Transform _windowRoot;
    /// <summary>表示されているウインドウを入れておくスタック</summary>
    Stack<WindowController> _windowStack = new Stack<WindowController>();

    void Start()
    {
        CreateNewWindow(_windowRoot.position);
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
            CloseActiveWindow();
    }

    /// <summary>
    /// 新しいウインドウを開く
    /// </summary>
    /// <param name="position">ウインドウを生成する座標</param>
    public void CreateNewWindow(Vector2 position)
    {
        WindowController window = default;

        if (_windowStack.Count > 0)
        {
            // アクティブウインドウを無効にする
            window = _windowStack.Pop();
            window.enabled = false;
            _windowStack.Push(window);
        }

        // 新しいウインドウを作る
        window = Instantiate(_windowPrefab, _windowRoot);
        window.transform.SetAsLastSibling();
        window.transform.position = position;
        _windowStack.Push(window);
    }

    /// <summary>
    /// アクティブウインドウを開き、別のウインドウをアクティブにする
    /// </summary>
    public void CloseActiveWindow()
    {
        if (_windowStack.Count > 0)
        {
            Destroy(_windowStack.Pop().gameObject);
        }

        if (_windowStack.Count > 0)
        {
            var window = _windowStack.Pop();
            window.enabled = true;
            _windowStack.Push(window);
        }
    }

    #region シングルトンパターンのためのコード
    /// <summary>シングルトンのインスタンスを保存しておく static 変数</summary>
    static WindowManager instance;

    public static WindowManager Instance
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
            instance = this;
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
        instance = FindObjectOfType<WindowManager>();

        if (!instance)
        {
            GameObject go = new GameObject();
            instance = go.AddComponent<WindowManager>();
            go.name = instance.GetType().Name;
            DontDestroyOnLoad(go);
        }
    }
    #endregion
}
