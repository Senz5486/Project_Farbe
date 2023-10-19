using UnityEngine;
using System;

/// <summary>
/// �A�j���[�V�����̊������Ď����A�������ɃC�x���g�𔭐�������N���X�B
/// �����: �n�
/// </summary>
[RequireComponent(typeof(Animator))]
public class CommonUtilsAnimationEndCheck : MonoBehaviour
{
    // �Ď��Ώۂ̃A�j���[�^�[
    private Animator targetAnimator;

    // �Ď�����A�j���[�V�����̖��O
    [SerializeField]
    private string targetAnimationName;

    // �A�j���[�V���������������Ƃ��ɔ���������C�x���g
    public event Action OnAnimationCompleted;

    private void Start()
    {
        targetAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (IsAnimationFinished())
        {
            OnAnimationCompleted?.Invoke(); // �C�x���g�̌Ăяo��
        }
    }

    /// <summary>
    /// �Ώۂ̃A�j���[�V�������I��������ǂ�����Ԃ��B
    /// </summary>
    /// <returns>�A�j���[�V�������I����Ă����true�A�����łȂ����false�B</returns>
    private bool IsAnimationFinished()
    {
        if (targetAnimator.GetCurrentAnimatorStateInfo(0).IsName(targetAnimationName) &&
            targetAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            return true;
        }
        return false;
    }
}