using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// デバッグ情報をUI上に表示するクラス
/// 制作者: 渡邊
/// </summary>
public class CommonUtilsMessager : MonoBehaviour
{
    public static CommonUtilsMessager Instance { get; private set; }

    [SerializeField]
    private Text debugText; // デバッグ情報を表示するTextコンポーネント

    /// <summary>
    /// オブジェクトが生成された際の処理
    /// </summary>
    private void Awake()
    {
        // インスタンスが未設定の場合、このオブジェクトを設定し、シーン遷移でも破棄しないように設定
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // すでにインスタンスが存在する場合、新たに生成されたこのオブジェクトは破棄
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// メッセージを表示
    /// </summary>
    public void ShowMessage(string message)
    {
        debugText.text = message;
    }

    /// <summary>
    /// メッセージを追加表示
    /// </summary>
    public void AppendMessage(string message)
    {
        debugText.text += "\n" + message;
    }

    /// <summary>
    /// 表示されているメッセージをクリア
    /// </summary>
    public void ClearMessage()
    {
        debugText.text = string.Empty;
    }
}