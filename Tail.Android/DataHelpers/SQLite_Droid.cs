using System;
using System.IO;
using SQLite;
using Tail.Droid.DataHelpers;
using Tail.Services.LocalStorage.Helpers;
using Xamarin.Forms;

[assembly: Dependency(typeof(SqLiteDroid))]
namespace Tail.Droid.DataHelpers
{
    public class SqLiteDroid : ISqLite
    {
        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = "Tail.db3";
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            var path = Path.Combine(documentsPath, sqliteFilename);
            var conn = new SQLiteConnection(path, true);

            return conn;
        }
    }
}
