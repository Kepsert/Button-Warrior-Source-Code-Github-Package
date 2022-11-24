using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum UpgradeButtons { GoldTrickle, DoubleTap, ExtraDamage, DamageBlock, FirstHitFree, AdditionalXP, LowerCoinCost, ExtraGold, SpawnSpeed }
public class UpgradeButton : MonoBehaviour
{
    [SerializeField] UpgradeButtons upgrades = UpgradeButtons.GoldTrickle;

    [SerializeField] TextMeshProUGUI buttonText;

    [SerializeField] int amount;

    private void Awake()
    {
        if (buttonText == null)
        {
            buttonText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    private void Start()
    {
        switch (upgrades)
        {
            case UpgradeButtons.GoldTrickle:
                buttonText.text = "Gain 1 additional gold every 8 seconds.";
                break;
            case UpgradeButtons.DoubleTap:
                buttonText.text = "Double tap button reduces gold cost by 1 extra gold.";
                break;
            case UpgradeButtons.ExtraDamage:
                buttonText.text = "Your attacks deal 1 additional damage.";
                break;
            case UpgradeButtons.DamageBlock:
                buttonText.text = "You take 2 less damage from all sources.";
                break;
            case UpgradeButtons.FirstHitFree:
                buttonText.text = "You can take 1 additional hit without taking damage.";
                break;
            case UpgradeButtons.AdditionalXP:
                buttonText.text = "You will gain additional exp per defeated enemy.";
                break;
            case UpgradeButtons.LowerCoinCost:
                buttonText.text = "The starting cost of buttons will be reduced by 2.";
                break;
            case UpgradeButtons.ExtraGold:
                buttonText.text = "Start adventures with an additional 25 gold.";
                break;
            case UpgradeButtons.SpawnSpeed:
                buttonText.text = "Enemies take an additional second to spawn.";
                break;
        }
    }

    public void UpgradePressed(string name)
    {
        UpgradeController.Instance.UpgradeSelected(name);
        MusicPlayer.Instance.PlaySoundEffectByName("ButtonPressedMenu");

        //TO-DO Continue to map.
    }

    public int Amount
    {
        get { return amount; }
    }
}
