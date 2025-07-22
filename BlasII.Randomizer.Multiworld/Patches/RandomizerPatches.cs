using BlasII.ModdingAPI;
using BlasII.Randomizer.Handlers;
using BlasII.Randomizer.Models;
using BlasII.Randomizer.Multiworld.Models;
using BlasII.Randomizer.Shops;
using HarmonyLib;
using System.Linq;
using UnityEngine;

namespace BlasII.Randomizer.Multiworld.Patches;

/// <summary>
/// Finds items from an internal storage instead of the Randomizer's
/// </summary>
[HarmonyPatch(typeof(ItemHandler), nameof(ItemHandler.GetItemAtLocation))]
class ItemHandler_GetItemAtLocation_Patch
{
    public static bool Prefix(string locationId, ref Item __result)
    {
        __result = Main.Multiworld.Scouter.GetItemAtLocation(locationId);
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

        string itemId = "MW";
        __instance.MappedItems = Main.Randomizer.ItemLocationStorage.AsSequence.ToDictionary(x => x.Id, x => itemId);
        return false;
    }
}

/// <summary>
/// Get validity for mw items
/// </summary>
[HarmonyPatch(typeof(ItemExtensions), nameof(ItemExtensions.IsValid))]
class ItemExtensions_IsValid_Patch
{
    public static bool Prefix(Item item, ref bool __result)
    {
        if (item is not MultiworldItem)
            return true;

        __result = true;
        return false;
    }
}

/// <summary>
/// Get sprite for mw items
/// </summary>
[HarmonyPatch(typeof(ItemExtensions), nameof(ItemExtensions.GetSprite))]
class ItemExtensions_GetSprite_Patch
{
    public static bool Prefix(Item item, ref Sprite __result)
    {
        if (item is not MultiworldItem mwitem)
            return true;

        __result = mwitem.GetSprite();
        return false;
    }
}

/// <summary>
/// Get name for mw items
/// </summary>
[HarmonyPatch(typeof(ItemExtensions), nameof(ItemExtensions.GetName))]
class ItemExtensions_GetName_Patch
{
    public static bool Prefix(Item item, ref string __result)
    {
        if (item is not MultiworldItem mwitem)
            return true;

        __result = mwitem.GetName();
        return false;
    }
}

/// <summary>
/// Get description for mw items
/// </summary>
[HarmonyPatch(typeof(ItemExtensions), nameof(ItemExtensions.GetDescription))]
class ItemExtensions_GetDescription_Patch
{
    public static bool Prefix(Item item, ref string __result)
    {
        if (item is not MultiworldItem mwitem)
            return true;

        __result = mwitem.GetDescription();
        return false;
    }
}

/// <summary>
/// Get value for mw items
/// </summary>
[HarmonyPatch(typeof(ItemExtensions), nameof(ItemExtensions.GetValue))]
class ItemExtensions_GetValue_Patch
{
    public static bool Prefix(Item item, ref ShopValue __result)
    {
        if (item is not MultiworldItem mwitem)
            return true;

        __result = mwitem.GetValue();
        return false;
    }
}

/// <summary>
/// Skip reward for mw items
/// </summary>
[HarmonyPatch(typeof(ItemExtensions), nameof(ItemExtensions.GiveReward))]
class ItemExtensions_GiveReward_Patch
{
    public static bool Prefix(Item item)
    {
        return item is not MultiworldItem;
    }
}
