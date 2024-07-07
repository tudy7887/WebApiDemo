namespace Learning.Services
{
    public class ImageManipulation
    {
        public string GetImage(byte[] ImageData)
        {
            return Convert.ToBase64String(ImageData, 0, ImageData.Length);
        }
        public byte[] SetImage(string Image)
        {
            if (File.Exists(Image))
            {
                return File.ReadAllBytes(Image);
            }
            return null;
        }
    }
}
