using BookBee.Model;

namespace BookBee.Persistences.Repositories.QuantityRepository
{
    public class QuantityRepository : IQuantityRepository
    {
        private readonly DataContext _dataContext;
        public QuantityRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void CreateQuantity(int Count)
        {
            var quantity = _dataContext.Quantities.FirstOrDefault(q => q.Count == Count);
            if (quantity == null)
            {
                quantity = new Quantity();
                quantity.Count = Count;
                _dataContext.Quantities.Add(quantity);
            }

        }
        public Quantity GetQuantity(int Count)
        {
            return _dataContext.Quantities.FirstOrDefault(q => q.Count == Count);
        }

        public bool IsSaveChanges()
        {
            return _dataContext.SaveChanges() > 0;
        }
    }
}
