using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ui
{
internal class BusinessView : UIBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI lvlText;
    [SerializeField] private TextMeshProUGUI incomeText;
    [SerializeField] private Button lvlUpBtn;
    [SerializeField] private TextMeshProUGUI lvlUpBtnText;
    [SerializeField] private Transform businessUpgradesRoot;
    [SerializeField] private BusinessUpgradeView businessUpgradeViewPrefab;
    [Header("Progress bar")]
    [SerializeField] private Image progressImg;
    [SerializeField] private Image progressBackgroundImg;
    [SerializeField] private Color activeProgressBarColor;
    [SerializeField] private Color inactiveProgressBarColor;
    [FormerlySerializedAs("buyBusinessStr")]
    [Header("LvlUp Btn text")]
    [SerializeField] private string buyBusinessOperationStr;
    [FormerlySerializedAs("lvlUpBusinessStr")]
    [SerializeField] private string lvlUpBusinessOperationStr;
    [SerializeField] private string costStr;

    internal struct Config
    {
        public string Id;
        public string Name;
        public float Progress;
        public int Level;
        public bool CanLvlUp;
        public double NextLevelCost;
        public double Income;
        public BusinessUpgradeView.Config[] Upgrades;
    }

    public event Action<string> BusinessUpgradeBtnClick;
    public event Action<int> BusinessLvlUpBtnClick;

    private Config _config;
    private readonly List<BusinessUpgradeView> _businessUpgradeViews = new();

    protected override void Awake()
    {
        lvlUpBtn.onClick.AddListener(OnLvlUpBtnClick);
    }

    public void Setup(Config config)
    {
        _config = config;

        UpdateView();
    }

    private void UpdateView()
    {
        nameText.text = _config.Name;

        progressImg.fillAmount = _config.Progress;
        progressBackgroundImg.color = _config.Level > 0
            ? activeProgressBarColor
            : inactiveProgressBarColor;

        lvlText.text = _config.Level.ToString(CultureInfo.InvariantCulture);
        incomeText.text = $"{_config.Income.ToString(CultureInfo.InvariantCulture)}$";
        lvlUpBtn.interactable = _config.CanLvlUp;
        var operationNameStr = _config.Level > 0
            ? lvlUpBusinessOperationStr
            : buyBusinessOperationStr;
        lvlUpBtnText.text = $"{operationNameStr}\n{costStr}: {_config.NextLevelCost}";

        SetupUpgradeViews();
    }

    private void SetupUpgradeViews()
    {
        int i;
        for (i = 0; i < _config.Upgrades.Length; i++)
        {
            SetupUpgradeView(i);
        }

        for (var j =_businessUpgradeViews.Count - 1; j >= i; j++)
        {
            DeleteUpgradeView(j);
        }
    }

    private void SetupUpgradeView(int index)
    {
        var upgradeConfig = _config.Upgrades[index];
        var businessUpgradeView = index < _businessUpgradeViews.Count
            ? _businessUpgradeViews[index]
            : GetNewBusinessUpgradeView();

        businessUpgradeView.Setup(upgradeConfig);
    }

    private BusinessUpgradeView GetNewBusinessUpgradeView()
    {
        var businessUpgradeView = Instantiate(businessUpgradeViewPrefab, businessUpgradesRoot);
        businessUpgradeView.BusinessUpgradeBtnClick += OnBusinessUpgradeBtnClick;
        _businessUpgradeViews.Add(businessUpgradeView);

        return businessUpgradeView;
    }

    private void DeleteUpgradeView(int index)
    {
        var businessUpgradeView = _businessUpgradeViews[index];
        Destroy(businessUpgradeView.gameObject);
        _businessUpgradeViews.RemoveAt(index);
    }

    private void OnLvlUpBtnClick()
    {
        BusinessLvlUpBtnClick?.Invoke(_config.Level);
    }

    private void OnBusinessUpgradeBtnClick(string businessUpgradeId)
    {
        BusinessUpgradeBtnClick?.Invoke(businessUpgradeId);
    }

    protected override void OnDestroy()
    {
        lvlUpBtn.onClick.RemoveAllListeners();
    }
}
}