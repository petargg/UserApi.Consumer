using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UserApi.Consumer
{
    public class UsersClient
    {
        private HttpClient client;

        public UsersClient()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("");
        }

        public async Task<User> GetUser(int id)
        {
            var response = await client.GetAsync("/api/users/" + id).ConfigureAwait(false);
            var user = JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());
            return user;
        }

        public async Task<IEnumerable<User>> GetUsers(IEnumerable<int> ids)
        {
            var response = await client
                .PostAsync("/api/users/GetMany", new StringContent(JsonConvert.SerializeObject(ids), Encoding.UTF8, "application/json"))
                .ConfigureAwait(false);

            var users = JsonConvert.DeserializeObject<IEnumerable<User>>(await response.Content.ReadAsStringAsync());

            return users;
        }
    }
}
