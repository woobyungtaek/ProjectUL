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
    public GameObject FieldGameObjPrefab { get => mFieldGameObjPrefab; }

    // 행동권을 얻은 객체
    [SerializeField]
    private TimeLineObject mCurrentTurnObj;

    [SerializeField]
    private int mAniCount;

    // 필드 크기 (갯수)
    [SerializeField]
    private int mRow = 3, mCol = 2;

    // 필드 슬롯 리스트
    [SerializeField]
    private Transform mFieldsTransform;
    [SerializeField]
    private List<FieldSlot> mFieldSlotList = new List<FieldSlot>();

    // 필드 게임오브젝트 리스트
    [SerializeField]
    private LinkedList<GameObject> mFieldGameObjLList = new LinkedList<GameObject>();

    [SerializeField]
    private Player mPlayer;

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
        else if(Input.GetKeyDown(KeyCode.Minus))
        {
            Time.timeScale -= 0.1f;
            if(Time.timeScale < 0.0f) { Time.timeScale = 0.0f; }
        }
        else if (Input.GetKeyDown(KeyCode.Equals))
        {
            Time.timeScale += 0.1f;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentSelectSlotIdx >= 0 && currentSelectSlotIdx < 6)
            {
                // 해당 필드에 오브젝트가 없을때
                if(mFieldSlotList[currentSelectSlotIdx].CurrentFieldObj != null) { return; }

                Debug.Log($"Test : {currentSelectSlotIdx}에 Scarecrow 생성");

                // Enemy는 생성하고 Init하면 TimeLine과 FieldObject가 초기화 된다.
                Scarecrow instScarecrow = new Scarecrow();
                instScarecrow.Init(mFieldSlotList[currentSelectSlotIdx]);
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
    
    private void Awake()
    {
        // 오브젝트 추가용
        ObserverCenter.Instance.AddObserver(ExcuteAddTimeLineObjectLList, Message.CreateTimeLineObject);
    }

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
        // Player 생성
        mPlayer = new Player();
        mPlayer.InitTimeLineObject("P");

        // Player GameObject 생성


        // Field Slot 삭제
        for (int idx = 0; idx < mFieldSlotList.Count; ++idx)
        {
            GameObjectPool.Destroy(mFieldSlotList[idx].gameObject);
        }
        mFieldSlotList.Clear();

        // Field Slot 생성
        for (int idx = 0; idx < mRow * mCol; ++idx)
        {
            mFieldSlotList.Add(GameObjectPool.Instantiate<FieldSlot>(mFieldSlotPrefab, mFieldsTransform));
            mFieldSlotList[idx].InitSlot(idx, mRow);
        }

        // 게임 플로우 시작
        mFlowFunc = TimeFlowFunc;
    }

    #region 대기열 관련

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

    #endregion

    #region Flow 재개 관련
    public void IncreaseAniCount()
    {
        // 애니메이션 카운트가 증가 합니다.
        mAniCount++;
    }

    public void DecreaseAniCount()
    {
        // 애니메이션 카운트가 감소 합니다.
        mAniCount--;
        if(mAniCount < 0)
        {
            Debug.LogError($"애니메이션 카운트 : {mAniCount}");
            return;
        }
    }

    public void ResumeFlowFunc()
    {
        if(mAniCount > 0) { return; }
        if(mFlowFunc != null) { return; }
        mFlowFunc = TimeFlowFunc;
    }

    #endregion

    #region 플레이어 행동 관련

    public void PopupWeaponSelectUI()
    {
        // 무기 UI를 표시합니다.
        // 팝업 UI를 가지고 있고 플레이어의 데이터를 넘겨서 표시한다.
        // 팝업 UI에서 무기를 선택하면 다시 넘겨 받아서 다음 단계로 넘어가야한다.
    }

    #endregion

    // 옵저버 함수
    public void ExcuteAddTimeLineObjectLList(Notification noti)
    {
        TimeLineObjNotiArg args = noti.Data as TimeLineObjNotiArg;
        if(args == null)
        {
            Debug.LogError("args가 null이다.");
        }
        mTimeLineObjList.AddLast(args.timelineObj);
    }
}
