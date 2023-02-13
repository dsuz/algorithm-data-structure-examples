using UnityEngine;
using DG.Tweening;

/// <summary>
/// ドキュメントコメント
/// </summary>
public class NodeTracer : MonoBehaviour
{
    [SerializeField] float _secondsBetweenNodes = 1f;

    public void Move(int nodeId)
    {
        var nodeDic = GraphHelper.NodeDictionary;
        transform.position = nodeDic[nodeId].transform.position;
    }

    public void Move(int[] nodeIds)
    {
        Sequence seq = DOTween.Sequence();
        var nodeDic = GraphHelper.NodeDictionary;

        for (int i = 1; i < nodeIds.Length; i++)
        {
            int nodeId = nodeIds[i];
            var nextPos = nodeDic[nodeId].transform.position;
            seq.Append(transform.DOMove(nextPos, _secondsBetweenNodes)
                .SetEase(Ease.Linear));
        }
    }
}
