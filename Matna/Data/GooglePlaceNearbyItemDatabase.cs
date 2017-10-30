using System.Collections.Generic;
using System.Threading.Tasks;
using Matna.Models;
using SQLite;

namespace Matna.Data
{
    public class GooglePlaceNearbyItemDatabase
    {
        readonly SQLiteAsyncConnection database;

        public GooglePlaceNearbyItemDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<GooglePlaceNearbyItem>().Wait();
        }

        public Task<List<GooglePlaceNearbyItem>> GetItemsAsync()
        {
            return database.Table<GooglePlaceNearbyItem>().ToListAsync();
        }

        public Task<GooglePlaceNearbyItem> GetItemAsync(string pid)
        {
            return database.Table<GooglePlaceNearbyItem>().Where(i => i.PlaceId == pid).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItemAsync(GooglePlaceNearbyItem item)
        {
            var rtn = await GetItemAsync(item.PlaceId);
            if (rtn == null)
            {
                return await database.InsertAsync(item);
            }
            else
            {
                return await database.UpdateAsync(item);
            }
        }

        public async Task<int> DeleteItemAsync(GooglePlaceNearbyItem item)
        {
            var rtn = await GetItemAsync(item.PlaceId);
            if (rtn != null)
            {
                return await database.DeleteAsync(item);
            }
            else return 0;
        }
    }
}
