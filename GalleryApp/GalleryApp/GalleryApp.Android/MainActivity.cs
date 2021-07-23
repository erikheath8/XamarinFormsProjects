using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Ubiety.Dns.Core;

namespace GalleryApp.Droid
{
    [Activity(Label = "GalleryApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
        ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static object Current { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            //Current = this;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
            [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            if (requestCode == 33)
            {
                var importer = (PhotoImporter)Resolver.Resolve<IPhotoImporter>();
                importer.ContinueWithPermission(grantResults[0] == Permission.Granted);
            }

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}