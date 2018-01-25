
namespace HLib.Data.LazyGetter
{

    using System;


    public class ReflectionPropertyValueProvider<T> : IPropertyValueProvider<T> where T : class
    {

        private readonly Type _propertyType;
        private readonly object _propertyOwner;
        private readonly string _propertyName;


        public ReflectionPropertyValueProvider(object owner, string propertyName)
        {
            _propertyOwner = owner;
            _propertyName = propertyName;
        }


        public ReflectionPropertyValueProvider(Type ownerType, string propertyName)
        {
            _propertyType = ownerType;
            _propertyName = propertyName;
        }


        public T Value
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_propertyName))
                {
                    if (_propertyOwner != null)
                    {
                        return _propertyOwner.GetType().GetProperty(_propertyName).GetValue(_propertyOwner, null) as T;
                    }

                    if (_propertyType != null)
                    {
                        return _propertyType.GetProperty(_propertyName).GetValue(null, null) as T;
                    }
                }

                return null;
            }
        }
    }
}
