namespace StayCloudAPI.WebAPI.Extensions
{
    public static class ConvertLstUrlsExtensions
    {
        public static List<string> ConvertLstUrls(List<string> lstUrls)
        {
            var result = new List<string>();

            foreach (var url in lstUrls)
            {
                var fileType = url.Split("/")[^1];
                var fileName = fileType.Split(".")[0];
                result.Add(fileName);
            }

            return result;
        }
    }
}
