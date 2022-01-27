using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*   
���� ������ �ۼ� �� ���� ����

1. Index�� Name�� ���� �ؾ��մϴ�.
2. CoordiList�鿡�� Vector3Int���� ���ϴ�.
    - x, y�� ��ǥ �Ǵ� �Ÿ�, ���� / z�� ��ǥ�� ������ �������� �ǹ��մϴ�.
    - Attack�� ���� ������ -1�� ǥ���Ѵ�. ���� �ִ� ��� �ش� ���� �Ӽ��� ���� �Ÿ��� �ǹ��Ѵ�.
    
3. Target�� �÷��̾ ���⸦ ���� ���� �� ���� ������ FieldSlot�� �ǹ��մϴ�.
4. Attack�� �÷��̾ ������ �����ϴ� FieldSlot�� �ǹ��մϴ�.

5. �Ʒ��� �Ӽ��� �߰��ϰų� Point�� �ϳ��ϳ� �߰�
6. CoordiList���� ��� ������� ��� ������ ���� ��ǥ�� ���յ� �ȴ�.
7. ������ ���ʿ��ϰ� ���� ���� ����(all �־���� �ٸ��� �߰��غ��� �ǹ� ����.)

                    Target                                             Attack
__z��_______________________________________________z��____________________________________________________________________________________
| 0   |   Point : Ư�� ��ǥ�� ���               |  0   |   Point : Ÿ�� ���� ��ǥ ����          (xy : ������ �Ÿ�)
| 1   |   All : Slot ��ü ���                   |  1   |   All : ���� �Ÿ��� ��ü ����          
| 2   |   Hor : ��ǥ���� ������ ��ü             |  2   |   Hor : ��ǥ ���� ������ ����          (x : ���� ���� �Ÿ�, y : ���� ���� �Ÿ�)
| 3   |   Ver : ��ǥ���� ������ ��ü             |  3   |   Ver : ��ǥ ���� ������ ����          (x : ���� ���� �Ÿ�, y : ���� ���� �Ÿ�)
| 4   |   RUp : ��ǥ���� ����� ��ü(�밢)       |  4   |   RUp : ��ǥ ���� ����� �밢 ����     (x : ���� ���� �Ÿ�, y : ���� ���� �Ÿ�)
| 5   |   LUp : ��ǥ���� �»��� ��ü(�밢)       |  5   |   LUp : ��ǥ ���� �»��� �밢 ����     (x : ���� ���� �Ÿ�, y : ���� ���� �Ÿ�)
| 6   |   Odd : ��ǥ ���� Ȧ�� ��ü (üũ)       |  6   |   Odd : ��ǥ ���� Ȧ�� ����            (x : ���� ���� , y : ���� ����)
| 7   |   Even: ��ǥ ���� ¦�� ��ü (üũ)       |  7   |   Even: ��ǥ ���� ¦�� ����            (x : ���� ���� , y : ���� ����)
| 8   |   onlyStuct : �������� �ִ� ĭ ��ü      |  8   |   onlyStuct : �������� �ִ� ���� ����  (x : ���� ���� , y : ���� ����)
| 9   |   onlyEnemy : ���� �մ� ĭ ��ü          |  9   |   onlyEnemy : ���� �մ� ���� ����      (x : ���� ���� , y : ���� ����)
| 10  |   onlyEmpty : ��ĭ ��ü                  |  10  |   Random : ������ ���� ����            (x : ���� ���� , y : ���� ����) // ������ ������ �Ӽ��� ����, ���� ������ ������ �����ϵ���
| 11  |                                          |  11  |   StaticPoint : ���� ��ǥ ����         (xy : ���� ��ǥ) 
------------------------------------------------------------------------------------------------------------------------------------------

8. ��ų �̸��� �����صΰ� Delegate�� �ְ� ȣ�� ��Ų��.
9. ������ ��� ��ų�� ���� �޶����� ���� ������ ������ ����ȴ�.
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
    StaticPoint
}

[System.Serializable]
public struct WeaponData
{
    // ���� �⺻ ����
    public int      Index;
    public string   Name;
    public float    Power;
    public float    Speed;

    // ���� ��ų ����Ʈ

    // ���� ���� ���� ��ǥ
    public List<Vector3Int> TargetCoordiList;

    // ���� ���� ���� ��ǥ
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
