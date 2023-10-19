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

    //音量設定-マスターボリューム
    public float MasterVolume { get; set; }

    //音量設定-SEボリューム
    public float SEVolume { get; set; }

    //音量設定-BGMボリューム
    public float BGMVolume { get; set; }

    //ビデオ設定-解像度
    public string ScreenResolution { get; set; }

    // コンストラクタで初期値を設定
    public CommonUtilsGameSettings()
    {
        IsFPSShown = CommonUtilsValueConstants.DEFAULT_FPSSHOWN;
        IsDebugMode = CommonUtilsValueConstants.DEFAULT_DEBUGMODE;
        MasterVolume = CommonUtilsValueConstants.DEFAULT_MASTERVOLUME;
        SEVolume = CommonUtilsValueConstants.DEFAULT_SEVOLUME;
        BGMVolume = CommonUtilsValueConstants.DEFAULT_BGMVOLUME;
        ScreenResolution = CommonUtilsValueConstants.DEFAULT_RESOLUTION;
    }
}