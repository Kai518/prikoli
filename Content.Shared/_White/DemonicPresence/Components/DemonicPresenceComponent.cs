using System.Diagnostics;
using Robust.Shared.Prototypes;

namespace Content.Shared._White.DemonicPressence.Components;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class DemonicPresenceComponent : Component
{
    [DataField]
    public HashSet<EntProtoId> PathChooseActionIds = new ()
            {
        "ActionDemonicPresenceBellial",
        "ActionDemonicPresenceMammon",
        "ActionDemonicPresenceLilith"
    };

    [DataField]
    public EntProtoId ShopActionId = "ActionDemonicPresenceShop";

    [DataField]
    public EntityUid? ShopActionContainer;

    [DataField]
    public float EnergyCap = 50;

    [ViewVariables(VVAccess.ReadWrite)]
    public float EnergyToChange;

    [DataField]
    public string Currency = "UnholyEnergy";
}
