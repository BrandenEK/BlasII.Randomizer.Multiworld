using BlasII.ModdingAPI.Helpers;
using MelonLoader;

namespace BlasII.Randomizer.Multiworld;

internal class Main : MelonMod
{
    public static Multiworld Multiworld { get; private set; }
    public static Randomizer Randomizer { get; private set; }

    public override void OnLateInitializeMelon()
    {
        Multiworld = new Multiworld();
        Randomizer = ModHelper.GetModByName("Randomizer") as Randomizer;
    }
}