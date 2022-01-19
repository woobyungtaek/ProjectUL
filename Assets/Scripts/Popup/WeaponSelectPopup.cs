using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelectPopup : Popup
{
    [Header("WeaponSelectPopup")]
    [SerializeField]
    private GameObject mWeaponButtonPrefab;

    [SerializeField]
    private Transform mButtonTransform;

    private void Start()
    {
        SetColliderSize();
    }

    public void InitWeaponSelectPopup(Player player)
    {
        // �÷��̾ ������ �ִ� ���� ����Ʈ�� ������
        // WeaponButton�� �����Ѵ�.
        List<Weapon> weaponList = player.WeaponList;

        foreach(Weapon weapon in weaponList)
        {
            WeaponSelectButtonUI inst = GameObjectPool.Instantiate<WeaponSelectButtonUI>(mWeaponButtonPrefab, mButtonTransform);
            inst.InitButtonUI(weapon);
        }

        gameObject.SetActive(true);
    }
}
