using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace OrderTest
{
    // Minimal test classes to simulate the behavior
    public abstract class Enumeration
    {
        public string DisplayName { get; }
        
        protected Enumeration(string displayName)
        {
            DisplayName = displayName;
        }
        
        public override bool Equals(object obj)
        {
            if (obj is not Enumeration other) return false;
            return GetType() == obj.GetType() && DisplayName == other.DisplayName;
        }
        
        public override int GetHashCode() => DisplayName.GetHashCode();
    }
    
    public class ProgrammingCard : Enumeration
    {
        public static readonly ProgrammingCard Move1 = new("Move 1");
        public static readonly ProgrammingCard RotateLeft = new("Rotate Left");
        public static readonly ProgrammingCard Move2 = new("Move 2");
        public static readonly ProgrammingCard PowerUp = new("Power Up");
        public static readonly ProgrammingCard UTurn = new("U-Turn");
        
        private ProgrammingCard(string displayName) : base(displayName) { }
        
        public static ProgrammingCard FromString(string cardName)
        {
            var fields = typeof(ProgrammingCard)
                .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                .Where(f => f.FieldType == typeof(ProgrammingCard))
                .Select(f => f.GetValue(null) as ProgrammingCard)
                .Where(card => card != null);

            return fields.FirstOrDefault(card => card!.DisplayName == cardName) 
                   ?? throw new ArgumentException($"Unknown card: {cardName}");
        }
    }
    
    class Program
    {
        static void Main()
        {
            var originalCards = new List<ProgrammingCard>
            {
                ProgrammingCard.Move1,
                ProgrammingCard.RotateLeft,
                ProgrammingCard.Move2,
                ProgrammingCard.PowerUp,
                ProgrammingCard.UTurn
            };

            // Test the exact conversion logic from your configuration
            var json = JsonSerializer.Serialize(
                originalCards.Select(card => card.DisplayName).ToList(),
                new JsonSerializerOptions());

            Console.WriteLine($"JSON: {json}");

            var deserializedCards = JsonSerializer.Deserialize<List<string>>(json, new JsonSerializerOptions())!
                .Select(ProgrammingCard.FromString)
                .ToList();

            bool orderPreserved = originalCards.SequenceEqual(deserializedCards);
            Console.WriteLine($"Order preserved: {orderPreserved}");

            for (int i = 0; i < originalCards.Count; i++)
            {
                Console.WriteLine($"Index {i}: Original='{originalCards[i].DisplayName}', Deserialized='{deserializedCards[i].DisplayName}'");
            }
        }
    }
}
