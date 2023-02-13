using UnityEngine;
using DG.Tweening;

/// <summary>
/// ドキュメントコメント
/// </summary>
public class NodeTracer : MonoBehaviour
{
    [SerializeField] float _secondsBetweenNodes = 1f;

    public void Move(int[] nodeIds)
    {
        var nodeDic = GraphHelper.NodeDictionary;
        Sequence seq = DOTween.Sequence();
        int nodeId = nodeIds[0];
        transform.position = nodeDic[nodeId].transform.position;

        for (int i = 1; i < nodeIds.Length; i++)
        {
            nodeId = nodeIds[i];
            var nextPos = nodeDic[nodeId].transform.position;
            seq.Append(transform.DOMove(nextPos, _secondsBetweenNodes)
                .SetEase(Ease.Linear));
        }
    }
}
