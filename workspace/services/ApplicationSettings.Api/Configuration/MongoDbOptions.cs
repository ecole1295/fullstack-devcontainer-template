namespace ApplicationSettings.Api.Configuration;

public class MongoDbOptions
{
    public const string SectionName = "MongoDb";
    
    public required string ConnectionString { get; set; }
    public required string DatabaseName { get; set; }
    public required string CollectionName { get; set; }
}