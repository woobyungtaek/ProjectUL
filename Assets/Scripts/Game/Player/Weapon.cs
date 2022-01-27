using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 무기는 무기데이터를 입력 받아서 초기화 된다.
static한 곳에서 함수와 프리팹, 이미지 등을 새로 셋팅 한다.
무기가 선택 되면 배틀 매니저가 무기를 사용한다.
 */

public class Weapon
{
    public Weapon(WeaponData data)
    {
        mWeaponData = data;
    }

    [SerializeField]
    // 무기 데이터
    private WeaponData mWeaponData;
    public WeaponData CurWeaponData
    {
        get { return mWeaponData; }
    }

    public delegate void WeaponFunc(Weapon weapon, FieldSlot fieldSlot);

    public WeaponFunc WeaponFunction;

    // 무기는 공격 함수
    // 장판 설치 함수가 따로 있으며
    // 장판 설치 함수는 실행 시 대상 fieldSlotList에 대해 실행된다.
}
