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
        for(int index = 0; index < 11; ++index)
        {
            WeaponData data = new WeaponData(index, $"Weapon_{index}", index + 1, 5 + Random.Range(0, 20f));

            mWeaponDataDict.Add(data.Name, data);
        }

        // ���� �Ӽ�
        // ��ü / Ÿ�ٸ�
        mWeaponDataDict["Weapon_0"].TargetCoordiList.Add(new Vector3Int(0, 0, (int)ETargetSelectType.All ));
        mWeaponDataDict["Weapon_0"].AttackCoordiList.Add(new Vector3Int(0, 0, (int)EAttackSelectType.Point));

        // ������ / Ÿ�ٸ�
        mWeaponDataDict["Weapon_1"].TargetCoordiList.Add(new Vector3Int(0, 0, (int)ETargetSelectType.Hor));
        mWeaponDataDict["Weapon_1"].AttackCoordiList.Add(new Vector3Int(0, 0, (int)EAttackSelectType.Point));

        // ������ / Ÿ�ٸ�
        mWeaponDataDict["Weapon_2"].TargetCoordiList.Add(new Vector3Int(1, 0, (int)ETargetSelectType.Ver));
        mWeaponDataDict["Weapon_2"].AttackCoordiList.Add(new Vector3Int(0, 0, (int)EAttackSelectType.Point));

        // ������ / ���� ���� ��ü
        mWeaponDataDict["Weapon_3"].TargetCoordiList.Add(new Vector3Int(1, 1, (int)ETargetSelectType.RightUp));
        mWeaponDataDict["Weapon_3"].AttackCoordiList.Add(new Vector3Int(0, 0, (int)EAttackSelectType.Ver));

        // ������ / ���� ���� ��ü
        mWeaponDataDict["Weapon_4"].TargetCoordiList.Add(new Vector3Int(1, 1, (int)ETargetSelectType.LeftUp));
        mWeaponDataDict["Weapon_4"].AttackCoordiList.Add(new Vector3Int(0, 0, (int)EAttackSelectType.Hor));

        // �� ĭ ��ü / ���� �밢
        mWeaponDataDict["Weapon_5"].TargetCoordiList.Add(new Vector3Int(0, 0, (int)ETargetSelectType.OnlyEmpty));
        mWeaponDataDict["Weapon_5"].AttackCoordiList.Add(new Vector3Int(0, 0, (int)EAttackSelectType.Hor));

        // ���� �ִ� ĭ ��ü
        mWeaponDataDict["Weapon_6"].TargetCoordiList.Add(new Vector3Int(0, 0, (int)ETargetSelectType.OnlyEnemy));

        // üũ Ȧ��
        mWeaponDataDict["Weapon_7"].TargetCoordiList.Add(new Vector3Int(0, 0, (int)ETargetSelectType.Odd));

        // üũ ¦��
        mWeaponDataDict["Weapon_8"].TargetCoordiList.Add(new Vector3Int(0, 0, (int)ETargetSelectType.Even));

        // �ռ� �Ӽ�
        // ���ڰ�
        mWeaponDataDict["Weapon_9"].TargetCoordiList.Add(new Vector3Int(2, 2, (int)ETargetSelectType.Hor));
        mWeaponDataDict["Weapon_9"].TargetCoordiList.Add(new Vector3Int(2, 2, (int)ETargetSelectType.Ver));

        // X��
        mWeaponDataDict["Weapon_10"].TargetCoordiList.Add(new Vector3Int(2, 2, (int)ETargetSelectType.RightUp));
        mWeaponDataDict["Weapon_10"].TargetCoordiList.Add(new Vector3Int(2, 2, (int)ETargetSelectType.LeftUp));


    }

    public WeaponData GetWeaponDataByName(string name)
    {
        return mWeaponDataDict[name];
    }
}
