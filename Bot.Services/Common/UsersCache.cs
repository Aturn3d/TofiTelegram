using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Services.Common
{
    internal class UsersCache
    {
        //private MemoryCache cache;
        //public UsersCache()
        //{
        //    cache = MemoryCache.Default;
        //}

        public static CachedUser GetOrAdd(int id)
        {
            var cache = MemoryCache.Default;
            var key = id.ToString();
            var user = (CachedUser)cache.Get(key);
            if (user == null){
                user = new CachedUser { IsProcessing = false };
                cache.Add(key, user, DateTimeOffset.Now.AddMinutes(10));
            }
            return user;
        }
    }
    internal class CachedUser
    {
        public bool IsProcessing { get; set; }
    }
}
