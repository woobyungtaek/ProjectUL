using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : TimeLineObject
{
    public override void TurnActionFunc()
    {
        // ���� �̻� ����
        // ��� ���� ��� �Ʒ� ����

        // ���� ������ UI On �Ǵ� �Է´��
        BattleManager.Instance.PopupWeaponSelectUI
            ();
    }
}