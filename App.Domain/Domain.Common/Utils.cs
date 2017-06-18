using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace App.Domain.Common
{
	public static class Utils
	{
        public static IEnumerable<FieldInfo> GetConstants(this Type type)
        {
            IEnumerable<FieldInfo> fields =
                from fi in (IEnumerable<FieldInfo>)type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy)
                where (!fi.IsLiteral ? false : !fi.IsInitOnly)
                select fi;
            return fields;
        }

        public static IEnumerable<T> GetConstantsValues<T>(this Type type)
        where T : class
        {
            IEnumerable<T> constants =
                from fi in type.GetConstants()

                select(T)(fi.GetRawConstantValue() as T);
            return constants;
        }
    }
}