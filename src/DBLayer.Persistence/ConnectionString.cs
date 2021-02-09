using DBLayer.Core;
using DBLayer.Core.Interface;
using DBLayer.Core.Utilities;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace DBLayer.Persistence
{
    public class ConnectionString: IConnectionString
    {
        public ConnectionString(IDictionary<string,string> properties,string connectionToken)
        {
            this.Properties = properties ?? new Dictionary<string, string>();
            this.ConnectionToken = connectionToken;
            _connectionString = ParsePropertyTokens(ConnectionToken, Properties);

        }
        private IDictionary<string, string> _properties;

        public IDictionary<string, string> Properties {
            get {
                return _properties;
            }
            private set
            {
                if (value.ContainsKey(PropertyConstants.PASSWORDKEY))
                {
                    var passwordKey = value[PropertyConstants.PASSWORDKEY];
                    if (!string.IsNullOrEmpty(passwordKey))
                    {
                        var password = value[PropertyConstants.PASSWORD];
                        //解密
                        value[PropertyConstants.PASSWORD] = AES.Decode(password, passwordKey);
                    }
                }
                _properties = value;
            }
        }

        public string ConnectionToken { get; }


        private string _connectionString;
        public string ConnectionValue
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_connectionString))
                {
                    _connectionString = ParsePropertyTokens(ConnectionToken, Properties);
                }
                return _connectionString;
            }
        }

        /// <summary>
        /// Replace properties by their values in the given string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        private string ParsePropertyTokens(string str, IDictionary<string, string> properties)
        {
            var OPEN = "${";
            var CLOSE = "}";

            
            if (string.IsNullOrWhiteSpace(str) || (properties?.Count??0)<=0)
            {
                return str;
            }

            var newString = str;
            var start = newString.IndexOf(OPEN);
            var end = newString.IndexOf(CLOSE);

            while (start > -1 && end > start)
            {
                var prepend = newString.Substring(0, start);
                var append = newString.Substring(end + CLOSE.Length);

                var index = start + OPEN.Length;
                var propName = newString.Substring(index, end - index);
                var propValue = properties[propName];
                if (propValue == null)
                {
                    newString = prepend + propName + append;
                }
                else
                {
                    newString = prepend + propValue + append;
                }
                start = newString.IndexOf(OPEN);
                end = newString.IndexOf(CLOSE);
            }
            return newString;
        }

        public override string ToString()
        {
            if(string.IsNullOrWhiteSpace(_connectionString))
            {
                _connectionString = ParsePropertyTokens(ConnectionToken, Properties);
            }
            return _connectionString;
        }
    }
}
