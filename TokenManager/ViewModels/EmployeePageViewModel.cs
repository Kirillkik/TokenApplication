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

namespace EmployeeManager.ViewModels
{
    public class EmployeePageViewModel : ReactiveObject
    {
        [Reactive] public Employee[] Employees { get; set; }
        [Reactive] public Department[] Departments { get; set; }
        [Reactive] public Token[] Tokens { get; set; }
        [Reactive] public Employee SelectedEmployee { get; set; } = new Employee();

        public EmployeePageViewModel()
        {
            DBContext db = new DBContext();
            Employees = db.Employees.Include(x => x.Department)
                                    .Include(x => x.Token)
                                    .ToArray();
            Departments = db.Departments.ToArray();
            Tokens = db.Tokens.ToArray();
        }

        private DelegateCommand updateEmployees;
        public DelegateCommand UpdateEmployees => updateEmployees ??= new DelegateCommand(() =>
        {
            DBContext db = new DBContext();
            Employees = db.Employees.Include(x => x.Department)
                                    .Include(x => x.Token)
                                    .ToArray();
            Departments = db.Departments.ToArray();
            Tokens = db.Tokens.ToArray();
        });

        private DelegateCommand addEmployee;
        public DelegateCommand AddEmployee => addEmployee ??= new DelegateCommand(() =>
        {
            DBContext db = new DBContext();
            var employee = new Employee()
            {
                Surname = SelectedEmployee.Surname,
                Name = SelectedEmployee.Name,
                Patronumic = SelectedEmployee.Patronumic,
                Mac = SelectedEmployee.Mac,
                Token = db.Tokens.Single(x => x.Id == SelectedEmployee.Token.Id),
                Department = db.Departments.Single(x => x.Id == SelectedEmployee.Department.Id)
        };
            db.Employees.Add(employee);
            db.SaveChanges();
            Update();
        });

        private DelegateCommand editEmployee;
        public DelegateCommand EditEmployee => editEmployee ??= new DelegateCommand(() =>
        {
            DBContext db = new DBContext();
            Employee Employee = db.Employees.Single(x => x.Id == SelectedEmployee.Id);
            Employee.Surname = SelectedEmployee.Surname;
            Employee.Name = SelectedEmployee.Name;
            Employee.Patronumic = SelectedEmployee.Patronumic;
            Employee.Mac = SelectedEmployee.Mac;
            Employee.Token = db.Tokens.Single(x => x.Id == SelectedEmployee.Token.Id);
            Employee.Department = db.Departments.Single(x => x.Id == SelectedEmployee.Department.Id);
            db.SaveChanges();
            Update();
        });

        private DelegateCommand deleteEmployee;
        public DelegateCommand DeleteEmployee => deleteEmployee ??= new DelegateCommand(() =>
        {
            DBContext db = new DBContext();
            Employee Employee = db.Employees.Single(x => x.Id == SelectedEmployee.Id);
            db.Employees.Remove(Employee);
            db.SaveChanges();
            Update();
        });

        private void Update()
        {
            DBContext db = new DBContext();
            Employees = db.Employees.Include(x => x.Department)
                                    .Include(x => x.Token)
                                    .ToArray();
            Departments = db.Departments.ToArray();
            Tokens = db.Tokens.ToArray();
            SelectedEmployee = new Employee();
            SelectedEmployee.Surname = "";
            SelectedEmployee.Name = "";
            SelectedEmployee.Patronumic = "";
            SelectedEmployee.Mac = "";
            SelectedEmployee.Token = null;
            SelectedEmployee.Department = null;
        }
    }
}
