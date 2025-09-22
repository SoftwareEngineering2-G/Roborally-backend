using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Roborally.infrastructure.persistence.GameBoard;

public class GameBoardConfiguration : IEntityTypeConfiguration<core.domain.Game.GameBoard>
{
    public void Configure(EntityTypeBuilder<core.domain.Game.GameBoard> builder)
    {
        builder.ToTable("GameBoards");
        builder.HasKey(b => b.Id);

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        
        var comparer = new ValueComparer<string[][]>(
            (a, b) => JsonSerializer.Serialize(a, jsonOptions) == JsonSerializer.Serialize(b, jsonOptions),
            v => v == null ? 0 : JsonSerializer.Serialize(v, jsonOptions).GetHashCode(),
            v => JsonSerializer.Deserialize<string[][]>(JsonSerializer.Serialize(v, jsonOptions), jsonOptions)!
        );

        builder.Property(b => b.SpaceNames)
            .HasColumnType("jsonb") 
            .HasConversion(
                v => JsonSerializer.Serialize(v, jsonOptions),                          // C# -> DB
                v => JsonSerializer.Deserialize<string[][]>(v, jsonOptions)             // DB -> C#
                     ?? Array.Empty<string[]>()
            )
            .Metadata.SetValueComparer(comparer);
    }
}