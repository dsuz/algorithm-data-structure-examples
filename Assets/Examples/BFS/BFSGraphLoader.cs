using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 以下の機能を提供するコンポーネント
/// 1. テキストからグラフのデータを読み込み、シーンにオブジェクトを構築する
/// 2. グラフの隣接行列のデータを保持し、提供する
/// 3. グラフのノード情報のデータを保持し、提供する
/// パスは Resources フォルダからのパスで指定することに注意すること
/// </summary>
public class BFSGraphLoader : SingletonMonoBehaviour<BFSGraphLoader>
{
    /// <summary>グラフのデータが入っているテキストアセットのパス</summary>
    [SerializeField] string _graphDataFilePath = "BFSGraphData";
    /// <summary>ノードオブジェクトのプレハブのパス</summary>
    [SerializeField] string _nodePrefabFilePath = "BFSNode";
    /// <summary>辺 (LineRenderer) のプレハブのパス</summary>
    [SerializeField] string _edgePrefabFilePath = "BFSEdgeLine";
    /// <summary>プレイヤーのプレハブのパス</summary>
    [SerializeField] string _playerPrefabFilePath = "BFSPlayer";
    /// <summary>隣接行列（無向）</summary>
    bool[,] _adjacencyMatrix;
    /// <summary>ノードオブジェクトのキャッシュ</summary>
    Dictionary<int, BFSNodeController> _nodeDictionary = new Dictionary<int, BFSNodeController>();
    /// <summary>辺オブジェクトのキャッシュ</summary>
    List<LineRenderer> _lines = new List<LineRenderer>();
    /// <summary>プレイヤーオブジェクトのキャッシュ</summary>
    GameObject _player;

    /// <summary>
    /// グラフの隣接行列（無向）
    /// </summary>
    public bool[,] AdjacencyMatrix => _adjacencyMatrix;
    /// <summary>
    /// グラフのノード情報
    /// </summary>
    public Dictionary<int, BFSNodeController> NodeDictionary => _nodeDictionary;   // TODO: 提供したいデータはノードの座標とノードの個数だけなので、それ以外の情報を隠す

    /// <summary>
    /// ノード・辺・プレイヤーのデータを全て削除する。
    /// </summary>
    void Clear()
    {
        foreach (var node in _nodeDictionary.Values)
            Destroy(node);
        _nodeDictionary.Clear();
        _lines.ForEach(line => Destroy(line.gameObject));
        _lines.Clear();

        if (_player)
            DestroyImmediate(_player);   // TODO: Immediate でなくしたい
    }

    /// <summary>
    /// グラフを定義したテキストからデータを読み込む
    /// </summary>
    public void LoadGraphData()
    {
        Clear();
        var textAsset = Resources.Load(_graphDataFilePath) as TextAsset;    // グラフデータが入っているテキストファイル
        var nodePrefab = Resources.Load<BFSNodeController>(_nodePrefabFilePath);
        var linePrefab = Resources.Load<LineRenderer>(_edgePrefabFilePath);
        var playerPrefab = Resources.Load(_playerPrefabFilePath) as GameObject;

        using (var sr = new StringReader(textAsset.text))
        {
            var p = Array.ConvertAll(Tools.ReadLine(sr).Split(), int.Parse);   // 0: 頂点数, 1: 辺データの数
            _adjacencyMatrix = new bool[p[0], p[0]];    // 頂点数から隣接行列を生成する

            for (int i = 0; i < p[0]; i++)
            {
                var nodeData = Array.ConvertAll(Tools.ReadLine(sr).Split(), int.Parse);   // 0: 頂点番号, 1: 頂点のX座標, 2: 頂点のY座標
                var node = Instantiate(nodePrefab, new Vector2(nodeData[1], nodeData[2]), Quaternion.identity);
                node.NodeId = nodeData[0];
                node.name = $"{nodePrefab.name} {nodeData[0]}";
                _nodeDictionary.Add(nodeData[0], node);
            }   // 頂点データを読み込み、画面にノードのプレハブを並べる

            for (int i = 0; i < p[1]; i++)
            {
                var edge = Array.ConvertAll(Tools.ReadLine(sr).Split(), int.Parse);   // edge: 要素数2で、どの頂点とどの頂点が接続されているかを示す
                _adjacencyMatrix[edge[0], edge[1]] = true;  // 双方向（無向）
                _adjacencyMatrix[edge[1], edge[0]] = true;  // 双方向（無向）
                var lineObject = Instantiate(linePrefab);
                lineObject.SetPosition(0, _nodeDictionary[edge[0]].transform.position);
                lineObject.SetPosition(1, _nodeDictionary[edge[1]].transform.position);
                lineObject.name = $"{_edgePrefabFilePath} {i}";
                _lines.Add(lineObject);
            }   // 辺のデータを読み込み、隣接行列にデータを書き込む

            // スタート地点に Player を置く
            var startNode = int.Parse(Tools.ReadLine(sr));
            _player = Instantiate(playerPrefab);
            BFSManager.Instance.CurrentNode = startNode;
        }
    }
}
