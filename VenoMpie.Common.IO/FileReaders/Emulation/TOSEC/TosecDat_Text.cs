using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VenoMpie.Common.IO.FileReaders.Emulation.DataFiles;
using VenoMpie.Common.IO.FileReaders.Emulation.DataFiles.Resources;

namespace VenoMpie.Common.IO.FileReaders.Emulation.TOSEC
{
    /// <summary>
    /// This class is for the old school TOSEC/TDC DAT files
    /// </summary>
    /// <remarks>
    /// I tried to make this as Generic as I can to maintain compatibility with new and old, it could probably be done alot better using RegEx but meh
    /// In fact I had this doing RegEx at some point but it was very inconsistent
    /// </remarks>
    public class TosecDat_Text : IAmDataFile
    {
        public datafile Contents { get; set; }
        private Dictionary<string, PropertyInfo> headerProperties = new Dictionary<string, PropertyInfo>();

        private Dictionary<string, PropertyInfo> gameProperties = new Dictionary<string, PropertyInfo>();
        private Dictionary<string, Tuple<Type, List<PropertyInfo>>> gameArrayProperties = new Dictionary<string, Tuple<Type, List<PropertyInfo>>>();
        private Dictionary<string, IList<object>> gameArraySubProperties = new Dictionary<string, IList<object>>();

        public TosecDat_Text()
        {
            Contents = new datafile();
            PopulateProperties();
        }
        /// <summary>
        /// Read the Properties of the XSD class into variables
        /// </summary>
        /// <remarks>
        /// Reflection is expensive, so we do it once and get it over with
        /// </remarks>
        private void PopulateProperties()
        {
            foreach (var pi in typeof(header).GetProperties().Where(a => !a.PropertyType.IsArray))
            {
                headerProperties.Add(pi.Name, pi);
            }
            foreach (var pi in typeof(game).GetProperties())
            {
                gameProperties.Add(pi.Name, pi);
                if (pi.PropertyType.IsArray)
                {
                    List<PropertyInfo> lpi = new List<PropertyInfo>();
                    foreach (var ipi in pi.PropertyType.GetElementType().GetProperties()) lpi.Add(ipi);
                    gameArrayProperties.Add(pi.Name, new Tuple<Type, List<PropertyInfo>>(pi.PropertyType.GetElementType(), lpi));
                }
            }
        }
        /// <summary>
        /// Read a TOSEC Dat File
        /// </summary>
        /// <param name="path">Full Path of the File</param>
        public void ReadFile(string path)
        {
            ReadFile(new FileStream(path, FileMode.Open));
        }
        /// <summary>
        /// Read a TOSEC Dat File
        /// </summary>
        /// <param name="stream">Steam with File Contents</param>
        public void ReadFile(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                Contents = new datafile();
                ReadHeader(reader);
                ReadGames(reader);
            }
        }
        /// <summary>
        /// Reads the Header
        /// </summary>
        /// <param name="reader"></param>
        private void ReadHeader(StreamReader reader)
        {
            Contents.header = new header();
            int currentLine = 0;
            string line;
            while (!reader.EndOfStream)
            {
                currentLine++;
                if (currentLine > headerProperties.Count) throw new Exception("Invalid Dat File");
                line = reader.ReadLine();
                if (line.Trim() == ")") return;
                Dictionary<string, string> parsedValues = ParseLine(line);
                KeyValuePair<string, string> singleValue = parsedValues.First();
                if (parsedValues.Count > 1 || singleValue.Value == "" || singleValue.Value == "(") continue;
                if (headerProperties.ContainsKey(singleValue.Key))
                {
                    headerProperties[singleValue.Key].SetValue(Contents.header, singleValue.Value);
                }
            }
        }
        /// <summary>
        /// Reads all the Games in the File into Contents
        /// </summary>
        /// <param name="reader"></param>
        private void ReadGames(StreamReader reader)
        {
            List<game> games = new List<game>();
            string line;
            game retGame = new game();
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                if (!string.IsNullOrWhiteSpace(line))
                {
                    if (line.Trim() == ")")
                    {
                        foreach (var item in gameArraySubProperties.Where(a => a.Value.Count > 0))
                        {
                            var castArray = Array.CreateInstance(gameArrayProperties[item.Key].Item1, item.Value.Count);
                            Array.Copy(item.Value.ToArray(), castArray, item.Value.Count);
                            gameProperties[item.Key].SetValue(retGame, castArray);
                        }
                        games.Add(retGame);
                        retGame = new game();
                        foreach (var key in gameArraySubProperties.Keys.ToList())
                            gameArraySubProperties[key] = new List<object>();

                        continue;
                    }
                    Dictionary <string, string> parsedValues = ParseLine(line);
                    KeyValuePair<string, string> singleValue = parsedValues.First();
                    if (parsedValues.Count > 1 && gameArrayProperties.ContainsKey(singleValue.Key))
                    {
                        if (!gameArraySubProperties.ContainsKey(singleValue.Key)) gameArraySubProperties.Add(singleValue.Key, new List<object>());
                        Tuple<Type, List<PropertyInfo>> singleArrayProp = gameArrayProperties[singleValue.Key];
                        object o = Activator.CreateInstance(singleArrayProp.Item1);
                        parsedValues.Remove(singleValue.Key);
                        foreach (var item in parsedValues)
                        {
                            var setPropValueItem = singleArrayProp.Item2.First(a => a.Name == item.Key);
                            if (setPropValueItem.PropertyType.IsEnum)
                                setPropValueItem.SetValue(o, Enum.Parse(setPropValueItem.PropertyType, item.Value));
                            else
                                setPropValueItem.SetValue(o, item.Value);
                        }
                        gameArraySubProperties[singleValue.Key].Add(o);
                    }
                    else
                    {
                        if (gameProperties.ContainsKey(singleValue.Key))
                        {
                            var gamePropSetItem = gameProperties[singleValue.Key];
                            if (gamePropSetItem.PropertyType.IsEnum)
                                gamePropSetItem.SetValue(retGame, Enum.Parse(gamePropSetItem.PropertyType, singleValue.Value));
                            else
                                gamePropSetItem.SetValue(retGame, singleValue.Value);
                        }
                    }
                }
            }
            Contents.game = games.ToArray();
        }
        /// <summary>
        /// Parse each line
        /// </summary>
        /// <param name="line"></param>
        /// <returns>Dictionary of AttributeName,Value</returns>
        private Dictionary<string, string> ParseLine(string line)
        {
            //DateTime outDate;
            char[] lineArray = line.ToCharArray();
            Dictionary<string, string> retValue = new Dictionary<string, string>();
            StringBuilder key = new StringBuilder(200);
            StringBuilder value = new StringBuilder(200);
            bool hasKey = false;
            bool hasApostrophy = false;
            for (int i = 0; i < lineArray.Length; i++)
            {
                if (lineArray[i] == '\t') continue;
                if (!hasKey)
                {
                    if (lineArray[i] != ' ')
                        key.Append(lineArray[i]);
                    else if (lineArray[i] == ' ' && key.Length > 0)
                        hasKey = true;
                }
                else
                {
                    if (!hasApostrophy && lineArray[i] == '"')
                        hasApostrophy = true;
                    else if (hasApostrophy && lineArray[i] == '"')
                    {
                        hasApostrophy = false;
                        retValue.Add(key.ToString(), value.ToString());
                        key.Length = 0;
                        value.Length = 0;
                        hasKey = false;
                    }
                    else if (hasApostrophy && lineArray[i] != '"')
                        value.Append(lineArray[i]);
                    else if (lineArray[i] == ' ')
                    {
                        //This is a hack for TDC as there are no quotes around the file date time
                        //if (!DateTime.TryParse(key.ToString(), out outDate))
                        //{
                            retValue.Add(key.ToString(), value.ToString());
                            key.Length = 0;
                            value.Length = 0;
                            hasKey = false;
                        //}
                        //else
                        //    value.Append(lineArray[i]);
                    }
                    else
                        value.Append(lineArray[i]);
                }
            }
            if (retValue.Count == 0) retValue.Add(key.ToString(), value.ToString());
            return retValue;
        }
        
        /// <summary>
        /// Write the Contents into an old school format file
        /// </summary>
        /// <param name="path">Full Path</param>
        public void WriteFile(string path)
        {
            WriteFile(path, FileMode.OpenOrCreate);
        }
        /// <summary>
        /// Write the Contents into an old school format file
        /// </summary>
        /// <param name="path">Full Path</param>
        /// <param name="fileMode">Mode to be used when opening the file</param>
        public void WriteFile(string path, FileMode fileMode)
        {
            WriteFile(new FileStream(path, fileMode));
        }
        /// <summary>
        /// Write the Contents into an old school format file
        /// </summary>
        /// <param name="stream">Stream to be written to</param>
        public void WriteFile(Stream stream)
        {
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.WriteLine("clrmamepro (");
                foreach (var headerProperty in headerProperties)
                {
                    object obj = headerProperty.Value.GetValue(Contents.header, null);
                    if(obj != null) writer.WriteLine("\t{0} \"{1}\"", headerProperty.Key, obj);
                }
                writer.WriteLine(")");
                writer.WriteLine("");
                foreach (var game in Contents.game)
                {
                    writer.WriteLine("game (");
                    foreach (var gameProperty in gameProperties)
                    {
                        object obj = gameProperty.Value.GetValue(game, null);
                        if (obj != null)
                        {
                            if (!gameArrayProperties.ContainsKey(gameProperty.Key))
                                writer.WriteLine("\t{0} \"{1}\"", gameProperty.Key, obj);
                            else
                            {
                                writer.Write("\t{0} ( ", gameProperty.Key);

                                Array reflectedArray = (Array)gameProperty.Value.GetValue(game, null);
                                var castArray = Array.CreateInstance(gameArrayProperties[gameProperty.Key].Item1, reflectedArray.Length);
                                Array.Copy(reflectedArray, castArray, reflectedArray.Length);

                                foreach (var arrItem in reflectedArray)
                                {
                                    foreach (var gameSubProperty in gameArrayProperties[gameProperty.Key].Item2)
                                    {
                                        object subobj = gameSubProperty.GetValue(arrItem, null);
                                        if (subobj != null)
                                        {
                                            writer.Write("{0} \"{1}\" ", gameSubProperty.Name, subobj);
                                        }
                                    }
                                }
                                writer.WriteLine(")");
                            }
                        }
                    }
                    writer.WriteLine(")");
                    writer.WriteLine("");
                }
            }
        }
    }
}
