using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Nameless.BeetleTracker {

    /// <summary>
    /// Extension methods for images
    /// </summary>
    public static class ImageExtension {

        #region Public Static Methods

        /// <summary>
        /// Converts an image to a stream.
        /// </summary>
        /// <param name="source">The image.</param>
        /// <param name="format">The image format.</param>
        /// <returns>The stream.</returns>
        public static Stream ToStream(this Image source, ImageFormat format = null) {
            if (source == null) { return null; }
            var innerFormat = format ?? source.RawFormat;
            var memoryStream = new MemoryStream();
            source.Save(memoryStream, innerFormat);
            memoryStream.Seek(offset: 0, loc: SeekOrigin.Begin);
            return memoryStream;
        }

        /// <summary>
        /// Converts an image to a byte array.
        /// </summary>
        /// <param name="source">The image.</param>
        /// <param name="format">The image format.</param>
        /// <returns>The byte array.</returns>
        public static byte[] ToByteArray(this Image source, ImageFormat format = null) {
            if (source == null) { return null; }
            byte[] result;
            var innerFormat = format ?? source.RawFormat;
            using (var memoryStream = new MemoryStream()) {
                source.Save(memoryStream, innerFormat);
                memoryStream.Seek(offset: 0, loc: SeekOrigin.Begin);
                result = memoryStream.ToArray();
            }
            return result;
        }

        /// <summary>
        /// Retrieves the image MIME type, by its decoder information.
        /// </summary>
        /// <param name="source">The image.</param>
        /// <returns>The MIME type.</returns>
        public static string GetMimeType(this Image source) {
            if (source == null) { return null; }

            var format = source.RawFormat;
            var codec = ImageCodecInfo.GetImageDecoders().FirstOrDefault(_ => _.FormatID == format.Guid);
            var mimeType = codec.MimeType;

            return mimeType;
        }

        #endregion Public Static Methods
    }
}