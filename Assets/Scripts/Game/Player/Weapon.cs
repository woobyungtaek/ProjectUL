using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private int mPower;

    public delegate void WeaponFunc(FieldSlot fieldSlot);

    public WeaponFunc WeaponFunction;
}
