using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities;
using WebStore.Infrastructure.Interfaces;
using WebStore.Models;

namespace WebStore.ViewComponents
{
    public class SectionsViewComponent : ViewComponent
    {
        private readonly ICatalogData _catalogData;

        public SectionsViewComponent(ICatalogData catalogData)
        {
            _catalogData = catalogData;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<SectionViewModel> sections = GetSections();
            return View(sections);
        }
        private List<SectionViewModel> GetSections()
        {
            IQueryable<Section> allSections = _catalogData.GetSections();
            Section[] parentSections = allSections.Where(p => p.ParentId == null).ToArray();
            List<SectionViewModel> parentSectionsList = new List<SectionViewModel>();

            foreach (Section parent in parentSections)
            {
                parentSectionsList.Add(new SectionViewModel
                {
                    Id = parent.Id,
                    Name = parent.Name,
                    Order = parent.Order,
                    ParentSection = null
                });
            }

            foreach (SectionViewModel parent in parentSectionsList)
            {
                IEnumerable<Section> childSections = allSections.Where(c => c.ParentId == parent.Id);

                foreach (Section child in childSections)
                {
                    parent.ChildSections.Add(new SectionViewModel
                    {
                        Id = child.Id,
                        Name = child.Name,
                        Order = child.Order,
                        ParentSection = parent
                    });
                }

                parent.ChildSections = parent.ChildSections.OrderBy(c => c.Order).ToList();
            }

            parentSectionsList = parentSectionsList.OrderBy(c => c.Order).ToList();
            return parentSectionsList;
        }
    }

}
