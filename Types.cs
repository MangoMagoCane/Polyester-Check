namespace Polyester;

public enum ClothingItems
{
    shirt, pants, socks, hat, underwear, watch, gloves, sweater, jacket, shorts,
}

public enum ClothingTypes
{
    cotton, wool, leather, denim, silk, bamboo, polyester, nylon, spandex, rayon, acrylic,
}

public sealed class DatabaseAppSettings
{
    public required string ConnectionString { get; set; } = "";
}
