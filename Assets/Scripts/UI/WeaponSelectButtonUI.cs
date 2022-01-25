using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectButtonUI : MonoBehaviour
{
    [SerializeField]
    private Weapon mCurrentWeapon;

    [SerializeField]
    private Text mButtonText;

    public void InitButtonUI(Weapon weapon)
    {
        mCurrentWeapon = weapon;

        // ButtonText를 Weapon의 이름으로 바꾸기
        mButtonText.text = weapon.CurWeaponData.Name;
    }

    public void OnButtonClicked()
    {
        BattleManager.Instance.SelectWeaponByUI(mCurrentWeapon);
    }
}
