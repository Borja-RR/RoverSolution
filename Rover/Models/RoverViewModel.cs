namespace Rover.Models
{
    public class RoverViewModel
    {
        public int PlateauSizeLongitude { get; set; }
        public int PlateauSizeLatitude { get; set; }
        public Rover Rover1 { get; set; }
        public Rover Rover2 { get; set; }
        public string Error { get; set; }
    }

    public class Rover
    {
        public Position Position { get; set; }
        public string Instructions { get; set; }
    }

    public class Position
    {
        public int Longitude { get; set; }
        public int Latitude { get; set; }
        public Heading Heading { get; set; }
    }

    public enum Heading
    {
        N = 360,
        E = 90,
        S = 180,
        W = 270,
        M = 0
    }
}
