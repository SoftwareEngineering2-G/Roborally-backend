using System.Text.Json;
using Roborally.core.domain.Deck;

// Test to verify order preservation in JSON serialization/deserialization
var originalCards = new List<ProgrammingCard>
{
    ProgrammingCard.Move1,
    ProgrammingCard.RotateLeft,
    ProgrammingCard.Move2,
    ProgrammingCard.PowerUp,
    ProgrammingCard.UTurn
};

// Serialize to JSON
var json = JsonSerializer.Serialize(
    originalCards.Select(card => card.DisplayName).ToList(),
    new JsonSerializerOptions());

Console.WriteLine($"JSON: {json}");

// Deserialize back
var deserializedCards = JsonSerializer.Deserialize<List<string>>(json, new JsonSerializerOptions())!
    .Select(ProgrammingCard.FromString)
    .ToList();

// Check if order is preserved
bool orderPreserved = originalCards.SequenceEqual(deserializedCards);
Console.WriteLine($"Order preserved: {orderPreserved}");

for (int i = 0; i < originalCards.Count; i++)
{
    Console.WriteLine($"Original[{i}]: {originalCards[i].DisplayName}, Deserialized[{i}]: {deserializedCards[i].DisplayName}");
}
