using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeLineThumbNailUI : MonoBehaviour
{
    [SerializeField]
    private int mSlotIdx;

    public int SlotIdx
    {
        get { return mSlotIdx; }
        set 
        {
            mSlotIdx = value;
            if(mTextUI != null)
            {
                mTextUI.text = mSlotIdx.ToString();
            }
        }
    }

    [SerializeField]
    private Text mTextUI;

    [SerializeField]
    private TimeLineObject mTimeLineObj;

    public TimeLineObject TimeLineObj
    {
        get { return mTimeLineObj; }
        set { mTimeLineObj = value; }
    }
}
