using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;

public class AddressableAutoNamer : MonoBehaviour
{
    [MenuItem("Tools/Addressables/Auto Set Address by Name")]
    public static void AutoSetAddressByName()
    {
        var settings = AddressableAssetSettingsDefaultObject.Settings;

        foreach (var group in settings.groups)
        {
            foreach (var entry in group.entries)
            {
                if (entry != null && entry.TargetAsset != null)
                {
                    entry.SetAddress(entry.TargetAsset.name);
                }
            }
        }
        
        Debug.Log("모든 Addressable 주소 이름으로 자동 변경 완료");
    }
}
