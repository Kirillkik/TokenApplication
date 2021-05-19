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
using System.Windows;

namespace TokenManager.ViewModelsupdateTokens
{
    public class TokensPageViewModel : ReactiveObject
    {
        [Reactive] public Token[] Tokens { get; set; }
        [Reactive] public Employee[] Employees { get; set; }
        [Reactive] public Token SelectedToken { get; set; } = new Token() { LifeDate = DateTime.Now.Date};

        public TokensPageViewModel()
        {
            DBContext db = new DBContext();
            Tokens = db.Tokens.ToArray();
        }

        private DelegateCommand updateTokens;
        public DelegateCommand UpdateTokens => updateTokens ??= new DelegateCommand(() =>
        {
            DBContext db = new DBContext();
            Tokens = db.Tokens.ToArray();
        });

        private DelegateCommand addToken;
        public DelegateCommand AddToken => addToken ??= new DelegateCommand(() =>
        {
            DBContext db = new DBContext();
            db.Tokens.Add(new Token() { Value = SelectedToken.Value, LifeDate = SelectedToken.LifeDate });
            db.SaveChanges();
            Update();
        });

        private DelegateCommand editToken;
        public DelegateCommand EditToken => editToken ??= new DelegateCommand(() =>
        {
            DBContext db = new DBContext();
            Token Token = db.Tokens.Single(x => x.Id == SelectedToken.Id);
            Token.Value = SelectedToken.Value;
            Token.LifeDate = SelectedToken.LifeDate;
            db.SaveChanges();
            Update();
        });

        private DelegateCommand deleteToken;
        public DelegateCommand DeleteToken => deleteToken ??= new DelegateCommand(() =>
        {
            DBContext db = new DBContext();
            Token token = db.Tokens.Single(x => x.Id == SelectedToken.Id);
            db.Tokens.Remove(token);
            db.SaveChanges();
            Update();
        });

        private DelegateCommand findEmployee;
        public DelegateCommand FindEmployee => findEmployee ??= new DelegateCommand(() =>
        {
            DBContext db = new DBContext();
            Employees = db.Employees.Include(x => x.Department)
                                    .Where(x => x.Token.Id == SelectedToken.Id)
                                    .ToArray();
        });
        private void Update()
        {
            DBContext db = new DBContext();
            Tokens = db.Tokens.ToArray();
            SelectedToken = new Token();
            SelectedToken.Value = "";
            SelectedToken.LifeDate = DateTime.Now.Date;
        }
    }
}
