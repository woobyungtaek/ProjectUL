using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy�� ��ӹ޴� �ֵ� 

public class Scarecrow : Enemy
{
    public override void MovePos(int dir)
    {
    }

    public override void ApplyFieldEffect()
    {
    }

    public override void TurnActionFunc()
    {
        // ����ƺ�� ����ϴ� �ִϸ��̼Ǹ��ϰ� �ƹ��� �ൿ�� ������ �ʽ��ϴ�.
        // 0.5�� ��� �� �ٽ� �ൿ�� ����� �ǵ��� �ؾ��մϴ�.
        Debug.Log("����ƺ� �ൿ ��");

        //���� �ִϸ��̼� ���
        CurrentFieldGameObject.PlayAnimationByAniState(EAniState.AttackAni);
    }
}
