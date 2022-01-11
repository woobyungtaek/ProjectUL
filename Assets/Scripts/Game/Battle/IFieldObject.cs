/*
 필드에 올라가는 객체들의 기본 형
상속받아서 사용해야한다.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFieldObject
{    
    // 이동 하기
    void MovePos(int dir);

    // Field State 영향 받기 함수
    // 객체 별로 어떤것에 영향 받을지 구현 가능
    void ApplyFieldEffect();
}
