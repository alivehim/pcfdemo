using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Extensions;
using UtilityTools.Data.Domain;

namespace UtilityTools.Core.Utilites
{
    public static class SettingsHelper
    {
        private static object GetValue(string key, UtilityToolsSetting utilityToolsSetting)
        {
            var type = typeof(UtilityToolsSetting);
            var prop = type.GetProperty(key);

            return prop.GetValue(utilityToolsSetting, null);
        }

        public static object DirectRead(string key, UtilityToolsSetting container)
        {
            return GetValue(key, container);
        }

        public static void DirectWrite(string key, object value, UtilityToolsSetting container)
        {
            var type = typeof(UtilityToolsSetting);
            var prop = type.GetProperty(key);

            if (value is DateTime)
            {
                //container.Values[key] = ((DateTime)value).ToBinary();

                prop.SetValue(container, ((DateTime)value).ToBinary());
            }
            else if (value is Enum)
            {
                //container.Values[key] = ((Enum)value).ToString();

                prop.SetValue(container, ((Enum)value).ToString());
            }
            else if (value is Color)
            {
                //container.Values[key] = ((Color)value).A + ":|:" + ((Color)value).R + ":|:" + ((Color)value).G + ":|:" + ((Color)value).B;
                prop.SetValue(container, ((Color)value).A + ":|:" + ((Color)value).R + ":|:" + ((Color)value).G + ":|:" + ((Color)value).B);
            }
            else
            {
                //container.Values[key] = value;
                prop.SetValue(container, value);
            }
        }

        ///// <summary>
        ///// 读取数组数据
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="subContainer"></param>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //public static Array ReadArraySettings(ApplicationDataContainer subContainer)
        //{
        //    try
        //    {
        //        if (subContainer.Values.ContainsKey("Count"))
        //        {
        //            int i = (int)subContainer.Values["Count"];
        //            var list = Array.CreateInstance(DirectRead("0", subContainer).GetType(), i);
        //            for (int j = 0; j < i; j++)
        //            {
        //                list.SetValue(DirectRead(j.ToString(), subContainer), j);
        //            }
        //            return list;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        public static bool ReadGroupSettings<T>(this IList<UtilityToolsSetting> mainContainer, out T source) where T : new()
        {
            if (mainContainer.IsNullOrEmpty())
            {
                source = default(T);
                return false;
            }

            var type = typeof(T);
            var obj = Activator.CreateInstance(type);
            foreach (var member in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                try
                {
                    var settting = mainContainer.SingleOrDefault(p => p.Key == member.Name);

                    if (settting != null)
                    {
                        if (member.PropertyType == typeof(bool))
                        {
                            member.SetValue(obj, bool.Parse(settting.Value));
                        }
                        else if(member.PropertyType == typeof(int))
                        {
                            member.SetValue(obj, int.Parse(settting.Value));
                        }
                        else
                        {
                            member.SetValue(obj, settting.Value);
                        }
                    }
                    //if (member.PropertyType.IsArray)
                    //{
                    //    var subContainer = mainContainer.CreateContainer(member.Name, ApplicationDataCreateDisposition.Always);
                    //    var res = ReadArraySettings(subContainer);
                    //    if (res == null || res.Length == 0)
                    //    {

                    //    }
                    //    else
                    //    {
                    //        if (member.PropertyType == typeof(DateTime[]))
                    //        {
                    //            List<DateTime> times = new List<DateTime>();
                    //            foreach (var time in res)
                    //            {
                    //                times.Add(DateTime.FromBinary((long)time));
                    //            }
                    //            member.SetValue(obj, times.ToArray());
                    //        }
                    //        else
                    //        {
                    //            member.SetValue(obj, res);
                    //        }
                    //    }
                    //}
                    //else if (member.PropertyType == typeof(DateTime))
                    //{
                    //    var l = (long?)DirectRead(member.Name, mainContainer);
                    //    if (l != null && l != default(long))
                    //    {
                    //        member.SetValue(obj, DateTime.FromBinary((long)l));
                    //    }

                    //}
                    //else if (member.PropertyType == typeof(Color))
                    //{
                    //    var s = (string)DirectRead(member.Name, mainContainer);
                    //    if (!s.IsNullorEmpty())
                    //    {
                    //        var sarray = s.Split(new string[] { ":|:" }, StringSplitOptions.RemoveEmptyEntries);
                    //        member.SetValue(obj, Color.FromArgb(byte.Parse(sarray[0]), byte.Parse(sarray[1]), byte.Parse(sarray[2]), byte.Parse(sarray[3])));
                    //    }
                    //}
                    //// Holy shit! WinRT's type is really different from the legacy type.
                    //else if (member.PropertyType.GetTypeInfo().IsEnum)
                    //{
                    //    var s = (string)DirectRead(member.Name, mainContainer);
                    //    if (s != null)
                    //    {
                    //        member.SetValue(obj, Enum.Parse(member.PropertyType, s));
                    //    }
                    //}
                    //else
                    //{
                    //    var v = DirectRead(member.Name, mainContainer);
                    //    if (v != null)
                    //    {
                    //        member.SetValue(obj, v);
                    //    }
                    //}
                }
                catch (Exception)
                {
                    continue;
                }
            }
            source = (T)obj;
            return true;
        }

        public static void WriteGroupSettings<T>(this UtilityToolsSetting mainContainer, T source) where T : new()
        {
            if (mainContainer == null)
            {
                return;
            }

            var type = typeof(T);
            foreach (var member in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var value = member.GetValue(source);
                //if (value is Array)
                //{
                //    var subContainer = mainContainer.CreateContainer(member.Name, ApplicationDataCreateDisposition.Always);
                //    WriteArraySettings(subContainer, value as Array);
                //}
                //else
                {
                    DirectWrite(member.Name, value, mainContainer);
                }
            }


        }
    }
}
