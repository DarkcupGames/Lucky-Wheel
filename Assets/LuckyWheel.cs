using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LuckyWheel : MonoBehaviour
{

    [SerializeField] private GameObject segmentPrefab;
    [SerializeField] private Color[] colors;

    public Item[] items;
    public ItemObject rewardPrefab; // Prefab phần thưởng
    public float radius = 3f; // Bán kính vòng quay
    public float spinDuration = 3f; // Thời gian quay
    public GameObject completeFx;
    private int numberOfRewards; // Số phần thưởng
    private bool isSpinning = false;
    public List<ItemObject> rewards;
    void Start()
    {
        numberOfRewards = items.Length;
        //CreateWheelBackground();
        //CreateWheel();
    }

    void CreateWheel()
    {
        for (int i = 0; i < numberOfRewards; i++)
        {
            float angle = i * (360f / numberOfRewards); // Góc giữa các phần thưởng
            Vector3 position = transform.position + Quaternion.Euler(0, 0, angle) * Vector3.up * radius;
            ItemObject reward = Instantiate(rewardPrefab, position, Quaternion.Euler(0, 0, angle), transform);
            reward.SetDataItem(items[i]);
            rewards.Add(reward);
        }
    }

    public void OnClickButtonSpin()
    {
        SpinWheel();
    }
    private void SpinWheel()
    {
        int selectedRewardIndex = GetRandomRewardIndexByRate();
        StartCoroutine(Spin(selectedRewardIndex));
    }

    private IEnumerator Spin(int selectedRewardIndex)
    {
        isSpinning = true;

        float anglePerSegment = 360f / numberOfRewards;

        float startAngle = transform.rotation.eulerAngles.z;

        float targetSegmentAngle = 360f - (selectedRewardIndex * anglePerSegment + anglePerSegment / 2f) + anglePerSegment/2; 

        float totalRotation = 360f * Random.Range(5, 7) + targetSegmentAngle;

        float elapsedTime = 0f;

        while (elapsedTime < spinDuration)
        {
            float t = elapsedTime / spinDuration;
            float smoothT = Mathf.SmoothStep(0f, 1f, t);
            float currentAngle = Mathf.Lerp(startAngle, totalRotation, smoothT);

            transform.rotation = Quaternion.Euler(0, 0, currentAngle);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0, 0, totalRotation % 360f);

        isSpinning = false;

        OnRewardReceived(selectedRewardIndex);
    }


    private void OnRewardReceived(int index)
    {
        //completeFx.GetComponent<ParticleSystem>().Play();

        Debug.Log($"You won: {items[index].name}");

    }

    void CreateWheelBackground()
    {
        float fillAmount = 1f / numberOfRewards;
        float angleStep = 360f / numberOfRewards;

        for (int i = 0; i < numberOfRewards; i++)
        {
            GameObject segment = Instantiate(segmentPrefab, transform);
            segment.name = "Segment_" + (i + 1);

            // Set Fill Amount
            Image image = segment.GetComponent<Image>();
            image.fillAmount = fillAmount;

            // Xen kẽ màu
            if(numberOfRewards%2 == 0)
            {
                image.color = colors[i % 2];
            }
            else
            {
                image.color = colors[i % 3];
                if(i == numberOfRewards-1 && i >=6)
                {
                    image.color = colors[colors.Length-1];
                }
            }

            // Xoay segment đúng vị trí
            segment.transform.localRotation = Quaternion.Euler(0, 0, -angleStep * i + angleStep / 2);
        }
    }

    private int GetRandomRewardIndexByRate()
    {
        float totalRate = 0f;
        foreach (var item in items)
        {
            totalRate += item.rate;
        }

        float randomPoint = Random.value * totalRate;

        float accumulatedRate = 0f;
        for (int i = 0; i < items.Length; i++)
        {
            accumulatedRate += items[i].rate;
            if (randomPoint <= accumulatedRate)
            {
                return i;
            }
        }

        return items.Length - 1; // fallback
    }

    public void ReloadWheel(Item[] items)
    {
        ClearContent();
        this.items = items;
        numberOfRewards = items.Length;
        CreateWheelBackground();
        CreateWheel();
    }

    public void ClearContent()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject); // Xóa toàn bộ obj con
        }
    }
}
