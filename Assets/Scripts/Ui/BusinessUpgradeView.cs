using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ui
{
internal class BusinessUpgradeView : UIBehaviour
{
    [SerializeField] private Button upgradeBtn;
    [SerializeField] private TextMeshProUGUI upgradeBtnText;
    [Header("Background")]
    [SerializeField] private Image backgroundImg;
    [SerializeField] private Color activeBackgroundColor;
    [SerializeField] private Color inactiveBackgroundColor;
    [Header("Text")]
    [SerializeField] private string alreadyBoughtStr;
    [SerializeField] private string costFormatStringStr;


    internal struct Config
    {
        public string Id;
        public string Name;
        public string EffectDescription;
        public double Price;
        public bool IsApplied;
        public bool CanBeApplied;
    }

    public event Action<string> BusinessUpgradeBtnClick;

    private Config _config;

    protected override void Awake()
    {
        upgradeBtn.onClick.AddListener(OnUpgradeBtnClick);
    }

    public void Setup(Config config)
    {
        _config = config;
        UpdateView();
    }

    private void UpdateView()
    {
        UpdateUpgradeBtn();
        UpdateBackground();
    }

    private void UpdateUpgradeBtn()
    {
        upgradeBtn.interactable = !_config.IsApplied && _config.CanBeApplied;

        var priceText = _config.IsApplied
            ? alreadyBoughtStr
            : string.Format(costFormatStringStr, _config.Price);
        upgradeBtnText.text = $"{_config.Name}\n{_config.EffectDescription}\n{priceText}";
    }

    private void UpdateBackground()
    {
        backgroundImg.color = _config.IsApplied
            ? activeBackgroundColor
            : inactiveBackgroundColor;
    }

    private void OnUpgradeBtnClick()
    {
        BusinessUpgradeBtnClick?.Invoke(_config.Id);
    }

    protected override void OnDestroy()
    {
        upgradeBtn.onClick.RemoveListener(OnUpgradeBtnClick);
    }
}
}