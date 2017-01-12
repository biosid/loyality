namespace RapidSoft.Etl.Logging.Dumps
{
    public struct EtlDumpSettings
    {
        public EtlDumpSettings(int? takeMessageCount, int? skipMessageCount)
            : this()
        {
            TakeMessageCount = takeMessageCount ?? int.MaxValue;
            SkipMessageCount = skipMessageCount ?? 0;
        }

        public int TakeMessageCount { get; set; }

        public int SkipMessageCount { get; set; }
    }
}