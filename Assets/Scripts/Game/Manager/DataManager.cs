using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    // 데이터를 변형하면 안되니깐 Weapon을 받아서 값을 복사해 놓는게 나을듯

    private Dictionary<string, WeaponData> mWeaponDataDict = new Dictionary<string, WeaponData>();

    private void Awake()
    {
        // 무기 데이터를 불러옵니다.
        // 임의로 생성해 봅시다잉
        CreateTestWeaponData();
    }

    private void CreateTestWeaponData()
    {
        for(int index = 0; index < 11; ++index)
        {
            WeaponData data = new WeaponData(index, $"Weapon_{index}", index + 1, 5 + Random.Range(0, 20f));

            mWeaponDataDict.Add(data.Name, data);
        }

        // 단일 속성
        // 전체 / 타겟만
        mWeaponDataDict["Weapon_0"].TargetCoordiList.Add(new Vector3Int(0, 0, (int)ETargetSelectType.All )      );

        // 가로줄 / 타겟만
        mWeaponDataDict["Weapon_1"].TargetCoordiList.Add(new Vector3Int(0, 0, (int)ETargetSelectType.Hor)       );

        // 세로줄 / 타겟만
        mWeaponDataDict["Weapon_2"].TargetCoordiList.Add(new Vector3Int(1, 0, (int)ETargetSelectType.Ver)       );

        // 가로줄 / 세로 전체
        mWeaponDataDict["Weapon_3"].TargetCoordiList.Add(new Vector3Int(1, 1, (int)ETargetSelectType.RightUp)   );

        // 세로줄 / 가로 전체
        mWeaponDataDict["Weapon_4"].TargetCoordiList.Add(new Vector3Int(1, 1, (int)ETargetSelectType.LeftUp)    );

        // 빈 칸 전체
        mWeaponDataDict["Weapon_5"].TargetCoordiList.Add(new Vector3Int(0, 0, (int)ETargetSelectType.OnlyEmpty) );

        // 적이 있는 칸 전체
        mWeaponDataDict["Weapon_6"].TargetCoordiList.Add(new Vector3Int(0, 0, (int)ETargetSelectType.OnlyEnemy) );

        // 체크 홀수
        mWeaponDataDict["Weapon_7"].TargetCoordiList.Add(new Vector3Int(0, 0, (int)ETargetSelectType.Odd));

        // 체크 짝수
        mWeaponDataDict["Weapon_8"].TargetCoordiList.Add(new Vector3Int(0, 0, (int)ETargetSelectType.Even));

        // 합성 속성
        // 십자가
        mWeaponDataDict["Weapon_9"].TargetCoordiList.Add(new Vector3Int(2, 2, (int)ETargetSelectType.Hor));
        mWeaponDataDict["Weapon_9"].TargetCoordiList.Add(new Vector3Int(2, 2, (int)ETargetSelectType.Ver));

        // X자
        mWeaponDataDict["Weapon_10"].TargetCoordiList.Add(new Vector3Int(2, 2, (int)ETargetSelectType.RightUp));
        mWeaponDataDict["Weapon_10"].TargetCoordiList.Add(new Vector3Int(2, 2, (int)ETargetSelectType.LeftUp));


    }

    public WeaponData GetWeaponDataByName(string name)
    {
        return mWeaponDataDict[name];
    }
}
