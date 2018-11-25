using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace UserApi.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new UsersClient();
            var idList = GetIds(1000);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //var user = GetOneUser(client).Result;
            //var users = GetUsersSynchrnously(idList, client).Result;
            //var users = GetUsersInParallel(idList, client).Result;
            //var users = GetUsersInParallelFixed(idList, client).Result;
            var users = GetUsersInParallelInWithBatches(idList, client).Result;

            stopwatch.Stop();
            Console.WriteLine("Time elapsed: {0:hh\\:mm\\:ss\\:fff}", stopwatch.Elapsed);
            Console.ReadLine();
        }

        //--------------------------------------------------------
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //var user = GetOneUser(client).Result; // 1sec for one user
        //var users = GetUsersSynchrnously(idList, client).Result; //1000 users = 1min 46 sec
        //var users = GetUsersInParallel(idList, client).Result; // 1000 users = 24 sec
        //var users = GetUsersInParallelFixed(idList, client).Result; // 1000 users = 33 sec
        //var users = GetUsersInParallelInWithBatches(idList, client).Result; // 1000 users = 2 sec
        //------------------------------------------------------------------------------------

        internal static async Task<User> GetOneUser(UsersClient client)
        {
            var user = await client.GetUser(1);
            return user;
        }

        internal static async Task<IEnumerable<User>> GetUsersSynchrnously(IEnumerable<int> userIds, UsersClient client)
        {
            var users = new List<User>();
            foreach (var id in userIds)
            {
                users.Add(await client.GetUser(id));
            }

            return users;
        }

        internal static async Task<IEnumerable<User>> GetUsersInParallel(IEnumerable<int> userIds, UsersClient client)
        {
            var tasks = userIds.Select(id => client.GetUser(id));
            var users = await Task.WhenAll(tasks);

            return users;
        }

        internal static async Task<IEnumerable<User>> GetUsersInParallelFixed(IEnumerable<int> userIds, UsersClient client)
        {
            var users = new List<User>();
            var batchSize = 100;
            int numberOfBatches = (int)Math.Ceiling((double)userIds.Count() / batchSize);

            for (int i = 0; i < numberOfBatches; i++)
            {
                var currentIds = userIds.Skip(i * batchSize).Take(batchSize);
                var tasks = currentIds.Select(id => client.GetUser(id));
                users.AddRange(await Task.WhenAll(tasks));
            }

            return users;
        }

        internal static async Task<IEnumerable<User>> GetUsersInParallelInWithBatches(IEnumerable<int> userIds, UsersClient client)
        {
            var tasks = new List<Task<IEnumerable<User>>>();
            var batchSize = 100;
            int numberOfBatches = (int)Math.Ceiling((double)userIds.Count() / batchSize);

            for (int i = 0; i < numberOfBatches; i++)
            {
                var currentIds = userIds.Skip(i * batchSize).Take(batchSize);
                tasks.Add(client.GetUsers(currentIds));
            }

            return (await Task.WhenAll(tasks)).SelectMany(u => u);
        }

        internal static List<int> GetIds(int number)
        {
            var idList = new List<int>();

            for(int i = 1; i <= number; i++)
            {
                idList.Add(i);
            }

            return idList;
        }
    }
}
