namespace FileUpload.Extensions
{
    public static class FormFIleExtensions
    {
        public static byte[] AsByteArray(this IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}