using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*   
무기 데이터 작성 시 주의 사항

1. Index와 Name은 유일 해야합니다.
2. CoordiList들에는 Vector3Int값이 들어갑니다.
    - x, y는 좌표 또는 거리, 개수 / z는 좌표를 생성할 유형값을 의미합니다.
    - Attack시 제한 없음은 -1로 표시한다. 값이 있는 경우 해당 값은 속성에 따라 거리를 의미한다.
    
3. Target은 플레이어가 무기를 선택 했을 때 선택 가능한 FieldSlot을 의미합니다.
4. Attack은 플레이어가 실제로 공격하는 FieldSlot을 의미합니다.

5. 아래에 속성을 추가하거나 Point로 하나하나 추가
6. CoordiList값을 모두 대상으로 삼기 때문에 여러 좌표를 조합도 된다.
7. 하지만 불필요하게 많이 넣진 말라(all 넣어놓고 다른거 추가해봐야 의미 없다.)

                    Target                                             Attack
__z값_______________________________________________z값____________________________________________________________________________________
| 0   |   Point : 특정 좌표만 대상               |  0   |   Point : 타겟 기준 좌표 공격          (xy : 떨어진 거리)
| 1   |   All : Slot 전체 대상                   |  1   |   All : 인접 거리내 전체 공격          
| 2   |   Hor : 좌표에서 가로줄 전체             |  2   |   Hor : 좌표 기준 가로줄 공격          (x : 우측 인접 거리, y : 좌측 인접 거리)
| 3   |   Ver : 좌표에서 세로줄 전체             |  3   |   Ver : 좌표 기준 세로줄 공격          (x : 상위 인접 거리, y : 하위 인접 거리)
| 4   |   RUp : 좌표에서 우상향 전체(대각)       |  4   |   RUp : 좌표 기준 우상향 대각 공격     (x : 우측 인접 거리, y : 좌측 인접 거리)
| 5   |   LUp : 좌표에서 좌상향 전체(대각)       |  5   |   LUp : 좌표 기준 좌상향 대각 공격     (x : 우측 인접 거리, y : 좌측 인접 거리)
| 6   |   Odd : 좌표 합이 홀수 전체 (체크)       |  6   |   Odd : 좌표 합이 홀수 공격            (x : 가로 인접 거리, y : 세로 인접 거리)
| 7   |   Even: 좌표 합이 짝수 전체 (체크)       |  7   |   Even: 좌표 합이 짝수 공격            (x : 가로 인접 거리, y : 세로 인접 거리)
| 8   |   onlyStuct : 구조물이 있는 칸 전체      |  8   |   onlyStuct : 구조물이 있는 슬롯 공격  (x : 가로 인접 거리, y : 세로 인접 거리)
| 9   |   onlyEnemy : 적이 잇는 칸 전체          |  9   |   onlyEnemy : 적이 잇는 슬롯 공격      (x : 가로 인접 거리, y : 세로 인접 거리)
| 10  |   onlyEmpty : 빈칸 전체                  |  10  |   Random : 임의의 슬롯 공격            (x : 가로 인접 거리, y : 세로 인접 거리)
| 11  |                                          |  11  |   StaticPoint : 고정 좌표 공격         (xy : 고정 좌표)
------------------------------------------------------------------------------------------------------------------------------------------

8. 스킬 이름을 저장해두고 Delegate에 넣고 호출 시킨다.
9. 장판의 경우 스킬에 따라 달라지며 직접 공격한 영역에 적용된다.
 */

public enum ETargetSelectType
{
    Point = 0,
    All,
    Hor,        Ver,
    RightUp,    LeftUp,
    Odd,        Even,
    OnlyStruct, OnlyEnemy, OnlyEmpty
}
public enum EAttackSelectType
{
    Point = 0,
    All,
    Hor, Ver,
    RightUp, LeftUp,
    Odd, Even,
    OnlyStruct, OnlyEnemy,
    Random, StaticPoint
}

[System.Serializable]
public struct WeaponData
{
    // 무기 기본 정보
    public int      Index;
    public string   Name;
    public float    Power;
    public float    Speed;

    // 무기 스킬 리스트

    // 무기 공격 가능 좌표
    public List<Vector3Int> TargetCoordiList;

    // 무기 실제 공격 좌표
    public List<Vector3Int> AttackCoordiList;

    public WeaponData(int idx, string name, float power, float speed)
    {
        Index = idx;
        Name  = name;
        Power = power;
        Speed = speed;

        TargetCoordiList = new List<Vector3Int>();
        AttackCoordiList = new List<Vector3Int>();
    }
}
