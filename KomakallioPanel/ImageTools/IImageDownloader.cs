﻿using SixLabors.ImageSharp;

namespace KomakallioPanel.ImageTools
{
    public interface IImageDownloader
    {
        Task<Image> DownloadImageAsync(Uri imageUrl);
    }
}
