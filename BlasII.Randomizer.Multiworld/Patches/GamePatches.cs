using BlasII.ModdingAPI.Assets;
using HarmonyLib;
using Il2CppPlaymaker.UI;
using Il2CppTGK.Game;

namespace BlasII.Randomizer.Multiworld.Patches;

/// <summary>
/// Checks the goal and attempts to send it
/// </summary>
[HarmonyPatch(typeof(ShowFinalBossVictoryMessageAction), nameof(ShowFinalBossVictoryMessageAction.OnEnter))]
class ShowFinalBossVictoryMessageAction_OnEnter_Patch
{
    public static void Postfix()
    {
        Main.Multiworld.OnDefeatFinalBoss(CalculateEnding());
    }

    private static int CalculateEnding()
    {
        bool gaveIncense = Main.Randomizer.GetQuestBool("ST00", "ENDING_A");

        if (!gaveIncense)
            return 0;

        bool hasPrayer = AssetStorage.PlayerInventory.IsItemEquipped(AssetStorage.Prayers["PR101"]);
        bool hasSword = CoreCache.EquipmentManager.GetCurrentWeapon() == AssetStorage.Weapons[WEAPON_IDS.MeaCulpa];

        if (!hasPrayer || !hasSword)
            return 1;

        return 2;
    }
}
