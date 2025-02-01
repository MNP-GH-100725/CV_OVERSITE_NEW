using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.Helpers
{



    public class RedisManager
    {
        private readonly IDistributedCache _distributedCache;

        public RedisManager(IDistributedCache distributedCache)
        {

            _distributedCache = distributedCache;

        }

        public RedisManager()
        {
        }

        public void SetCache(RedisManagerDto redisManagerDto)
        {
            _distributedCache.Set(redisManagerDto.key, redisManagerDto.value, redisManagerDto.options);
            
        }

        public void Refresh(string key)
        {
            _distributedCache.Refresh(key);

        }
        public void Remove(string key)
        {
            _distributedCache.Remove(key);
        }

        public bool CheckCache(string key)
        {
            bool Obj = false;
            int i = 0;
            while (i < 5)
            {
                var redisList = _distributedCache.GetString(key);
                if (redisList != null)
                {
                    Obj = true;
                    i = 5;
                }
                else
                {
                    i++;
                }
            }
            return Obj;
        }
        public T GetCache<T>(string key)
        {
            T Obj = Activator.CreateInstance<T>();
            int i = 0;
            while (i < 5)
            {
                var redisList = _distributedCache.GetString(key);

                if (redisList != null)
                {
                    Obj = JsonConvert.DeserializeObject<T>((redisList));
                    i = 5;
                }
                else
                {
                    i++;
                }
            }
            return Obj;
        }
    }
    public class RedisManagerDto {

        public string key { get; set; }
        public byte[] value { get; set; }
        public DistributedCacheEntryOptions options { get; set; }
    }



}
