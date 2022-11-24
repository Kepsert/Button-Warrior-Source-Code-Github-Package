using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FightShopController : MonoBehaviour
{
    public static FightShopController Instance;

    [SerializeField] int defaultCoinCost = 20;
    int currentCoinCost;
    [SerializeField] int coinCostRemoval = 1;
    int currentCoinCostRemoval;
    [SerializeField] int costIncrease = 1;

    [SerializeField] List<GameObject> ShopButtons;

    [SerializeField] GameObject AttackButtonPrefab;
    [SerializeField] GameObject ShieldButtonPrefab;
    [SerializeField] GameObject JumpButtonPrefab;
    [SerializeField] GameObject SlideButtonPrefab;

    [SerializeField] TextMeshProUGUI coinCostText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        Init();
        if (GameController.Instance.GetCurrentCoins() >= currentCoinCost)
        {
            foreach (GameObject shop in ShopButtons)
            {
                shop.gameObject.GetComponent<Button>().interactable = true;
            }
        }
        else
        {
            foreach (GameObject shop in ShopButtons)
            {
                shop.gameObject.GetComponent<Button>().interactable = false;
            }
        }
    }

    public void Init()
    {
        currentCoinCostRemoval = coinCostRemoval;
        currentCoinCost = defaultCoinCost - (UpgradeController.Instance.LowerCoinCost*2);
        if (currentCoinCost < 0)
        {
            currentCoinCost = 0;
        }
        coinCostText.text = currentCoinCost.ToString();
    }

    public void AddToCoinCost()
    {
        currentCoinCost++;
        coinCostText.text = currentCoinCost.ToString();
    }

    public void RemoveFromCoinCost()
    {
        currentCoinCost -= (currentCoinCostRemoval+UpgradeController.Instance.DoubleTap);
        if (currentCoinCost < 0)
        {
            currentCoinCost = 0;
        }
        coinCostText.text = currentCoinCost.ToString();
    }

    public void RefreshShop()
    {
        if (GameController.Instance.GetCurrentCoins() >= currentCoinCost)
        {
            foreach (GameObject shop in ShopButtons)
            {
                shop.gameObject.GetComponent<Button>().interactable = true;
            }
        }
        else
        {
            foreach (GameObject shop in ShopButtons)
            {
                shop.gameObject.GetComponent<Button>().interactable = false;
            }
        }
    }

    public void BuyAttackButton()
    {
        if (RhythmController.Instance.RhythmMode)
        {
            RhythmController.Instance.ChangeScore(Conductor.Instance.GiveScoreBasedOnTiming());
        }
        GameController.Instance.SpawnButton(AttackButtonPrefab, currentCoinCost);
        currentCoinCost += costIncrease;
        coinCostText.text = currentCoinCost.ToString();
        RefreshShop();
    }

    public void BuyShieldButton()
    {
        if (RhythmController.Instance.RhythmMode)
        {
            RhythmController.Instance.ChangeScore(Conductor.Instance.GiveScoreBasedOnTiming());
        }
        GameController.Instance.SpawnButton(ShieldButtonPrefab, currentCoinCost);
        currentCoinCost += costIncrease;
        coinCostText.text = currentCoinCost.ToString();
        RefreshShop();
    }

    public void BuyJumpButton()
    {
        if (RhythmController.Instance.RhythmMode)
        {
            RhythmController.Instance.ChangeScore(Conductor.Instance.GiveScoreBasedOnTiming());
        }
        GameController.Instance.SpawnButton(JumpButtonPrefab, currentCoinCost);
        currentCoinCost += costIncrease;
        coinCostText.text = currentCoinCost.ToString();
        RefreshShop();
    }

    public void BuySlideButton()
    {
        if (RhythmController.Instance.RhythmMode)
        {
            RhythmController.Instance.ChangeScore(Conductor.Instance.GiveScoreBasedOnTiming());
        }
        GameController.Instance.SpawnButton(SlideButtonPrefab, currentCoinCost);
        currentCoinCost += costIncrease;
        coinCostText.text = currentCoinCost.ToString();
        RefreshShop();
    }
}
