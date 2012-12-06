using BookWorm.Models;

namespace BookWorm.Repository
{
    public class Repository<Model>
    {
        public virtual Model<Model> Create(Model model)
        {
            throw new System.NotImplementedException();
        }
    }

    public class Model<T>
    {
        public int Id { get; set; }
    }
}