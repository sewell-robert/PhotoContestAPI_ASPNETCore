using System.Collections.Generic;
using System.Threading.Tasks;
using PhotoContestAPI.Models;

namespace PhotoContestAPI.Services
{
    public interface ICosmosDbService
    {
        Task<IEnumerable<PhotoData>> GetItemsAsync(string query);
        Task<PhotoData> GetItemAsync(string id);
        Task AddItemAsync(PhotoData photoData);
        Task UpdateItemAsync(PhotoData photoData);
        Task DeleteItemAsync(string id);
    }
}
