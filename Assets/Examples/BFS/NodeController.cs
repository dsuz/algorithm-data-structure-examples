using UniRx;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ドキュメントコメント
/// </summary>
public class NodeController : MonoBehaviour
{
    [SerializeField] Text _label;

    void Start()
    {
        this.ObserveEveryValueChanged(node => node.name)
            .Subscribe(name => _label.text = name);
    }

    public void OnClick()
    {
        BFSManager.Instance.Search(int.Parse(name));
    }
}
