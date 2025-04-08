using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class RowAddItem : MonoBehaviour
{
    public TMP_InputField nameField;
    public TMP_InputField rateField;
    public Button buttonRemove;
    public TextMeshProUGUI stt;
    public int index;
    private SaveData saveData;
    public Item itemData = new Item();

    private void Awake()
    {
        saveData = GetComponentInParent<SaveData>();
    }
    private void Start()
    {
        rateField.contentType = TMP_InputField.ContentType.DecimalNumber;
        nameField.onValueChanged.AddListener(UpdateNameInWheel);
        rateField.onValueChanged.AddListener(UpdateRateInWheel);
        buttonRemove.onClick.AddListener(OnclickBtnRemove);
    }

    private void UpdateNameInWheel(string value)
    {
        itemData.name = value;
        saveData.ReloadWheel();
    }

    private void UpdateRateInWheel(string input)
    {
        if (float.TryParse(input, out float rate))
        {
            Debug.Log(rate);
            itemData.rate = rate; // Gán giá trị đã chuyển đổi
        }
        else
        {
            Debug.LogWarning($"Invalid rate value: {input}");
            itemData.rate = 0f; // Gán mặc định nếu nhập sai
        }
    }

    public void OnclickBtnRemove()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
        saveData.ReloadWheel();
    }

    public void SetIndex(int index)
    {
        this.index = index;
        stt.text = index.ToString();
    }
}
