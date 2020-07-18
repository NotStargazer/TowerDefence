/// <summary>
/// Stores data on how many small and large units exist in the wave.
/// </summary>
public struct Wave
{
    public int SmallUnits;
    public int LargeUnits;
    public int TotalEnemies { get { return SmallUnits + LargeUnits; } }
}
