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

    public void CreateThumbNail(TimeLineObject timelineObj)
    {
        // ���� �� �ʱ�ȭ �ѹ� ���ְ�
        // Update������ TimeLineThumbNailUI�� ���� ��Ų��.
        TimeLineThumbNailUI inst = GameObjectPool.Instantiate<TimeLineThumbNailUI>(profilePrefab, mProfileParent);
        inst.TimeLineObj = timelineObj;
        mThumbNailLList.AddLast(inst);
    }
}
