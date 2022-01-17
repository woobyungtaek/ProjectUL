using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethod;


public static class WeaponEffect
{
    //Weapon의 파워도 알아야하네...

    #region Attack
    public static void NormalAttack(Weapon weapon, FieldSlot slot)
    {
        IFieldObject fieldObj = slot.CurrentFieldObj;
        if (fieldObj == null) { return; }

        fieldObj.Hit(3f, Vector3.zero);
    }

    #endregion

    #region MovePos
    private static void MoveDir(Vector3 dir, FieldSlot slot)
    {

    }
    public static void MoveLeft(Weapon weapon, FieldSlot slot)
    {

    }
    public static void MoveRight(Weapon weapon, FieldSlot slot)
    {

    }
    public static void MoveForward(Weapon weapon, FieldSlot slot)
    {

    }
    public static void MoveBack(Weapon weapon, FieldSlot slot)
    {

    }
    public static void MoveRandom(Weapon weapon, FieldSlot slot)
    {

    }
    #endregion
}