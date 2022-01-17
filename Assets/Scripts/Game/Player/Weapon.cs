using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 ����� ���ⵥ���͸� �Է� �޾Ƽ� �ʱ�ȭ �ȴ�.
static�� ������ �Լ��� ������, �̹��� ���� ���� ���� �Ѵ�.
���Ⱑ ���� �Ǹ� ��Ʋ �Ŵ����� ���⸦ ����Ѵ�.

 */

public class Weapon
{
    private float mPower;
    private Vector3 mHitDir = Vector3.zero;

    // ���� ������ ��ǥ ����Ʈ
    // ���ݽ� Ÿ�� ����Ʈ

    public float Power
    {
        get { return mPower; }
    }
    public Vector3 HitDir
    {
        get { return mHitDir; }
    }

    public delegate void WeaponFunc(Weapon weapon, FieldSlot fieldSlot);

    public WeaponFunc WeaponFunction;

    // ����� ���� �Լ�
    // ���� ��ġ �Լ��� ���� ������
    // ���� ��ġ �Լ��� ���� �� ��� fieldSlotList�� ���� ����ȴ�.
}
