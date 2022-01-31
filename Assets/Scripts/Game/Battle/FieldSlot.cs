﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethod;

[System.Serializable]
public class FieldSlot : MonoBehaviour
{
    // 필드 번호
    [SerializeField]
    private Vector2Int mFieldCoordi = new Vector2Int(-1, -1);
    private bool mbEvenSlot;
    public Vector2Int FieldCoordi { get => mFieldCoordi; }
    public bool IsEven { get => mbEvenSlot; }

    // (0, up)부터 시계방향으로 순서대로
    [SerializeField]
    private FieldSlot[] mNearSlot = new FieldSlot[8];

    [SerializeField]
    private GameObject AttackTargetMark;

    // 인접 슬롯 설정
    public void SetNearSlot(int dir, FieldSlot slot)
    {
        mNearSlot[dir] = slot;
    }

    #region 인접 슬롯 얻기

    public FieldSlot Up { get => mNearSlot[0]; }
    public FieldSlot RightUp { get => mNearSlot[1]; }
    public FieldSlot Right { get => mNearSlot[2]; }
    public FieldSlot RightDown { get => mNearSlot[3]; }
    public FieldSlot Down { get => mNearSlot[4]; }
    public FieldSlot LeftDown { get => mNearSlot[5]; }
    public FieldSlot Left { get => mNearSlot[6]; }
    public FieldSlot LeftUp { get => mNearSlot[7]; }

    #endregion

    // 슬롯 이미지
    [SerializeField]
    private SpriteRenderer mSlotImg;

    // 필드 상태
    [SerializeField]
    private FieldStateObject mCurrentFieldState;

    // 필드 오브젝트
    [SerializeField]
    private IFieldObject mCurrentFieldObj;
    public IFieldObject CurrentFieldObj
    {
        get { return mCurrentFieldObj; }
        set
        {
            mCurrentFieldObj = value;
        }
    }


    public void InitSlot(Vector2Int coordi)
    {
        if (mSlotImg == null)
        {
            mSlotImg = GetComponent<SpriteRenderer>();
        }

        mFieldCoordi = coordi;

        mbEvenSlot = ((mFieldCoordi.x + mFieldCoordi.y) % 2 == 0);

        // 현재 idx와 col, row로 게임오브젝트 포지션 설정
        transform.localPosition = new Vector3(coordi.x * 2.5f, 0, coordi.y * 4.5f);
    }

    public void RemoveFieldObject()
    {
        mCurrentFieldObj.RemoveObject();
        mCurrentFieldObj = null;
    }

    public void SelectSlot(Vector2Int coordi)
    {
        if (mFieldCoordi != coordi)
        {
            // White로 변경
            mSlotImg.color = Color.white;
            return;
        }

        // Red로 변경
        mSlotImg.color = Color.red;
    }


    public void HitSlot(Weapon weapon)
    {
        if (mCurrentFieldObj != null)
        {
            mCurrentFieldObj.Hit(weapon.CurWeaponData.Power, Vector3.zero);
        }
    }

    public void ChangeSlotState_Reset()
    {
        // 표시 색 변경
        mSlotImg.color = Color.white;
        AttackTargetMark.SetActive(false);
    }

    public void ChangeSlotState_PossibleSelect()
    {
        // 표시 색 변경
        mSlotImg.color = Color.blue;
    }

    public void ChangeAttackTarget(bool bAttackTarget)
    {
        AttackTargetMark.SetActive(bAttackTarget);
    }
}
