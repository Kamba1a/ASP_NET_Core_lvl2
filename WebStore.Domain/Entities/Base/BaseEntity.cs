using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities.Base
{
    /// <summary>
    /// Базовая сущность имеющая Id
    /// </summary>
    public class BaseEntity : IBaseEntity
    {
        [Key] // поле Id будет ключом
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //База данных формирует значение при вставке строки (в современных версиях атрибут не нужен - значение и так по-умолчанию)
        public int Id { get; set; }
    }
}