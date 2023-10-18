
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// �V�[���̓ǂݍ��݂Ɛi���\�����Ǘ�����N���X
/// �����: �n�
/// </summary>
public class CommonUtilsSceneManager : MonoBehaviour
{
    /// <summary>
    /// �i����\������o�[
    /// </summary>
    public Image loadingBar;

    /// <summary>
    /// �i����\������e�L�X�g
    /// </summary>
    public Text loadingText;

    private AsyncOperation asyncOperation;

    /// <summary>
    /// �w�肵���V�[����񓯊��œǂݍ��݂܂��B
    /// </summary>
    /// <param name="sceneName">�ǂݍ��ރV�[���̖��O</param>
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
            loadingBar.fillAmount = progress; // Image��fillAmount�v���p�e�B���g���Đi����ݒ�
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