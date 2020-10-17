using System;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.DrawerLayout.Widget;
using AndroidX.Lifecycle;
using AndroidX.RecyclerView.Widget;
using AndroidX.ViewPager2.Adapter;
using AndroidX.ViewPager2.Widget;
using Google.Android.Material.AppBar;
using Google.Android.Material.BottomAppBar;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Tabs;
using Fragment = AndroidX.Fragment.App.Fragment;
using FragmentManager = AndroidX.Fragment.App.FragmentManager;

namespace SLAndroidMaterialApp
{
    [Register("com.sample.sl.MainFragment")]
    public class MainFragment : Fragment, TabLayoutMediator.ITabConfigurationStrategy
    {
        private TabLayout _tabLayout;
        private ViewPager2 _viewPager2;
        private TabLayoutMediator _tabLayoutMediator;
        private DrawerLayout _drawerLayout;
        
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_main, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            
            var appBar = view.FindViewById<BottomAppBar>(Resource.Id.fragment_main_bottomAppBar);
            if (appBar != null)
            {
                appBar.NavigationClick += (sender, args) =>
                {
                    var fragment = new BottomNavigationDrawerFragment();
                    fragment.Show(ChildFragmentManager, fragment.Tag);
                };
            }

            var topAppBar = view.FindViewById<MaterialToolbar>(Resource.Id.fragment_main_topAppBar);
            if (topAppBar != null)
            {
                topAppBar.NavigationClick += (sender, args) =>
                {
                    _drawerLayout = Activity.FindViewById<DrawerLayout>(Resource.Id.fragment_main_drawer_layout);
                    _drawerLayout.OpenDrawer((int) GravityFlags.Start);
                };
            }

            var button = view.FindViewById<FloatingActionButton>(Resource.Id.fragment_main_add_button);
            button.Click += (sender, args) =>
            {
                var fragment = new CreateEventFragment();
                fragment.Show(ChildFragmentManager, fragment.Tag);
            };

            _tabLayout = view.FindViewById<TabLayout>(Resource.Id.fragment_main_tab_layout);

            _viewPager2 = view.FindViewById<ViewPager2>(Resource.Id.fragment_main_view_pager);
            _viewPager2.Adapter = new ViewPagerAdapter(ChildFragmentManager, Lifecycle);

            _tabLayoutMediator = new TabLayoutMediator(_tabLayout, _viewPager2, this);
            _tabLayoutMediator.Attach();
        }

        private class ViewPagerAdapter : FragmentStateAdapter
        {
            public ViewPagerAdapter(FragmentManager fragmentManager, Lifecycle lifecycle) : base(fragmentManager, lifecycle)
            {
            }

            public override int ItemCount => 3;
            
            public override Fragment CreateFragment(int p0)
            {
                return new TabFragment();
            }
        }
        
        private class TabFragment : Fragment
        {
            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            {
                var recyclerView = new RecyclerView(Context);
                recyclerView.SetLayoutManager(new GridLayoutManager(Context, Resources.GetInteger(Resource.Integer.main_fragments_columns)));
                recyclerView.SetAdapter(new MainAdapter(this));
                return recyclerView;
            }
        }

        private class MainAdapter : RecyclerView.Adapter
        {
            private readonly Fragment _fragment;
        
            public MainAdapter(Fragment fragment)
            {
                _fragment = fragment;
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                var textView = LayoutInflater.From(parent.Context)
                    .Inflate(Resource.Layout.main_recycler_item, parent, false);

                return new MainViewHolder(textView);
            }
            
            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                holder.ItemView.Click -= ItemViewOnClick;
                holder.ItemView.Click += ItemViewOnClick;
            }

            private void ItemViewOnClick(object sender, EventArgs e)
            {
                var fragment = new DetailsFragment();

                var transaction = _fragment.Activity.SupportFragmentManager.BeginTransaction();
                transaction.Add(Resource.Id.activity_main_container, fragment, fragment.Tag);
                transaction.AddToBackStack(null);
                
                transaction.Commit();
            }

            public override int ItemCount => 10;
        }

        private class MainViewHolder : RecyclerView.ViewHolder
        {
            public MainViewHolder(View itemView) : base(itemView)
            {
            }
        }

        public void OnConfigureTab(TabLayout.Tab tab, int number)
        {
            tab.SetText(number switch
            {
                0 => "All",
                1 => "Job",
                2 => "Home",
            });
        }
    }
}