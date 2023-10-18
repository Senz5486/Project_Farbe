using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���C�����j���[�̃��C���V�X�e���N���X
/// �����: �n�
/// </summary>
public class MainMenuSystem : MonoBehaviour
{
    /// <summary>
    /// �Q�[���o�[�W������\�L����Text
    /// </summary>
    [SerializeField] private Text gameVesionText;
    private void Awake()
    {
        gameVesionText.text = CommonUtilsMessageConstants.GAMEVERSION_TEXT + Application.version; //�N�����Ƀo�[�W������ǂݍ���

    }
    void Start()
    {
        CommonUtilsGameSettings gameSetting = CommonUtilsSaveSystem.Instance.LoadSettings();// �Q�[���ݒ�����[�h
        CommonUtilsSaveSystem.Instance.CreateNewGameData(2);
    }

    
    void Update()
    {
        
    }
}
