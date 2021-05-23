using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows;
using Database.DatabaseContext;
using Database.Models;
using DevExpress.Mvvm;
using Microsoft.EntityFrameworkCore;
using Net.Pkcs11Interop.Common;
using Net.Pkcs11Interop.HighLevelAPI;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RutokenPkcs11Interop;

namespace TokenChecker.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        [Reactive] public string Surname { get; set; }
        [Reactive] public string Name { get; set; }
        [Reactive] public string Patronumic { get; set; }
        public MainWindowViewModel()
        {
        }

        private DelegateCommand checkData;
        public DelegateCommand CheckData => checkData ??= new DelegateCommand(() =>
        {
            DBContext db = new DBContext();
            Employee employee = db.Employees.Include(x => x.Token)
                                            .FirstOrDefault(x => x.Surname.ToLower().CompareTo(Surname.ToLower()) == 0 &&
                                                            x.Name.ToLower().CompareTo(Name.ToLower()) == 0 &&
                                                            x.Patronumic.ToLower().CompareTo(Patronumic.ToLower()) == 0);
            if(employee == null)
            {
                ShowExceptionMessage("Пользователь не найден");
                return;
            }

            if(string.IsNullOrEmpty(employee.Token?.Value))
            {
                ShowExceptionMessage("У пользователя отсутствуют данные о токене");
                return;
            }

            var tokens = GetRuTokens();

            if(tokens.Length == 0)

            {
                ShowExceptionMessage("Рутокен не обнаружен, пожалуйста вставье устройство");
                return;
            }

            if (employee.Token.Value != tokens.FirstOrDefault())
            {
                ShowExceptionMessage("В доступе отказано, токен не совпадает");
                return;
            }

            if (employee.Token.LifeDate < DateTime.Now)
            {
                ShowExceptionMessage("В доступе отказано, срок действия токена окончен");
                return;
            }

            MessageBox.Show("Доступ разрешен", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
        });

        private string[] GetRuTokens()
        {
            List<string> serialNumbers = new List<string>();
            try
            {
                using (var pkcs11 = new Pkcs11(Settings.RutokenEcpDllDefaultPath, AppType.MultiThreaded))
                {
                    List<Slot> slots = pkcs11.GetSlotList(SlotsType.WithTokenPresent);
                    foreach (var slot in slots)
                    {
                        TokenInfo tokenInfo = slot.GetTokenInfo();
                        serialNumbers.Add(tokenInfo.SerialNumber);
                    }
                }
            }
            catch (Pkcs11Exception ex)
            {
                Console.WriteLine($"Operation failed [Method: {ex.Method}, RV: {ex.RV}]");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Operation failed [Message: {ex.Message}]");
            }
            return serialNumbers.ToArray();
        }

        public void ShowExceptionMessage(string exceptionMessage)
        {
            MessageBox.Show(exceptionMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
