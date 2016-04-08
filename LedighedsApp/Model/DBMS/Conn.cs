using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.UI.Xaml;

namespace LedighedsApp.Model.DBMS
{
    static class Conn
    {
        private static string _dbFile = "Storage.db";
        private static string _dbPath = string.Empty;
        public static string DbPath
        {
            get
            {
                if (string.IsNullOrEmpty(_dbPath))
                {
                    ValidateDatabase();
                    _dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, _dbFile);
                }
                return _dbPath;
            }
        }

        private static async void ValidateDatabase()
        {
            bool isDatabaseExisting = false;
            try
            {
                StorageFile storageFile = await ApplicationData.Current.LocalFolder.GetFileAsync(_dbFile);
                isDatabaseExisting = true;
            }
            catch (Exception ex)
            {
                isDatabaseExisting = false;
            }

            if (!isDatabaseExisting)
            {
                StorageFile databaseFile = await Package.Current.InstalledLocation.GetFileAsync(_dbFile);
                await databaseFile.CopyAsync(ApplicationData.Current.LocalFolder);
            }
        }

        public static T DelegateFunc<T>(Func<SQLiteConnection, T> funcToRun) where T : new()
        {
            using (var db = new SQLiteConnection(DbPath))
            {
                db.Trace = true;
                return funcToRun(db);
            }
        }

        public static void RunAction(Action<SQLiteConnection> actionToRun)
        {
            using (var db = new SQLiteConnection(DbPath))
            {
                db.Trace = true;
                db.BeginTransaction();
                actionToRun(db);
                db.Commit();
            }
        }

        public static void Insert(object obj)
        {
            RunAction(db => db.Insert(obj));
        }

        public static void InsertRange(IEnumerable<object> obj)
        {
            RunAction(db => db.InsertAll(obj));
        }

        public static void Update(object obj)
        {
            RunAction(db => db.Update(obj));
        }

        public static void UpdateRange(IEnumerable<object> obj)
        {
            RunAction(db => db.UpdateAll(obj));
        }

        public static void Delete(object obj)
        {
            RunAction(db => db.Delete(obj));
        }

        public static void DeleteRange(IEnumerable<object> obj)
        {
            using (var db = new SQLiteConnection(DbPath))
            {
                db.Trace = true;
                db.BeginTransaction();
                foreach (object objs in obj)
                    db.Delete(objs);
                db.Commit();
            }
        }

        public static List<T> GetItems<T>(string query, params object[] args) where T : new()
        {
            return DelegateFunc<List<T>>(db => db.Query<T>(query, args));
        }

        public static T GetSingleItem<T>(string query, params object[] args) where T : new()
        {
            return DelegateFunc<T>(db => db.Query<T>(query, args).FirstOrDefault());
        }
    }
}
