using System;

namespace SplinterForgeAutomation.Models
{
    public class HeroDetailsResponse
    {
        public Equipment Equipment { get; set; } = new Equipment();
        public DateTime CreatedDate { get; set; }
        public Skill[] Skills { get; set; } = Array.Empty<Skill>();
        public LoadOut[] LoadOuts { get; set; } = Array.Empty<LoadOut>();
        public Upgrade[] Upgrades { get; set; } = Array.Empty<Upgrade>();
        public string Player { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int Xp { get; set; }
        public int Rank { get; set; }
        public int SkillPts { get; set; }
        public int Rarity { get; set; }
        public int Edition { get; set; }
        public string Id { get; set; } = string.Empty;
    }

    public class Equipment
    {
        public string Weapon { get; set; } = string.Empty;
        public string Offhand { get; set; } = string.Empty;
        public string Head { get; set; } = string.Empty;
        public string Necklace { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string Hands { get; set; } = string.Empty;
        public string Ring { get; set; } = string.Empty;
        public string Legs { get; set; } = string.Empty;
        public string Feet { get; set; } = string.Empty;
        public string Back { get; set; } = string.Empty;
        public string Relic { get; set; } = string.Empty;
    }

    public class Skill
    {
        // Define properties for Skill (if needed)
    }

    public class LoadOut
    {
        // Define properties for LoadOut (if needed)
    }

    public class Upgrade
    {
        // Define properties for Upgrade (if needed)
    }
}
