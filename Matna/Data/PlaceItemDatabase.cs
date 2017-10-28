using System.Collections.Generic;
using System.Threading.Tasks;
using Matna.Models;
using SQLite;

namespace Matna.Data
{
    public class PlaceItemDatabase
    {
        readonly SQLiteAsyncConnection database;

        public PlaceItemDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<PlaceItem>().Wait();
        }

        public Task<List<PlaceItem>> GetItemsAsync()
        {
            return database.Table<PlaceItem>().ToListAsync();
        }

        public Task<List<PlaceItem>> GetItemsCameraCenterAsync()
        {
            return database.QueryAsync<PlaceItem>("SELECT * FROM [PlaceItem] WHERE [Name] = 'CameraCenter' LIMIT 1");
        }

        public Task<PlaceItem> GetItemAsync(int id)
        {
            return database.Table<PlaceItem>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(PlaceItem item)
        {
            if (item.ID != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(PlaceItem item)
        {
            return database.DeleteAsync(item);
        }
    }
}
