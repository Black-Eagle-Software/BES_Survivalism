using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SurvivalismRedux.Models.Interfaces;
using SurvivalismRedux.Scripting;

namespace SurvivalismRedux.Models {
    public abstract class RequirementBase : IScenarioRequirement {
        protected RequirementBase(RequirementTags name) {
            this.Name = name;
        }

        public RequirementTags Name { get; }
        public abstract bool DoesScenarioMeetRequirements(StorySubject story, Day day, Player player, Party party);
    }

    public class FirstDayRequirement : RequirementBase {
        public FirstDayRequirement(RequirementTags name, int day) : base(name) {
            this._day = day;    //needs to check if greater than 0
        }

        public override bool DoesScenarioMeetRequirements(StorySubject story, Day day, Player player, Party party) {
            return day.Index >= this._day;
        }

        private readonly int _day;
    }

    public class LastDayRequirement : RequirementBase {
        public LastDayRequirement(RequirementTags name, int day) : base(name) {
            this._day = day;    //needs to check if greater than 0
        }

        public override bool DoesScenarioMeetRequirements(StorySubject story, Day day, Player player, Party party) {
            return day.Index <= this._day;
        }

        private readonly int _day;
    }

    public class PartyArchetypeRequirement : RequirementBase {
        public PartyArchetypeRequirement(RequirementTags name, Archetype type) : base(name) {
            this._archetype = type;
        }

        public override bool DoesScenarioMeetRequirements(StorySubject story, Day day, Player player, Party party) {
            return party.Any(p => p.PlayerArchetype == this._archetype);
        }

        private readonly Archetype _archetype;
    }

    public class PartySizeRequirement : RequirementBase {
        public PartySizeRequirement(RequirementTags name, int size) : base(name) {
            this._size = size;  //needs to check if greater than 0
        }

        public override bool DoesScenarioMeetRequirements(StorySubject story, Day day, Player player, Party party) {
            return party.Count >= this._size;
        }

        private readonly int _size;
    }

    public class PlayerArchetypeRequirement : RequirementBase {
        public PlayerArchetypeRequirement(RequirementTags name, Archetype type) : base(name) {
            this._archetype = type;
        }

        public override bool DoesScenarioMeetRequirements(StorySubject story, Day day, Player player, Party party) {
            return player.PlayerArchetype == this._archetype;
        }

        private readonly Archetype _archetype;
    }

    public class PlayerStatMinimumRequirement : RequirementBase {
        public PlayerStatMinimumRequirement(RequirementTags name, Stats stat, int value) : base(name) {
            this._stat = stat;
            this._statValue = value;    //needs to check if greater than 0
        }

        public override bool DoesScenarioMeetRequirements(StorySubject story, Day day, Player player, Party party) {
            return player.CheckStatValue(this._stat.ToString()) >= this._statValue;
        }

        private readonly Stats _stat;
        private readonly int _statValue;
    }

    public class PlayerStatMaximumRequirement : RequirementBase {
        public PlayerStatMaximumRequirement(RequirementTags name, Stats stat, int value) : base(name) {
            this._stat = stat;
            this._statValue = value;    //needs to check if greater than 0
        }

        public override bool DoesScenarioMeetRequirements(StorySubject story, Day day, Player player, Party party) {
            return player.CheckStatValue(this._stat.ToString()) <= this._statValue;
        }

        private readonly Stats _stat;
        private readonly int _statValue;
    }

    public class StorylineRequirement : RequirementBase {
        public StorylineRequirement(RequirementTags name, StorySubject story) : base(name) {
            this._story = story;
        }

        public override bool DoesScenarioMeetRequirements(StorySubject story, Day day, Player player, Party party) {
            return story == this._story;
        }

        private readonly StorySubject _story;
    }

    public class TimeOfDayRequirement : RequirementBase {
        public TimeOfDayRequirement(RequirementTags name, Day.TimeOfDay tod) : base(name) {
            this._tod = tod;
        }

        public override bool DoesScenarioMeetRequirements(StorySubject story, Day day, Player player, Party party) {
            return day.Time == this._tod;
        }

        private readonly Day.TimeOfDay _tod;
    }
}
