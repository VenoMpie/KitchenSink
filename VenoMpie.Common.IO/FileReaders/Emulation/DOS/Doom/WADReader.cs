using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VenoMpie.Common.IO.FileReaders.Emulation.DOS.Doom.Lumps;
using VenoMpie.Common.IO.FileReaders.Emulation.DOS.Doom.Lumps.Map;
using VenoMpie.Common.IO.FileReaders.Emulation.DOS.Doom.Lumps.Sound;

/// <summary>
/// This is still HEAVILY WIP. Haven't gotten around to finish this (was thinking of building a WAD editor using this)
/// </summary>
namespace VenoMpie.Common.IO.FileReaders.Emulation.DOS.Doom
{
    #region WAD File
    public enum WadType : int
    {
        IWAD = 0x44415749,
        PWAD = 0x44415750
    }
    public class WadHeader
    {
        public WadType Identifier { get; set; }
        public uint NumberOfLumps { get; set; }
        public uint DirectoryPointer { get; set; }

        public WadHeader(BinaryReader reader)
        {
            Identifier = (WadType)reader.ReadUInt32();
            NumberOfLumps = reader.ReadUInt32();
            DirectoryPointer = reader.ReadUInt32();
        }
    }
    #endregion
    public class WADFile_Doom : WADFile<Things, LineDefs>
    {
        public Lump EnDoom { get; set; }
        public WADFile_Doom(Stream stream) : base(stream)
        {
            EnDoom = SplitLump(Lumps, "ENDOOM");
        }
    }
    public class WADFile_Heretic : WADFile<Things, LineDefs>
    {
        public Lump EndText { get; set; }
        public WADFile_Heretic(Stream stream) : base(stream)
        {
            EndText = SplitLump(Lumps, "ENDTEXT");
        }
    }
    public class WADFile_Strife : WADFile<Things, LineDefs>
    {
        public Lump EndStrife { get; set; }
        public WADFile_Strife(Stream stream) : base(stream)
        {
            EndStrife = SplitLump(Lumps, "ENDSTRF");
        }
    }
    public class WADFile_Hexen : WADFile<Things_Hexen, LineDefs_Hexen>
    {
        public WADFile_Hexen(Stream stream) : base(stream) { }
    }
    public abstract class WADFile<THINGSCLASS, LINEDEFSCLASS> where THINGSCLASS : IThings, new() where LINEDEFSCLASS : ILineDefs, new()
    {
        public WadHeader Header { get; private set; }
        public List<Lump> Lumps { get; private set; }
        public List<Map> Maps { get; private set; }

        public PlayPalette ColorPallet { get; set; }
        public Lump ColorMap { get; set; }
        public Lump Help1 { get; set; }
        public Lump Help2 { get; set; }
        public Lump TitleScreen { get; set; }
        public Lump Credits { get; set; }
        public Lump Victory { get; set; }
        public Lump Rabbit { get; set; }
        public Lump WhatsNextInD2 { get; set; }

        public List<Lump> Demos { get; set; }
        public List<Lump> Textures { get; set; }
        public List<Lump> Wallpatches { get; set; }

        #region Sound Lumps
        public List<Lump> MIDIInstrumentPatches { get; set; }
        public DMXGUS GUSInstrumentPatches { get; set; }
        public List<Lump> Music { get; set; }
        public List<PCSpeaker> PCSpeakerSounds { get; set; }
        public List<Lump> SoundcardSounds { get; set; }
        #endregion
        #region Graphics Lumps
        public List<Lump> StatusBarGraphics { get; set; }
        public List<Lump> SummaryScreenGraphics { get; set; }
        public List<Lump> MenuGraphics { get; set; }
        #endregion

        public WADFile(Stream stream)
        {
            //First we get the formalities out of the way
            if (stream == null) return;
            Lumps = new List<Lump>();
            Maps = new List<Map>();

            //We need to identify is this is a valid wad file, else throw exception
            //So, we create a new BinaryReader to read the file (this will be used for the header and for the Lumps)
            BinaryReader reader = new BinaryReader(stream);
            Header = new WadHeader(reader);
            if (!Enum.IsDefined(typeof(WadType), Header.Identifier)) throw new Exception("Not a valid WAD file");

            //Move the filestream to the pointer in the header and start reading each Lump (Header + Data, we'll do it to stream the file in later)
            stream.Position = Header.DirectoryPointer;
            for (int i = 0; i < Header.NumberOfLumps; i++)
            {
                Lumps.Add(new Lump(reader));
            }

            //So this portion will strip out all the maps and their related lumps
            Regex mapRegex = new Regex(@"^E{1}\d{0,2}M{1}\d{0,2}$|^MAP\d{0,2}$");
            Map addMap = null;
            foreach (var item in Lumps.ToList())
            {
                if (mapRegex.IsMatch(item.Name))
                {
                    if (addMap != null)
                    {
                        addMap.Reject.Populate(addMap.Sectors.ObjectList.Count);
                        Maps.Add(addMap);
                    }
                    addMap = new Map(item);
                    Lumps.Remove(item);
                }
                else
                {
                    switch (item.Name)
                    {
                        case "THINGS":
                            THINGSCLASS thingsClass = new THINGSCLASS();
                            thingsClass.SetLump(item);
                            thingsClass.Populate();
                            addMap.Things = thingsClass;
                            Lumps.Remove(item);
                            break;
                        case "LINEDEFS":
                            LINEDEFSCLASS linedefsClass = new LINEDEFSCLASS();
                            linedefsClass.SetLump(item);
                            linedefsClass.Populate();
                            addMap.LineDefs = linedefsClass;
                            Lumps.Remove(item);
                            break;
                        case "SIDEDEFS":
                            addMap.SideDefs = new SideDefs(item);
                            Lumps.Remove(item);
                            break;
                        case "VERTEXES":
                            addMap.Vertexes = new Vertices(item);
                            Lumps.Remove(item);
                            break;
                        case "SEGS":
                            addMap.Segs = new Segments(item);
                            Lumps.Remove(item);
                            break;
                        case "SSECTORS":
                            addMap.SSectors = new SubSectors(item);
                            Lumps.Remove(item);
                            break;
                        case "NODES":
                            addMap.Nodes = new Nodes(item);
                            Lumps.Remove(item);
                            break;
                        case "SECTORS":
                            addMap.Sectors = new Sectors(item);
                            Lumps.Remove(item);
                            break;
                        case "REJECT":
                            addMap.Reject = new Reject(item);
                            Lumps.Remove(item);
                            break;
                        case "BLOCKMAP":
                            addMap.BlockMap = item;
                            Lumps.Remove(item);
                            break;
                        default:
                            if (addMap != null)
                            {
                                addMap.Reject.Populate(addMap.Sectors.ObjectList.Count);
                                Maps.Add(addMap);
                                addMap = null;
                            }
                            break;
                    }
                }
            }
            //We are done with the maps and the reader can be closed. We already read all the lumps into memory so we can work it from there
            if (addMap != null) Maps.Add(addMap);
            reader.Close();

            ColorPallet = SplitLump(Lumps, "PLAYPAL", a => new PlayPalette(a));

            ColorMap = SplitLump(Lumps, "COLORMAP");
            Demos = SplitLumps(Lumps, "DEMO");
            Textures = SplitLumps(Lumps, "TEXTURE");
            Wallpatches = SplitLumps(Lumps, "PNAMES");

            PCSpeakerSounds = SplitLumps(Lumps, "DP", a => new PCSpeaker(a));
            SoundcardSounds = SplitLumps(Lumps, "DS");
            GUSInstrumentPatches = SplitLump(Lumps, "DMXGUS", a => new DMXGUS(a));
            MIDIInstrumentPatches = SplitLumps(Lumps, "GENMIDI");
            Music = SplitLumps(Lumps, "D_");

            Help1 = SplitLump(Lumps, "HELP1");
            Help2 = SplitLump(Lumps, "HELP2");
            TitleScreen = SplitLump(Lumps, "TITLEPIC");
            Credits = SplitLump(Lumps, "CREDIT");
            Victory = SplitLump(Lumps, "VICTORY2");
            Rabbit = SplitLump(Lumps, "PFUB1");
            WhatsNextInD2 = SplitLump(Lumps, "PFUB2");

            StatusBarGraphics = SplitLumps(Lumps, "ST");
            SummaryScreenGraphics = SplitLumps(Lumps, "WI");
            MenuGraphics = SplitLumps(Lumps, "M_");
        }

        protected List<Lump> SplitLumps(List<Lump> lumps, string startsWith)
        {
            var retList = lumps.Where(a => a.Name.StartsWith(startsWith)).ToList();
            lumps.RemoveAll(a => a.Name.StartsWith(startsWith));
            return retList;
        }
        protected Lump SplitLump(List<Lump> lumps, string startsWith)
        {
            var retLump = lumps.FirstOrDefault(a => a.Name.StartsWith(startsWith));
            if (retLump != null)
                lumps.Remove(retLump);

            return retLump;
        }
        protected List<T> SplitLumps<T>(List<Lump> lumps, string startsWith, Func<Lump, T> func)
        {
            List<T> retList = new List<T>();
            var lumpList = lumps.Where(a => a.Name.StartsWith(startsWith)).ToList();
            lumps.RemoveAll(a => a.Name.StartsWith(startsWith));
            foreach (var lump in lumpList)
            {
                retList.Add(func(lump));
            }
            return retList;
        }
        protected T SplitLump<T>(List<Lump> lumps, string startsWith, Func<Lump, T> func)
        {
            var retLump = lumps.FirstOrDefault(a => a.Name.StartsWith(startsWith));
            if (retLump == null)
                return default(T);

            lumps.Remove(retLump);

            T retVal = func(retLump);

            return retVal;
        }
    }
}
