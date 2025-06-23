using BlasII.Randomizer.Models;

namespace BlasII.Randomizer.Multiworld.Models;

public class MultiworldItem : Item
{
    public string Player { get; set; }

    public MultiworldItem(bool progression, string player)
    {
        Id = "internal location id";
        Name = "Item name";
        Type = ItemType.Invalid;
        Progression = progression; // Replaced with 'class'
        Count = 0;
        Player = player;
    }
}
