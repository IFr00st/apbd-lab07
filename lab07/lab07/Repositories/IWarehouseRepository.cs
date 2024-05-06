namespace lab07.Repositories;

public interface IWarehouseRepository
{
    Task<bool> DoesProductExist(int id);
    Task<bool> DoesWarehouseExist(int id);
    Task<bool> DoesRightOrderExist(int id, int amount);
    Task<bool> DoesProductOrderExist(int id);
    Task UpdateFulfilledAt(int id);
    Task<int> InsertIntoProductWarehouse(int ProductId, int WarehouseId, int Amount);
}