using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemObject: MonoBehaviour
{
    private Image image;
    private TextMeshProUGUI nameItem;

    private void Awake()
    {
        nameItem = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetDataItem(Item item)
    {
        nameItem.text = item.name;
    }
}