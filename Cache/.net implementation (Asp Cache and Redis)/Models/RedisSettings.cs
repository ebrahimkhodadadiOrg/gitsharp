namespace Cache.Redis.Models;

public class RedisSettings
{
    /// <summary>
    /// Redis Host Address
    /// </summary>
    public string Host { get; set; }

    /// <summary>
    /// The port where redis is running
    /// </summary>
    public string Port { get; set; }

    /// <summary>
    /// In case the redis is not connected
    /// </summary>
    public bool AbortOnConnectFail { get; set; }

    /// <summary>
    /// If a response is given later than the second that I set in Redis async requests, it will be timeout.
    /// </summary>
    public int AsyncTimeOutMilliSecond { get; set; }

    /// <summary>
    /// Redis In order for it to decrease to timeout if the response is delayed after the second that I set in abnormal requests.
    /// </summary>
    public int ConnectTimeOutMilliSecond { get; set; }
}