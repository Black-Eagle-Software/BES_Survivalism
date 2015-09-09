using SurvivalismRedux.Models;
using SurvivalismRedux.Scripting.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using SurvivalismRedux.Factory;
using SurvivalismRedux.Managers;
using SurvivalismRedux.MessageTypes;
using SurvivalismRedux.Models.Interfaces;

namespace SurvivalismRedux.Scripting.Lua {
    public class LuaTocReader : IScriptTocReader {
        #region Constructors

        public LuaTocReader() {
            this._tags = Enum.GetNames(typeof(TocTags));
        }

        #endregion


        #region Properties



        #endregion


        #region Methods

        public Scenario ReadTocFileFromStream(Stream file, string tocFilename, string expectedPath) {
            var lines = new List<string>();
            using (var sr = new StreamReader(file)) {
                while (!sr.EndOfStream) {
                    lines.Add(sr.ReadLine());
                }
            }
            var result = new Scenario { TocFilename = tocFilename };
            //this will get messy
            var files = new List<string>();
            foreach (var line in lines) {
                if (line.Length == 0) continue;
                if (line.StartsWith("##")) {
                    //found a header line
                    this.CheckHeaderTag(line, result);
                } else if (line.StartsWith("#")) {
                    //should have found a comment line, ignore it
                    break;
                } else {
                    //found a filePath line
                    files.Add($"{expectedPath}{Path.DirectorySeparatorChar}{line}");
                }
            }
            result.FilePaths = files.ToArray();
            return result;
        }

        public Scenario ReadTocFileFromFilePath(string filePath) {
            if (string.IsNullOrEmpty(filePath)) return null;
            var lines = File.ReadAllLines(filePath);
            var result = new Scenario { TocFilename = Path.GetFileName(filePath) };
            //this will get messy
            var files = new List<string>();
            foreach (var line in lines) {
                if (line.Length == 0) continue;
                if (line.StartsWith("##")) {
                    //found a header line
                    this.CheckHeaderTag(line, result);
                } else if (line.StartsWith("#")) {
                    //should have found a comment line, ignore it
                    break;
                } else {
                    //found a filePath line
                    var lPath = Directory.GetParent(filePath);
                    files.Add($"{lPath}{Path.DirectorySeparatorChar}{line}");
                }
            }
            result.FilePaths = files.ToArray();
            return result;
        }

        private void CheckHeaderTag(string line, Scenario scenario) {
            //first, clean away the cruft
            var ss = line.TrimStart('#');
            ss = ss.Trim();
            //now, find out what the tag is
            var colonIndex = ss.IndexOf(':');
            var tag = ss.Substring(0, colonIndex);
            var value = ss.Substring(colonIndex + 1);
            if (!this._tags.Any(t => t.Equals(tag, StringComparison.OrdinalIgnoreCase))) {
                return;
            }
            //found a tag, so pull out the value
            var tagTag = (TocTags)Enum.Parse(typeof(TocTags), tag);
            switch (tagTag) {
                case TocTags.API_Version:
                    scenario.ApiVersion = int.Parse(value);
                    break;
                case TocTags.Author:
                    scenario.Author = value;
                    break;
                case TocTags.Description:
                    scenario.Description = value;
                    break;
                case TocTags.Requirements:
                    this.ReadRequirementsTags(value, scenario);
                    break;
                case TocTags.Title:
                    scenario.Title = value;
                    break;
                case TocTags.Version:
                    scenario.Version = value;
                    break;
            }
        }

        private void ReadRequirementsTags(string value, Scenario scenario) {
            //found a requirement, so read the tag(s) and values and add to scenario
            var tags = value.Split(';');
            foreach (var s in tags) {
                if (string.IsNullOrEmpty(s)) continue;
                //Console.WriteLine( $"Found tag:{s.Trim()}" );
                //Messenger.Default.Send( new PrintMessage( $"Found tag:{s.Trim()}", PrintMessage.MessageType.DEBUG ) );
                //try and break up tags into header and values outside and inside of () respectively
                var para1 = s.IndexOf('(');
                var header = s.Substring(0, para1);
                var vLength = s.Length - 1 - (para1 + 1);
                var reqValues = s.Substring(para1 + 1, vLength);
                Console.WriteLine(reqValues);
                var hTag = (RequirementTags)Enum.Parse(typeof(RequirementTags), header);
                IScenarioRequirement req;
                switch (hTag) {
                    case RequirementTags.FirstDay:
                        req = new FirstDayRequirement(hTag, int.Parse(reqValues));
                        break;
                    case RequirementTags.LastDay:
                        req = new LastDayRequirement(hTag, int.Parse(reqValues));
                        break;
                    case RequirementTags.PartyArchetype:
                        req = new PartyArchetypeRequirement(hTag, ArchetypeFactory.Instance.GetArchetypeFromString(reqValues));
                        break;
                    case RequirementTags.PartySize:
                        req = new PartySizeRequirement(hTag, int.Parse(reqValues));
                        break;
                    case RequirementTags.PlayerArchetype:
                        req = new PlayerArchetypeRequirement(hTag, ArchetypeFactory.Instance.GetArchetypeFromString(reqValues));
                        break;
                    case RequirementTags.PlayerStatMinimum:
                        var sV1 = reqValues.Split(',');
                        sV1[0] = sV1[0].Trim('"');
                        req = new PlayerStatMinimumRequirement(hTag, (Stats)Enum.Parse(typeof(Stats), sV1[0].ToUpper()), int.Parse(sV1[1]));
                        break;
                    case RequirementTags.PlayerStatMaximum:
                        var sV2 = reqValues.Split(',');
                        sV2[0] = sV2[0].Trim('"');
                        req = new PlayerStatMaximumRequirement(hTag, (Stats)Enum.Parse(typeof(Stats), sV2[0].ToUpper()), int.Parse(sV2[1]));
                        break;
                    case RequirementTags.Storyline:
                        req = new StorylineRequirement(hTag, StorySubjectManager.Instance.GetStorySubject(reqValues));
                        break;
                    case RequirementTags.TimeOfDay:
                        var tc = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(reqValues.Trim('"'));
                        req = new TimeOfDayRequirement(hTag, (Day.TimeOfDay)Enum.Parse(typeof(Day.TimeOfDay), tc));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                scenario.Requirements.Add(req);
            }
        }

        #endregion


        #region Fields

        private readonly string[] _tags;

        #endregion
    }
}
