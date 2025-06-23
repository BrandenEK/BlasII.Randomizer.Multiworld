using BlasII.ModdingAPI;
using BlasII.Randomizer.Handlers;
using BlasII.Randomizer.Models;
using BlasII.Randomizer.Multiworld.Models;
using HarmonyLib;
using System.Collections.Generic;

namespace BlasII.Randomizer.Multiworld.Patches;

/// <summary>
/// Finds items from an internal storage instead of the Randomizer's
/// </summary>
[HarmonyPatch(typeof(ItemHandler), nameof(ItemHandler.GetItemAtLocation))]
class ItemHandler_GetItemAtLocation_Patch
{
    public static bool Prefix(string locationId, ref Item __result)
    {
        __result = new MultiworldItem(false, 1);
        return false;
    }
}

/// <summary>
/// Overrides the Randomizer's shuffle with a fake one
/// </summary>
[HarmonyPatch(typeof(ItemHandler), nameof(ItemHandler.ShuffleItems))]
class ItemHandler_ShuffleItems_Pstch
{
    public static bool Prefix(ItemHandler __instance)
    {
        ModLog.Info("Overriding ItemHandler shuffle");

        var mapping = new Dictionary<string, string>();
        foreach (var location in Main.Randomizer.ItemLocationStorage.AsSequence)
        {
            mapping.Add(location.Id, location.Id);
        }

        __instance.MappedItems = mapping;
        return false;
    }
}
