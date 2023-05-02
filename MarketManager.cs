using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketManager : MonoBehaviour
{
    [SerializeField] private Sprite[] characterSprites;
    [SerializeField] private Image middleCharacterImage;
    [SerializeField] private Text moneyText;
    [SerializeField] private GameObject insufficientBalancePopup;
    [SerializeField] private StoreItem[] storeItems;

    private int selectedCharacterIndex;
    private int money;

    private void Start()
    {
        selectedCharacterIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);
        money = PlayerPrefs.GetInt("Money", 0);
        UpdateUI();
    }

    private void UpdateUI()
    {
        middleCharacterImage.sprite = characterSprites[selectedCharacterIndex];
        moneyText.text = $"Money: {money}";

        foreach (var item in storeItems)
        {
            item.UpdateUI();
        }
    }

    public void SelectCharacter(int index)
    {
        selectedCharacterIndex = index;
        PlayerPrefs.SetInt("SelectedCharacterIndex", selectedCharacterIndex);
        UpdateUI();
    }

    public void BuyItem(int index)
    {
        var item = storeItems[index];
        if (money < item.Price)
        {
            insufficientBalancePopup.SetActive(true);
            return;
        }

        money -= item.Price;
        PlayerPrefs.SetInt("Money", money);
        item.IsPurchased = true;
        item.UpdateUI();
        UpdateUI();
    }
}

public interface IStoreItem
{
    void UpdateUI();
}

public class StoreItem : MonoBehaviour, IStoreItem
{
    [SerializeField] private int price;
    [SerializeField] private GameObject purchaseButton;
    [SerializeField] private GameObject purchasedButton;

    private Market market;
    private int isPurchased;

    public bool IsPurchased
    {
        get => isPurchased == 1;
        set
        {
            isPurchased = value ? 1 : 0;
            PlayerPrefs.SetInt(name, isPurchased);
        }
    }

    private void Start()
    {
        market = FindObjectOfType<Market>();
        isPurchased = PlayerPrefs.GetInt(name, 0);
        UpdateUI();
    }

    public void UpdateUI()
    {
        purchaseButton.SetActive(!IsPurchased);
        purchasedButton.SetActive(IsPurchased);
    }

    public void BuyItem()
    {
        market.BuyItem(transform.GetSiblingIndex());
    }
}