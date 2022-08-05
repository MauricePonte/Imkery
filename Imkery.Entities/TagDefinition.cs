using Imkery.Entities;

namespace Imkery.Entities
{
    public class TagDefinition : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string GetDescription() => Name;
    }
}
