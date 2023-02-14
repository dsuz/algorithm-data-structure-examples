using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// ドキュメントコメント
/// </summary>
public class BFSNodeController : MonoBehaviour
{
    [SerializeField] Text _label;
    int _nodeId;

    public int NodeId
    {
        get => _nodeId;

        set
        {
            _nodeId = value;
            _label.text = _nodeId.ToString();
        }
    }

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
        }   // 右（中）クリックならば探索せずにワープする
    }
}
