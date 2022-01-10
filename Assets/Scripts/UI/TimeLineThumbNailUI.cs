using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLineThumbNailUI : MonoBehaviour
{
    [SerializeField]
    private TimeLineObject mTimeLineObj;

    public TimeLineObject TimeLineObj
    {
        get { return mTimeLineObj; }
        set { mTimeLineObj = value; }
    }
}
