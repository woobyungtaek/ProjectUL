using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldStateObject : TimeLineObject
{
    // 파워, 속성, 등등 필요?

    public override void TurnActionFunc()
    {
        // 자신의 타입과 값을 넘겨서
        // 자신을 참조한 slot의 StateEffect가 실행되도록 해야한다.
    }
}
