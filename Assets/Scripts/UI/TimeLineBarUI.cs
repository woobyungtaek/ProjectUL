using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLineBarUI : MonoBehaviour
{
    // 프로필 UI 프리팹
    [SerializeField]
    private GameObject profilePrefab;

    // 프로필 생성 위치
    [SerializeField]
    private RectTransform mProfileParent;

    [SerializeField]
    private LinkedList<TimeLineThumbNailUI> mThumbNailLList = new LinkedList<TimeLineThumbNailUI>();

    private float mWidth;
    private float mHalfWidth;

    private void Start()
    {
        mWidth = mProfileParent.rect.width;
        mHalfWidth = mWidth / 2.0f;
    }

    private void Update()
    {
        if(mThumbNailLList.Count < 0) { return; }

        foreach(TimeLineThumbNailUI ui in mThumbNailLList)
        {
            // 사실 100분율을 구해야함
            float value = ui.TimeLineObj.CurrentTime / 100.0f;

            ui.gameObject.transform.localPosition = (Vector3.left * (mWidth * value)) + new Vector3(mHalfWidth, 0, 0);
        }
    }

    public void CreateThumbNail(TimeLineObject timelineObj)
    {
        // 생성 후 초기화 한번 해주고
        // Update문에서 TimeLineThumbNailUI를 갱신 시킨다.
        TimeLineThumbNailUI inst = GameObjectPool.Instantiate<TimeLineThumbNailUI>(profilePrefab, mProfileParent);
        inst.TimeLineObj = timelineObj;
        mThumbNailLList.AddLast(inst);
    }
}
