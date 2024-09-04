using Content.Server._White.DemonicPressence.System;
using Content.Server.Actions;
using Content.Server.Store.Components;
using Content.Server.Store.Systems;
using Content.Shared._White.DemonicPressence.Components;
using Content.Shared._White.DemonicPressence.Systems;
using Content.Shared.Actions;

namespace Content.Server._White.DemonicPresence.System;

/// <summary>
/// This handles...
/// </summary>
public sealed class DemonicPresenceSystem : EntitySystem
{
    [Dependency] private readonly StoreSystem _store = default!;
    [Dependency] private readonly ActionsSystem _actions = default!;
    [Dependency] private readonly UnholyEnergySystem _unholyEnergy = default!;
    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<DemonicPresenceComponent, DemonicPresenceShopActionEvent>(OnShop);
        SubscribeLocalEvent<DemonicPresenceComponent, DemonicPresenceChoosePathActionEvent>(OnChoosePath);

        SubscribeLocalEvent<DemonicPresenceComponent, DemonicPresenceSpawnActionEvent>(OnSpawn);
    }

    private void OnShop(EntityUid uid, DemonicPresenceComponent component, DemonicPresenceShopActionEvent args)
    {
        if (!TryComp<StoreComponent>(uid, out var store))
            return;
        _store.ToggleUi(uid, uid, store);
    }

    public void OnChoosePath(EntityUid uid, DemonicPresenceComponent component,
        DemonicPresenceChoosePathActionEvent args)
    {
        foreach (var action in _actions.GetActions(uid))
        {
            if (component.PathChooseActionIds.Contains(Prototype(action.Id)!.ID))
            {
                _actions.RemoveAction(uid, action.Id);
            }
        }

        if (!TryComp<StoreComponent>(uid, out var store))
            return;

        store.Categories.Add(args.CategoryId);
    }

    public void OnSpawn(EntityUid uid, DemonicPresenceComponent component, DemonicPresenceSpawnActionEvent args)
    {
        if (args.Handled)
        {
            return;
        }

        args.Handled = true;
        if (!_unholyEnergy.ChangeEnergy(uid, component, -args.EnergyCost))
        {
            return;
        }
        SpawnAtPosition(args.Prototype, Transform(uid).Coordinates);
    }
}
