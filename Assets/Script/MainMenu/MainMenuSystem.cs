using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// メインメニューのメインシステムクラス
/// 制作者: 渡邊
/// </summary>
public class MainMenuSystem : MonoBehaviour
{
    /// <summary>
    /// ゲームバージョンを表記するText
    /// </summary>
    [SerializeField] private Text gameVesionText;
    private void Awake()
    {
        gameVesionText.text = CommonUtilsMessageConstants.GAMEVERSION_TEXT + Application.version; //起動時にバージョンを読み込む

    }
    void Start()
    {
        CommonUtilsGameSettings gameSetting = CommonUtilsSaveSystem.Instance.LoadSettings();// ゲーム設定をロード
        CommonUtilsSaveSystem.Instance.CreateNewGameData(2);
    }

    
    void Update()
    {
        
    }
}
