/// <summary>
/// �Q�[���i�s�x�E�L�����N�^�[�X�e�[�^�X�ۑ��Ŏg�p����ݒ���i�[����N���X
/// �����: �n�
/// </summary>
[System.Serializable]
public class CommonUtilsGameSaveData
{
    /// <summary>
    /// �v���C���[�Ɋւ��镨
    /// </summary>
    /* �v���C���[�̃��x�� */
    public int PlayerLevel { get; set; }

    /* �v���C���[�̖��O */
    public string PlayerName { get; set; }
    /* �v���C���[�̗̑� */
    public float PlayerHealth { get; set; }
    /* �v���C���[�̖��� */
    public float PlayerMP { get; set; }

    /// <summary>
    /// �X�g�[���[�i�s�Ɋւ��镨
    /// </summary>

    /// <summary>
    /// ���̑��K�v�ȃv���p�e�B�[���K�v�ȏꍇ�͒ǉ����Ă��������B
    /// </summary>

    // �R���X�g���N�^
    public CommonUtilsGameSaveData()
    {
        // �v���p�e�B�̏�����
        PlayerLevel = 1;
        PlayerName = "�v���C���[������͂��Ă�������";
        PlayerHealth = 100.0f;
        PlayerMP = 0.0f;
    }
}
