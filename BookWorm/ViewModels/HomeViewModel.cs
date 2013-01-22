using System.Collections.Generic;

namespace BookWorm.ViewModels
{
    public class HomeViewModel
    {
        public List<BookInformation> Books { get; set; }
        public List<IBasePostInformation> News { get; set; }
    }
}