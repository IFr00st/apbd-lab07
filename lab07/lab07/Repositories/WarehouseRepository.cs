using Microsoft.Data.SqlClient;

namespace lab07.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly IConfiguration _configuration;
    public WarehouseRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<bool> DoesProductExist(int id)
    {
        var query = "SELECT 1 FROM Product WHERE IdProduct = @ID";

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@ID", id);

        await connection.OpenAsync();

        var res = await command.ExecuteScalarAsync();

        return res is not null;
    }
    
    public async Task<bool> DoesWarehouseExist(int id)
    {
        var query = "SELECT 1 FROM Warehouse WHERE IdWarehouse = @ID";

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@ID", id);

        await connection.OpenAsync();

        var res = await command.ExecuteScalarAsync();

        return res is not null;
    }
    
    public async Task<bool> DoesRightOrderExist(int id, int amount)
    {
        var query = "SELECT 1 FROM Order WHERE IdProduct = @ID AND Amount = @amount AND FulfilledAt IS NULL";

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@ID", id);
        command.Parameters.AddWithValue("@amount", amount);

        await connection.OpenAsync();

        var res = await command.ExecuteScalarAsync();

        return res is not null;
    }
    
    
    public async Task<bool> DoesProductOrderExist(int id)
    {
        var query = "SELECT 1 FROM Product_Warehouse WHERE IdOrder = @ID";

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@ID", id);

        await connection.OpenAsync();

        var res = await command.ExecuteScalarAsync();

        return res is null;
    } 

    public async Task UpdateFulfilledAt(int id)
    {
        DateTime x = new DateTime();
        var query = "UPDATE Order SET FulfilledAt = @date WHERE IdOrder = @ID";

        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@ID", id);
        command.Parameters.AddWithValue("@date", x);

        await connection.OpenAsync();
        await command.ExecuteScalarAsync();
    }

    public async Task<int> InsertIntoProductWarehouse(int ProductId, int WarehouseId, int Amount)
    {
        DateTime x = new DateTime();
        var query = "SELECT 1 FROM Order WHERE IdOrder = @ID AND Amount = @amount AND FulfilledAt IS NULL";
        
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@ID", ProductId);
        command.Parameters.AddWithValue("@amount", Amount);

        await connection.OpenAsync();

        var reader = await command.ExecuteReaderAsync();
        var ordOrderId = reader.GetOrdinal("OrderId");

        var OrderId = reader.GetInt32(ordOrderId);
        
        
        await using SqlCommand command2 = new SqlCommand();
        
        var query2 = "SELECT 1 FROM Product WHERE IdProduct = @ID";

        command2.Connection = connection;
        command2.CommandText = query2;
        command2.Parameters.AddWithValue("@ID", ProductId);

        var reader2 = await command2.ExecuteReaderAsync();
        var ProdutPrice = reader2.GetOrdinal("Price");

        var price = reader2.GetInt32(ProdutPrice) * Amount;
        
        await using SqlCommand command3 = new SqlCommand();
        
        var query3 = "INSERT INTO (IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt) VALUES (@idwarehouse, @idproduct, @idorder, @amount, @price, @createdat)";

        command3.Connection = connection;
        command3.CommandText = query3;
        command3.Parameters.AddWithValue("@idwarehouse", WarehouseId);
        command3.Parameters.AddWithValue("@idproduct", ProductId);
        command3.Parameters.AddWithValue("@idorder", OrderId);
        command3.Parameters.AddWithValue("@amount", Amount);
        command3.Parameters.AddWithValue("@price", price);
        command3.Parameters.AddWithValue("@createdat", x);
        var reader3 = await command3.ExecuteReaderAsync();
        var y = reader3.GetOrdinal("IdProductWarehouse");
        return reader3.GetInt32(y);
    }

}