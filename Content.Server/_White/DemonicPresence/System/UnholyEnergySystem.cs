using Content.Server.Store.Components;
using Content.Server.Store.Systems;
using Content.Shared._White.DemonicPressence.Components;
using Content.Shared.FixedPoint;
using Content.Shared.Storage;

namespace Content.Server._White.DemonicPressence.System;

/// <summary>
/// This handles...
/// </summary>
public sealed class UnholyEnergySystem : EntitySystem
{
    [Dependency] private readonly StoreSystem _store = default!;
    /// <inheritdoc/>
    public override void Initialize()
    {

    }

    public bool ChangeEnergy(EntityUid uid, float energyToChange, string currency, float? cap = null)
    {
        if (!TryComp(uid, out StoreComponent? store)
            || !store.Balance.TryGetValue(currency, out var value))
        {
            return false;
        }

        if (value.Value + energyToChange < 0)
        {
            return false;
        }

        var currencyDict = new Dictionary<string, FixedPoint2>();
        if (cap != null && value.Value + energyToChange > cap)
        {
            currencyDict.Add(currency, (FixedPoint2)cap - value.Value);
            _store.TryAddCurrency(currencyDict, uid, store);
            return true;
        }

        currencyDict.Add(currency, energyToChange);
        _store.TryAddCurrency(currencyDict, uid, store);
        return true;
    }

    public bool ChangeEnergy(EntityUid uid, DemonicPresenceComponent component, float energyToChange)
    {
        return ChangeEnergy(uid, energyToChange, component.Currency, component.EnergyCap);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<DemonicPresenceComponent>();

        while (query.MoveNext(out var uid, out var component))
        {
            if (component.EnergyToChange == 0)
                continue;
            ChangeEnergy(uid, component, component.EnergyToChange);
            component.EnergyToChange = 0;
        }
    }
}
