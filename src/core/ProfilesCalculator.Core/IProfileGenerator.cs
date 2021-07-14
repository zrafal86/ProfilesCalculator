using ProfilesCalculator.Core.Models;
using System.Collections.Generic;

namespace ProfilesCalculator.Core
{
    public interface IProfileGenerator
    {
        double OrginalProfileLenght { get; set; }
        double OrginalProfileCutWaste { get; set; }
        IEnumerable<Profile> ProfileList { get; set; }
        IEnumerable<NewProfile> GenerateNewProfiles();
        NewProfile GenereteProfile();
    }
}
