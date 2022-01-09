using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FieldSlot : MonoBehaviour
{
    // 필드 번호
    [SerializeField]
    private int mFieldIdx = 0;

    // 슬롯 이미지
    [SerializeField]
    private SpriteRenderer mSlotImg;

    // 필드 상태
    [SerializeField]
    private FieldStateObject mCurrentFieldState;

    // 필드 오브젝트
    [SerializeField]
    private IFieldObject mCurrentFieldObj;


    public void InitSlot(int idx, int row, IFieldObject fieldObj = null)
    {
        if(mSlotImg == null)
        {
            mSlotImg = GetComponent<SpriteRenderer>();
        }

        mFieldIdx = idx;

        // 현재 idx와 col, row로 게임오브젝트 포지션 설정
        transform.localPosition = new Vector3(idx % row, idx / row, 0);

        mCurrentFieldObj = fieldObj;
    }

    public void SelectSlot(int idx)
    {
        if(mFieldIdx != idx)
        {
            // White로 변경
            mSlotImg.color = Color.white;
            return;
        }

        // Red로 변경
        mSlotImg.color = Color.red;
    }

    // 필드 오브젝트가 설정되면
    // 필드 오브젝트의 그림을 현재 위치에 표시해줘야함
    // 그 작업은 필드 오브젝트가 직접 생성하고 가지고 있으며
    // 슬롯은 위치만 알려준다.
}
