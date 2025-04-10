using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserManager : MonoBehaviour
{
    public static UserManager Instance;
    [Header("UI References")]
    public TMP_InputField usernameInputField;
    public Button okButton;
    public GameObject popup;

    public string currentUsername;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Gắn sự kiện click cho nút OK
        okButton.onClick.AddListener(OnOkButtonClicked);
    }

    void OnOkButtonClicked()
    {
        string inputUsername = usernameInputField.text.Trim();

        if (string.IsNullOrEmpty(inputUsername))
        {
            Debug.LogWarning("❗ Vui lòng nhập tên người dùng.");
            return;
        }

        currentUsername = inputUsername;
        popup.SetActive(false);
        Debug.Log("✅ Username đã nhập: " + currentUsername);
    }

    // Ví dụ callback nếu bạn dùng cùng với ItemDataManager
    void OnDataLoaded(ItemData data)
    {
        Debug.Log("📦 Dữ liệu đã tải: " + JsonUtility.ToJson(data));
    }

    void OnError(string error)
    {
        Debug.LogError(error);
    }
}
