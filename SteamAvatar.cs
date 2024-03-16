using Steamworks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Steamvoice
{
    public static class SteamAvatar
    {
        public static ImageSource? GetImageFromAvatar(CSteamID steamID)
        {
            int friendAvatar = SteamFriends.GetLargeFriendAvatar(steamID);
            uint imageWidth, imageHeight;

            bool success = SteamUtils.GetImageSize(friendAvatar, out imageWidth, out imageHeight);

            if (success && imageWidth > 0 && imageHeight > 0)
            {
                byte[] avatarData = new byte[imageWidth * imageHeight * 4];
                success = SteamUtils.GetImageRGBA(friendAvatar, avatarData, (int)(imageWidth * imageHeight * 4));
                if (success)
                {
                    BitmapSource bitmapSource = BitmapSource.Create((int)imageWidth, (int)imageHeight, 96, 96, PixelFormats.Bgra32, null, avatarData, (int)imageWidth * 4);
                    return bitmapSource;
                }
            }
            return null;
        }
    }
}
