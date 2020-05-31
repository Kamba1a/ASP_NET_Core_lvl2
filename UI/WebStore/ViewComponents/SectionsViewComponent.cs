using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.DTO.Catalog;
using WebStore.Domain.Entities;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;

namespace WebStore.ViewComponents
{
    public class SectionsViewComponent : ViewComponent
    {
        private readonly ICatalogData _catalogData;
        private int? _currentParentSectionId;

        public SectionsViewComponent(ICatalogData catalogData)
        {
            _catalogData = catalogData;
        }
        public IViewComponentResult Invoke(string sectionId)
        {
            int? currentSectionId = int.TryParse(sectionId, out int id) ? id : (int?)null;

            List<SectionViewModel> sections = GetSections(currentSectionId);

            return View(new SectionCompleteViewModel
            {
                Sections = sections,
                CurrentSectionId = currentSectionId,
                ParentSectionId = _currentParentSectionId
            });
        }
        private List<SectionViewModel> GetSections(int? currentSectionId)
        {
            IEnumerable<SectionDTO> allSections = _catalogData.GetSections();
            SectionDTO[] parentSections = allSections.Where(p => p.ParentId == null).ToArray();
            List<SectionViewModel> parentSectionsList = new List<SectionViewModel>();

            foreach (SectionDTO parent in parentSections)
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
                IEnumerable<SectionDTO> childSections = allSections.Where(c => c.ParentId == parent.Id);

                foreach (SectionDTO child in childSections)
                {
                    if (child.Id == currentSectionId) _currentParentSectionId = parent.Id;

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
