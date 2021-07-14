using ProfilesCalculator.Core.Models;
using System.Collections.Generic;

namespace ProfilesCalculator.Core
{
    public class CutProfiles : IProfileGenerator
    {
        public double OrginalProfileLenght { get; set; }
        public double OrginalProfileCutWaste { get; set; }
        public IEnumerable<Profile> ProfileList { get; set; }

        public CutProfiles(ref IEnumerable<Profile> profileList, int orginalProfileLenght, int orginalProfileCutWaste)
        {
            ProfileList = profileList;
            OrginalProfileLenght = orginalProfileLenght;
            OrginalProfileCutWaste = orginalProfileCutWaste;
        }

        public IEnumerable<NewProfile> GenerateNewProfiles()
        {
            List<NewProfile> listOfNewProfiles = new();

            double sum = OrginalProfileLenght;
            do
            {
                var newProfile = GenereteProfile();
                sum = newProfile.CalculateLenghtSum();
                if (sum > 0)
                {
                    listOfNewProfiles.Add(newProfile);
                }
            } while (sum > 0);

            return listOfNewProfiles.ToArray();
        }

        public NewProfile GenereteProfile()
        {
            var newProfile = new NewProfile(OrginalProfileLenght);
            foreach (Profile profile in ProfileList)
            {
                if (profile.Quantity > 0)
                {
                    if (profile.Length <= newProfile.WasteLength)
                    {
                        AddProfile(ref newProfile, profile);
                        while (profile.Quantity > 0)
                        {
                            if (profile.Length <= newProfile.WasteLength)
                            {
                                AddProfile(ref newProfile, profile);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
            return newProfile;
        }

        private void AddProfile(ref NewProfile newProfile, Profile profile)
        {
            newProfile.WasteLength -= profile.Length + OrginalProfileCutWaste;
            newProfile.Add(profile);
            profile.Quantity -= 1;
        }
    }
}
