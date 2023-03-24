using Core.Ecs;
using Infrastructure;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ui
{
internal class PlayerDataUiController : UIBehaviour, IUpdatable
{
    [SerializeField] private TextMeshProUGUI currentBalanceText;
   
    private PlayerEcsMediator _playerEcsMediator;

    public void Init(PlayerEcsMediator playerEcsMediator)
    {
        _playerEcsMediator = playerEcsMediator;
    }

    public void OnUpdate()
    {
        UpdatePlayerBalance();
    }

    private void UpdatePlayerBalance()
    {
        var playerBalance = _playerEcsMediator.GetPlayerBalance();
        currentBalanceText.text = $"Balance: {playerBalance}$";
    }
}
}

