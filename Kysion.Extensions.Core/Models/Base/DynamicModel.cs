using System.Dynamic;

namespace Kysion.Extensions.Core.Models.Base
{
    public class DynamicModel : DynamicObject
    {
        public Dictionary<string, object?> dicProperty { get; set; } = new();

        public int Count
        {
            get
            {
                return dicProperty.Count;
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            // Converting the property name to lowercase
            // so that property names become case-insensitive.

            // If the property name is found in a dictionary,
            // set the result parameter to the property value and return true.
            // Otherwise, return false.
            if (!dicProperty.ContainsKey(binder.Name))
            {
                dicProperty.Add(binder.Name, null);
            }

            return dicProperty.TryGetValue(binder.Name, out result!);
        }

        public override bool TrySetMember(SetMemberBinder binder, object? value)
        {
            // Converting the property name to lowercase
            // so that property names become case-insensitive.

            if (!dicProperty.Keys.Contains(binder.Name))
            {
                dicProperty.Add(binder.Name, value);
            }
            else if (dicProperty[binder.Name] != null) { }
            {
                dicProperty[binder.Name] = value;
            }
            // You can always add a value to a dictionary,
            // so this method always returns true.
            return true;
        }
    }
}
