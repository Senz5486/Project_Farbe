/// <summary>
/// ゲーム進行度・キャラクターステータス保存で使用する設定を格納するクラス
/// 制作者: 渡邊
/// </summary>
[System.Serializable]
public class CommonUtilsGameSaveData
{
    /// <summary>
    /// プレイヤーに関する物
    /// </summary>
    /* プレイヤーのレベル */
    public int PlayerLevel { get; set; }

    /* プレイヤーの名前 */
    public string PlayerName { get; set; }
    /* プレイヤーの体力 */
    public float PlayerHealth { get; set; }
    /* プレイヤーの魔力 */
    public float PlayerMP { get; set; }

    /// <summary>
    /// ストーリー進行に関する物
    /// </summary>

    /// <summary>
    /// その他必要なプロパティーが必要な場合は追加してください。
    /// </summary>

    // コンストラクタ
    public CommonUtilsGameSaveData()
    {
        // プロパティの初期化
        PlayerLevel = 1;
        PlayerName = "プレイヤー名を入力してください";
        PlayerHealth = 100.0f;
        PlayerMP = 0.0f;
    }
}
