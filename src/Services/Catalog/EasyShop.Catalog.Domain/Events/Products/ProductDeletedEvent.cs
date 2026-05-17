namespace EasyShop.Catalog.Domain.Events;

public record ProductDeletedEvent
{
    public Guid ProductId { get; set; }
}
