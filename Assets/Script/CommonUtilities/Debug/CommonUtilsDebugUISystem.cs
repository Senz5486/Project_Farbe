using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
/// <summary>
/// �f�o�b�O���UI��֕\������N���X
/// �����: �n�
/// </summary>
public class CommonUtilsDebugUISystem : MonoBehaviour
{
    /// <summary>
    /// Debug���b�Z�[�W��\�����邽�߂�Text�R���|�[�l���g
    /// </summary>
    [SerializeField]
    private Text debugText;

    /// <summary>
    /// �ۑ����郍�O�̍ő�s��
    /// </summary>
    private const int MAX_LOG_COUNT = 10;

    /// <summary>
    /// ���O��ۑ����郊�X�g
    /// </summary>
    private List<string> logs = new List<string>();

    private static CommonUtilsDebugUISystem instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // �V�[���J�ڂ��Ă��j�����Ȃ�
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// ���O��ǉ�����UI�ɕ\������
    /// </summary>
    public static void Log(string message)
    {

        if (instance == null || !CommonUtilsSaveSystem.Instance.Settings.IsDebugMode) return; // DebugUI���V�[���ɂȂ��ꍇ�܂��̓f�o�b�O���[�h���I�t�̏ꍇ�A�������Ȃ�

        instance.AddLog(message);
    }

    private void AddLog(string message)
    {
        logs.Add(message);

        // ���O�̍ő�s���𒴂����ꍇ�͌Â����̂��폜
        while (logs.Count > MAX_LOG_COUNT)
        {
            logs.RemoveAt(0);
        }

        // UI�ɕ\��
        debugText.text = string.Join("\n", logs);
    }
}