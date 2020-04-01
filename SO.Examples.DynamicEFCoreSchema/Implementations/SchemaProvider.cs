using Microsoft.Extensions.Options;
using SO.Examples.DynamicEFCoreSchema.Contracts;

namespace SO.Examples.DynamicEFCoreSchema.Implementations
{
    public class SchemaProvider : ISchemaProvider
    {
        private DatabaseSettings Settings { get; }
        
        public SchemaProvider(IOptions<DatabaseSettings> settings)
        {
            this.Settings = settings.Value;
        }
        
        public string GetSchema()
        {
            return this.Settings.Schema;
        }
    }
}