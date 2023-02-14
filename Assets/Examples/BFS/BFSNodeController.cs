using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// ノードの機能を提供するコンポーネント
/// </summary>
public class BFSNodeController : MonoBehaviour
{
    [SerializeField] Text _label;
    int _nodeId;

    /// <summary>
    /// ノード番号。
    /// ノード番号を代入すると、ラベルの数字がノード番号に変わる。
    /// </summary>
    public int NodeId
    {
        get => _nodeId;

        set
        {
            _nodeId = value;
            _label.text = _nodeId.ToString();
        }
    }

    /// <summary>
    /// クリックされた時の挙動を記述する
    /// </summary>
    /// <param name="eventData"></param>
    public void OnClick(BaseEventData eventData)
    {
        var ped = (PointerEventData) eventData;
        
        if (ped.button == PointerEventData.InputButton.Left)
        {
            BFSManager.Instance.Search(_nodeId);
        }   // 左クリックで探索開始する
        else
        {
            BFSManager.Instance.CurrentNode = _nodeId;
        }   // 右（中）クリックならば探索せずプレイヤーがそのノードにワープする
    }
}
