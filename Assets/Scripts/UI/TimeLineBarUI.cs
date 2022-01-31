using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLineBarUI : MonoBehaviour
{
    // ������ UI ������
    [SerializeField]
    private GameObject profilePrefab;

    // ������ ���� ��ġ
    [SerializeField]
    private RectTransform mProfileParent;

    [SerializeField]
    private LinkedList<TimeLineThumbNailUI> mThumbNailLList = new LinkedList<TimeLineThumbNailUI>();

    private float mWidth;
    private float mHalfWidth;

    private void Awake()
    {
        // ������Ʈ �߰���
        ObserverCenter.Instance.AddObserver(ExcuteCreateThumbNail, Message.CreateTimeLineObject);

        // ������Ʈ ���ſ�
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
            // ��� 100������ ���ؾ���
            float value = ui.TimeLineObj.CurrentTime / 100.0f;

            ui.gameObject.transform.localPosition = (Vector3.left * (mWidth * value)) + new Vector3(mHalfWidth, 0, 0);
        }
    }

    // �߰��� �Լ�
    public void ExcuteCreateThumbNail(Notification noti)
    {
        TimeLineObjNotiArg args = noti.Data as TimeLineObjNotiArg;
        if(args == null)
        {
            Debug.LogError("args�� �����ϴ�.");
            return;
        }
        
        // ������Ʈ �����ϱ�
        TimeLineThumbNailUI inst = GameObjectPool.Instantiate<TimeLineThumbNailUI>(profilePrefab, mProfileParent);
        inst.SlotStr = args.slotStr;
        inst.TimeLineObj = args.timelineObj;

        mThumbNailLList.AddLast(inst);
    }

    // ���ſ� �Լ�
    public void ExcuteRemoveThumbNail(Notification noti)
    {
        TimeLineObjNotiArg args = noti.Data as TimeLineObjNotiArg;
        if (args == null)
        {
            Debug.LogError("args�� �����ϴ�.");
            return;
        }

        foreach (TimeLineThumbNailUI ui in mThumbNailLList)
        {
            if (ui.TimeLineObj == args.timelineObj)
            {
                // ui �ʱ�ȭ �� Ǯ�� ��ȯ
                ui.TimeLineObj = null;
                GameObjectPool.Destroy(ui.gameObject);

                mThumbNailLList.Remove(ui);
                break;
            }
        }
    }
}
