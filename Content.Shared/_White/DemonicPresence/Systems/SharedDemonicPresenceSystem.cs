using System.Linq;
using Content.Shared._White.DemonicPressence.Components;
using Content.Shared.Actions;

namespace Content.Shared._White.DemonicPressence.Systems;

/// <summary>
/// This handles...
/// </summary>
public sealed class SharedDemonicPresenceSystem : EntitySystem
{
    [Dependency] private readonly SharedActionsSystem _actions = default!;
    /// <inheritdoc/>
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<DemonicPresenceComponent, ComponentInit>(OnInit);
        SubscribeLocalEvent<DemonicPresenceComponent, ComponentShutdown>(OnShutdown);
    }

    public void OnInit(EntityUid uid, DemonicPresenceComponent component, ComponentInit args)
    {
        foreach (var action in component.PathChooseActionIds)
        {
            _actions.AddAction(uid, action);
        }

        _actions.AddAction(uid, ref component.ShopActionContainer, component.ShopActionId);
    }

    public void OnShutdown(EntityUid uid, DemonicPresenceComponent component, ComponentShutdown args)
    {
        foreach (var action in _actions.GetActions(uid))
        {
            if (component.PathChooseActionIds.Contains(Prototype(action.Id)!.ID))
            {
                _actions.RemoveAction(uid, action.Id);
            }
        }
    }
}

public sealed partial class DemonicPresenceShopActionEvent : InstantActionEvent
{
}

public sealed partial class DemonicPresenceChoosePathActionEvent : InstantActionEvent
{
    [DataField]
    public string CategoryId { get; set; }
}

public sealed partial class DemonicPresenceSpawnActionEvent : InstantActionEvent
{
    [DataField]
    public float EnergyCost;

    [DataField]
    public string Prototype;
}
