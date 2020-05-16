using System.Collections.Generic;

namespace WebStore.Domain.Models
{
    public class SectionViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        /// <summary>Дочерние секции</summary>
        public List<SectionViewModel> ChildSections { get; set; }

        /// <summary>Родительская секция</summary>
        public SectionViewModel ParentSection { get; set; }

        public SectionViewModel()
        {
            ChildSections = new List<SectionViewModel>();
        }
    }
}
