using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player : TimeLineObject
{
    [SerializeField]
    private List<Weapon> mWeaponList = new List<Weapon>();

    public List<Weapon> WeaponList
    {
        get { return mWeaponList; }
    }

    public void InitPlayer()
    {
        InitTimeLineObject("P");

        mWeaponList.Clear();
        for(int cnt = 0; cnt < 4; ++cnt)
        {
            // ObjectPool�� ���� �����;���
            WeaponData data = DataManager.Instance.GetWeaponDataByName($"Weapon_{cnt}");
            Weapon weapon = new Weapon(data);

            mWeaponList.Add(weapon);
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