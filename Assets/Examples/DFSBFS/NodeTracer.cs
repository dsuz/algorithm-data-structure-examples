using UnityEngine;
using DG.Tweening;
using System;

/// <summary>
/// ノード間を移動する機能を提供するコンポーネント
/// </summary>
public class NodeTracer : MonoBehaviour
{
    [Tooltip("ノード間をアニメーションで移動する時にかかる時間（秒）")]
    [SerializeField] float _secondsBetweenNodes = 1f;

    /// <summary>
    /// 指定されたノードにワープする。
    /// 現在のノードと指定されたノードがつながっていなくても関係なくワープする。
    /// </summary>
    /// <param name="nodeId"></param>
    public void Move(int nodeId)
    {
        var nodePositions = GraphLoader.Instance.NodePositions;
        transform.position = nodePositions[nodeId];
    }

    /// <summary>
    /// 配列として渡されたノードを順番に移動する。
    /// DOTween を使ってアニメーションで移動する。
    /// </summary>
    /// <param name="nodeIds">ノードIDが格納された配列。配列の順番にノードを移動する。</param>
    /// <param name="callback">移動が完了した時に実行するコールバック</param>
    public void Move(int[] nodeIds, Action callback)
    {
        Sequence seq = DOTween.Sequence();
        var nodePositions = GraphLoader.Instance.NodePositions;

        for (int i = 1; i < nodeIds.Length; i++)
        {
            int nodeId = nodeIds[i];
            var nextPos = nodePositions[nodeId];
            seq.Append(transform.DOMove(nextPos, _secondsBetweenNodes)
                .SetEase(Ease.Linear));
        }

        seq.OnComplete(new TweenCallback(callback));
    }
}
