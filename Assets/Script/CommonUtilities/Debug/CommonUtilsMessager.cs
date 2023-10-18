using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �f�o�b�O����UI��ɕ\������N���X
/// �����: �n�
/// </summary>
public class CommonUtilsMessager : MonoBehaviour
{
    public static CommonUtilsMessager Instance { get; private set; }

    [SerializeField]
    private Text debugText; // �f�o�b�O����\������Text�R���|�[�l���g

    /// <summary>
    /// �I�u�W�F�N�g���������ꂽ�ۂ̏���
    /// </summary>
    private void Awake()
    {
        // �C���X�^���X�����ݒ�̏ꍇ�A���̃I�u�W�F�N�g��ݒ肵�A�V�[���J�ڂł��j�����Ȃ��悤�ɐݒ�
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // ���łɃC���X�^���X�����݂���ꍇ�A�V���ɐ������ꂽ���̃I�u�W�F�N�g�͔j��
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// ���b�Z�[�W��\��
    /// </summary>
    public void ShowMessage(string message)
    {
        debugText.text = message;
    }

    /// <summary>
    /// ���b�Z�[�W��ǉ��\��
    /// </summary>
    public void AppendMessage(string message)
    {
        debugText.text += "\n" + message;
    }

    /// <summary>
    /// �\������Ă��郁�b�Z�[�W���N���A
    /// </summary>
    public void ClearMessage()
    {
        debugText.text = string.Empty;
    }
}