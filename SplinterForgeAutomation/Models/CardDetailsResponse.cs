using System.Collections.Generic;

namespace SplinterForgeAutomation.Models
{
    public class CardDetailsResponse
    {
        public string? Player { get; set; } // Nullable string

        // Initialize Cards collection to an empty list to avoid null reference
        public List<Card> Cards { get; set; } = new List<Card>();
    }

    public class Card
    {
        public string? Player { get; set; } = ""; // Initialized to an empty string
        public string? Uid { get; set; } = ""; // Initialized to an empty string
        public int CardDetailId { get; set; }
        public int Xp { get; set; }
        public bool Gold { get; set; }
        public int Edition { get; set; }
        public string? Delegated_to { get; set; } = ""; // Initialized to an empty string
        // ...other properties from the response
    }
}
