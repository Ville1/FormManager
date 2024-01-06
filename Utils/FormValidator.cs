using FormManager.Data.HttpData.Response;
using System.Reflection;

namespace FormManager.Utils
{
    public class FormValidator
    {
        private object data;
        private PropertyInfo[] properties;

        public ValidationErrorResponse Response { get; set; }

        public FormValidator(object requestData)
        {
            data = requestData;
            properties = requestData.GetType().GetProperties();
            Response = new ValidationErrorResponse();
        }

        public void IsRequired<PropertyType>(string propertyName)
        {
            PropertyInfo propertyInfo = GetPropertyInfo<PropertyType>(propertyName);
            object? value = propertyInfo.GetValue(data);
            if(
                value == null ||//Null
                typeof(PropertyType) == typeof(string) && (string)value == string.Empty ||//Empty string
                typeof(PropertyType) == typeof(Guid) && (Guid)value == Guid.Empty//Empty guid
            ) {
                AddError(propertyName, Resources.Localization.MissingRequiredData);
            }
        }

        public void InList<PropertyType>(string propertyName, IEnumerable<PropertyType> validValues)
        {
            PropertyInfo propertyInfo = GetPropertyInfo<PropertyType>(propertyName);
            object? value = propertyInfo.GetValue(data);
            if(value != null && !validValues.Contains((PropertyType)value)) {
                AddError(propertyName, Resources.Localization.InvalidValueSelected);
            }
        }

        private void AddError(string propertyName, string errorMessage)
        {
            propertyName = propertyName.Uncapitalize();
            if (!Response.Errors.ContainsKey(propertyName)) {
                Response.Errors.Add(propertyName, new List<string>() { errorMessage });
            } else if (!Response.Errors[propertyName].Contains(errorMessage)) {
                Response.Errors[propertyName].Add(errorMessage);
            }
        }

        private PropertyInfo GetPropertyInfo<PropertyType>(string propertyName)
        {
            PropertyInfo? propertyInfo = properties.FirstOrDefault(p => p.Name == propertyName);
            if(propertyInfo == null || !propertyInfo.CanRead) {
                throw new Exception(string.Format("Request type \"{0}\" does not have accessable property \"{1}\"", data.GetType().Name, propertyName));
            }
            if(propertyInfo.PropertyType != typeof(PropertyType)) {
                throw new Exception(string.Format("Request property \"{0}.{1}\" does not have type of \"{1}\"", data.GetType().Name, propertyName, typeof(PropertyType).Name));
            }
            return propertyInfo;
        }
    }
}
