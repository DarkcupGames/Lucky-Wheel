using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveData : MonoBehaviour
{
    public LuckyWheel luckyWheel;
    public RowAddItem rowAddItem;
    public Transform content;

    public Button buttonAddRow;
    public Button buttonSave;
    public List<Item> items = new List<Item>();


    private void Start()
    {
        buttonAddRow.onClick.AddListener(() =>
        {
            RowAddItem newRow = Instantiate(rowAddItem, content);
            newRow.SetIndex(items.Count);
            ReloadWheel();
        });
        buttonSave.onClick.AddListener(() =>
        {
            SaveDataItem();
        });
    }

    public void ReloadWheel()
    {
        UpdateListItem();
        luckyWheel.ReloadWheel(items.ToArray());
    }

    public void UpdateListItem()
    {
        items.Clear();
        foreach(RowAddItem rowAddItem in GetComponentsInChildren<RowAddItem>())
        {
            items.Add(rowAddItem.itemData);
        }
    }

    public void SaveDataItem()
    {
        UpdateListItem();
        //JsonManager.SaveItems(items.ToArray());
        ItemData itemData = new ItemData();
        itemData.items = items.ToArray();
        ItemDataManager.Instance.SaveItemData(UserManager.Instance.currentUsername, itemData);
    }
}
