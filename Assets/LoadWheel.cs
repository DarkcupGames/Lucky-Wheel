using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadWheel : MonoBehaviour
{
    public LuckyWheel luckyWheel;
    public string username;
    private void Start()
    {
        
    ItemDataManager.Instance.LoadItemData(
    username,
    onSuccess: (data) =>
    {
        Debug.Log("✅ Dữ liệu đã load: " + JsonUtility.ToJson(data));
        luckyWheel.ReloadWheel(data.items);
    },
    onError: (err) =>
    {
        Debug.LogError(err);
    }
    );

    }
}
