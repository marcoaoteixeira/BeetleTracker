using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Nameless.BeetleTracker.Helpers {

    /// <summary>
    /// Helper for web related things.
    /// </summary>
    public static class WebHelper {

        #region Public Static Methods

        /// <summary>
        /// Converts an image to a base64 representation.
        /// </summary>
        /// <param name="imagePath">The image path.</param>
        /// <returns>The Base64 representation.</returns>
        /// <remarks>Return will be like: data:[mime-type];base64,[encoded-data]</remarks>
        /// <exception cref="ArgumentNullException">if <paramref name="imagePath"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">
        /// if <paramref name="imagePath"/> is empty or white spaces.
        /// </exception>
        /// <exception cref="FileNotFoundException">
        /// if <paramref name="imagePath"/> does not point to a valid file.
        /// </exception>
        /// <exception cref="InvalidOperationException">if could not find the image codec.</exception>
        public static string EncodeImageAsBase64(string imagePath) {
            Prevent.ParameterNullOrWhiteSpace(imagePath, nameof(imagePath));

            if (!File.Exists(imagePath)) {
                throw new FileNotFoundException(Properties.Resources.WebHelperEncodeImageBase64FileNotFound, Path.GetFileName(imagePath));
            }

            using (var image = Image.FromFile(imagePath)) {
                return EncodeImageAsBase64(image);
            }
        }

        /// <summary>
        /// Converts an image to a base64 representation.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns>The Base64 representation.</returns>
        /// <remarks>Return will be like: data:[mime-type];base64,[encoded-data]</remarks>
        /// <exception cref="ArgumentNullException">if <paramref name="image"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">if could not find the image codec.</exception>
        public static string EncodeImageAsBase64(Image image) {
            Prevent.ParameterNull(image, nameof(image));

            using (var memoryStream = new MemoryStream()) {
                var format = image.RawFormat;
                var codec = ImageCodecInfo.GetImageDecoders().FirstOrDefault(_ => _.FormatID == format.Guid);
                if (codec == null) {
                    throw new InvalidOperationException(Properties.Resources.WebHelperEncodeImageBase64CodecNotFound);
                }

                image.Save(memoryStream, format);

                return EncodeImageByteArrayAsBase64(memoryStream.ToArray(), codec.MimeType);
            }
        }

        /// <summary>
        /// Converts an array of bytes into an image (base64) representation.
        /// </summary>
        /// <param name="image">The image array of bytes.</param>
        /// <param name="mimeType">The image mime type.</param>
        /// <returns>The Base64 representation.</returns>
        /// <remarks>Return will be like: data:[mime-type];base64,[encoded-data]</remarks>
        /// <exception cref="ArgumentNullException">if <paramref name="image"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">if could not find the image codec.</exception>
        public static string EncodeImageByteArrayAsBase64(byte[] image, string mimeType) {
            Prevent.ParameterNull(image, nameof(image));
            Prevent.ParameterNullOrWhiteSpace(mimeType, nameof(mimeType));

            var base64 = Convert.ToBase64String(image);
            return $"data:{mimeType};base64,{base64}";
        }

        #endregion Public Static Methods
    }
}