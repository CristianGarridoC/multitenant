namespace Product.Application.Product.Queries.GetAll;

public record GetAllResponse(IEnumerable<Product> Products);
public record Product(int Id, string Name, string Description, int Duration);