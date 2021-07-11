using DBLayer.Core;
using DBLayer.Core.Interface;
using DBLayer.Core.Logging;
using DBLayer.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
/*------------------------------------------------------------------------------
* 单元名称：数据库层--数据处理扩展类
* 单元描述： 
* 创 建 人：吴涛
* 创建日期： 
* 修改日志
* 修 改 人   修改日期    修改内容
* 
* ----------------------------------------------------------------------------*/
namespace DBLayer.Persistence
{
    public static class DALExtensions
    {
        private static readonly Func<Action<LogLevel, string, Exception>> Logger = () => LogManager.CreateLogger(typeof(DALExtensions));
        public static IEnumerable<T> ReadEnumerable<T>(this IDataReader reader, Func<IDataReader, T> map)
        {
            while (reader.Read())
            {
                yield return map(reader);
            }
        }

        public static IEnumerable<T> ReadList<T>(this IDataReader reader, Func<IDataReader, T> map)
        {
            try
            {
                var collection = new List<T>(reader.ReadEnumerable(map));
                return collection;
            }
            catch (Exception ex)
            {
                if (reader != null && !reader.IsClosed) { reader.Close(); }
                throw ex;
            }
        }

        public static IEnumerable<T> ReadList<T>(this IDataReader reader, Func<IDataReader, T> map, ref int totalCount)
        {
            try
            {
                var collection =new List<T>(reader.ReadEnumerable(map));
                reader.NextResult();
                reader.Read();
                totalCount = reader.ReadValue<int>("TotalRecords");

                return collection;
            }
            catch (Exception ex)
            {
                if (reader != null && !reader.IsClosed) { reader.Close(); }
                throw ex;
            }

        }


        /// <summary>
        /// read self sql to dictionary<string, object>
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="logger"></param>
        /// <param name="isInclusion"></param>
        /// <param name="extendList"></param>
        /// <returns></returns>
        public static IDictionary<string, object> ReadSelf(this IDataReader reader, params string[] inclusionList)
        {
            try
            {
                var item = new Dictionary<string, object>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var name = reader.GetName(i);
                    //不可读
                    if (inclusionList?.Count()>0 && inclusionList.IsExcluded(name))
                    {
                        continue;
                    }

                    try
                    {
                        item.Add(name, reader.GetValue(i));
                    }
                    catch (Exception ex)
                    {
                        Logger().LogWarning(ex, ex.GetDetailMessage());
                        continue;
                    }
                }
                return item;
            }
            catch (Exception ex)
            {
                Logger().LogError(ex, ex.GetDetailMessage());
                if (reader != null && !reader.IsClosed) { reader.Close(); }
                throw ex;
            }

        }
        /// <summary>
        /// read self sql to dictionary<string, string>
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="logger"></param>
        /// <param name="inclusionList"></param>
        /// <returns></returns>
        public static IDictionary<string, string> ReadSelfString(this IDataReader reader, params string[] inclusionList)
        {
            try
            {
                var item = new Dictionary<string, string>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var name = reader.GetName(i);
                    //不可读
                    if ((inclusionList?.Count()>0 && inclusionList.IsExcluded(name)))
                    {
                        continue;
                    }

                    try
                    {
                        item.Add(name, (reader.GetValue(i) is DBNull ? "" : reader.GetValue(i).ToString()));
                    }
                    catch (Exception ex)
                    {
                        Logger().LogWarning(ex, ex.GetDetailMessage());
                        continue;
                    }
                }
                return item;
            }
            catch (Exception ex)
            {
                Logger().LogError(ex, ex.GetDetailMessage());
                if (reader != null && !reader.IsClosed) { reader.Close(); }
                throw ex;
            }
        }

        public static T ReadObject<T>(this IDataReader reader, params string[] inclusionList)
            where T : new()
        {
            try
            {
                var item = new T();
                var entityType = item.GetType();
                //var propertyInfos = entityType.GetProperties();

                var propertyInfos = entityType.GetCachedProperties();

                foreach (var property in propertyInfos)
                {
                    //不可读
                    if (!property.Key.CanRead
                        || !property.Key.CanWrite
                        || (inclusionList?.Count()>0 && inclusionList.IsExcluded(property.Key.Name)))
                    {
                        continue;
                    }

                    // We need to catch this exception in cases when we're upgrading and the column might not exist yet.
                    // It'd be nice to have a cleaner way of doing this.
                    try
                    {
                        var fieldName = property.Key.GetFieldName();

                        object value = null;
                        if (reader.ReaderExists(fieldName)) 
                        {
                            value = reader[fieldName];
                        } 
                        else if (reader.ReaderExists(property.Key.Name))
                        {
                            value = reader[property.Key.Name];
                        }

                        if (value != null && !(value is DBNull))
                        {
                            if (property.Key.PropertyType != typeof(Uri))
                            {
                                property.Value.Setter(item, value);
                                //if (property.Key.PropertyType == value.GetType())
                                //{
                                //    property.SetValue(item, value, null);
                                //}
                                //else
                                //{
                                //    property.SetValue(item, value.ChangeType(property.PropertyType), null);
                                //}
                            }
                            else
                            {
                                var url = value as string;
                                if (!string.IsNullOrEmpty(url))
                                {
                                    if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out Uri uri))
                                    {
                                        property.Value.Setter(item, uri);
                                        //property.SetValue(item, uri, null);
                                    }
                                }
                            }
                        }
                    }
                    catch (IndexOutOfRangeException ex)
                    {
                        Logger().LogWarning(ex, ex.GetDetailMessage());
                    }
                }

                return item;
            }
            catch (Exception ex)
            {
                Logger().LogError(ex, ex.GetDetailMessage());
                if (reader != null && !reader.IsClosed) { reader.Close(); }
                throw ex;
            }
        }

        public static T ReadValue<T>(this IDataReader reader, string columnName)
        {
            try
            {
                return reader.ReadValue<T>(columnName, default);
            }
            catch (Exception ex)
            {
                if (reader != null && !reader.IsClosed) { reader.Close(); }
                throw ex;
            }
            
        }

        public static T ReadValue<T>(this IDataReader reader, string columnName, T defaultValue)
        {
            try
            {
                return reader.ReadValue(columnName, value =>(T)value.ChangeType(typeof(T)), defaultValue);
            }
            catch (Exception ex)
            {
                if (reader != null && !reader.IsClosed) { reader.Close(); }
                throw ex;
            }
            
        }

        public static T ReadValue<T>(this IDataReader reader, string columnName, Func<object, T> map, T defaultValue)
        {
            try
            {
                var value = reader[columnName];
                if (value != null && value != DBNull.Value)
                {
                    return map(value);
                }
                return defaultValue;
            }
            catch (FormatException)
            {
                return defaultValue;
            }
            catch (IndexOutOfRangeException)
            {
                return defaultValue;
            }
            catch (Exception ex) 
            {
                if (reader != null && !reader.IsClosed) { reader.Close(); }
                throw ex;
            }
        }

        /// <summary>
        /// SqlDataReader对象是否包含此字段
        /// </summary>
        /// <param name="dataReader">SqlDataReader实例对象</param>
        /// <param name="columnName">字段名称</param>
        /// <returns></returns>
        private static bool ReaderExists(this IDataReader dataReader, string columnName)
        {
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                if (dataReader.GetName(i).Equals(columnName,StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                };
            }
            return false;
        }

        public static StringBuilder CreateAllEntityDicSql<T>(this IDataSource dataSource,string[] inclusionList, string prex = "")
        {
            var sqlFields = new StringBuilder();
            if ((inclusionList?.Length ?? 0) <= 0)
            {
                sqlFields.Append("*");
                return sqlFields;
            }

            var entityType = typeof(T);
            //var propertyInfos = entityType.GetProperties();
            var propertyInfos = entityType.GetCachedProperties();
            var prexAppend = string.IsNullOrEmpty(prex) ? "" : (prex + ".");
            foreach (var property in propertyInfos)
            {
                //不可读
                if (!property.Key.CanRead || !property.Key.CanWrite || (inclusionList?.Count()>0 && inclusionList.IsExcluded(property.Key.Name)))
                {
                    continue;
                }

                var fieldName = property.Key.GetFieldName();

                sqlFields.Append(prexAppend);
                sqlFields.AppendFormat(dataSource.DbFactory.DbProvider.FieldFormat, fieldName);
                sqlFields.Append(" ");
                sqlFields.AppendFormat(dataSource.DbFactory.DbProvider.FieldFormat, property.Key.Name);
                sqlFields.Append(",");
            }

            if (sqlFields.Length > 0)
            {
                sqlFields.Length -= 1;
            }
            return sqlFields;
        }
    } //end class
}// end namespace
