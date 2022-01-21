using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ExtensionMethod;


public class BattleManager : Singleton<BattleManager>
{
    // 프리팹들
    [SerializeField]
    private GameObject mFieldSlotPrefab;
    [SerializeField]
    private GameObject mFieldGameObjPrefab;
    [SerializeField]
    private WeaponSelectPopup mWeaponSelectPopup;
    public GameObject FieldGameObjPrefab { get => mFieldGameObjPrefab; }

    // 행동권을 얻은 객체
    [SerializeField]
    private TimeLineObject mCurrentTurnObj;

    [SerializeField]
    private int mAniCount;

    // 필드 크기 (갯수)
    [SerializeField]
    private readonly int mX = 5, mY = 5;

    private readonly Vector2Int[] mNearCoordiArr
        = new Vector2Int[] {
            Vector2Int.up,
            new Vector2Int(1,1),
            Vector2Int.right,
            new Vector2Int(1,-1),
            Vector2Int.down,
            new Vector2Int(-1,-1),
            Vector2Int.left,
            new Vector2Int(-1,1)
        };

    // 필드 슬롯 리스트
    [SerializeField]
    private Transform mFieldsTransform;
    [SerializeField]
    private List<List<FieldSlot>> mFieldSlotList = new List<List<FieldSlot>>();
    [SerializeField]
    private List<FieldSlot> mTargetFieldSlotList = new List<FieldSlot>();

    // 필드 게임오브젝트 리스트
    [SerializeField]
    private LinkedList<GameObject> mFieldGameObjLList = new LinkedList<GameObject>();


    [SerializeField]
    private Player mPlayer;
    [SerializeField]
    private Weapon mSelectWeapon;
    [SerializeField]
    private FieldSlot mSelectSlot;

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
    private Vector2Int mSelectCoordi = new Vector2Int(-1, -1);

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
            int num = (mSelectCoordi.y * mX) +mSelectCoordi.x;
            if (num >= 0 && num < mX * mY)
            {
                // 해당 필드에 오브젝트가 없을때
                if(mFieldSlotList[mSelectCoordi.x][mSelectCoordi.y].CurrentFieldObj != null) { return; }

                Debug.Log($"Test : {mSelectCoordi}에 Scarecrow 생성");

                // Enemy는 생성하고 Init하면 TimeLine과 FieldObject가 초기화 된다.
                Scarecrow instScarecrow = new Scarecrow();
                instScarecrow.Init(mFieldSlotList[mSelectCoordi.x][mSelectCoordi.y]);
            }
        }
        else if( Input.GetKeyDown(KeyCode.Escape))
        {
            TestSelectSlot(-1);
        }
    }

    private void TestSelectSlot(int idx)
    {
        mSelectCoordi = new Vector2Int(idx % mX, idx / mX);

        foreach(List<FieldSlot> fieldSlots in mFieldSlotList)
        {
            foreach (FieldSlot slot in fieldSlots)
            {
                slot.SelectSlot(mSelectCoordi);
            }
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
        mPlayer.InitPlayer();

        // Player GameObject 생성

        // Field Slot 삭제
        foreach(List<FieldSlot> fieldSlots in mFieldSlotList)
        {
            foreach(FieldSlot slot in fieldSlots)
            {
                GameObjectPool.Destroy(slot.gameObject);
            }
            fieldSlots.Clear();
        }
        mFieldSlotList.Clear();

        // Field Slot 생성
        for (int x = 0; x < mX; ++x)
        {
            mFieldSlotList.Add(new List<FieldSlot>());
            for (int y = 0; y < mY; ++y)
            {
                mFieldSlotList[x].Add(GameObjectPool.Instantiate<FieldSlot>(mFieldSlotPrefab, mFieldsTransform));
                mFieldSlotList[x][y].InitSlot(new Vector2Int(x, y));
            }
        }

        // 인접 슬롯 설정
        for (int x = 0; x < mX; ++x)
        {
            for (int y = 0; y < mY; ++y)
            {
                for(int dir = 0; dir < 8; ++dir)
                {
                    int rX = x + mNearCoordiArr[dir].x;
                    int rY = y + mNearCoordiArr[dir].y;

                    // 범위에 없는 경우
                    if(rX <0 || rX >= mX ||rY < 0|| rY>= mY)
                    {
                        mFieldSlotList[x][y].SetNearSlot(dir, null);
                        continue;
                    }

                    // 범위에 있는 경우
                    mFieldSlotList[x][y].SetNearSlot(dir, mFieldSlotList[rX][rY]);
                }
            }
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
        if(mWeaponSelectPopup == null) { return; }
        mWeaponSelectPopup.InitWeaponSelectPopup(mPlayer);

        //if(mFieldSlotList[0].CurrentFieldObj != null)
        //{
        //    mFieldSlotList[0].CurrentFieldObj.Hit(3f, Vector3.zero);
        //}
        //ResumeFlowFunc();
    }

    public void SelectWeaponByUI(Weapon weapon)
    {
        if(mSelectWeapon == weapon) { return; }

        // 공격 가능 슬롯 초기화
        mTargetFieldSlotList.Clear();

        foreach(var slots in mFieldSlotList)
        {
            foreach(var slot in slots)
            {
                slot.ChangeSlotState_Reset();
            }
        }

        // Weapon의 정보 대로 값을 보여준다.
        mSelectWeapon = weapon;

        // 선택 가능한 slotList 만들기
        var list = mSelectWeapon.CurWeaponData.TargetCoordiList;
        foreach(var coordi in list)
        {
            AddTargetList(coordi);
        }

        // 중복제거 (Linq)
        mTargetFieldSlotList = mTargetFieldSlotList.Distinct().ToList();

        // 슬롯 상태 변경 (Select 가능하게)
        foreach (var slot in mTargetFieldSlotList)
        {
            slot.ChangeSlotState_PossibleSelect();
        }
    }

    private void AddTargetList(Vector3Int area)
    {
        //일단 z값 읽어서 속성
        ETargetSelectType selectType = (ETargetSelectType)area.z;

        int x = area.x;
        int y = area.y;

        switch(selectType)
        {
            case ETargetSelectType.Point:
                {
                    // x, y 좌표가 있는지 확인 후
                    if( x < 0 || y < 0 || x >= mX || y >= mY) { break; }
                    mTargetFieldSlotList.Add(mFieldSlotList[x][y]);
                }
                break;
            case ETargetSelectType.All:
                {
                    foreach(List<FieldSlot> slots in mFieldSlotList)
                    {
                        mTargetFieldSlotList.AddRange(slots);
                    }
                }
                break;
            case ETargetSelectType.Hor:
                {
                    // x, y 좌표있고 y값이 고정이 되어야함
                    foreach (List<FieldSlot> slots in mFieldSlotList)
                    {
                        mTargetFieldSlotList.Add(slots[y]);
                    }
                }
                break;
            case ETargetSelectType.Ver:
                {
                    mTargetFieldSlotList.AddRange(mFieldSlotList[x]);
                }
                break;
            case ETargetSelectType.RightUp:
                {
                    // 일단 해당 좌표 추가
                    mTargetFieldSlotList.Add(mFieldSlotList[x][y]);

                    FieldSlot inst = mFieldSlotList[x][y].RightUp;
                    while(inst != null)
                    {
                        mTargetFieldSlotList.Add(inst);
                        inst = inst.RightUp;
                    }

                    inst = mFieldSlotList[x][y].LeftDown;
                    while(inst != null)
                    {
                        mTargetFieldSlotList.Add(inst);
                        inst = inst.LeftDown;
                    }
                }
                break;
            case ETargetSelectType.LeftUp:
                {
                    // 일단 해당 좌표 추가
                    mTargetFieldSlotList.Add(mFieldSlotList[x][y]);

                    FieldSlot inst = mFieldSlotList[x][y].LeftUp;
                    while (inst != null)
                    {
                        mTargetFieldSlotList.Add(inst);
                        inst = inst.LeftUp;
                    }

                    inst = mFieldSlotList[x][y].RightDown;
                    while (inst != null)
                    {
                        mTargetFieldSlotList.Add(inst);
                        inst = inst.RightDown;
                    }
                }
                break;
            case ETargetSelectType.Odd:
                {
                    for(int idxX = 0; idxX < mX; ++idxX)
                    {
                        for (int idxY = 0; idxY < mY; ++idxY)
                        {
                            if( (idxX+idxY) % 2 == 0 ) { continue; }
                            mTargetFieldSlotList.Add(mFieldSlotList[idxX][idxY]);
                        }
                    }
                }
                break;
            case ETargetSelectType.Even:
                {
                    for (int idxX = 0; idxX < mX; ++idxX)
                    {
                        for (int idxY = 0; idxY < mY; ++idxY)
                        {
                            if ((idxX + idxY) % 2 != 0) { continue; }
                            mTargetFieldSlotList.Add(mFieldSlotList[idxX][idxY]);
                        }
                    }
                }
                break;
            case ETargetSelectType.OnlyStruct:
                {
                    // 추후 구현
                }
                break;
            case ETargetSelectType.OnlyEnemy:
                {
                    foreach(List<FieldSlot> slotList in mFieldSlotList)
                    {
                        foreach(FieldSlot slot in slotList)
                        {
                            if(slot.CurrentFieldObj == null) { continue; }
                            
                            Enemy enemy = slot.CurrentFieldObj as Enemy;
                            if(enemy == null) { continue; }

                            mTargetFieldSlotList.Add(slot);
                        }
                    }
                }
                break;
            case ETargetSelectType.OnlyEmpty:
                {
                    foreach (List<FieldSlot> slotList in mFieldSlotList)
                    {
                        foreach (FieldSlot slot in slotList)
                        {
                            if (slot.CurrentFieldObj != null) { continue; }
                            mTargetFieldSlotList.Add(slot);
                        }
                    }
                }
                break;
        }
    }

    #endregion

    #region FieldSlot관련


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
