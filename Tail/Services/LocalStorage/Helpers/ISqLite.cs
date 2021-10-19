using SQLite;

namespace Tail.Services.LocalStorage.Helpers
{
    public interface ISqLite
    {
        SQLiteConnection GetConnection();
    }
}
