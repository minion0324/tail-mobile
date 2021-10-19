using System;
using System.IO;
using SQLite;
using Tail.iOS.DataHelpers;
using Tail.Services.LocalStorage.Helpers;
using Xamarin.Forms;

[assembly: Dependency(typeof(SqLiteiOs))]
namespace Tail.iOS.DataHelpers
{
    public class SqLiteiOs : ISqLite
    {
        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = "Tail.db3";
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            var libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder
            var path = Path.Combine(libraryPath, sqliteFilename);
            var conn = new SQLiteConnection(path, true);

            return conn;
        }
    }
}
