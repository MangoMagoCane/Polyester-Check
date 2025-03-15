namespace Polyester;

public sealed class DatabaseAppSettings
{
    public required string ConnectionString { get; set; } = "";
}

public enum ClothingItem
{
    TShirt, LongSleeve, Pants, Socks, Hat, Underwear, Watch, Gloves, Sweater, Jacket, Shorts,
}

public enum ClothingType
{
    Cotton, Wool, Leather, Denim, Silk, Bamboo, Polyester, Nylon, Spandex, Rayon, Acrylic,
}

// public readonly struct ClothingPercentages
// {
//     public double Weight { get; init; }
//     public double DefaultPolyester { get; init; }
//
//     public ClothingPercentages(double Weight, double DefaultPolyester)
//     {
//         this.Weight = Weight;
//         this.DefaultPolyester = DefaultPolyester;
//     }
//
//     public override string ToString() => $"({Weight}, {DefaultPolyester})";
// }
