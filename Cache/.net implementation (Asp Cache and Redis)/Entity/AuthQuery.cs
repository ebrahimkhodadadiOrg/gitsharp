using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache.Redis.Entity
{
    [Document(StorageType = StorageType.Json, Prefixes = new[] { "AuthQuery" })]
    public class AuthQuery : CacheBaseEntity
    {
        public int Id { get; set; }
        public string ShopName { get; set; }
        [Indexed]
        [Searchable]
        public string AccessAddress { get; set; }
        public string ThemeName { get; set; }
        public string Title { get; set; }
        public bool IsEnable { get; set; }
        public bool IsExpired { get; set; }
        public DateTime CreateDate { get; set; }
        public string Name { get; set; }
        public string Tell { get; set; }
        public string Email { get; set; }
        {
    }
}
