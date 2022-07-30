using Imkery.Entities;
using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace Imkery.Client.Components.ObjectFormComponent
{
    public abstract class ObjectField<TItem> where TItem : class, IEntity<TItem>, new()
    {
        public ObjectField(string propertyPath, string label)
        {
            PropertyPath = propertyPath;
            Label = label;
        }


        public TItem Item { get; set; }
        public string PropertyPath { get; set; }

        public string Label { get; set; }

        public RenderFragment<TItem> DeviatingView { get; set; }
    }

    public class DeviatingUIField<TItem> : ObjectField<TItem> where TItem : class, IEntity<TItem>, new()
    {
        public DeviatingUIField(RenderFragment<TItem> deviatingView) : base("", "")
        {
            DeviatingView = deviatingView;
        }
    }
    public class ObjectField<TItem, T> : ObjectField<TItem> where TItem : class, IEntity<TItem>, new()
    {
        public ObjectField(string propertyPath, string label) : base(propertyPath, label) 
        {
          
        }

        private PropertyInfo _propertyInfo;
        public T Value
        {
            get
            {
                DetermineProperty();
                return (T)_propertyInfo?.GetValue(Item);
            }
            set
            {
                DetermineProperty();
                _propertyInfo?.SetValue(Item, value);
            }
        }

        private void DetermineProperty()
        {
            var typeOfItem = typeof(TItem);
            _propertyInfo = typeOfItem.GetProperty(PropertyPath);
        }
    }
}
