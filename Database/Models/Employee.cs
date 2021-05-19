using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Database.Models
{
    public class Employee : ReactiveObject
    {
        // Поля
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Reactive] public string Surname { get; set; }
        [Reactive] public string Name { get; set; }
        [Reactive] public string Patronumic { get; set; }
        [Reactive] public string Mac { get; set; }

        // Ключи
        [Reactive]
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }

        [Reactive]
        [ForeignKey("TokenId")]
        public Token Token { get; set; }
    }
}
