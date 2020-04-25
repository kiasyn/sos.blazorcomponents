using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Sos.BlazorComponents
{
    public enum DatePickerView
    {
        Days,
        Months,
        Decades,
    }
    public class DatePickerBase : InputBase<DateTime?>
    {
        [Parameter]
        public DatePickerView View { get; set; } = DatePickerView.Days;
        [Parameter]
        public DayOfWeek StartDayOfWeek { get; set; } = DayOfWeek.Wednesday;
        [Parameter]
        public DateTime? StartDate { get; set; }
        [Parameter]
        public DateTime? EndDate { get; set; }
        [Parameter]
        public DateTime ViewDate { get; set; } = DateTime.UtcNow;
        [Parameter]
        public string DateFormat { get; set; } = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
        [Parameter]
        public bool CloseOnSelect { get; set; } = true;
        [Parameter]
        public Func<DateTime, bool> DateDisabledCallback { get; set; }

        private string _inputValue;
        public string InputValue
        {
            get => _inputValue; set
            {
                _inputValue = value;
                InputValueChanged();
            }
        }

        public IEnumerable<DayOfWeek> WeekDays => GetWeekDays(StartDayOfWeek).ToList();
        public DateTime StartOfMonth => ViewDate.StartOfMonth();
        public DateTime EndOfMonth => ViewDate.EndOfMonth();
        public DateTime DayViewStartDate => StartOfMonth.StartOfWeek(this.StartDayOfWeek);
        public DateTime DayViewEndDate => GetDayViewEndDate();

        private DateTime GetDayViewEndDate()
        {
            var dayViewEndDate = EndOfMonth.EndOfWeek((DayOfWeek)(((int)this.StartDayOfWeek + 7) % 7));
            var weeks = (dayViewEndDate - DayViewStartDate).TotalDays / 7;
            dayViewEndDate = dayViewEndDate.AddDays((6 - weeks) * 7);
            return dayViewEndDate;
        }

        public DateTime MonthViewStartDate => ViewDate.StartOfYear();
        public DateTime MonthViewEndDate => ViewDate.EndOfYear();
        public DateTime StartOfDecade => ViewDate.StartOfDecade();
        public DateTime EndOfDecade => ViewDate.EndOfDecade();
        public DateTime DecadeViewStartDate => StartOfDecade.SafeAddYears(-1);
        public DateTime DecadeViewEndDate => EndOfDecade.SafeAddYears(1);
        public DateTime StartOfCentury => ViewDate.StartOfCentury();
        public DateTime EndOfCentury => ViewDate.EndOfCentury();
        public DateTime StartOfMillenium => ViewDate.StartOfMillenium();
        public DateTime EndOfMillenium => ViewDate.EndOfMillenium();
        public DateTime CenturyViewStartDate => StartOfMillenium.SafeAddYears(-100);
        public DateTime CenturyViewEndDate => EndOfMillenium.SafeAddYears(100);
        public bool Expanded { get; set; }

        protected override void OnInitialized()
        {
            this.InputValue = Value?.ToString(DateFormat);
        }

        protected override bool TryParseValueFromString(string value, out DateTime? result, out string validationErrorMessage)
        {
            Console.WriteLine($"TryParseValueFromString: {value}");
            validationErrorMessage = null;
            var parsed = DateTime.TryParseExact(value, DateFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out var dt);
            result = parsed ? (DateTime?)dt : null;
            return parsed;
        }

        protected override string FormatValueAsString(DateTime? value)
        {
            return value?.ToString(DateFormat);
        }

        protected void Next()
        {
            ViewDate = this.View switch
            {
                DatePickerView.Decades => ViewDate.SafeAddYears(100),
                DatePickerView.Months => ViewDate.SafeAddYears(1),
                DatePickerView.Days => ViewDate.AddMonths(1),
                _ => throw new NotImplementedException()
            };
        }

        protected void Previous()
        {
            ViewDate = this.View switch
            {
                DatePickerView.Decades => ViewDate.SafeAddYears(-100),
                DatePickerView.Months => ViewDate.SafeAddYears(-1),
                DatePickerView.Days => ViewDate.AddMonths(-1),
                _ => throw new NotImplementedException()
            };
        }

        protected IEnumerable<DayOfWeek> GetWeekDays(DayOfWeek startDayOfWeek)
        {
            for (int i = 0; i < 7; i++)
            {
                var dow = (DayOfWeek)(((int)startDayOfWeek + i) % 7);
                yield return dow;
            }
        }

        protected bool IsDateDisabled(DateTime dt)
        {
            return (StartDate != null && dt < StartDate)
                || (EndDate != null && dt > EndDate)
                || (DateDisabledCallback?.Invoke(dt) == true);
        }

        protected void SetDate(DateTime dt)
        {
            if (IsDateDisabled(dt))
            {
                return;
            }

            this.Value = dt;
            this.InputValue = dt.ToString(DateFormat);
            if (CloseOnSelect)
            {
                this.Expanded = false;
            }
        }

        protected void SetView(DatePickerView view, int? year = null, int? month = null, int? day = null)
        {
            this.View = view;
            this.ViewDate = new DateTime(year ?? ViewDate.Year, month ?? ViewDate.Month, day ?? ViewDate.Day);
        }

        protected void InputValueChanged()
        {
            if (DateTime.TryParseExact(InputValue, DateFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out var dt))
            {
                this.Value = dt;
            }
        }

        protected void InputValueBlur()
        {
            this.InputValue = this.Value?.ToString(DateFormat);
        }
    }
}
