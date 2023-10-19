using UnityEngine;
using System;

/// <summary>
/// アニメーションの完了を監視し、完了時にイベントを発生させるクラス。
/// 制作者: 渡邊
/// </summary>
[RequireComponent(typeof(Animator))]
public class CommonUtilsAnimationEndCheck : MonoBehaviour
{
    // 監視対象のアニメーター
    private Animator targetAnimator;

    // 監視するアニメーションの名前
    [SerializeField]
    private string targetAnimationName;

    // アニメーションが完了したときに発生させるイベント
    public event Action OnAnimationCompleted;

    private void Start()
    {
        targetAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (IsAnimationFinished())
        {
            OnAnimationCompleted?.Invoke(); // イベントの呼び出し
        }
    }

    /// <summary>
    /// 対象のアニメーションが終わったかどうかを返す。
    /// </summary>
    /// <returns>アニメーションが終わっていればtrue、そうでなければfalse。</returns>
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