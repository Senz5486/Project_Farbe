
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// シーンの読み込みと進捗表示を管理するクラス
/// 制作者: 渡邊
/// </summary>
public class CommonUtilsSceneManager : MonoBehaviour
{
    /// <summary>
    /// 進捗を表示するバー
    /// </summary>
    public Image loadingBar;

    /// <summary>
    /// 進捗を表示するテキスト
    /// </summary>
    public Text loadingText;

    private AsyncOperation asyncOperation;

    /// <summary>
    /// 指定したシーンを非同期で読み込みます。
    /// </summary>
    /// <param name="sceneName">読み込むシーンの名前</param>
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            loadingBar.fillAmount = progress; // ImageのfillAmountプロパティを使って進捗を設定
            loadingText.text = CommonUtilsMessageConstants.SCENE_TRANSITION_LOADINGTEXT + (int)(progress * 100) + CommonUtilsMessageConstants.SCENE_TRANSITION_LOADINGSYMBOL;

            if (progress >= 0.9f)
            {
                loadingText.text = CommonUtilsMessageConstants.SCENE_TRANSITION_FINISH_ANYKEYTEXT;
                if (Input.anyKeyDown)
                {
                    asyncOperation.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }
}