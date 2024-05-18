using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;
using App2.Models;

namespace App2.Data
{
    public class DatabaseService
    {
        public SQLiteAsyncConnection Database;

        public DatabaseService(string dbPath)
        {
            Database = new SQLiteAsyncConnection(dbPath);
            Database.CreateTableAsync<Ispolnitel>().Wait();
            Database.CreateTableAsync<Zakazchik>().Wait();
            Database.CreateTableAsync<Zakaz>().Wait();
            Database.CreateTableAsync<Konkurs>().Wait();
        }

        public async Task<Ispolnitel> AuthenticateUserAsync(string login, string password)
        {
            try
            {
                var user = await GetUserByLoginAsync(login);

                if (user != null && user.Passwordd == password)
                {
                    return user;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка аутентификации: {ex.Message}");
            }

            return null;
        }

        public async Task<bool> UserExists(string username)
        {
            try
            {
                var user = await Database.Table<Ispolnitel>().Where(u => u.Loginad == username).FirstOrDefaultAsync();
                return user != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при проверке существования пользователя: " + ex);
                return false;
            }
        }

        public async Task<bool> User(string username)
        {
            try
            {
                var user = await Database.Table<Zakazchik>().Where(u => u.Loginad == username).FirstOrDefaultAsync();
                return user != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при проверке существования пользователя: " + ex);
                return false;
            }
        }

        public async Task<Ispolnitel> GetUserByLoginAsync(string login)
        {
            return await Database.Table<Ispolnitel>().Where(u => u.Loginad == login).FirstOrDefaultAsync();
        }

        public async Task<List<Zakaz>> GetProjectsByCategoryFromDatabase(string category)
        {
            try
            {
                return await Database.Table<Zakaz>().Where(z => z.Kategoria == category).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении проектов по категории: {ex.Message}");
                return null;
            }
        }

        public async Task<List<Konkurs>> GetProjects(string category)
        {
            try
            {
                return await Database.Table<Konkurs>().Where(z => z.Kategoria == category).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении проектов по категории: {ex.Message}");
                return null;
            }
        }

        public async Task<Ispolnitel> GetLoggedInUserAsync(string username, string password)
        {
            return await Database.Table<Ispolnitel>().Where(i => i.Loginad == username && i.Passwordd == password).FirstOrDefaultAsync();
        }

        public Task<List<Konkurs>> GetProjectsAsync()
        {
            return Database.Table<Konkurs>().ToListAsync();
        }

        public Task<List<Zakaz>> GetProjectsFromDatabase()
        {
            return Database.Table<Zakaz>().ToListAsync();
        }

        public Task<List<Ispolnitel>> GetItemsAsync()
        {
            return Database.Table<Ispolnitel>().ToListAsync();
        }

        public async Task<List<Konkurs>> GeAsync(int category)
        {
            try
            {
                return await Database.Table<Konkurs>().Where(z => z.IdIspolnitel == category).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении проектов по категории: {ex.Message}");
                return null;
            }
        }

        public async Task<List<Konkurs>> Gesync(int category)
        {
            try
            {
                return await Database.Table<Konkurs>().Where(z => z.IdZakazchik == category).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении проектов по категории: {ex.Message}");
                return null;
            }
        }

        public async Task<List<Zakaz>> Get(int category)
        {
            try
            {
                return await Database.Table<Zakaz>().Where(z => z.IdIspolnitel == category).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении проектов по категории: {ex.Message}");
                return null;
            }
        }

        public async Task<List<Zakaz>> Ge(int category)
        {
            try
            {
                return await Database.Table<Zakaz>().Where(z => z.IdZakazchik == category).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении проектов по категории: {ex.Message}");
                return null;
            }
        }

        public Task<Ispolnitel> GetItemAsync(int id)
        {
            return Database.GetAsync<Ispolnitel>(id);
        }

        public Task<Ispolnitel> GetUserByUsernameAsync(string username)
        {
            return Database.Table<Ispolnitel>().Where(i => i.Loginad == username).FirstOrDefaultAsync();
        }

        public Task<int> Save(Konkurs item)
        {
            if (item.ID_koncurs != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                return Database.InsertAsync(item);
            }
        }

        public Task<int> SaveAsync(Zakaz item)
        {
            if (item.ID_zakaz != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                return Database.InsertAsync(item);
            }
        }

        public Task<int> Savee(Konkurs item)
        {
            if (item.ID_koncurs != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                return Database.InsertAsync(item);
            }
        }

        public Task<int> SaveItemAsync(Ispolnitel item)
        {
            if (item.ID_Ispolnitel != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                return Database.InsertAsync(item);
            }
        }

        public Task<int> UpdateUserProfile(Ispolnitel userProfile)
        {
            return Database.UpdateAsync(userProfile);
        }

        public Task<int> DeleteItemAsync(Ispolnitel item)
        {
            return Database.DeleteAsync(item);
        }

        public Task<int> DeleteAsync(Zakaz projectItemCopy)
        {
            return Database.DeleteAsync(projectItemCopy);
        }

        public Task<int> Delete(Konkurs projecttem)
        {
            return Database.DeleteAsync(projecttem);
        }

        public Task<List<Zakazchik>> GetItemmAsync()
        {
            return Database.Table<Zakazchik>().ToListAsync();
        }

        public Task<Zakazchik> GetItemmsAsync(int id)
        {
            return Database.GetAsync<Zakazchik>(id);
        }

        public Task<Zakazchik> GetUserUsernameAsync(string username)
        {
            return Database.Table<Zakazchik>().Where(i => i.Loginad == username).FirstOrDefaultAsync();
        }

        public Task<int> DeleteItemAsync(Zakazchik item)
        {
            return Database.DeleteAsync(item);
        }

        public Task<int> SaveItemAsync(Zakazchik item)
        {
            if (item.ID_zakazchik != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                return Database.InsertAsync(item);
            }
        }

        public async Task<Zakazchik> GetLogedInUserAsync(string username, string password)
        {
            return await Database.Table<Zakazchik>().Where(i => i.Loginad == username && i.Passwordd == password).FirstOrDefaultAsync();
        }

        public Task<int> UpateUserProfile(Zakazchik userProfile)
        {
            return Database.UpdateAsync(userProfile);
        }
    }
}
