using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

// コメント
public class FighterInput : MonoBehaviour
{
    [Tooltip("コマンド入力の有効期間（秒）")]
    [SerializeField] float _timeInQueue = 0.3f;
    [Tooltip("入力キューをクリーンアップする間隔（秒）")]
    [SerializeField] float _cleanupQueueInterval = 0.1f;
    Queue<InputData> _inputQueue = new Queue<InputData>();
    Coroutine _cleanupQueue;

    public Queue<InputData> InputQueue => _inputQueue;

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) 
        {
            InputData inputData = new InputData(context.ReadValue<Vector2>(), Time.timeSinceLevelLoad);
            _inputQueue.Enqueue(inputData);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Debug.Log("Attack");
        }
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

public class InputData
{
    Vector2 _direction;
    float _time;

    public Vector2 Direction => _direction;
    public float Time => _time;

    InputData() { }

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