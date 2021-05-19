using Database.DatabaseContext;
using Database.Models;
using DevExpress.Mvvm;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TokenManager.ViewModels
{
    public class DepartmentsPageViewModel : ReactiveObject
    {
        [Reactive] public Department[] Departments { get; set; }
        [Reactive] public Department SelectedDepartment { get; set; } = new Department();

        public DepartmentsPageViewModel()
        {
            DBContext db = new DBContext();
            Departments = db.Departments.ToArray();
        }

        private DelegateCommand updateDepartments;
        public DelegateCommand UpdateDepartments => updateDepartments ??= new DelegateCommand(() =>
        {
            DBContext db = new DBContext();
            Departments = db.Departments.ToArray();
        });

        private DelegateCommand addDepartment;
        public DelegateCommand AddDepartment => addDepartment ??= new DelegateCommand(() =>
        {
            DBContext db = new DBContext();
            db.Departments.Add(new Department() { Value = SelectedDepartment.Value});
            db.SaveChanges();
            Update();
        });

        private DelegateCommand editDepartment;
        public DelegateCommand EditDepartment => editDepartment ??= new DelegateCommand(() =>
        {
            DBContext db = new DBContext();
            Department Department = db.Departments.Single(x => x.Id == SelectedDepartment.Id);
            Department.Value = SelectedDepartment.Value;
            db.SaveChanges();
            Update();
        });

        private DelegateCommand deleteDepartment;
        public DelegateCommand DeleteDepartment => deleteDepartment ??= new DelegateCommand(() =>
        {
            DBContext db = new DBContext();
            Department Department = db.Departments.Single(x => x.Id == SelectedDepartment.Id);
            db.Departments.Remove(Department);
            db.SaveChanges();
            Update();
        });

        private void Update()
        {
            DBContext db = new DBContext();
            Departments = db.Departments.ToArray();
            SelectedDepartment = new Department();
            SelectedDepartment.Value = "";
        }
    }
}
