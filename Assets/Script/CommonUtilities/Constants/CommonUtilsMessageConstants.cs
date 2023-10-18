/// <summary>
/// メッセージの定数クラス
/// 制作者: 渡邊
/// </summary>
public static class CommonUtilsMessageConstants
{
    /// <summary>
    /// デバック用メッセージ一覧
    /// </summary>
    // デバッグ情報のprefix
    private const string DEBUG_PREFIX = "[DEBUG]";
    /* 正常時のメッセージ一覧 */
    // ゲームのスタート時のメッセージ
    public const string GAME_STARTED = DEBUG_PREFIX + "メインゲームを開始しました";
    // シーン遷移時のメッセージ
    public const string SCENE_TRANSITION = DEBUG_PREFIX + "シーンをロードしました";
    /* エラー時のメッセージ一覧 */
    // エラーメッセージ
    public const string ERROR_GAME_STARTED = DEBUG_PREFIX + "メインゲーム開始に失敗しました";
    // シーン遷移時のメッセージ
    public const string ERROR_SCENE_TRANSITION = DEBUG_PREFIX + "シーンのロードに失敗しました";
    // セーブスロット番号が無効
    public const string ERROR_SAVE_SLOTINVALID = DEBUG_PREFIX + "指定されたスロット番号が無効です";
    // セーブデータが存在しない
    public const string ERROR_SAVE_DATANOTEXIST = DEBUG_PREFIX + "このスロットのセーブデータは存在しません";
    // セーブデータが読み込めない
    public const string ERROR_SAVE_DATABROKEN = DEBUG_PREFIX + "セーブデータが破損しています";
    /// <summary>
    /// ゲーム用メッセージ一覧
    /// </summary>
    // ゲームバージョン表記
    public const string GAMEVERSION_TEXT = "ゲームバージョン:";
    // ローディング中のメッセージ
    public const string SCENE_TRANSITION_LOADINGTEXT = "読み込み中:";
    // ローディング進行度を表す記号
    public const string SCENE_TRANSITION_LOADINGSYMBOL = "%";
    // ローディング終了後の遷移出来ます表示
    public const string SCENE_TRANSITION_FINISH_ANYKEYTEXT = "キーを押して続行...";


}
