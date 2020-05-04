using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        [Parameter]
        public EventCallback<IEnumerable<TKey>> Changed { get; set; }

        protected override bool TryParseValueFromString(string value, out List<TKey> result, out string validationErrorMessage)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnParametersSet()
        {
            if (ItemTemplate == null)
            {
                throw new Exception("Missing required ItemTemplate");
            }
        }

        protected bool IsSelected(TItem item)
        {
            var key = Key(item);
            return CurrentValue.Contains(key);
        }

        protected async Task ToggleItem(TItem item)
        {
            var key = Key(item);

            if (!Value.Remove(key))
            {
                Value.Add(key);
            }

            this.CurrentValue = Value.ToList();
            await Changed.InvokeAsync(this.CurrentValue);
        }
    }
}
