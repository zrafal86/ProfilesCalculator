namespace ProfilesCalculator.Core.Models
{
    public class Profile
    {

        public string Name { get; set; }

        public float Length { get; set; }

        public int Quantity { get; set; }

        public override string ToString()
        {
            return $"Profile( {Name} ) - Length - {Length})";
        }
    }
}
