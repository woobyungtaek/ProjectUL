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
        // �÷��̾ ������ �ִ� ���� ����Ʈ�� ������
        // WeaponButton�� �����Ѵ�.
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
