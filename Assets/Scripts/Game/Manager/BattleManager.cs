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
    private WeaponSelectUI mWeaponSelectUI;
    public GameObject FieldGameObjPrefab { get => mFieldGameObjPrefab; }

    // 메인 카메라
    private Camera mMainCamera;

    // 행동권을 얻은 객체
    [SerializeField]
    private TimeLineObject mCurrentTurnObj;

    [SerializeField]
    private int mAniCount;

    // 필드 크기 (갯수)
    [SerializeField]
    private readonly int mX = 10, mY = 10;

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
    [SerializeField] // 슬롯 전체
    private List<List<FieldSlot>> mFieldSlotList = new List<List<FieldSlot>>();
    [SerializeField] // 무기 선택 시 타겟
    private List<FieldSlot> mTargetFieldSlotList = new List<FieldSlot>();
    [SerializeField] // 타겟 선택 시 범위
    private List<FieldSlot> mAttackFieldSlotList = new List<FieldSlot>();

    // 필드 게임오브젝트 리스트
    [SerializeField]
    private LinkedList<GameObject> mFieldGameObjLList = new LinkedList<GameObject>();


    [SerializeField]
    private Player mPlayer;
    [SerializeField]
    private Weapon mSelectWeapon;

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
        if (!Input.GetMouseButtonDown(0)) { return; }

        // raycasthit 체크
        Ray ray = mMainCamera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit)) { return; }

        // Slot인지 확인
        FieldSlot hitSlot = hit.collider.gameObject.GetComponent<FieldSlot>();
        if (hitSlot == null) { return; }

        if (hitSlot.CurrentFieldObj == null)
        {
            Debug.Log($"Test : {mSelectCoordi}에 Scarecrow 생성");

            // Enemy는 생성하고 Init하면 TimeLine과 FieldObject가 초기화 된다.
            Scarecrow instScarecrow = new Scarecrow();
            instScarecrow.Init(hitSlot);
        }
        else
        {
            Debug.Log($"Test : {mSelectCoordi}에 Scarecrow 제거 해야함");
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
        mMainCamera = Camera.main;
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
        foreach (List<FieldSlot> fieldSlots in mFieldSlotList)
        {
            foreach (FieldSlot slot in fieldSlots)
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
                for (int dir = 0; dir < 8; ++dir)
                {
                    int rX = x + mNearCoordiArr[dir].x;
                    int rY = y + mNearCoordiArr[dir].y;

                    // 범위에 없는 경우
                    if (rX < 0 || rX >= mX || rY < 0 || rY >= mY)
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
        if (mTurnWaitLList.Count != 0)
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

    private void PlayerInputFunc()
    {
        if (!Input.GetMouseButtonDown(0)) { return; }

        // raycasthit 체크
        Ray ray = mMainCamera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit)) { return; }

        // Slot인지 확인
        FieldSlot hitSlot = hit.collider.gameObject.GetComponent<FieldSlot>();
        if (hitSlot == null) { return; }

        bool bContain = mTargetFieldSlotList.Contains(hitSlot);
        if (bContain == false) { return; }

        // 공격 리스트에 해당하는 몬스터 빨간색 표시
        foreach (FieldSlot slot in mAttackFieldSlotList)
        {
            // 이전 내용 끄기
            slot.ChangeAttackTarget(false);
        }
        mAttackFieldSlotList.Clear();

        // Weapon의 공격 범위를 가져온다.
        foreach (Vector3Int coordi in mSelectWeapon.CurWeaponData.AttackCoordiList)
        {
            AddAttackList(hitSlot, coordi);
        }

        // 공격 리스트에 해당하는 몬스터 빨간색 표시
        foreach (FieldSlot slot in mAttackFieldSlotList)
        {
            slot.ChangeAttackTarget(true);
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
        if (mAniCount < 0)
        {
            Debug.LogError($"애니메이션 카운트 : {mAniCount}");
            return;
        }
    }

    public void ResumeFlowFunc()
    {
        if (mAniCount > 0) { return; }
        if (mFlowFunc != null) { return; }
        mFlowFunc = TimeFlowFunc;
    }

    #endregion

    #region 플레이어 행동 관련

    public void OnWeaponSelectUI()
    {
        // 무기 선택 UI 켜기
        if (mWeaponSelectUI == null) { return; }
        mWeaponSelectUI.InitWeaponSelectUI(mPlayer);

        // 유저 입력 켜기
        mFlowFunc = PlayerInputFunc;
    }

    public void SelectWeaponByUI(Weapon weapon)
    {
        if (mSelectWeapon == weapon) { return; }

        // 공격 가능 슬롯 초기화
        mTargetFieldSlotList.Clear();

        foreach (var slots in mFieldSlotList)
        {
            foreach (var slot in slots)
            {
                slot.ChangeSlotState_Reset();
            }
        }

        // Weapon의 정보 대로 값을 보여준다.
        mSelectWeapon = weapon;

        // 선택 가능한 slotList 만들기
        var list = mSelectWeapon.CurWeaponData.TargetCoordiList;
        foreach (var coordi in list)
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

        switch (selectType)
        {
            case ETargetSelectType.Point:
                {
                    // 현재 SelectTarget 좌표기준으로 계산된 좌표 값
                    if (x < 0 || y < 0 || x >= mX || y >= mY) { break; }
                    mTargetFieldSlotList.Add(mFieldSlotList[x][y]);
                }
                break;
            case ETargetSelectType.All:
                {
                    foreach (List<FieldSlot> slots in mFieldSlotList)
                    {
                        mTargetFieldSlotList.AddRange(slots);
                    }
                }
                break;
            case ETargetSelectType.Hor:
                {
                    if (y < 0 || y >= mY) { break; }
                    foreach (List<FieldSlot> slots in mFieldSlotList)
                    {
                        mTargetFieldSlotList.Add(slots[y]);
                    }
                }
                break;
            case ETargetSelectType.Ver:
                {
                    if (x < 0 || x >= mX) { break; }
                    mTargetFieldSlotList.AddRange(mFieldSlotList[x]);
                }
                break;
            case ETargetSelectType.RightUp:
                {
                    if (x < 0 || y < 0 || x >= mX || y >= mY) { break; }
                    mTargetFieldSlotList.Add(mFieldSlotList[x][y]);

                    FieldSlot inst = mFieldSlotList[x][y].RightUp;
                    while (inst != null)
                    {
                        mTargetFieldSlotList.Add(inst);
                        inst = inst.RightUp;
                    }

                    inst = mFieldSlotList[x][y].LeftDown;
                    while (inst != null)
                    {
                        mTargetFieldSlotList.Add(inst);
                        inst = inst.LeftDown;
                    }
                }
                break;
            case ETargetSelectType.LeftUp:
                {
                    if (x < 0 || y < 0 || x >= mX || y >= mY) { break; }
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
                    for (int idxX = 0; idxX < mX; ++idxX)
                    {
                        for (int idxY = 0; idxY < mY; ++idxY)
                        {
                            if ((idxX + idxY) % 2 == 0) { continue; }
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
                    foreach (List<FieldSlot> slotList in mFieldSlotList)
                    {
                        foreach (FieldSlot slot in slotList)
                        {
                            if (slot.CurrentFieldObj == null) { continue; }

                            Enemy enemy = slot.CurrentFieldObj as Enemy;
                            if (enemy == null) { continue; }

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

    private void AddAttackList(FieldSlot slot, Vector3Int area)
    {
        EAttackSelectType selectType = (EAttackSelectType)area.z;

        Vector2Int slotCoordi = slot.FieldCoordi;
        switch (selectType)
        {
            case EAttackSelectType.Point:
                {
                    // 무기 좌표 + 선택 좌표
                    int aX = area.x + slotCoordi.x;
                    int aY = area.x + slotCoordi.y;
                    if (aX < 0 || aY < 0 || aX >= mX || aY >= mY) { break; }
                    mAttackFieldSlotList.Add(mFieldSlotList[aX][aY]);
                }
                break;
            case EAttackSelectType.All:
                {
                    // 전체 중 오브젝트가 있는 경우
                    foreach (var list in mFieldSlotList)
                    {
                        mAttackFieldSlotList.AddRange(list);
                    }
                }
                break;
            case EAttackSelectType.Hor:
                {
                    // max, min 설정 (max, min 까지 포함 해야함)
                    int max = slotCoordi.x + area.y; // 우
                    int min = slotCoordi.x - area.x; // 좌

                    if (min > max)
                    {
                        int temp = min;
                        min = max;
                        max = temp;
                    }

                    IEnumerable<FieldSlot> list = from slots in mFieldSlotList
                                                  from cSlot in slots
                                                  where cSlot.FieldCoordi.y == slotCoordi.y
                                                  where min <= cSlot.FieldCoordi.x && cSlot.FieldCoordi.x <= max
                                                  select cSlot;
                    mAttackFieldSlotList.AddRange(list);
                }
                break;
            case EAttackSelectType.Ver:
                {
                    // max, min 설정 (max, min 까지 포함 해야함)
                    int max = slotCoordi.y + area.x; // 상 
                    int min = slotCoordi.y - area.y; // 하
                    if (min > max)
                    {
                        int temp = min;
                        min = max;
                        max = temp;
                    }

                    IEnumerable<FieldSlot> list = from cSlot in mFieldSlotList[slotCoordi.x]
                                                  where min <= cSlot.FieldCoordi.y && cSlot.FieldCoordi.y <= max
                                                  select cSlot;

                    mAttackFieldSlotList.AddRange(list);
                }
                break;
            case EAttackSelectType.RightUp:
                {
                    // 최소 좌표 최대 좌표 (이미 카운트가 적용된 상태)
                    Vector2Int maxCoordi = new Vector2Int(slotCoordi.x + area.y, slotCoordi.y + area.y);
                    Vector2Int minCoordi = new Vector2Int(slotCoordi.x - area.x, slotCoordi.y + (area.x * -1));

                    // 최소 값 조정
                    if (minCoordi.x > maxCoordi.x)
                    {
                        Vector2Int temp = minCoordi;
                        minCoordi = maxCoordi;
                        maxCoordi = temp;
                    }

                    // 둘다 한 쪽으로 범위가 나간경우는 타겟 없음
                    if (!(minCoordi.x < mX && minCoordi.y < mY)) { break; }
                    if (!(maxCoordi.x >= 0 && maxCoordi.y >= 0)) { break; }

                    FieldSlot inst = slot;
                    if (maxCoordi.x <= slotCoordi.x)
                    {
                        // start는 max부터 leftdown
                        inst = mFieldSlotList[maxCoordi.x][maxCoordi.y];
                        while(true)
                        {
                            if(inst == null) { break; }
                            mAttackFieldSlotList.Add(inst);
                            if(inst.FieldCoordi == minCoordi) { break; }
                            inst = inst.LeftDown;
                        }
                        break;
                    }
                    else if(minCoordi.x >= slotCoordi.x)
                    {
                        // start는 min부터 rightup
                        inst = mFieldSlotList[minCoordi.x][minCoordi.y];
                        while (true)
                        {
                            if (inst == null) { break; }
                            mAttackFieldSlotList.Add(inst);
                            if (inst.FieldCoordi == maxCoordi) { break; }
                            inst = inst.RightUp;
                        }
                        break;
                    }

                    // 그렇지 않은경우 양쪽으로 찾아가면됨
                    FieldSlot start = inst;
                    while(true)
                    {
                        if (inst == null) { break; }
                        mAttackFieldSlotList.Add(inst);
                        if (inst.FieldCoordi == minCoordi) { break; }
                        inst = inst.LeftDown;
                    }
                    inst = start;
                    while(true)
                    {
                        if (inst == null) { break; }
                        mAttackFieldSlotList.Add(inst);
                        if (inst.FieldCoordi == maxCoordi) { break; }
                        inst = inst.RightUp;
                    }
                }
                break;
            case EAttackSelectType.LeftUp:
                {
                    // 최소 좌표 최대 좌표 (이미 카운트가 적용된 상태)
                    Vector2Int maxCoordi = new Vector2Int(slotCoordi.x - area.x, slotCoordi.y + area.x);
                    Vector2Int minCoordi = new Vector2Int(slotCoordi.x + area.y, slotCoordi.y + (area.y * -1));

                    // 최소 값 조정
                    if (minCoordi.x < maxCoordi.x)
                    {
                        Vector2Int temp = minCoordi;
                        minCoordi = maxCoordi;
                        maxCoordi = temp;
                    }

                    if (!( minCoordi.x >= 0 && minCoordi.y < mY )) { break; }
                    if (!( maxCoordi.x < mX && maxCoordi.y >= 0 )) { break; }

                    FieldSlot inst = slot;
                    if (maxCoordi.x >= slotCoordi.x)
                    {
                        // start는 max부터 RightDown
                        inst = mFieldSlotList[maxCoordi.x][maxCoordi.y];
                        while (true)
                        {
                            if (inst == null) { break; }
                            mAttackFieldSlotList.Add(inst);
                            if (inst.FieldCoordi == minCoordi) { break; }
                            inst = inst.RightDown;
                        }
                        break;
                    }
                    else if (minCoordi.x <= slotCoordi.x)
                    {
                        // start는 min부터 LeftUp
                        inst = mFieldSlotList[minCoordi.x][minCoordi.y];
                        while (true)
                        {
                            if (inst == null) { break; }
                            mAttackFieldSlotList.Add(inst);
                            if (inst.FieldCoordi == maxCoordi) { break; }
                            inst = inst.LeftUp;
                        }
                        break;
                    }

                    // 그렇지 않은경우 양쪽으로 찾아가면됨
                    FieldSlot start = inst;
                    while (true)
                    {
                        if (inst == null) { break; }
                        mAttackFieldSlotList.Add(inst);
                        if (inst.FieldCoordi == minCoordi) { break; }
                        inst = inst.RightDown;
                    }
                    inst = start;
                    while (true)
                    {
                        if (inst == null) { break; }
                        mAttackFieldSlotList.Add(inst);
                        if (inst.FieldCoordi == maxCoordi) { break; }
                        inst = inst.LeftUp;
                    }
                }
                break;
            case EAttackSelectType.Odd:
                break;
            case EAttackSelectType.Even:
                break;
            case EAttackSelectType.OnlyStruct:
                break;
            case EAttackSelectType.OnlyEnemy:
                break;
            case EAttackSelectType.Random:
                break;
            case EAttackSelectType.StaticPoint:
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
        if (args == null)
        {
            Debug.LogError("args가 null이다.");
        }
        mTimeLineObjList.AddLast(args.timelineObj);
    }
}