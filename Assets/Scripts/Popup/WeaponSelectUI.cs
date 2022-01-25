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
        // 플레이어가 가지고 있는 무기 리스트를 가지고
        // WeaponButton을 생성한다.
        List<Weapon> weaponList = player.WeaponList;

        foreach(Weapon weapon in weaponList)
        {
            WeaponSelectButtonUI inst = GameObjectPool.Instantiate<WeaponSelectButtonUI>(mWeaponButtonPrefab, mButtonTransform);
            inst.InitButtonUI(weapon);
        }

        gameObject.SetActive(true);
    }
}
