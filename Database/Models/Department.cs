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
    public class Department : ReactiveObject
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Reactive] public int Id { get; set; }
        [Reactive] public string Value { get; set; }

        public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();

        public override string ToString()
        {
            return Value;
        }
    }
}
