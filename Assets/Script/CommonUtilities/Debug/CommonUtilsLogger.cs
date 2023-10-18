/// <summary>
/// �f�o�b�O�����Ǘ��E�o�͂���N���X
/// �����: �n�
/// </summary>
public class CommonUtilsLogger
{
    /// <summary>
    /// ���b�Z�[�W�����O�Ƃ��ďo��
    /// </summary>
    public static void Log(string message)
    {
        if (CommonUtilsMessager.Instance != null)
        {
            CommonUtilsMessager.Instance.AppendMessage(message);
        }
    }

    /// <summary>
    /// �G���[���b�Z�[�W�����O�Ƃ��ďo��
    /// </summary>
    public static void LogError(string error)
    {
        if (CommonUtilsMessager.Instance != null)
        {
            CommonUtilsMessager.Instance.AppendMessage($"ERROR: {error}");
        }
    }

    /// <summary>
    /// ���O���N���A
    /// </summary>
    public static void ClearLog()
    {
        if (CommonUtilsMessager.Instance != null)
        {
            CommonUtilsMessager.Instance.ClearMessage();
        }
    }
}