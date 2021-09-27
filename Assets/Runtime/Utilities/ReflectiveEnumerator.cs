using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class ReflectiveEnumerator
{
    #region public-method
    public static IEnumerable<Type> GetEnumerableOfType<T>(params object[] constructorArgs)
    {
        var objects = new List<Type>();
        foreach (var type in
            Assembly.GetAssembly(typeof(T)).GetTypes()
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
        {
            objects.Add(type);
        }
        return objects;
    }
    #endregion public-method
}