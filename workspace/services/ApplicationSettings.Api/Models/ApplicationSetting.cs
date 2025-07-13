using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApplicationSettings.Api.Models;

public class ApplicationSetting
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("key")]
    public required string Key { get; set; }

    [BsonElement("value")]
    public required string Value { get; set; }

    [BsonElement("description")]
    public string? Description { get; set; }

    [BsonElement("category")]
    public string? Category { get; set; }

    [BsonElement("isEncrypted")]
    public bool IsEncrypted { get; set; } = false;

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}