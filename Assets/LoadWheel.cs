using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadWheel : MonoBehaviour
{
    public LuckyWheel luckyWheel;
    private void Start()
    {
        luckyWheel.ReloadWheel(JsonManager.LoadItems());
    }
}
