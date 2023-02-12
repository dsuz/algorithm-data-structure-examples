using System.IO;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// コマンド一覧を提供するクラス（シングルトン）
/// コマンド一覧はテキスト (csv形式) として Resources/CommandDefinition.csv に定義し、データとして読み込む
/// シングルトンなので、呼び出す時は CommandDatabase.Instance.CommandList と書いてコマンド一覧を取得する
/// </summary>
public class CommandDatabase
{
    /// <summary>コマンド定義ファイルの場所（Resourcesフォルダの下）</summary>
    static string _commandDefinitionFilePath = "CommandDefinition";
    static CommandDatabase _instance = new CommandDatabase();
    static Dictionary<string, Vector2[]> _commandDictionary;

    public static CommandDatabase Instance => _instance;
    public Dictionary<string, Vector2[]> CommandList => _commandDictionary;

    private CommandDatabase()
    {
        LoadDatabase();
    }   // 初回呼び出し時に定義ファイルを読み込む

    /// <summary>
    /// データを定義ファイルから読み込む
    /// 実行中に定義ファイルを変更した場合は、これを呼ぶと読み込み直す
    /// </summary>
    public static void LoadDatabase()
    {
        _commandDictionary = new Dictionary<string, Vector2[]>();
        var textAsset = Resources.Load<TextAsset>(_commandDefinitionFilePath);

        using (var sr = new StringReader(textAsset.text))
        {
            while (sr.Peek() > -1)
            {
                var line = sr.ReadLine();

                if (line.Substring(0, 2).Equals("//"))
                {
                    continue;
                }   // コメント行をスキップする
                else
                {
                    var data = line.Split(','); // 0: コマンド名（キー）, 1 以降: コマンド（方向）

                    if (_commandDictionary.ContainsKey(data[0]))
                    {
                        Debug.LogError($"キー {data[0]} が重複しています");
                        continue;
                    }   // キーが既にあった場合
                    else
                    {
                        ParseCommandData(data);
                    }
                }
            }
        }
    }

    /// <summary>
    /// string[] のデータをパースしてコマンド辞書に追加する
    /// </summary>
    /// <param name="data">先頭の要素がキー、それより後ろの要素がコマンド</param>
    static void ParseCommandData(string[] data)
    {
        string key = data[0];
        var cmd = new List<Vector2>();

        for (int i = 1; i < data.Length; i++)
        {
            switch (data[i].ToUpper())
            {
                case "UP":
                    cmd.Add(Vector2.up);
                    break;
                case "DOWN":
                    cmd.Add(Vector2.down);
                    break;
                case "RIGHT":
                    cmd.Add(Vector2.right);
                    break;
                case "LEFT":
                    cmd.Add(Vector2.left);
                    break;
                case "":
                    break;
                default:
                    Debug.LogError($"以下の行で不正なデータを発見しました。\n{string.Join(',', data)}");
                    break;                    
            }
        }

        _commandDictionary.Add(key, cmd.ToArray());
    }
}
