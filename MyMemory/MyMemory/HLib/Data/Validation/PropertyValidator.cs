

namespace HLib.Data.Validation
{

    using System.Collections.Generic;
    using System.Threading.Tasks;


    public class PropertyValidator
    {

        private readonly object _owner;
        private readonly Dictionary<string, List<IValidator>> _propertyValidators;


        public PropertyValidator(object owner)
        {
            _owner = owner;
            _propertyValidators = new Dictionary<string, List<IValidator>>();
        }


        public string Validate(string propertyName)
        {
            if (!_propertyValidators.ContainsKey(propertyName)) return string.Empty;

            var validators = _propertyValidators[propertyName];
            var result = string.Empty;

            Parallel.ForEach(
                validators,
                (validator, state) =>
                    {
                    if (!validator.Validate(GetValue(propertyName)))
                    {
                        result = validator.Message;
                        state.Break();
                    }
                });

            return result;
        }


        public void AddValidator(string propertyName, IValidator validator)
        {
            if (!_propertyValidators.ContainsKey(propertyName))
            {
                _propertyValidators.Add(propertyName, new List<IValidator>());
            }

            var list = _propertyValidators[propertyName];
            list.Add(validator);
        }


        private object GetValue(string propertyName)
        {
            return _owner.GetType().GetProperty(propertyName).GetValue(_owner, null);
        }
    }
}
