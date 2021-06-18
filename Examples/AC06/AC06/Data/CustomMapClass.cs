namespace AC06.Data
{
    public class CustomMapClass
    {
        public string MyEnglishName { get; set; }
        public bool IsNETDeveloper { get; set; }
        public int NETDeveloperYear { get; set; }
        public Nest Nest { get; set; }
    }

    public class Nest
    {
        public bool IsNETDeveloper { get; set; }
        public int NETDeveloperYear { get; set; }
    }
}
