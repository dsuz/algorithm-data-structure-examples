using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.LookDev;

/// <summary>
/// 幅優先探索を使って無効グラフから経路を探索し、経路を見つける機能を提供するコンポーネント。
/// 探索に成功してから経路を復元する方法は以下のページの「動的計画法の最適解を復元する 2 つの方法」> 「方法 2： 汎用的に使える良い方法」のやり方を参考にした。
/// https://qiita.com/drken/items/0c7bab0384438f285f93
/// </summary>
public class SearchManager : SingletonMonoBehaviour<SearchManager>
{
    [Tooltip("移動不可の時に鳴らす音")]
    [SerializeField] AudioClip _ngSound;
    /// <summary>現在いるノード（探索開始ノード）</summary>
    int _currentNode;
    /// <summary>プレイヤーオブジェクトのキャッシュ</summary>
    NodeTracer _player;
    /// <summary>ノード間移動中フラグ。アニメーション再生中は true にする。true の時は外部からの呼び出しを受け付けない。</summary>
    bool _isTweening = false;

    /// <summary>
    /// プレイヤーが現在いるノードのID（探索の開始ノードであり、探索結果を移動中には変化しない）。
    /// ノードID を代入すると、プレイヤーがそのノードにワープする。ワープ前のノードとワープ後のノードが連結されていなくても関係なくワープする。
    /// </summary>
    public int CurrentNode
    {
        get => _currentNode;

        set
        {
            if (_isTweening)
            {
                Debug.Log("移動中は操作不可です");

                if (_ngSound)
                    AudioSource.PlayClipAtPoint(_ngSound, Camera.main.transform.position);
            }
            else
            {
                _currentNode = value;

                if (!_player)
                {
                    var go = GameObject.FindGameObjectWithTag("Player");
                    _player = go.GetComponent<NodeTracer>();
                }

                if (_player)
                    _player.Move(_currentNode);
                else
                    Debug.LogError($"プレイヤーが見つかりません。");
            }
        }
    }

    /// <summary>
    /// 経路を探索する
    /// </summary>
    /// <param name="targetNode">目標ノードのノード番号</param>
    /// <param name="searchMethod">探索方法</param>
    public void Search(int targetNode, SearchMethod searchMethod)
    {
        if (_isTweening)
        {
            Debug.Log("移動中は操作不可です");

            if (_ngSound)
                AudioSource.PlayClipAtPoint(_ngSound, Camera.main.transform.position);
        }
        else if (searchMethod == SearchMethod.DFS)
        {
            DFSSearch(CurrentNode, targetNode);
        }
        else if (searchMethod == SearchMethod.BFS)
        {
            BFSSearch(_currentNode, targetNode);
        }
    }

    /// <summary>
    /// 深さ優先探索で経路を探索する
    /// </summary>
    /// <param name="currentNode">現在のノード</param>
    /// <param name="targetNode">目標のノード</param>
    void DFSSearch(int currentNode, int targetNode)
    {
        var adjMatrix = GraphLoader.Instance.AdjacencyMatrix;    // 隣接行列
        var stack = new Stack<int>();
        stack.Push(currentNode);
        int[] route = null;
        DFSSearchRecursive(stack, adjMatrix, targetNode, ref route);
        
        if (route == null)
        {
            print("ルートが見つかりませんでした。");

            if (_ngSound)
                AudioSource.PlayClipAtPoint(_ngSound, Camera.main.transform.position);
        }
        else
        {
            _isTweening = true;
            _player.Move(route, () => _isTweening = false);
        }   // 経路を移動する
    }

    /// <summary>
    /// 深さ優先探索で経路探索を行う（再起呼び出しの部分）
    /// </summary>
    /// <param name="stack">探索中ルートが入ったスタック</param>
    /// <param name="adjMatrix">隣接行列</param>
    /// <param name="targetNode">目標のノード</param>
    /// <param name="route">ゴールへの経路を受け取る変数</param>
    void DFSSearchRecursive(Stack<int> stack, bool[,] adjMatrix, int targetNode, ref int[] route)
    {
        var currentNode = stack.Peek();
        var n = adjMatrix.GetLength(0); // ノード数

        for (int i = 0; i < n; i++)
        {
            if (adjMatrix[currentNode, i] && !stack.Contains(i) && (route == null || !route.Contains(targetNode)))
            {
                stack.Push(i);
                print($"探索中: {string.Join('>', stack.Reverse())}");

                if (i == targetNode)
                {
                    _currentNode = i;
                    route = stack.Reverse().ToArray();
                    print($"ゴールに着いた。ルートは {string.Join('>', route)}");
                    break;
                }   // ゴールについた
                else
                {
                    DFSSearchRecursive(stack, adjMatrix, targetNode, ref route);
                    stack.Pop();
                }   // 次のノードに移動する
            }   // 該当ノードに移動でき、かつ該当ノードが未訪問（かつまだゴールに着いてない）の場合
        }   // 次のノードを探す
    }

    /// <summary>
    /// 幅優先探索で経路を探索する
    /// </summary>
    /// <param name="currentNode">探索を開始するノード</param>
    /// <param name="targetNode">目標地点となるノード</param>
    void BFSSearch(int currentNode, int targetNode)
    {
        var adjMatrix = GraphLoader.Instance.AdjacencyMatrix;    // 隣接行列
        int n = adjMatrix.GetLength(0); // ノードの数
        Queue<NodeInfo> queue = new Queue<NodeInfo>();  // 幅優先探索に使うキュー
        queue.Enqueue(new NodeInfo(currentNode, -1));   // 探索を開始するノードの情報をまずキューに入れる。-1 は「どのノードからも来ていない」ことを意味する。
        int[] routeArray = Enumerable.Repeat<int>(int.MinValue, n).ToArray();   // どのノードが探索済みか保存しておく変数。値が MinValue の場合は未到達であることを表す。添え字がノードIDで、値はどのノードからそのノードに到達したかを表す。
        routeArray[currentNode] = -1;   // 探索開始ノードは「どのノードからも来ていない」ことを意味する -1 を入れる。

        while (queue.Count > 0)
        {
            var from = queue.Dequeue();

            for (int i = 0; i < n; i++)
            {
                if (adjMatrix[from.NodeId, i] && routeArray[i] == int.MinValue)
                {
                    queue.Enqueue(new NodeInfo(i, from.NodeId));
                    routeArray[i] = from.NodeId;

                    if (i == targetNode)
                    {
                        queue.Clear();
                        break;
                    }   // 目標地点に到達したらこれ以上探索しない
                }
            }
        }   // 幅優先探索

        // 探索が終了したら、まず移動可能か判定する
        if (routeArray[targetNode] > int.MinValue)
        {
            routeArray = TraceRoute(currentNode, targetNode, routeArray);
            Debug.Log($"移動可: {string.Join(" > ", routeArray)}");
            _isTweening = true;
            _player.Move(routeArray, () => _isTweening = false);
            _currentNode = targetNode;  // 移動先のノードが次の探索開始ノードになる
        }   // 移動可能ならルートを復元して移動する
        else
        {
            Debug.Log("移動不可");

            if (_ngSound)
                AudioSource.PlayClipAtPoint(_ngSound, Camera.main.transform.position);
        }   // 到達不可能
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
        var route = new List<int> { goalNodeId };
        int nodeId = goalNodeId;

        while (nodeId != startNodeId)
        {
            nodeId = routeArray[nodeId];
            route.Add(nodeId);
        }

        route.Reverse();
        return route.ToArray();
    }
}

/// <summary>
/// 幅優先探索のキューに入れるノード情報を定義する
/// </summary>
struct NodeInfo
{
    /// <summary>ノード番号</summary>
    public int NodeId;
    /// <summary>NodeIdのノードにどのノードから到達したのかを表すノード番号</summary>
    public int FromNodeId;

    public NodeInfo(int nodeId, int fromNodeId)
    {
        this.NodeId = nodeId;
        this.FromNodeId = fromNodeId;
    }
}

/// <summary>
/// 探索メソッドの名前を定義する
/// </summary>
public enum SearchMethod
{
    BFS,
    DFS,
}