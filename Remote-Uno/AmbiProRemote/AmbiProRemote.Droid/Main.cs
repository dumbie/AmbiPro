using Android.Runtime;
using Com.Nostra13.Universalimageloader.Core;
using System;
using Windows.UI.Xaml.Media;

namespace AmbiProRemote.Droid
{
    [global::Android.App.ApplicationAttribute(
        Label = "@string/ApplicationName",
        Icon = "@drawable/Icon",
        Theme = "@style/AppTheme",
        LargeHeap = true,
        HardwareAccelerated = true
    )]
    public class Application : Windows.UI.Xaml.NativeApplication
    {
        public Application(IntPtr javaReference, JniHandleOwnership transfer) : base(() => new App(), javaReference, transfer)
        {
            ConfigureUniversalImageLoader();
        }

        private void ConfigureUniversalImageLoader()
        {
            // Create global configuration and initialize ImageLoader with this config
            ImageLoaderConfiguration config = new ImageLoaderConfiguration.Builder(Context).Build();
            ImageLoader.Instance.Init(config);
            ImageSource.DefaultImageLoader = ImageLoader.Instance.LoadImageAsync;
        }
    }
}