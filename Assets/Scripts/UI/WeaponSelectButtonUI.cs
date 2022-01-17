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

        // ButtonText�� Weapon�� �̸����� �ٲٱ�
        // mButtonText.text = "";
    }

    public void OnButtonClicked()
    {
        Debug.Log("���⼱��");
        BattleManager.Instance.SelectWeaponByUI(mCurrentWeapon);
    }
}
