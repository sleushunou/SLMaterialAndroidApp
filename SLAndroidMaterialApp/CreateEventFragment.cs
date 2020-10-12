using System;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Text;
using Android.Text.Style;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using Google.Android.Material.AppBar;
using Google.Android.Material.Chip;
using Google.Android.Material.DatePicker;
using Google.Android.Material.TextField;
using Java.Lang;
using Object = Java.Lang.Object;
using String = System.String;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace SLAndroidMaterialApp
{
    public class CreateEventFragment : DialogFragment, IMaterialPickerOnPositiveButtonClickListener
    {
        private MaterialToolbar _materialToolbar;
        private TextInputEditText _dateEditText;
        private TextInputEditText _tagsEditText;
        private MaterialDatePicker _datePicker;
        private AutoCompleteTextView _typeTextView;
        
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_create_event, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            _materialToolbar = view.FindViewById<MaterialToolbar>(Resource.Id.fragment_create_event_topAppBar);
            _materialToolbar.NavigationClick += MaterialToolbarOnNavigationClick;

            _dateEditText =
                view.FindViewById<TextInputEditText>(Resource.Id.fragment_create_event_date_edit_text);
            _dateEditText.FocusChange += DateEditTextOnFocusChange;

            _tagsEditText =
                view.FindViewById<TextInputEditText>(Resource.Id.fragment_create_event_tags_edit_text);
            _tagsEditText.AfterTextChanged += TagsEditTextOnAfterTextChanged;

            _typeTextView =
                view.FindViewById<AutoCompleteTextView>(Resource.Id.fragment_create_type_text_view_dropdown);
            _typeTextView.Adapter = new ArrayAdapter<string>(
                Context,
                Resource.Layout.dropdown_menu_popup_item,
                new[] {"Job", "Home" });
        }

        private void TagsEditTextOnAfterTextChanged(object sender, AfterTextChangedEventArgs e)
        {
            var length = e.Editable.Length();
            var editable = e.Editable;
            
            if (length > 1 && e.Editable.CharAt(e.Editable.Length() - 1) == ' ')
            {
                var chip = ChipDrawable.CreateFromResource(Context, Resource.Xml.input_chip);
                chip.Text = editable.SubSequence(0, length);
                chip.SetBounds(0, 0, chip.IntrinsicWidth, chip.IntrinsicHeight);
                
                var span = new ImageSpan(chip);
                
                editable.SetSpan(span, 0, length, SpanTypes.ExclusiveExclusive);
            }
        }

        private void DateEditTextOnFocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {
                _datePicker = MaterialDatePicker.Builder.DatePicker().Build();
                _datePicker.Show(ChildFragmentManager, _datePicker.Tag);
                _datePicker.AddOnDismissListener(new MaterialDatePickerListener(_datePicker, 
                    () => _dateEditText.ClearFocus()));
                _datePicker.AddOnPositiveButtonClickListener(this);
            }
        }

        public override void OnStart()
        {
            base.OnStart();
            if (Dialog != null)
            {
                var tabletMinSize = GetPxFromDp(600);
                if (Context.Resources.DisplayMetrics.WidthPixels > tabletMinSize
                    && Context.Resources.DisplayMetrics.HeightPixels > tabletMinSize)
                {    
                    Dialog.Window.SetLayout((int) GetPxFromDp(400), (int) GetPxFromDp(580)); 
                }
                else
                {
                    Dialog.Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
                    Dialog.Window.SetBackgroundDrawable(new ColorDrawable(Android.Graphics.Color.Transparent));
                }
            }
        }

        public void OnPositiveButtonClick(Object unixDate)
        {
            _datePicker.RemoveOnPositiveButtonClickListener(this);
            
            var start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var date= start.AddMilliseconds((long)unixDate).ToLocalTime();
            
            _dateEditText.Text = date.ToString("dd.MM.yyyy");
        }

        private void MaterialToolbarOnNavigationClick(object sender, Toolbar.NavigationClickEventArgs e)
        {
            Dismiss();
        }
        
        private float GetPxFromDp(float dp) {
            return dp * Context.Resources.DisplayMetrics.Density;
        }
        
        private class MaterialDatePickerListener : Java.Lang.Object, IDialogInterfaceOnDismissListener
        {
            private readonly MaterialDatePicker _materialDatePicker;
            private readonly Action _action;
            
            public MaterialDatePickerListener(
                MaterialDatePicker materialDatePicker,
                Action action)
            {
                _materialDatePicker = materialDatePicker;
                _action = action;
            }
            
            public void OnDismiss(IDialogInterface? dialog)
            {
                _materialDatePicker.RemoveOnDismissListener(this);
                _action.Invoke();
            }
        }
    }
}