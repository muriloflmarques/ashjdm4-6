using System;
using System.Reflection;

namespace Tms.Test
{
    //MSTest for dotnet core does not have PrivateObject implemented yet, so I've created my own
    public class PrivateObject
    {
        private readonly object _obj;

        public PrivateObject(object obj) =>
            this._obj = obj;

        public object Invoke(string methodName, params object[] args)
        {
            var methodInfo = _obj.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance)
                ??
                throw new Exception($"{methodName} not found in class {_obj.GetType()}");

            return methodInfo.Invoke(_obj, args);
        }

        public void SetPrivateProperty(string propertyName, object value)
        {
            var prop = this._obj.GetType().GetProperty(propertyName)
                ??
                throw new Exception($"{propertyName} found in class {_obj.GetType()}");

            if(!prop.CanWrite)
                prop = this._obj.GetType().BaseType.GetProperty(propertyName)
                ??
                throw new Exception($"{propertyName} found in class {_obj.GetType()}");

            prop.SetValue(this._obj, value);
        }
    }
}