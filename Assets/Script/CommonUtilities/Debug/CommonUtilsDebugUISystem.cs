using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
/// <summary>
/// デバッグ情報UI上へ表示するクラス
/// 制作者: 渡邊
/// </summary>
public class CommonUtilsDebugUISystem : MonoBehaviour
{
    /// <summary>
    /// Debugメッセージを表示するためのTextコンポーネント
    /// </summary>
    [SerializeField]
    private Text debugText;

    /// <summary>
    /// 保存するログの最大行数
    /// </summary>
    private const int MAX_LOG_COUNT = 10;

    /// <summary>
    /// ログを保存するリスト
    /// </summary>
    private List<string> logs = new List<string>();

    private static CommonUtilsDebugUISystem instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // シーン遷移しても破棄しない
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// ログを追加してUIに表示する
    /// </summary>
    public static void Log(string message)
    {

        if (instance == null || !CommonUtilsSaveSystem.Instance.Settings.IsDebugMode) return; // DebugUIがシーンにない場合またはデバッグモードがオフの場合、何もしない

        instance.AddLog(message);
    }

    private void AddLog(string message)
    {
        logs.Add(message);

        // ログの最大行数を超えた場合は古いものを削除
        while (logs.Count > MAX_LOG_COUNT)
        {
            logs.RemoveAt(0);
        }

        // UIに表示
        debugText.text = string.Join("\n", logs);
    }
}