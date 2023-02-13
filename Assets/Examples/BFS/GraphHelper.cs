using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

/// <summary>
/// ドキュメントコメント
/// </summary>
public static class GraphHelper
{
    static string _graphDataFilePath = "GraphData";
    static string _nodePrefabFilePath = "Node";
    static string _edgePrefabFilePath = "EdgeLine";
    static string _playerPrefabFilePath = "BFSPlayer";
    /// <summary>双方向隣接行列</summary>
    static bool[,] _adjacencyMatrix;
    static Dictionary<int, GameObject> _nodeDictionary = new Dictionary<int, GameObject>();
    static List<LineRenderer> _lines = new List<LineRenderer>();
    static NodeTracer _player;

    public static bool[,] AdjacencyMatrix => _adjacencyMatrix;
    public static Dictionary<int, GameObject> NodeDictionary => _nodeDictionary;

    /// <summary>
    /// ノード・辺・プレイヤーのデータを全て削除する。
    /// </summary>
    static void Clear()
    {
        foreach (var node in _nodeDictionary.Values)
            GameObject.Destroy(node);
        _nodeDictionary.Clear();
        _lines.ForEach(line => GameObject.Destroy(line.gameObject));
        _lines.Clear();
        GameObject.DestroyImmediate(_player?.gameObject);
    }

    /// <summary>
    /// グラフを定義したテキストからデータを読み込む
    /// </summary>
    public static void LoadGraphData()
    {
        Clear();
        var textAsset = Resources.Load<TextAsset>(_graphDataFilePath);
        var nodePrefab = Resources.Load<GameObject>(_nodePrefabFilePath);

        using (var sr = new StringReader(textAsset.text))
        {
            var p = Array.ConvertAll(Tools.ReadLine(sr).Split(), int.Parse);   // 0: 頂点数, 1: 辺データの数
            _adjacencyMatrix = new bool[p[0], p[0]];    // 頂点数から隣接行列を生成する

            for (int i = 0; i < p[0]; i++)
            {
                var nodeData = Array.ConvertAll(Tools.ReadLine(sr).Split(), int.Parse);   // 0: 頂点番号, 1: 頂点のX座標, 2: 頂点のY座標
                var nodeObject = GameObject.Instantiate(nodePrefab, new Vector2(nodeData[1], nodeData[2]), Quaternion.identity);
                nodeObject.name = $"{nodeData[0]}";
                _nodeDictionary.Add(nodeData[0], nodeObject);
            }   // 頂点データを読み込み、画面にノードのプレハブを並べる

            for (int i = 0; i < p[1]; i++)
            {
                var edge = Array.ConvertAll(Tools.ReadLine(sr).Split(), int.Parse);   // 要素数2で、どの頂点とどの頂点が接続されているかを示す
                _adjacencyMatrix[edge[0], edge[1]] = true;
                _adjacencyMatrix[edge[1], edge[0]] = true;
                var linePrefab = Resources.Load<LineRenderer>(_edgePrefabFilePath);
                var lineObject = GameObject.Instantiate(linePrefab);
                lineObject.SetPosition(0, _nodeDictionary[edge[0]].transform.position);
                lineObject.SetPosition(1, _nodeDictionary[edge[1]].transform.position);
                lineObject.name = $"{_edgePrefabFilePath}{i}";
                _lines.Add(lineObject);
            }   // 辺のデータを読み込み、隣接行列に無向グラフとしてのデータを書き込む

            // スタート地点に Player を置く
            var startNode = int.Parse(Tools.ReadLine(sr));
            var playerPrefab = Resources.Load<NodeTracer>(_playerPrefabFilePath);
            _player = GameObject.Instantiate<NodeTracer>(playerPrefab);
            BFSManager.Instance.CurrentNode = startNode;
        }
    }
}
