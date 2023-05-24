using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Framework.Exceptions;

namespace Framework.CommonUtility
{
    public enum LinkListType { Pages = 0, Collections = 1, Article = 2, News = 3, Product = 4 }

    public static class CommonUtility
    {
        public static void GetIntegerConfigurations(this IConfiguration configuration, string name, int defaultValue, out int value)
        {
            _ = int.TryParse(configuration[name], out value);
            if (value == 0)
                value = defaultValue;
        }
        public static string ConfigUserName(string userName, string emailDomain)
        {
            string result = "";
            if (CheckIsPhoneNumber(userName))
                result = userName + emailDomain;
            else if (CheckIsEmail(userName))
                result = userName;

            return result;
        }
        public static bool CheckIsPhoneNumber(string phoneNumber)
        {
            Regex regex = new Regex("^(?=.*\\d)[0-9]*$");
            return regex.IsMatch(phoneNumber);
        }
        public static bool CheckIsEmail(string email)
        {
            Regex regex = new Regex("^([a-zA-Z0-9_\\-\\.]+)@([a-zA-Z0-9_\\-\\.]+)\\.([a-zA-Z]{2,5})$");
            return regex.IsMatch(email);
        }
        public static JsonSerializerSettings GetSettingJsonCamelCase()
        {
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            return new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            };
        }

        public static bool ValidateJSON(string s)
        {
            try
            {
                JToken.Parse(s);
                return true;
            }
            catch (JsonReaderException)
            {
                return false;
            }
        }
        public static T deepClone<T>(T obj)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Newtonsoft.Json.JsonConvert.SerializeObject(obj));
        }
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }
        public static string GetLinkListUrl(LinkListType type, int Id, string Slug)
        {
            if (string.IsNullOrEmpty(Slug))
                return "/notfound";
            switch (type)
            {
                case LinkListType.Pages:
                    break;
                case LinkListType.Collections:
                    return "/collection/" + Slug;
                case LinkListType.Article:
                    return "/article/" + Slug;
                case LinkListType.News:
                    return "/news/" + Slug;
                case LinkListType.Product:
                    return "/product/" + Slug;
                default:
                    return "/notfound";
            }
            return "/notfound";
        }
        public static string CreateNewOTP(int digitLength)
        {
            string _start = "1", _end = "9";
            for (int i = 1; i < digitLength; i++)
            {
                _start += "0";
                _end += "9";
            }

            long.TryParse(_start, out long start);
            long.TryParse(_end, out long end);

            var rand = new Random();
            string newTemp = rand.NextInt64(start, end).ToString();

            if (newTemp.Length != digitLength)
                throw new RestException(System.Net.HttpStatusCode.InternalServerError, "OTP Error");

            return newTemp;
        }
    }
}
