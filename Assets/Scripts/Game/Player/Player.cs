using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : TimeLineObject
{
    private List<Weapon> mWeaponList = new List<Weapon>();

    public List<Weapon> WeaponList
    {
        get { return mWeaponList; }
    }

    public void InitPlayer()
    {
        InitTimeLineObject("P");

        mWeaponList.Clear();
        for(int cnt = 0; cnt < 10; ++cnt)
        {
            // ObjectPool�� ���� �����;���
            mWeaponList.Add(new Weapon());
        }
    }

    public override void TurnActionFunc()
    {
        // ���� �̻� ����
        // ��� ���� ��� �Ʒ� ����

        // ���� ��Ʋ �Ŵ����� ��ȴ� �� �ʿ䰡 ����
        BattleManager.Instance.PopupWeaponSelectUI();
    }
}