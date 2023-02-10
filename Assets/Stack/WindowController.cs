using System;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// ウインドウを制御するコンポーネント
/// このコンポーネントを無効にすると、ボタンを押せなくなるよう制御する
/// コンポーネントを無効→有効にすると、最後にクリックしたボタンが選択される
/// </summary>
public class WindowController : MonoBehaviour
{
    /// <summary>ボタンのプレハブ</summary>
    [SerializeField] Button _buttonPrefab;
    /// <summary>ボタンを置くルートオブジェクト</summary>
    [SerializeField] Transform _buttonRoot;
    [SerializeField] AudioSource _audio;
    /// <summary>操作した時に鳴る音</summary>
    [SerializeField] AudioClip _selectSoundEffect;
    /// <summary>最後にクリックされたボタン</summary>
    Button _lastClickedButton;

    void Start()
    {
        // ボタンを８個並べて、操作した時のコールバックを設定する
        for (int i = 0; i < 8; i++)
        {
            var button = Instantiate(_buttonPrefab, _buttonRoot);
            
            if (i == 0)
                button.Select();

            button.onClick.AddListener(() =>
            {
                _lastClickedButton = button;
                WindowManager.Instance.CreateNewWindow(button.transform.position);
            });

            button.OnSelectAsObservable().Subscribe(new MenuSfx(_audio, _selectSoundEffect));
        }
    }

    void OnDisable()
    {
        Array.ForEach(GetComponentsInChildren<Button>(), b => b.interactable = false);
    }

    void OnEnable()
    {
        Array.ForEach(GetComponentsInChildren<Button>(), b => b.interactable = true);

        if (_lastClickedButton)
            _lastClickedButton.Select();
    }
}

class MenuSfx : IObserver<BaseEventData>
{
    AudioSource _audioSource;
    AudioClip _audioClip;

    public MenuSfx(AudioSource audioSource, AudioClip audioClip)
    {
        _audioSource = audioSource;
        _audioClip = audioClip;
        _audioSource.clip = _audioClip;
    }

    public void OnNext(BaseEventData value)
    {
        _audioSource.Play();
    }

    public void OnCompleted() {}

    public void OnError(Exception error)
    {
        throw new NotImplementedException();
    }
}
