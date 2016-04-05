namespace CommonInterfaces
{
    public interface IWaterLevelData
    {
        bool GrowPoolEmpty { get; set; }
        bool GrowPoolFull { get; set; }
        bool ReservoirEmpty { get; set; }
    }
}