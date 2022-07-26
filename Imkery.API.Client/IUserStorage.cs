using Imkery.Entities;
using System.Threading.Tasks;

namespace Imkery.API.Client
{
    public interface IUserStorage
    {
        Task<LocalImkeryUser> GetUser();
        Task StoreUser(LocalImkeryUser user);
    }
}