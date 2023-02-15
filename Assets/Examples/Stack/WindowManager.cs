using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スタックでウインドウを管理するコンポーネント
/// シングルトンパターンのコンポーネントとして作ってある
/// </summary>
public class WindowManager : SingletonMonoBehaviour<WindowManager>
{
    [Tooltip("ウインドウのプレハブ")]
    [SerializeField] WindowController _windowPrefab;
    [Tooltip("ウインドウのルートとなるオブジェクト")]
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
}
