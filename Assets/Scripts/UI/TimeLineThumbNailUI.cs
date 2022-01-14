using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeLineThumbNailUI : MonoBehaviour
{
    [SerializeField]
    private string mSlotStr;

    public string SlotStr
    {
        get { return mSlotStr; }
        set 
        {
            mSlotStr = value;
            if(mTextUI != null)
            {
                mTextUI.text = mSlotStr;
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
