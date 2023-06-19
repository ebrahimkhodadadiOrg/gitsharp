using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Cache.Redis.Entity;

public abstract class CacheBaseEntity
{
    [RedisIdField]
    [Indexed]
    [IgnoreDataMember]
    public string CacheId { get; set; }

    [Indexed]
    [IgnoreDataMember]
    public int ShopId { get; set; }

    [StringLength(80)]
    [IgnoreDataMember]
    public string? CreatedBy { get; set; }
    [IgnoreDataMember]
    public DateTime? Created { get; set; }

    [StringLength(80)]
    [IgnoreDataMember]
    public string? ModifyBy { get; set; }
    [IgnoreDataMember]
    public DateTime? Modified { get; set; }
}
