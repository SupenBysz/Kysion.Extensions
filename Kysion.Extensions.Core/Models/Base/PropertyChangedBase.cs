using Kysion.Extensions.Core.Contracts;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using YamlDotNet.Serialization;

namespace Kysion.Extensions.Core.Models.Base
{
    public static class ExpressionExtensions
    {
        public static string NameForProperty<TDelegate>(this Expression<TDelegate> propertyExpression)
        {
            Expression body;
            var expression = propertyExpression.Body as UnaryExpression;
            if (expression != null)
                body = expression.Operand;
            else
                body = propertyExpression.Body;

            var member = body as MemberExpression;
            if (member == null)
                throw new ArgumentException("属性必须是成员表达式");

            return member.Member.Name;
        }
    }

    public class PropertyChangedBase : INotifyPropertyChanged, INotifyPropertyChangedDispatcher
    {

        private Action<Action> _propertyChangedDispatcher = Execute.DefaultPropertyChangedDispatcher;
        public event PropertyChangedEventHandler? PropertyChanged;

        [XmlIgnore, JsonIgnore, YamlIgnore]
        public virtual Action<Action> PropertyChangedDispatcher
        {
            get { return _propertyChangedDispatcher; }
            set { _propertyChangedDispatcher = value; }
        }

        public virtual void NotifyOfPropertyChange<TProperty>(Expression<Func<TProperty>> property)
        {
            OnPropertyChanged(property.NameForProperty());
        }

        public string GetPropertyName<TProperty>(Expression<Func<TProperty>> property)
        {
            return property.NameForProperty();
        }

        public virtual void NotifyOfPropertyChange([CallerMemberName] string propertyName = "")
        {
            OnPropertyChanged(propertyName);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChangedDispatcher(() =>
                {
                    var handler = PropertyChanged;
                    if (handler != null)
                        handler(this, new PropertyChangedEventArgs(propertyName));
                });
            }
        }

        public virtual bool SetAndNotify(Action callback, [CallerMemberName] string propertyName = "")
        {
            try
            {
                callback();
                NotifyOfPropertyChange(propertyName: propertyName);
            }
            catch (Exception)
            {
                //
            }
            return true;
        }

        //protected virtual bool SetAndNotify<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        //{
        //    if (!EqualityComparer<T>.Default.Equals(field, value))
        //    {
        //        field = value;
        //        this.NotifyOfPropertyChange(propertyName: propertyName);
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        public void Refresh()
        {
            NotifyOfPropertyChange();
        }
    }
}
