using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EAniState
{
    // 실행할 애니메이션 이름과 동일해야합니다.
    Idle = 0,
    CreateAni,
    AttackAni,
    Max
}

public class FieldGameObject : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer mSpriteRenderer;

    [SerializeField]
    private Animator mAnimator;

    //Static
    private static Dictionary<string, WaitForSeconds> mWaitSecDict = new Dictionary<string, WaitForSeconds>();

    private void Awake()
    {
        if (mWaitSecDict.Count == 0)
        {
            // WaitForSeconds객체 만들기
            RuntimeAnimatorController ac = mAnimator.runtimeAnimatorController;
            int count = ac.animationClips.Length;
            for (int idx = 0; idx < count; ++idx)
            {
                mWaitSecDict.Add(ac.animationClips[idx].name, new WaitForSeconds(ac.animationClips[idx].length));
            }
        }
    }

    public void PlayAnimationByAniState(EAniState aniState, bool bWait = true)
    {
        mAnimator.SetInteger("CurrentState", (int)aniState);

        if (aniState == EAniState.Idle) { return; }

        // 대기 상태가 아닌 경우만 시간 대기를 한다.
        Debug.Log($"{aniState.ToString()} 재생");
        StartCoroutine(DelayChangeAnyState(aniState, bWait));
    }

    IEnumerator DelayChangeAnyState(EAniState aniState, bool bWait)
    {
        // BattleManager의 Count값 증가
        if (bWait == true) { BattleManager.Instance.IncreaseAniCount(); }

        yield return mWaitSecDict[aniState.ToString()];
        Debug.Log($"{aniState.ToString()}  끝");

        // Idle로 전환
        mAnimator.SetInteger("CurrentState", (int)EAniState.Idle);

        // BattleManager의 Count값 감소
        if (bWait == true) { BattleManager.Instance.DecreaseAniCount(); }
        BattleManager.Instance.ResumeFlowFunc();

    }
}
