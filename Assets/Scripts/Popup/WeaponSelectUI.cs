using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelectUI : MonoBehaviour
{
    [SerializeField]
    private GameObject mWeaponButtonPrefab;

    [SerializeField]
    private Transform mButtonTransform;

    private List<WeaponSelectButtonUI> mButtonList = new List<WeaponSelectButtonUI>();

    public void InitWeaponSelectUI(Player player)
    {
        // 플레이어가 가지고 있는 무기 리스트를 가지고
        // WeaponButton을 생성한다.
        List<Weapon> weaponList = player.WeaponList;

        foreach(var weapon in weaponList)
        {
            WeaponSelectButtonUI inst = GameObjectPool.Instantiate<WeaponSelectButtonUI>(mWeaponButtonPrefab, mButtonTransform);
            mButtonList.Add(inst);
            inst.InitButtonUI(weapon);
        }

        gameObject.SetActive(true);
    }

    public void DisableWeaponSelectUI()
    {
        foreach(var button in mButtonList)
        {
            GameObjectPool.Destroy(button.gameObject);
        }
        gameObject.SetActive(false);
    }
}
