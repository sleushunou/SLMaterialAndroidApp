using Android.OS;
using Android.Views;
using Google.Android.Material.BottomSheet;

namespace SLAndroidMaterialApp
{
    public class BottomNavigationDrawerFragment : BottomSheetDialogFragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_bottomsheet, container, false);
        }
    }
}