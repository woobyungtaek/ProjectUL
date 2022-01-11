using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    // 프리팹들
    [SerializeField]
    private GameObject mFieldSlotPrefab;
    [SerializeField]
    private GameObject mFieldGameObjPrefab;

    // 행동권을 얻은 객체
    [SerializeField]
    private TimeLineObject mCurrentTurnObj;

    // 필드 크기 (갯수)
    [SerializeField]
    private int mRow = 3, mCol = 2;

    // 필드 슬롯 리스트
    [SerializeField]
    private List<FieldSlot> mFieldSlotList = new List<FieldSlot>();

    // 필드 게임오브젝트 리스트
    [SerializeField]
    private LinkedList<GameObject> mFieldGameObjLList = new LinkedList<GameObject>();

    // 플로우용 Delegate
    private delegate void FlowFunc();
    private FlowFunc mFlowFunc;

    // 시간흘러야하는 객체 리스트
    [SerializeField]
    private LinkedList<TimeLineObject> mTimeLineObjList = new LinkedList<TimeLineObject>();

    // 행동권 대기 연결 리스트 (꺼내고 지울때 0, 넣을때는 tail, 죽었을 때 index 지우기) 
    [SerializeField]
    private LinkedList<TimeLineObject> mTurnWaitLList = new LinkedList<TimeLineObject>();

    // 타임라인UI
    [SerializeField]
    private TimeLineBarUI mTimeLineBarUI;

    #region TestVal

    [SerializeField]
    private int currentSelectSlotIdx = -1;

    private void TestInput()
    {
        if(Input.anyKeyDown == false) { return; }

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
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentSelectSlotIdx >= 0 && currentSelectSlotIdx < 6)
            {
                if(mFieldSlotList[currentSelectSlotIdx].CurrentFieldObj != null) { return; }

                Debug.Log($"Test : {currentSelectSlotIdx}에 Scarecrow 생성");

                // 배틀 매니저에 FieldObject를 생성하는 함수가 있고
                // 이 함수를 호출시 FieldSlotList에 추가
                // FieldObject가 자동화가 되려면?


                // 오브젝트 풀에서 가져와야해서 생성 후 따로 초기화 과정을 거치는게 좋을 듯
                // 일단 바로 생성하는걸로 해두자. 일반 오브젝트 풀은 좀 다시 볼 필요가 있을 듯
                Scarecrow instScarecrow = new Scarecrow();
                instScarecrow.InitTimeLineObject(currentSelectSlotIdx);

                mFieldSlotList[currentSelectSlotIdx].CurrentFieldObj = instScarecrow;

                // 얘는 TimeObject가 따로 호출에서 추가되어야함
                // 그렇다면 TimeLine관련된건 자동화가 됨
                mTimeLineObjList.AddLast(instScarecrow);
            }
        }
        else if( Input.GetKeyDown(KeyCode.Escape))
        {
            TestSelectSlot(-1);
        }
    }

    private void TestSelectSlot(int idx)
    {
        currentSelectSlotIdx = idx;
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
            mFieldSlotList[idx].InitSlot(idx, mRow);
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

            // currentAttackObj의 TurnActionFunc을 실행
            mCurrentTurnObj.TurnActionFunc();
            return;
        }

        // 없다면 객체들 시간 흐르기
        foreach (TimeLineObject timeLineObj in mTimeLineObjList)
        {
            timeLineObj.FlowTime();
        }
    }

    // 대기 열에 추가 함수
    public void AddTurnWaitObj(TimeLineObject timeObj)
    {
        mTurnWaitLList.AddLast(timeObj);
    }
}
