using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethod;

// Enemy를 상속받는 애들 

public class Scarecrow : Enemy
{
    public override void MovePos(int dir)
    {
    }

    public override void ApplyFieldEffect()
    {
    }
    public override void RemoveObject()
    {
        this.RemoveFieldGameObject();
        ObjectPool.ReturnInst<Scarecrow>(this);
    }

    public override void TurnActionFunc()
    {
        // 허수아비는 띠용하는 애니메이션만하고 아무런 행동을 취하지 않습니다.
        // 0.5초 대기 후 다시 행동이 재시작 되도록 해야합니다.
        Debug.Log("허수아비 행동 함");

        //공격 애니메이션 재생
        CurrentFieldGameObject.PlayAnimationByAniState(EAniState.AttackAni);
    }
}
