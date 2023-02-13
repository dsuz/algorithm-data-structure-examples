using System.IO;

/// <summary>
/// 共通する処理を集めたクラス
/// </summary>
public static class Tools
{
    /// <summary>
    /// StringReader からコメントを除去して一行読み込む。行全体がコメントならばスキップして再度読み込む。
    /// </summary>
    /// <returns>読み込んだ結果（最後に到達してから呼んだ場合は空文字が返る）</returns>
    public static string ReadLine(StringReader sr)
    {
        string ret = "";

        while (ret.Length == 0)
        {
            if (sr.Peek() == -1)
                return ret;

            ret = sr.ReadLine();
            var commentStartIndex = ret.IndexOf("//");

            if (commentStartIndex > -1)
            {
                ret = ret.Substring(0, commentStartIndex).Trim();
            }
        }

        return ret;
    }
}
