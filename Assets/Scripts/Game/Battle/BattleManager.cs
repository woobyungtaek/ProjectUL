using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    // 프리팹들
    [SerializeField]
    private GameObject mFieldSlotPrefab;

    // 행동권을 얻은 객체
    [SerializeField]
    private TimeLineObject mCurrentTurnObj;

    // 필드 크기 (갯수)
    [SerializeField]
    private int mRow = 3, mCol = 2;

    // 필드 슬롯 리스트
    [SerializeField]
    private List<FieldSlot> mFieldSlotList = new List<FieldSlot>();

    // 플로우용 Delegate
    private delegate void FlowFunc();
    private FlowFunc mFlowFunc;

    // 시간흘러야하는 객체 리스트
    [SerializeField]
    private LinkedList<TimeLineObject> mTimeLineObjList = new LinkedList<TimeLineObject>();

    // 행동권 대기 연결 리스트 (꺼내고 지울때 0, 넣을때는 tail, 죽었을 때 index 지우기) 
    [SerializeField]
    private LinkedList<TimeLineObject> mTurnWaitLList = new LinkedList<TimeLineObject>();



    #region TestVal
    private int currentSelectSlotIdx = -1;
    private void TestInput()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            Debug.Log("Test : 0 Slot Select");
            TestSelectSlot(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Test : 1 Slot Select");
            TestSelectSlot(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Test : 2 Slot Select");
            TestSelectSlot(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("Test : 3 Slot Select");
            TestSelectSlot(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("Test : 4 Slot Select");
            TestSelectSlot(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Debug.Log("Test : 5 Slot Select");
            TestSelectSlot(5);
        }
    }

    private void TestSelectSlot(int idx)
    {
        for (int index = 0; index < mFieldSlotList.Count; ++index)
        {
            mFieldSlotList[index].SelectSlot(idx);
        }
    }
    #endregion

    private void Start()
    {
        InitBattle();
        mFlowFunc = InitBattle;
    }

    private void Update()
    {
        if (mFlowFunc != null)
        {
            mFlowFunc();
        }
    }

    // 전투 초기화
    private void InitBattle()
    {
        // Field Slot 삭제
        for (int idx = 0; idx < mFieldSlotList.Count; ++idx)
        {
            GameObjectPool.Destroy(mFieldSlotList[idx].gameObject);
        }
        mFieldSlotList.Clear();

        // Field Slot 생성
        for (int idx = 0; idx < mRow * mCol; ++idx)
        {
            mFieldSlotList.Add(GameObjectPool.Instantiate<FieldSlot>(mFieldSlotPrefab));
            mFieldSlotList[idx].InitSlot(idx, mRow, new Enemy());
        }

        // 게임 플로우 시작
        mFlowFunc = TimeFlowFunc;
    }

    // 대기 함수 
    private void TimeFlowFunc()
    {
        TestInput();

        // 공격 대기 리스트 확인
        if(mTurnWaitLList.Count != 0)
        {
            // 있다면 대기 리스트에서 팝
            mCurrentTurnObj = mTurnWaitLList.First.Value;

            // 대기 리스트에서 지우기
            mTurnWaitLList.RemoveFirst();

            // 행동이 모두 완료 될때까지 시간 흐르지 않기
            mFlowFunc = null;

            // 1프레임 쉬고 실행되는 코루틴 실행
            // (TimeFlowFunc() 가 종료 될 수 있도록)
            // currentAttackObj의 TurnActionFunc을 실행하여
            // 객체가 스스로 행동을 할 수 있도록 한다.
            mCurrentTurnObj.TurnActionFunc();
            return;
        }

        // 없다면 객체들 시간 흐르기
        foreach (TimeLineObject timeLineObj in mTimeLineObjList)
        {
            timeLineObj.FlowTime();
        }
    }
}
