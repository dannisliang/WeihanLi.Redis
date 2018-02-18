﻿// ReSharper disable once CheckNamespace
using System;
using System.Threading.Tasks;
using StackExchange.Redis;
using WeihanLi.Common.Helpers;
using WeihanLi.Extensions;

namespace WeihanLi.Redis
{
    public interface IHashClient
    {
        #region Expire

        bool Expire(string key, TimeSpan? expiresIn);

        Task<bool> ExpireAsync(string key, TimeSpan? expiresIn);

        #endregion Expire

        #region Exists

        bool Exists(string key, string fieldName);

        Task<bool> ExistsAsync(string key, string fieldName);

        #endregion Exists

        #region Get

        string Get(string key, string fieldName);

        Task<string> GetAsync(string key, string fieldName);

        T Get<T>(string key, string fieldName);

        Task<T> GetAsync<T>(string key, string fieldName);

        #endregion Get

        #region Set

        bool Set<T>(string key, string fieldName, T value);

        bool Set<T>(string key, string fieldName, T value, When when);

        bool Set<T>(string key, string fieldName, T value, When when, CommandFlags commandFlags);

        Task<bool> SetAsync<T>(string key, string fieldName, T value);

        Task<bool> SetAsync<T>(string key, string fieldName, T value, When when);

        Task<bool> SetAsync<T>(string key, string fieldName, T value, When when, CommandFlags commandFlags);

        #endregion Set

        #region Remove

        bool Remove(string key, string fieldName);

        Task<bool> RemoveAsync(string key, string fieldName);

        #endregion Remove
    }

    /// <summary>
    /// https://github.com/antirez/redis/issues/167
    /// Hash does not support expire on field
    /// </summary>
    internal class HashClient : BaseRedisClient, IHashClient
    {
        public HashClient() : base(LogHelper.GetLogHelper<HashClient>(), new RedisWrapper("Hash/"))
        {
        }

        public bool Exists(string key, string fieldName) => Wrapper.Database.HashExists(key, fieldName);

        public Task<bool> ExistsAsync(string key, string fieldName) => Wrapper.Database.HashExistsAsync(key, fieldName);

        public bool Expire(string key, TimeSpan? expiresIn)
        {
            return Wrapper.Database.KeyExpire(key, expiresIn);
        }

        public Task<bool> ExpireAsync(string key, TimeSpan? expiresIn)
        {
            return Wrapper.Database.KeyExpireAsync(key, expiresIn);
        }

        public string Get(string key, string fieldName) => Wrapper.Database.HashGet(key, fieldName);

        public T Get<T>(string key, string fieldName) => Wrapper.Wrap<T>(() => Wrapper.Database.HashGet(key, fieldName));

        public Task<string> GetAsync(string key, string fieldName) => Wrapper.WrapAsync<string>(() => Wrapper.Database.HashGetAsync(key, fieldName));

        public Task<T> GetAsync<T>(string key, string fieldName) => Wrapper.WrapAsync<T>(() => Wrapper.Database.HashGetAsync(key, fieldName));

        public bool Remove(string key, string fieldName) => Wrapper.Database.HashDelete(key, fieldName);

        public Task<bool> RemoveAsync(string key, string fieldName) => Wrapper.Database.HashDeleteAsync(key, fieldName);

        public bool Set<T>(string key, string fieldName, T value) => Wrapper.Database.HashSet(key, fieldName, value.ToJsonOrString());

        public bool Set<T>(string key, string fieldName, T value, When when) => Wrapper.Database.HashSet(key, fieldName, value.ToJsonOrString(), when);

        public bool Set<T>(string key, string fieldName, T value, When when, CommandFlags commandFlags) => Wrapper.Database.HashSet(key, fieldName, value.ToJsonOrString(), when, commandFlags);

        public Task<bool> SetAsync<T>(string key, string fieldName, T value) => Wrapper.Database.HashSetAsync(key, fieldName, value.ToJsonOrString());

        public Task<bool> SetAsync<T>(string key, string fieldName, T value, When when) => Wrapper.Database.HashSetAsync(key, fieldName, value.ToJsonOrString(), when);

        public Task<bool> SetAsync<T>(string key, string fieldName, T value, When when, CommandFlags commandFlags) => Wrapper.Database.HashSetAsync(key, fieldName, value.ToJsonOrString(), when, commandFlags);
    }
}