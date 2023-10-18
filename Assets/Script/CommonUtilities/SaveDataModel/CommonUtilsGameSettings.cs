/// <summary>
/// ゲーム設定で使用する設定を格納するクラス
/// 制作者: 渡邊
/// </summary>
[System.Serializable]  // シリアル化可能にするアノテーション
public class CommonUtilsGameSettings
{
    // FPS表示を行うかどうかのフラグ
    public bool IsFPSShown { get; set; }

    // デバックモード中かどうかのフラグ
    public bool IsDebugMode { get; set; }

    public float MasterVolume { get; set; }
    public float SEVolume { get; set; }
    public float BGMVolume { get; set; }

    // コンストラクタで初期値を設定
    public CommonUtilsGameSettings()
    {
        IsFPSShown = false;
        IsDebugMode = false;
        MasterVolume = 0.5f;
        SEVolume = 0.5f;
        BGMVolume = 0.5f;
    }
}