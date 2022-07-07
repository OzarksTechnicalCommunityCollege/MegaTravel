using MegaTravelAPI.Models;
using MegaTravelAPI.Data;

namespace MegaTravelAPI.IRepository
{
    public interface IAgent
    {
        /// <summary>
        /// Logs in an agent
        /// </summary>
        /// <param name="tokenData"></param>
        /// <returns></returns>
        Task<LoginResponse> LoginAgent(LoginModel tokenData);

        /// <summary>
        /// Finds the user data for a user based on username
        /// </summary>
        /// <param name="tokenData"></param>
        /// <returns></returns>
        Task<MegaTravelAPI.Data.Agent> FindByName(string username);
    }
}
