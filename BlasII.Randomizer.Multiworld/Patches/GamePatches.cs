using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Assets;
using HarmonyLib;
using Il2CppPlaymaker.UI;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.UI;
using Il2CppTGK.Game.Cutscenes;

namespace BlasII.Randomizer.Multiworld.Patches;

/// <summary>
/// Attempts to send goal when watching the final cutscene
/// </summary>
//[HarmonyPatch(typeof(PlayCutscene), nameof(PlayCutscene.PlayAndWait))]
//class PlayCutscene_PlayAndWait_Patch
//{
//    public static void Prefix(PlayCutscene __instance)
//    {
//        ModLog.Error("cutscene : " + __instance.cutsceneId?.name);
//        // Ending C: CTS18_id
//        // worst CTS19_id
//        //[03:17:52.383] [Multiworld] A: CTS20_id
//        //[03:17:52.383][Multiworld] C: CTS104_id
//    }
//}


//[HarmonyPatch(typeof(CreditsWindowLogic), nameof(CreditsWindowLogic.CheckIfEndingC))]
//class CreditsWindowLogic_CheckIfEndingC_Patch
//{
//    public static void Postfix(CreditsWindowLogic __instance)
//    {
//        ModLog.Warn("Checking for goal");
//        ModLog.Info("A: " + __instance.endingACutsceneId?.name);
//        ModLog.Info("C: " + __instance.endingCCutsceneId?.name);
//    }
//}

[HarmonyPatch(typeof(ShowFinalBossVictoryMessageAction), nameof(ShowFinalBossVictoryMessageAction.OnEnter))]
class ShowFinalBossVictoryMessageAction_OnEnter_Patch
{
    public static void Postfix()
    {
        ModLog.Info("Checking for goal completion");
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
