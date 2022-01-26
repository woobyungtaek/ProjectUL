using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EAniState
{
    // ������ �ִϸ��̼� �̸��� �����ؾ��մϴ�.
    Idle = 0,
    CreateAni,
    AttackAni,
    HitAni,
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
            // WaitForSeconds��ü �����
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

        // ��� ���°� �ƴ� ��츸 �ð� ��⸦ �Ѵ�.
        Debug.Log($"{aniState.ToString()} ���");
        StartCoroutine(DelayChangeAnyState(aniState, bWait));
    }

    IEnumerator DelayChangeAnyState(EAniState aniState, bool bWait)
    {
        // BattleManager�� Count�� ����
        if (bWait == true) { BattleManager.Instance.IncreaseAniCount(); }

        yield return mWaitSecDict[aniState.ToString()];
        Debug.Log($"{aniState.ToString()}  ��");

        // Idle�� ��ȯ
        mAnimator.SetInteger("CurrentState", (int)EAniState.Idle);

        // BattleManager�� Count�� ����
        if (bWait == true) { BattleManager.Instance.DecreaseAniCount(); }
        BattleManager.Instance.ResumeFlowFunc();

    }
}
