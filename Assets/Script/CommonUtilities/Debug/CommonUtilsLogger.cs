/// <summary>
/// デバッグ情報を管理・出力するクラス
/// 制作者: 渡邊
/// </summary>
public class CommonUtilsLogger
{
    /// <summary>
    /// メッセージをログとして出力
    /// </summary>
    public static void Log(string message)
    {
        if (CommonUtilsMessager.Instance != null)
        {
            CommonUtilsMessager.Instance.AppendMessage(message);
        }
    }

    /// <summary>
    /// エラーメッセージをログとして出力
    /// </summary>
    public static void LogError(string error)
    {
        if (CommonUtilsMessager.Instance != null)
        {
            CommonUtilsMessager.Instance.AppendMessage($"ERROR: {error}");
        }
    }

    /// <summary>
    /// ログをクリア
    /// </summary>
    public static void ClearLog()
    {
        if (CommonUtilsMessager.Instance != null)
        {
            CommonUtilsMessager.Instance.ClearMessage();
        }
    }
}