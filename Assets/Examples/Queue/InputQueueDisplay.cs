using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 入力キューの内容を画面に表示するコンポーネント
/// GridLayout を追加したオブジェクトの子に Image を追加して表示を構成している
/// </summary>
public class InputQueueDisplay : MonoBehaviour
{
    [Tooltip("入力キューを管理しているオブジェクト")]
    [SerializeField] FighterInput _input;
    [Tooltip("上方向の入力を表す画像")]
    [SerializeField] Sprite _upArrow;
    [Tooltip("下方向の入力を表す画像")]
    [SerializeField] Sprite _downArrow;
    [Tooltip("右方向の入力を表す画像")]
    [SerializeField] Sprite _rightArrow;
    [Tooltip("左方向の入力を表す画像")]
    [SerializeField] Sprite _leftArrow;
    [Tooltip("表示のルートとなるオブジェクト")]
    [SerializeField] GridLayoutGroup _displayRoot;
    [Tooltip("画像の色")]
    [SerializeField] Color _arrowColor = Color.red;

    void OnEnable()
    {
        _input.OnQueueUpdate += UpdateQueueDisplay;
    }

    void OnDisable()
    {
        _input.OnQueueUpdate -= UpdateQueueDisplay;
    }

    /// <summary>
    /// 入力内容表示を更新する
    /// </summary>
    /// <param name="inputArray"></param>
    void UpdateQueueDisplay(InputData[] inputArray)
    {
        foreach (Transform tx in _displayRoot.transform)
        {
            Destroy(tx.gameObject);
        }   // まず全て消す

        foreach (InputData input in inputArray)
        {
            Sprite sprite = null;

            // 入力内容から、どの画像を使うか選ぶ
            if (input.Direction == Vector2.up)
                sprite = _upArrow;
            else if (input.Direction == Vector2.down)
                sprite = _downArrow;
            else if (input.Direction == Vector2.right)
                sprite = _rightArrow;
            else if (input.Direction == Vector2.left)
                sprite = _leftArrow;
            else
                continue;

            // スクリプトでオブジェクトを組み上げて設置する
            GameObject arrow = new GameObject(sprite.name, typeof(RectTransform), typeof(Image));
            var arrowImage = arrow.GetComponent<Image>();
            arrowImage.sprite = sprite;
            arrowImage.color = _arrowColor;
            arrow.transform.SetParent(_displayRoot.transform);
            arrow.transform.localScale = Vector3.one;
        }
    }
}
