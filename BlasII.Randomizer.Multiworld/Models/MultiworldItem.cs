using BlasII.Randomizer.Models;

namespace BlasII.Randomizer.Multiworld.Models;

public class MultiworldItem : Item
{
    public int Player { get; set; }

    public MultiworldItem(bool progression, int player)
    {
        Id = "internal location id";
        Name = "name of item";
        Type = ItemType.Invalid;
        Progression = progression; // Replaced with 'class'
        Count = 0;
        Player = player;
    }
}
