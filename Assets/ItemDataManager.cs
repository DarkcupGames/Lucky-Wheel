using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ItemDataManager : MonoBehaviour
{
    public static ItemDataManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    // ⚠️ Đổi IP nếu chạy trên điện thoại thật hoặc Docker server
    private string apiUrl = "http://localhost:3000/data";

    // GỌI HÀM NÀY ĐỂ LẤY DỮ LIỆU ITEMDATA
    public void LoadItemData(string username, Action<ItemData> onSuccess, Action<string> onError)
    {
        StartCoroutine(LoadItemDataCoroutine(username, onSuccess, onError));
    }

    // GỌI HÀM NÀY ĐỂ GỬI ITEMDATA LÊN SERVER
    public void SaveItemData(string username, ItemData itemData)
    {
        StartCoroutine(SaveItemDataCoroutine(username, itemData));
    }

    // ---------------- PRIVATE ---------------- //

    private IEnumerator LoadItemDataCoroutine(string username, Action<ItemData> onSuccess, Action<string> onError)
    {
        string url = $"{apiUrl}?username={UnityWebRequest.EscapeURL(username)}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                onError?.Invoke("❌ Lỗi khi lấy dữ liệu: " + request.error);
            }
            else
            {
                try
                {
                    ItemData data = JsonUtility.FromJson<ItemData>(request.downloadHandler.text);
                    onSuccess?.Invoke(data);
                }
                catch (Exception e)
                {
                    onError?.Invoke("❌ Lỗi parse JSON: " + e.Message);
                }
            }
        }
    }

    IEnumerator SaveItemDataCoroutine(string username, ItemData itemData)
    {
        SaveRequest requestData = new SaveRequest
        {
            username = username,
            data = itemData // Không stringify nữa
        };

        string jsonBody = JsonUtility.ToJson(requestData);

        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                //onError?.Invoke("❌ Lỗi khi lưu dữ liệu: " + request.error);
            }
            else
            {
                //onSuccess?.Invoke(request.downloadHandler.text);
            }
        }
    }

    [Serializable]
    private class SaveRequest
    {
        public string username;
        public ItemData data; // 👈 Truyền object thật thay vì string JSON
    }
}
