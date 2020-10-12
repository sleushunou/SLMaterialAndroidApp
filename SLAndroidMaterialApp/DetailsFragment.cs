using System;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.CoordinatorLayout.Widget;
using AndroidX.Fragment.App;
using Google.Android.Material.AppBar;
using Google.Android.Material.BottomAppBar;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;

namespace SLAndroidMaterialApp
{
    [Register("com.sample.sl.DetailsFragment")]
    public class DetailsFragment : Fragment
    {
        private CoordinatorLayout _root;
        private FloatingActionButton _actionButton;
        
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_details, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _root = view.FindViewById<CoordinatorLayout>(Resource.Id.fragment_details_root);

            // var appBar = view.FindViewById<BottomAppBar>(Resource.Id.fragment_details_bottomAppBar);
            // if (appBar != null)
            // {
            //     appBar.NavigationClick += (sender, args) =>
            //     {
            //         var fragment = new BottomNavigationDrawerFragment();
            //         fragment.Show(ChildFragmentManager, fragment.Tag);
            //     };
            // }
            
            var topAppBar = view.FindViewById<MaterialToolbar>(Resource.Id.fragment_main_topAppBar);
            if (topAppBar != null)
            {
                topAppBar.NavigationClick += (sender, args) =>
                {
                    Activity.SupportFragmentManager.PopBackStack();
                };
            }

            _actionButton = view.FindViewById<FloatingActionButton>(Resource.Id.fragment_main_share_button);

            var addToCalendarButton = view.FindViewById<Button>(Resource.Id.fragment_details_add_to_calendar_button);
            addToCalendarButton.Click += AddToCalendarButtonOnClick;
        }

        private void AddToCalendarButtonOnClick(object sender, EventArgs e)
        {
            var snackBar = Snackbar
                .Make(_root, "Event has been added to calendar", BaseTransientBottomBar.LengthShort);
            snackBar.SetAnchorView(_actionButton);
            snackBar.SetAction("Undo", view => { });
            snackBar.Show();
        }
    }
}