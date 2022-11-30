namespace pocketbase_csharp_sdk.Enum
{
    public enum ThumbFormat
    {
        CropToWxHFromCenter,
        CropToWxHFromTop,
        CropToWxHFromBottom,
        FitInsideWxHViewbox,
        ResizeToHeight,
        ResizeToWidth
    }

    internal static class ThumbFormatHelper
    {
        
        public static string GetNameForQuery(ThumbFormat? thumbFormat) 
        {
            return thumbFormat switch
            {
                ThumbFormat.CropToWxHFromCenter => "WxH",
                ThumbFormat.CropToWxHFromTop => "WxT",
                ThumbFormat.CropToWxHFromBottom => "WxB",
                ThumbFormat.FitInsideWxHViewbox => "WxHf",
                ThumbFormat.ResizeToHeight => "0xH",
                ThumbFormat.ResizeToWidth => "Wx0",
                _ => string.Empty
            };
        }

    }

}