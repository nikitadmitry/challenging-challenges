// ReSharper disable CheckNamespace
namespace System.IO
// ReSharper restore CheckNamespace
{
    public static class StreamExtensions
    {
        public static MemoryStream ConvertToMemoryStream(this Stream content)
        {
            var memoryStream = new MemoryStream();
            var stream = content;
            byte[] buffer = new byte[4096];
            int chunkSize;
            do
            {
                chunkSize = stream.Read(buffer, 0, buffer.Length);
                memoryStream.Write(buffer, 0, chunkSize);
            } while (chunkSize != 0);

            memoryStream.Position = 0;
            return memoryStream;
        }

        public static byte[] ToByteArray(this Stream stream)
        {
            stream.Position = 0;
            byte[] buffer = new byte[stream.Length];
            for (int totalBytesCopied = 0; totalBytesCopied < stream.Length; )
                totalBytesCopied += stream.Read(buffer, totalBytesCopied, Convert.ToInt32(stream.Length) - totalBytesCopied);
            return buffer;
        }
    }
}