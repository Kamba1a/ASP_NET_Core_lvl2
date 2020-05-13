using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities.Base
{
    /// <summary>
    /// Сущность имеющая наименование и Id
    /// </summary>
    public class NamedEntity : BaseEntity, INamedEntity
    {
        public string Name { get; set; }
    }
}