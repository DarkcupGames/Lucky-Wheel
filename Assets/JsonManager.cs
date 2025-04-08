using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;

public class JsonManager : MonoBehaviour
{
    private static string filePath = "Assets/Resources/items.json";

    private void Start()
    {
        // Đảm bảo filePath được khởi tạo đúng cho thư mục Resources

        // Kiểm tra nếu filePath hợp lệ
        if (string.IsNullOrEmpty(filePath))
        {
            Debug.LogError("File path is invalid!");
        }
    }

    // Lưu Items vào file JSON trong Resources (chỉ hoạt động trong Editor)
    public static void SaveItems(Item[] items)
    {
        if (items == null)
        {
            Debug.LogError("Items array is null!");
            return;
        }

        if (string.IsNullOrEmpty(filePath))
        {
            Debug.LogError("File path is null or empty!");
            return;
        }

        try
        {
            string json = JsonConvert.SerializeObject(items, Formatting.Indented);
            // Kiểm tra nếu thư mục "Resources" đã tồn tại hay chưa
            string directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Lưu file vào thư mục Resources trong Editor
            File.WriteAllText(filePath, json);

            // Refresh lại AssetDatabase để Unity nhận diện file mới
            AssetDatabase.Refresh();
            Debug.Log("Saved to Resources: " + filePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error saving items: {e.Message}");
        }
    }

    // Đọc Items từ file JSON trong Resources (chỉ hoạt động trong Editor)
    public static Item[] LoadItems()
    {
        if (string.IsNullOrEmpty(filePath))
        {
            Debug.LogError("File path is null or empty!");
            return null;
        }

        if (!File.Exists(filePath))
        {
            Debug.LogWarning("File not found in Resources: " + filePath);
            return null;
        }

        try
        {
            string json = File.ReadAllText(filePath);
            Item[] items = JsonConvert.DeserializeObject<Item[]>(json);
            Debug.Log("Loaded from Resources: " + filePath);
            return items;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error loading items: {e.Message}");
            return null;
        }
    }
}
