using System.Collections.Generic;

namespace ProfilesCalculator.Core.Models
{
    public class NewProfile
    {
        private List<Profile> _partsOfNewProfile = new List<Profile>();

        public List<Profile> PartsOfNewProfile => _partsOfNewProfile;

        public double NewLenght { get; set; }

        public double WasteLength { get; set; }

        public double Max { get; }

        public NewProfile(double max)
        {
            Max = max;
            WasteLength = max;
        }

        #region notused
        public bool ReachMax(Profile p) => CalculateLenghtSum() + p.Length >= Max;
        #endregion

        public double CalculateLenghtSum()
        {
            double sum = 0;
            foreach (Profile p in _partsOfNewProfile)
            {
                sum += p.Length;
            }

            return sum;
        }

        public void Add(Profile profile)
        {
            PartsOfNewProfile.Add(profile);
            NewLenght = CalculateLenghtSum();
        }

        #region notused
        public double CalculateWaste(Profile p1)
        {
            return (CalculateLenghtSum() + p1.Length) - Max;
        }
        #endregion
        public override string ToString()
        {
            return $"NewProfile: \n\tlenght: {NewLenght}, \n\twaste length: {WasteLength}, \n\tnumber of profiles: {PartsOfNewProfile.Count})";
        }
    }
}
