using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 入力キューを管理するコンポーネント
/// </summary>
public class FighterInput : MonoBehaviour
{
    [Tooltip("コマンド入力の有効期間（秒）")]
    [SerializeField] float _timeInQueue = 0.3f;
    [Tooltip("入力キューをクリーンアップする間隔（秒）")]
    [SerializeField] float _cleanupQueueInterval = 0.1f;
    Queue<InputData> _inputQueue = new Queue<InputData>();
    Coroutine _cleanupQueue;
    FighterController _fighter;

    public Queue<InputData> InputQueue => _inputQueue;

    /// <summary>
    /// 方向入力をハンドルするメソッドとして Input System から呼ばれる
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) 
        {
            InputData inputData = new InputData(context.ReadValue<Vector2>(), Time.timeSinceLevelLoad);
            _inputQueue.Enqueue(inputData);
        }
    }

    /// <summary>
    /// 攻撃ボタンの入力をハンドルするメソッドとして Input System から呼ばれる
    /// </summary>
    /// <param name="context"></param>
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            var command = CheckCommand();
            InvokeFighter(command);
            _inputQueue.Clear();
        }
    }

    /// <summary>
    /// 引数で指定されたメソッド（引数なし）を FighterController クラスから呼び出す
    /// </summary>
    /// <param name="command"></param>
    void InvokeFighter(string command)
    {
        if (!_fighter)
            _fighter = FindFighter();

        if (_fighter)
        {
            Type fighterType = _fighter.GetType();
            MethodInfo mi = fighterType.GetMethod(command);

            if (mi != null)
                mi.Invoke(_fighter, null);
            else
                Debug.LogError($"コマンドで指定されているメソッド {command} が見つかりません。");
        }
        else
            Debug.LogError($"{_fighter.GetType().Name} が見つかりません。");
    }

    /// <summary>
    /// FighterController を見つける処理。
    /// FighterController コンポーネントは Player タグをつけた GameObject に追加しておく必要がある
    /// </summary>
    /// <returns></returns>
    FighterController FindFighter()
    {
        FighterController fighter = null;
        GameObject go = GameObject.FindGameObjectWithTag("Player");

        if (go)
            fighter = go.GetComponent<FighterController>();

        return fighter;
    }

    /// <summary>
    /// 入力キューとコマンドリストを比較して、コマンドと一致しているか調べる
    /// </summary>
    /// <returns>一致したコマンドの名前</returns>
    string CheckCommand()
    {
        string cmdString = "";
        var cmdList = CommandDatabase.Instance.CommandList;
        var inputArray = _inputQueue.ToArray();

        foreach (var cmd in cmdList)
        {
            var cmdQueue = new Queue<Vector2>(cmd.Value);

            for (int i = 0; i < inputArray.Length && cmdQueue.Count > 0; i++)
            {
                if (inputArray[i].Direction == cmdQueue.Peek())
                {
                    cmdQueue.Dequeue();
                }
            }

            if (cmdQueue.Count == 0)
            {
                cmdString = cmd.Key;
                break;
            }   // コマンド成立
        }

        return cmdString;
    }

    /// <summary>
    /// 有効期限の切れた入力をキューから削除する
    /// </summary>
    IEnumerator CleanUpInputQueue()
    {
        while (true)
        {
            while (_inputQueue.Count > 0)
            {
                if (_inputQueue.Peek().Time + _timeInQueue < Time.timeSinceLevelLoad)
                {
                    _inputQueue.Dequeue();
                }
                else
                    break;
            }

            yield return new WaitForSeconds(_cleanupQueueInterval);
        }
    }

    void OnEnable()
    {
        _cleanupQueue = StartCoroutine(CleanUpInputQueue());
    }

    void OnDisable()
    {
        StopCoroutine(_cleanupQueue);
    }
}

/// <summary>
/// 入力キューのデータ型となるクラス
/// 入力された方向と、いつ入力されたかを記録する
/// 構造体にしたかったが、デフォルトコンストラクターの利用を禁止するためクラスとして作っている
/// </summary>
public class InputData
{
    Vector2 _direction;
    float _time;

    public Vector2 Direction => _direction;
    public float Time => _time;

    private InputData() { } // デフォルトコンストラクターの利用を禁止している

    public InputData(Vector2 direction, float time)
    {
        _direction = direction;
        _time = time;
    }

    public override string ToString()
    {
        return $"{_direction.ToString()}, {_time.ToString("0.000")}";
    }
}