using Database.DatabaseContext;
using Database.Models;
using DevExpress.Mvvm;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TokenManager.ViewModels
{
    public class EmployeesTokenViewModel : ReactiveObject
    {
        [Reactive] public Employee[] Employees { get; set; }
        [Reactive] public Department[] Departments { get; set; }
        [Reactive] public Token[] Tokens { get; set; }
        [Reactive] public Employee SelectedEmployee { get; set; } = new Employee();

        public EmployeesTokenViewModel()
        {
            DBContext db = new DBContext();
            Employees = db.Employees.Include(x => x.Department)
                                    .Include(x => x.Token)
                                    .ToArray();
            Tokens = db.Tokens.ToArray();
            Departments = db.Departments.ToArray();
        }

        private DelegateCommand updateEmployees;
        public DelegateCommand UpdateEmployees => updateEmployees ??= new DelegateCommand(() =>
        {
            DBContext db = new DBContext();
            Employees = db.Employees.Include(x => x.Department)
                                    .Include(x => x.Token)
                                    .ToArray();
            Tokens = db.Tokens.ToArray();
            Departments = db.Departments.ToArray();
        });
        
        private DelegateCommand editEmployee;
        public DelegateCommand EditEmployee => editEmployee ??= new DelegateCommand(() =>
        {
            DBContext db = new DBContext();
            Employee Employee = db.Employees.Single(x => x.Id == SelectedEmployee.Id);
            Employee.Token = db.Tokens.Single(x => x.Id == SelectedEmployee.Token.Id);
            db.SaveChanges();
            Update();
        });

        private void Update()
        {
            DBContext db = new DBContext();
            Employees = db.Employees.Include(x => x.Token)
                                    .ToArray();
            Tokens = db.Tokens.ToArray();
            Departments = db.Departments.ToArray();
            SelectedEmployee = new Employee();
            SelectedEmployee.Surname = "";
            SelectedEmployee.Name = "";
            SelectedEmployee.Patronumic = "";
            SelectedEmployee.Mac = "";
            SelectedEmployee.Department = null;
            SelectedEmployee.Token = null;
        }
    }
}
