/// <summary>
/// �Q�[���ݒ�Ŏg�p����ݒ���i�[����N���X
/// �����: �n�
/// </summary>
[System.Serializable]  // �V���A�����\�ɂ���A�m�e�[�V����
public class CommonUtilsGameSettings
{
    // FPS�\�����s�����ǂ����̃t���O
    public bool IsFPSShown { get; set; }

    // �f�o�b�N���[�h�����ǂ����̃t���O
    public bool IsDebugMode { get; set; }

    public float MasterVolume { get; set; }
    public float SEVolume { get; set; }
    public float BGMVolume { get; set; }

    // �R���X�g���N�^�ŏ����l��ݒ�
    public CommonUtilsGameSettings()
    {
        IsFPSShown = false;
        IsDebugMode = false;
        MasterVolume = 0.5f;
        SEVolume = 0.5f;
        BGMVolume = 0.5f;
    }
}