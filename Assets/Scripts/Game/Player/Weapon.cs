using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 ����� ���ⵥ���͸� �Է� �޾Ƽ� �ʱ�ȭ �ȴ�.
static�� ������ �Լ��� ������, �̹��� ���� ���� ���� �Ѵ�.
���Ⱑ ���� �Ǹ� ��Ʋ �Ŵ����� ���⸦ ����Ѵ�.

 */

[System.Serializable]
public class Weapon
{
    public Weapon(WeaponData data)
    {
        mWeaponData = data;
    }

    [SerializeField]
    // ���� ������
    private WeaponData mWeaponData;

    public delegate void WeaponFunc(Weapon weapon, FieldSlot fieldSlot);

    public WeaponFunc WeaponFunction;

    // ����� ���� �Լ�
    // ���� ��ġ �Լ��� ���� ������
    // ���� ��ġ �Լ��� ���� �� ��� fieldSlotList�� ���� ����ȴ�.
}
