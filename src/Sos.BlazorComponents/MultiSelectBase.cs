using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sos.BlazorComponents
{
    public class MultiSelectBase<TItem, TKey> : InputBase<List<TKey>>
    {
        [Parameter]
        public Func<TItem, TKey> Key { get; set; }

        [Parameter]
        public IEnumerable<TItem> Options { get; set; }

        [Parameter]
        public RenderFragment<TItem> ItemTemplate { get; set; }

        protected override bool TryParseValueFromString(string value, out List<TKey> result, out string validationErrorMessage)
        {
            Console.WriteLine("TryParseValueFromString");
            throw new System.NotImplementedException();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        protected bool IsSelected(TItem item)
        {
            var key = Key(item);
            return Value.Contains(key);
        }

        protected void ToggleItem(TItem item)
        {
            var key = Key(item);

            if (!Value.Remove(key))
            {
                Value.Add(key);
            }

            this.Value = Value.ToList();
        }
    }
}
