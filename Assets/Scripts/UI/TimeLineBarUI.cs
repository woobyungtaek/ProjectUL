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

    private void Awake()
    {
        // 오브젝트 추가용
        ObserverCenter.Instance.AddObserver(ExcuteCreateThumbNail, Message.CreateTimeLineObject);

        // 오브젝트 제거용
        ObserverCenter.Instance.AddObserver(ExcuteRemoveThumbNail, Message.RemoveTimeLineObject);
    }

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

    // 추가용 함수
    public void ExcuteCreateThumbNail(Notification noti)
    {
        TimeLineObjNotiArg args = noti.Data as TimeLineObjNotiArg;
        if(args == null)
        {
            Debug.LogError("args가 없습니다.");
            return;
        }
        
        // 오브젝트 생성하기
        TimeLineThumbNailUI inst = GameObjectPool.Instantiate<TimeLineThumbNailUI>(profilePrefab, mProfileParent);
        inst.SlotStr = args.slotStr;
        inst.TimeLineObj = args.timelineObj;

        mThumbNailLList.AddLast(inst);
    }

    // 제거용 함수
    public void ExcuteRemoveThumbNail(Notification noti)
    {
        TimeLineObjNotiArg args = noti.Data as TimeLineObjNotiArg;
        if (args == null)
        {
            Debug.LogError("args가 없습니다.");
            return;
        }

        foreach(TimeLineThumbNailUI ui in mThumbNailLList)
        {
            if(ui.TimeLineObj == args.timelineObj)
            {
                // ui 초기화 및 풀로 반환
                ui.TimeLineObj = null;
                GameObjectPool.Destroy(ui.gameObject);

                mThumbNailLList.Remove(ui);
                break;
            }
        }
    }
}
