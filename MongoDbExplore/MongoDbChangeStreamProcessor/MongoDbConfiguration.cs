using System.ComponentModel.DataAnnotations;

namespace MongoDbChangeStreamProcessor;

public class MongoDbConfiguration
{
    [Required(AllowEmptyStrings = false)]
    public string? ConnectionString { get; set; }

    [Required(AllowEmptyStrings = false)]
    public string? DatabaseName { get; set; }
}
