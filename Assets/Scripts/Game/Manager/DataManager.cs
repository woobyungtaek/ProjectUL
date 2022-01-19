using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    // �����͸� �����ϸ� �ȵǴϱ� Weapon�� �޾Ƽ� ���� ������ ���°� ������

    private Dictionary<string, WeaponData> mWeaponDataDict = new Dictionary<string, WeaponData>();

    private void Awake()
    {
        // ���� �����͸� �ҷ��ɴϴ�.
        // ���Ƿ� ������ ���ô���
        CreateTestWeaponData();
    }

    private void CreateTestWeaponData()
    {
        for(int index = 0; index < 5; ++index)
        {
            WeaponData data = new WeaponData(index, $"Weapon_{index}", index + 1, 5 + Random.Range(0, 20f));

            mWeaponDataDict.Add(data.Name, data);
        }

        // ��ü / Ÿ�ٸ�
        mWeaponDataDict["Weapon_0"].TargetCoordiList.Add(new Vector3Int(0,0,1));
        mWeaponDataDict["Weapon_0"].AttackCoordiList.Add(new Vector3Int(0,0,0));

        // ������ / Ÿ�ٸ�
        mWeaponDataDict["Weapon_1"].TargetCoordiList.Add(new Vector3Int(0, 0, 2));
        mWeaponDataDict["Weapon_1"].AttackCoordiList.Add(new Vector3Int(0, 0, 0));

        // ������ / Ÿ�ٸ�
        mWeaponDataDict["Weapon_2"].TargetCoordiList.Add(new Vector3Int(1, 0, 3));
        mWeaponDataDict["Weapon_2"].AttackCoordiList.Add(new Vector3Int(0, 0, 0));

        // ������ / ���� ��ü
        mWeaponDataDict["Weapon_3"].TargetCoordiList.Add(new Vector3Int(1, 0, 2));
        mWeaponDataDict["Weapon_3"].AttackCoordiList.Add(new Vector3Int(-1, -1, 3));

        // ������ / ���� ��ü
        mWeaponDataDict["Weapon_4"].TargetCoordiList.Add(new Vector3Int(2, 0, 3));
        mWeaponDataDict["Weapon_4"].AttackCoordiList.Add(new Vector3Int(-1, -1, 2));
    }

    public WeaponData GetWeaponDataByName(string name)
    {
        return mWeaponDataDict[name];
    }
}
