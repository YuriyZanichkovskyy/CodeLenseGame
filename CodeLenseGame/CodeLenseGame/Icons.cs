// Decompiled with JetBrains decompiler
// Type: Microsoft.VisualStudio.CodeSense.CodeHealth.Icons
// Assembly: Microsoft.VisualStudio.CodeSense.CodeHealth, Version=12.0.0.0, Culture=neutral, PublicKeyToken=f432e2ea019d5ec3
// MVID: 88EA7E98-9489-4542-91DF-060677E0C16A
// Assembly location: D:\Analysis\Microsoft.VisualStudio.CodeSense.CodeHealth.dll

using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CodeLenseGame
{
    public static class Icons
    {
        private static readonly Lazy<ImageSource> _game2048Icon =
            new Lazy<ImageSource>(() => LoadImage("pack://application:,,,/CodeLenseGame;component/icons/2048.ico"));

        public static ImageSource Game2048Icon
        {
            get { return _game2048Icon.Value; }
        }

        private static ImageSource LoadImage(string uri)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(uri);
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            bitmapImage.Freeze();
            return (ImageSource)bitmapImage;
        }
    }
}
