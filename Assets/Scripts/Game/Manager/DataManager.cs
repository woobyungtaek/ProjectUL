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
        for(int index = 0; index < 5; ++index)
        {
            WeaponData data = new WeaponData(index, $"Weapon_{index}", index + 1, 5 + Random.Range(0, 20f));

            mWeaponDataDict.Add(data.Name, data);
        }

        // 전체 / 타겟만
        mWeaponDataDict["Weapon_0"].TargetCoordiList.Add(new Vector3Int(0,0,1));
        mWeaponDataDict["Weapon_0"].AttackCoordiList.Add(new Vector3Int(0,0,0));

        // 가로줄 / 타겟만
        mWeaponDataDict["Weapon_1"].TargetCoordiList.Add(new Vector3Int(0, 0, 2));
        mWeaponDataDict["Weapon_1"].AttackCoordiList.Add(new Vector3Int(0, 0, 0));

        // 세로줄 / 타겟만
        mWeaponDataDict["Weapon_2"].TargetCoordiList.Add(new Vector3Int(1, 0, 3));
        mWeaponDataDict["Weapon_2"].AttackCoordiList.Add(new Vector3Int(0, 0, 0));

        // 가로줄 / 세로 전체
        mWeaponDataDict["Weapon_3"].TargetCoordiList.Add(new Vector3Int(1, 0, 2));
        mWeaponDataDict["Weapon_3"].AttackCoordiList.Add(new Vector3Int(-1, -1, 3));

        // 세로줄 / 가로 전체
        mWeaponDataDict["Weapon_4"].TargetCoordiList.Add(new Vector3Int(2, 0, 3));
        mWeaponDataDict["Weapon_4"].AttackCoordiList.Add(new Vector3Int(-1, -1, 2));
    }

    public WeaponData GetWeaponDataByName(string name)
    {
        return mWeaponDataDict[name];
    }
}
