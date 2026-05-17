namespace EasyShop.Catalog.Domain.Events;

public record ProductUpdatedEvent
{
    public Guid ProductId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public Guid CategoryId { get; set; }
}