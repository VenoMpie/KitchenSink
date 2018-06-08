namespace VenoMpie.Common.IO.FileReaders.Emulation.General
{
    public interface IGameEntry
    {
        string Additional { get; set; }
        string Date { get; set; }
        string DumpFlags { get; set; }
        string FullName { get; set; }
        string Name { get; set; }
        string Publisher { get; set; }

        string BuildRegex();
        void Parse(string line);
    }
}