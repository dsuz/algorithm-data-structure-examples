using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// コメント
/// </summary>
public class InputQueueDisplay : MonoBehaviour
{
    [SerializeField] FighterInput _input;
    [SerializeField] Sprite _upArrow;
    [SerializeField] Sprite _downArrow;
    [SerializeField] Sprite _rightArrow;
    [SerializeField] Sprite _leftArrow;
    [SerializeField] GridLayoutGroup _displayRoot;
    [SerializeField] Color _arrowColor = Color.red;

    void Start()
    {
        _input.InputQueue.ObserveEveryValueChanged(queue => queue.Count)
            .Subscribe(_ => UpdateQueueDisplay(_input.InputQueue.ToArray()));
    }

    void UpdateQueueDisplay(InputData[] inputArray)
    {
        foreach (Transform tx in _displayRoot.transform)
        {
            Destroy(tx.gameObject);
        }

        foreach (InputData input in inputArray)
        {
            Sprite sprite = null;

            if (input.Direction.normalized == Vector2.up)
                sprite = _upArrow;
            else if (input.Direction.normalized == Vector2.down)
                sprite = _downArrow;
            else if (input.Direction.normalized == Vector2.right)
                sprite = _rightArrow;
            else if (input.Direction.normalized == Vector2.left)
                sprite = _leftArrow;
            else
                continue;

            GameObject arrow = new GameObject(sprite.name, typeof(RectTransform), typeof(Image));
            var arrowImage = arrow.GetComponent<Image>();
            arrowImage.sprite = sprite;
            arrowImage.color = _arrowColor;
            arrow.transform.SetParent(_displayRoot.transform);
            arrow.transform.localScale = Vector3.one;
        }
    }
}
