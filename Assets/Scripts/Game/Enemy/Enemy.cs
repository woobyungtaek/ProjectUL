using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 적은 무조건 타임라인에서 행동 하며
// 필드에 올라와 있어야 한다.
[System.Serializable]
public class Enemy : TimeLineObject, IFieldObject
{
    private int mHP;
    private bool mbMove = true;

    public FieldGameObject CurrentFieldGameObject { get; set; }

    public void Init(FieldSlot slot)
    {
        mSpeed = Random.Range(3f, 12f);

        // 배당 받은 슬롯이 들어온다.
        slot.CurrentFieldObj = this;

        // 타임 라인 관련 초기화
        InitTimeLineObject(slot.FieldIndex.ToString());

        // FieldObject는 mySlot위치에 AniObject를 생성시켜야한다.
        CurrentFieldGameObject = GameObjectPool.Instantiate<FieldGameObject>(BattleManager.Instance.FieldGameObjPrefab, slot.transform.parent);
        CurrentFieldGameObject.gameObject.transform.position = slot.gameObject.transform.position;
        CurrentFieldGameObject.PlayAnimationByAniState(EAniState.CreateAni);
    }

    public virtual void MovePos(int dir)
    {
        // 기본값 정의 후에 분별 되는 애들만 재정의 하도록 하자

        // 움직임 관련되서는 일단 이렇게 해보자
        // 움직임이 변경되는 애들은 bool값으로 하나 있어야하고
        // 안되는 친구들은 아예 재정의 x
        // 매번 되는 친구들은 bool값 없이
    }
    public virtual void ApplyFieldEffect()
    {
        // 필드 이펙트가 실행되면, Effect종류와 파워값이 들어온다.
        // Enemy의 종류에 따라 적용되면 된다.
    }

    public override void TurnActionFunc()
    {
        // Enemy면 Enemy의 가상 함수를 실행해야한다.
        // 외부에서 TurnActionFunc를 실행하면
        // Enemy를 상속받은 실제 class가 재정의한 함수를 실행
        // 행동을 선택하는 방식이 다다르기 때문에 각각이 정의해서 사용한다.
    }
}
