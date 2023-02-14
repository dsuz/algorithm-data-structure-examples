using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 参考: 以下のページの「動的計画法の最適解を復元する 2 つの方法」> 「方法 2： 汎用的に使える良い方法」
/// https://qiita.com/drken/items/0c7bab0384438f285f93
/// </summary>
public class BFSManager : SingletonMonoBehaviour<BFSManager>
{
    [SerializeField] AudioClip _ngSound;
    int _currentNode = -1;
    int _targetNode = -1;
    int[] _routeArray;
    BFSNodeTracer _player;

    public int CurrentNode
    {
        get => _currentNode;

        set
        {
            _currentNode = value;
            GetPlayer().Move(_currentNode);
        }
    }

    public void Search(int targetNode)
    {
        Search(_currentNode, targetNode);
    }

    public void Search()
    {
        Search(_currentNode, _targetNode);
    }

    public void Search(int currentNode, int targetNode)
    {
        var adjMatrix = BFSGraphLoader.Instance.AdjacencyMatrix;
        int n = BFSGraphLoader.Instance.NodeDictionary.Count;
        Queue<NodeInfo> queue = new Queue<NodeInfo>();
        queue.Enqueue(new NodeInfo(currentNode, -1));
        _routeArray = Enumerable.Repeat<int>(int.MinValue, n).ToArray();
        _routeArray[currentNode] = -1;

        while (queue.Count > 0)
        {
            var from = queue.Dequeue();

            for (int i = 0; i < n; i++)
            {
                if (adjMatrix[from.NodeId, i] && _routeArray[i] == int.MinValue)
                {
                    queue.Enqueue(new NodeInfo(i, from.NodeId));
                    _routeArray[i] = from.NodeId;

                    if (i == targetNode)
                    {
                        queue.Clear();
                        break;
                    }   // 目標地点に到達したら抜ける
                }
            }
        }

        if (_routeArray[targetNode] > int.MinValue)
        {
            Debug.Log("移動可");
            var routeArray = TraceRoute(currentNode, targetNode, _routeArray);
            GetPlayer().Move(routeArray);
            _currentNode = targetNode;
        }
        else
        {
            Debug.Log("移動不可");

            if (_ngSound)
                AudioSource.PlayClipAtPoint(_ngSound, Camera.main.transform.position);
        }
    }

    /// <summary>
    /// ルートを格納した配列を逆からトレースして、経路を探す
    /// </summary>
    /// <param name="startNodeId"></param>
    /// <param name="goalNodeId"></param>
    /// <param name="routeArray"></param>
    /// <returns>開始ノードから終了ノードまでの経路を順番に格納した配列</returns>
    int[] TraceRoute(int startNodeId, int goalNodeId, int[] routeArray)
    {
        var route = new List<int>();
        route.Add(goalNodeId);
        int nodeId = goalNodeId;

        while (nodeId != startNodeId)
        {
            nodeId = routeArray[nodeId];
            route.Add(nodeId);
        }

        route.Reverse();
        Debug.Log(string.Join(" > ", route));
        return route.ToArray();
    }

    BFSNodeTracer GetPlayer()
    {
        if (_player == null)
        {
            var go = GameObject.FindGameObjectWithTag("Player");
            _player = go.GetComponent<BFSNodeTracer>();

            if (_player == null )
            {
                Debug.LogError($"プレイヤーが見つかりません。");
            }
        }

        return _player;
    }
}

struct NodeInfo
{
    public int NodeId;
    public int FromNodeId;

    public NodeInfo(int nodeId, int fromNodeId)
    {
        this.NodeId = nodeId;
        this.FromNodeId = fromNodeId;
    }
}
