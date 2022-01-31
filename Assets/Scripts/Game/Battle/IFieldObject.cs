/*
 필드에 올라가는 객체들의 기본 형
상속받아서 사용해야한다.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtensionMethod
{
    public static class FieldObjExtention
    {
        // IFieldObject 중 공통 구현부를 확장메서드로 관리
        // 재정의가 필요하지 않은 이상 공통으로 써보고 나중에 바꾸자

        // 필드오브젝트 초기화
        public static void InitFieldObject(this IFieldObject fieldObject, FieldSlot slot)
        {
            // FieldObject는 mySlot위치에 AniObject를 생성시켜야한다.
            fieldObject.CurrentFieldGameObject = GameObjectPool.Instantiate<FieldGameObject>(BattleManager.Instance.FieldGameObjPrefab, slot.transform.parent);
            fieldObject.CurrentFieldGameObject.gameObject.transform.position = slot.gameObject.transform.position;
            fieldObject.CurrentFieldGameObject.PlayAnimationByAniState(EAniState.CreateAni, false);
        }

        // 필드 오브젝트 (적) 지우기
        public static void RemoveFieldGameObject(this IFieldObject fieldobject)
        {
            if(fieldobject.CurrentFieldGameObject != null)
            {
                GameObjectPool.Destroy(fieldobject.CurrentFieldGameObject.gameObject);
            }
        }

        // 필드오브젝트 피격
        public static void Hit(this IFieldObject fieldObject, float power, Vector3 dir)
        {
            Debug.Log("ExtentionHit");
            fieldObject.CurrentFieldGameObject.PlayAnimationByAniState(EAniState.HitAni);
        }
                
    }
}

public interface IFieldObject : IReUseObject
{
    // 반드시 있어야할 객체를 getset으로 구현
    FieldGameObject CurrentFieldGameObject { get; set; }

    // 이동 하기
    void MovePos(int dir);

    // Field State 영향 받기 함수
    // 객체 별로 어떤것에 영향 받을지 구현 가능
    void ApplyFieldEffect();

    void RemoveObject();
}
