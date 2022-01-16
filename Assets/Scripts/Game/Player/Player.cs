using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : TimeLineObject
{
    public override void TurnActionFunc()
    {
        // 상태 이상 실행
        // 살아 남은 경우 아래 실행

        // 무기 선택을 UI On 또는 입력대기
        BattleManager.Instance.PopupWeaponSelectUI
            ();
    }
}