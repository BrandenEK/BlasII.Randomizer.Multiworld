using BlasII.Framework.UI;
using BlasII.ModdingAPI;
using HarmonyLib;
using Il2Cpp;
using UnityEngine;
using UnityEngine.UI;

namespace BlasII.Randomizer.Multiworld.Patches;

[HarmonyPatch(typeof(UITearsControl), nameof(UITearsControl.Update))]
class UITearsControl_Update_Patch
{
    public static void Postfix(UITearsControl __instance)
    {
        Transform t = __instance.transform;
        Transform connect = t.Find("Connection");

        if (connect == null)
            connect = CreateConnection(t);

        Image image = connect.GetComponent<Image>();
        image.sprite = Main.Multiworld.IconStorage.GetConnectionIcon(false);
    }

    private static Transform CreateConnection(Transform parent)
    {
        RectTransform connect = UIModder.Create(new RectCreationOptions()
        {
            Name = "Connection",
            Parent = parent,
            Pivot = Vector2.one,
            XRange = Vector2.one,
            YRange = Vector2.zero,
            Position = new Vector2(0, 0),
            Size = new Vector2(64, 64),
        });

        return connect.AddImage().rectTransform;
    }
}
