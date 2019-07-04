﻿//----------------------------------------------------------------------------------
// <copyright file="ObjectExtensions.cs" company="Prakrishta Technologies">
//     Copyright (c) 2019 Prakrishta Technologies. All rights reserved.
// </copyright>
// <author>Arul Sengottaiyan</author>
// <date>2/9/2019</date>
// <summary>Extension clss that has methods for extending Object behavior</summary>
//-----------------------------------------------------------------------------------

namespace Prakrishta.Infrastructure.Extensions
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;

    /// <summary>
    /// Object extension class
    /// </summary>
    public static class ObjectExtensions
    {
        #region |Methods|

        /// <summary>
        /// Get copy of the object with no reference to original
        /// </summary>
        /// <typeparam name="T">The generic type parameter</typeparam>
        /// <param name="source">The object that is going to copied</param>
        /// <returns>The copy of original object</returns>
        public static T DeepCopy<T>(this T source)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source));
        }

        /// <summary>
        /// Get object Type Code
        /// </summary>
        /// <param name="@value">The source object value</param>
        /// <returns>The <see cref="TypeCode"/></returns>
        public static TypeCode GetTypeCode(this object @value)
        {
            if (value != null)
            {
                return Type.GetTypeCode(value.GetType());
            }

            return TypeCode.Empty;
        }

        /// <summary>
        /// The method does conversion to specified type, returns default value if the value is null or empty
        /// </summary>
        /// <typeparam name="T">The generic type parameter</typeparam>
        /// <param name="@value">The source object</param>
        /// <param name="defaultValue">The defaultValue of the type</param>
        /// <returns>The result <see cref="T"/></returns>
        public static T GetValue<T>(this object @value, T defaultValue = default(T))
        {
            T result = defaultValue;

            try
            {
                if (value.IsNullOrDBNull())
                {
                    return result;
                }

                Type conType = typeof(T);

                if (value is string)
                {
                    if (!string.IsNullOrEmpty(value.ToString()))
                    {
                        result = (T)Convert.ChangeType(value, conType, CultureInfo.CurrentCulture);
                    }
                }
                else
                {
                    result = (T)Convert.ChangeType(value, conType, CultureInfo.CurrentCulture);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// The method does conversion to specified type if the given value is not empty
        /// </summary>
        /// <typeparam name="T">The generic type parameter</typeparam>
        /// <param name="@value">The source object</param>
        /// <param name="defaultValue">The defaultValue of the type</param>
        /// <returns>The result <see cref="T"/></returns>
        public static T? GetValueOrNull<T>(this object @value) where T : struct
        {
            T? result = new T?();

            try
            {
                if (value.IsNullOrDBNull())
                {
                    return result;
                }

                Type conType = typeof(T);

                if (value is string)
                {
                    if (!string.IsNullOrEmpty(value.ToString()))
                    {
                        result = (T)Convert.ChangeType(value, conType, CultureInfo.CurrentCulture);
                    }
                }
                else
                {
                    result = (T)Convert.ChangeType(value, conType, CultureInfo.CurrentCulture);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// if the object of this method is called on is not null, runs the given function and returns the value.
        /// if the object is null, returns default(TResult)
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="target">Source object</param>
        /// <param name="getValue">Delegate method to be executed if not null</param>
        /// <returns>The <see cref="TResult"/></returns>
        public static TResult IfNotNull<TSource, TResult>(this TSource target, Func<TSource, TResult> getValue)
        {
            if (target != null)
                return getValue(target);
            else
                return default(TResult);
        }

        /// <summary>
        /// The "IN" clause similar to SQL in clause
        /// </summary>
        /// <typeparam name="T">The generic type</typeparam>
        /// <param name="source">The original source</param>
        /// <param name="valueList">The collection of values</param>
        /// <returns>True if any parameter value present in collection otherwise false</returns>
        public static bool In<T>(this T source, IEnumerable<T> valueList)
        {
            return valueList.Contains(source);
        }

        /// <summary>
        /// The "IN" clause similar to SQL in clause
        /// </summary>
        /// <typeparam name="T">The generic type</typeparam>
        /// <param name="source">The original source</param>
        /// <param name="values">The collection of values</param>
        /// <returns>True if any parameter value present in collection otherwise false</returns>
        public static bool In<T>(this T source, params T[] values)
        {
            return values.Contains(source);
        }

        /// <summary>
        /// when dealing with databases, a new null type shows up, the DBNull. 
        /// This extention method detects it along with the null
        /// </summary>
        /// <param name="value">Object value</param>
        /// <returns>Indicates if object is null or DBNull</returns>
        public static bool IsNullOrDBNull(this object value)
        {
            if (value == null || value.GetType() == typeof(DBNull))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check if object is numeric type
        /// </summary>
        /// <param name="value">Object value</param>
        /// <returns>Indicates if the object is numeric type</returns>
        public static bool IsNumber(this object value)
        {
            return value is sbyte
                    || value is byte
                    || value is short
                    || value is ushort
                    || value is int
                    || value is uint
                    || value is long
                    || value is ulong
                    || value is float
                    || value is double
                    || value is decimal;
        }

        /// <summary>
        /// Converts object value to specified type
        /// </summary>
        /// <param name="@value">The source object</param>
        /// <param name="type">The target type</param>
        /// <returns>Converted type</returns>
        public static object To(this object @value, Type type)
        {
            if (!value.IsNullOrDBNull())
            {
                Type targetType = type;

                if (value.GetType() == targetType)
                {
                    return value;
                }

                TypeConverter converter = TypeDescriptor.GetConverter(value);
                if (converter != null)
                {
                    if (converter.CanConvertTo(targetType))
                    {
                        return converter.ConvertTo(value, targetType);
                    }
                }

                converter = TypeDescriptor.GetConverter(targetType);
                if (converter != null)
                {
                    if (converter.CanConvertFrom(value.GetType()))
                    {
                        return converter.ConvertFrom(value);
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Converts object value to specified type
        /// </summary>
        /// <param name="@value">The source object</param>
        /// <param name="type">The target type</param>
        /// <returns>Converted type</returns>
        public static T To<T>(this object @value, T defaultValue = default(T)) 
        {
            if (!value.IsNullOrDBNull())
            {
                Type targetType = typeof(T);

                if (value.GetType() == targetType)
                {
                    return (T)value;
                }

                TypeConverter converter = TypeDescriptor.GetConverter(value);
                if (converter != null)
                {
                    if (converter.CanConvertTo(targetType))
                    {
                        return (T)converter.ConvertTo(value, targetType);
                    }
                }

                converter = TypeDescriptor.GetConverter(targetType);
                if (converter != null)
                {
                    if (converter.CanConvertFrom(value.GetType()))
                    {
                        return (T)converter.ConvertFrom(value);
                    }
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Serialize an Object To an XML Document
        /// </summary>
        /// <param name="@value">The value<see cref="object"/></param>
        /// <param name="fileName">The path of the Destination XML File</param>
        public static void WriteToXML(this object @value, string fileName)
        {
            using (TextWriter writer = new StreamWriter(fileName))
            {
                XmlSerializer serializer = new XmlSerializer(value.GetType());
                serializer.Serialize(writer, value);
            }
        }

        #endregion
    }
}
