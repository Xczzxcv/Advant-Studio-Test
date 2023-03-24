using System.Collections.Generic;
using Core.Ecs;
using Core.Ecs.Components;
using Core.GameConfigs;
using Infrastructure;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ui
{
internal class AllBusinessesUiController : UIBehaviour, IUpdatable
{
    [SerializeField] private Transform businessesRootTransform;
    [SerializeField] private BusinessUiController businessControllerPrefab;

    private readonly Dictionary<string, BusinessUiController> _businessControllers = new();
    
    private EcsWorld _world;
    private BusinessEcsMediator _businessEcsMediator;
    private IBusinessEntitiesProvider _businessEntitiesProvider;
    private IGameConfigCollectionsProvider _gameConfigCollectionsProvider;
    private EcsFilter _businessEntitiesFilter;

    public void Init(EcsWorld world,
        BusinessEcsMediator businessEcsMediator,
        IBusinessEntitiesProvider businessEntitiesProvider,
        IGameConfigCollectionsProvider gameConfigCollectionsProvider)
    {
        _world = world;
        _businessEcsMediator = businessEcsMediator;
        _businessEntitiesProvider = businessEntitiesProvider;
        _gameConfigCollectionsProvider = gameConfigCollectionsProvider;
        _businessEntitiesFilter = _world.Filter<BusinessComponent>().End();

        foreach (var businessEntity in _businessEntitiesFilter)
        {
            SpawnBusinessController(businessEntity);
        }
    }

    private void SpawnBusinessController(int businessEntity)
    {
        var businessController = Instantiate(businessControllerPrefab, businessesRootTransform);
        businessController.Init(_world,
            _businessEcsMediator,
            _businessEntitiesProvider,
            _gameConfigCollectionsProvider);
        var businessId = GetBusinessId(businessEntity);
        businessController.Setup(businessId);
        _businessControllers.Add(businessId, businessController);
    }

    private string GetBusinessId(int businessEntity)
    {
        var businessComp = _world.GetComponent<BusinessComponent>(businessEntity);
        var businessId = businessComp.Id;
        return businessId;
    }

    public void OnUpdate()
    {
        foreach (var businessEntity in _businessEntitiesFilter)
        {
            UpdateBusiness(businessEntity);
        }
    }

    private void UpdateBusiness(int businessEntity)
    {
        var businessId = GetBusinessId(businessEntity);
        if (!_businessControllers.TryGetValue(businessId, out var businessController))
        {
            Debug.LogError($"Can't get business by entity {businessEntity}");
            return;
        }

        businessController.UpdateBusiness();
    }
}
}
