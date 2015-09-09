using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SurvivalismRedux.Scripting;

namespace SurvivalismRedux.Models.Interfaces {
    public interface IScenarioRequirement {
        RequirementTags Name { get; }

        bool DoesScenarioMeetRequirements(StorySubject story, Day day, Player player, Party party);
    }
}
