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
            // ObjectPool로 부터 가져와야함
            mWeaponList.Add(new Weapon());
        }
    }

    public override void TurnActionFunc()
    {
        // 상태 이상 실행
        // 살아 남은 경우 아래 실행

        // 굳이 배틀 매니저를 들렸다 갈 필요가 없네
        BattleManager.Instance.PopupWeaponSelectUI();
    }
}