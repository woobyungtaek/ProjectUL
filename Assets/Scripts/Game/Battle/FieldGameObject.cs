using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EAniState
{
    Idle = 0,
    Create,
    Max
}

public class FieldGameObject : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer mSpriteRenderer;

    [SerializeField]
    private Animator mAnimator;

    public void PlayAnimation(EAniState eAniState)
    {
        mAnimator.SetInteger("CurrentState", (int)eAniState);
        float playSec = mAnimator.GetCurrentAnimatorClipInfo(0).Length;
        Debug.Log($"{playSec}");
    }
}
