using BookBee.Model;

namespace BookBee.Persistences.Repositories.QuantityRepository
{
    public interface IQuantityRepository
    {
        void CreateQuantity(int Count);
        Quantity GetQuantity(int Count);
        bool IsSaveChanges();
    }
}
