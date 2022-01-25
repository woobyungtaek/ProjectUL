using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelectUI : MonoBehaviour
{
    [SerializeField]
    private GameObject mWeaponButtonPrefab;

    [SerializeField]
    private Transform mButtonTransform;

    public void InitWeaponSelectUI(Player player)
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
