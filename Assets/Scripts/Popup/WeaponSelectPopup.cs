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
        // 플레이어가 가지고 있는 무기 리스트를 가지고
        // WeaponButton을 생성한다.
    }
}
