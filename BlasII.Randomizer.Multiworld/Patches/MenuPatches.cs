using BlasII.Framework.Menus;
using BlasII.ModdingAPI;
using BlasII.Randomizer.Services;
using HarmonyLib;

namespace BlasII.Randomizer.Multiworld.Patches;

/// <summary>
/// Prevents the Randomizer menu from being registered
/// </summary>
[HarmonyPatch(typeof(MenuRegister), nameof(MenuRegister.RegisterNewGameMenu))]
class MenuRegister_RegisterNewGameMenu_Patch
{
    public static bool Prefix(ModMenu menu)
    {
        if (menu is RandomizerMenu)
        {
            ModLog.Info("Preventing randomizer menu from being registered");
            return false;
        }

        return true;
    }
}
