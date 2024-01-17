namespace SplinterForgeAutomation.Models
{
    public class Player
    {
        public string Username { get; set; } = ""; // Initialized to an empty string
        public string Token { get; set; } = ""; // Initialized to an empty string
        public string Type { get; set; } = ""; // Initialized to an empty string
        public string[] Team { get; set; } = Array.Empty<string>(); // Initialized to an empty array
        public string Hero { get; set; } = ""; // Initialized to an empty string
        public int Gear { get; set; } // No change since int can't be null, defaults to 0
        public string Source { get; set; } = ""; // Initialized to an empty string

        public string Key { get; set; } = ""; // Initialized to an empty string
        public string[] FightTeam { get; set; } = Array.Empty<string>(); // Initialized to an empty array
    }
}