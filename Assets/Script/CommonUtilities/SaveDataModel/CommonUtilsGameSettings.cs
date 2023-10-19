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

    //���ʐݒ�-�}�X�^�[�{�����[��
    public float MasterVolume { get; set; }

    //���ʐݒ�-SE�{�����[��
    public float SEVolume { get; set; }

    //���ʐݒ�-BGM�{�����[��
    public float BGMVolume { get; set; }

    //�r�f�I�ݒ�-�𑜓x
    public string ScreenResolution { get; set; }

    // �R���X�g���N�^�ŏ����l��ݒ�
    public CommonUtilsGameSettings()
    {
        IsFPSShown = CommonUtilsValueConstants.DEFAULT_FPSSHOWN;
        IsDebugMode = CommonUtilsValueConstants.DEFAULT_DEBUGMODE;
        MasterVolume = CommonUtilsValueConstants.DEFAULT_MASTERVOLUME;
        SEVolume = CommonUtilsValueConstants.DEFAULT_SEVOLUME;
        BGMVolume = CommonUtilsValueConstants.DEFAULT_BGMVOLUME;
        ScreenResolution = CommonUtilsValueConstants.DEFAULT_RESOLUTION;
    }
}