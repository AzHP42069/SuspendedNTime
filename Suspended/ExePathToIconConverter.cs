using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace Suspended
{
    public class ExePathToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string exePath && !string.IsNullOrEmpty(exePath))
            {
                // Placeholder blank image (required because converter must return something synchronously)
                BitmapImage placeholder = new BitmapImage();

                exePath = "C:\\Users\\Bassem Nomany\\Pictures\\74701825-7E4E-4BFC-BF27-F2449412F895.jpeg";
                // Fire & forget async load
                _ = LoadIconAsync(exePath, placeholder);

                return placeholder;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        private async Task LoadIconAsync(string exePath, BitmapImage target)
        {
            try
            {
                StorageFile file = await StorageFile.GetFileFromPathAsync(exePath);
                var thumbnail = await file.GetThumbnailAsync(
                    ThumbnailMode.SingleItem,
                    48,
                    ThumbnailOptions.UseCurrentScale
                );

                await target.SetSourceAsync(thumbnail);
            }
            catch
            {
                //set a fallback icon
                target.UriSource = new Uri("ms-appx:///Assets/info.png");
            }
        }
    }
}
