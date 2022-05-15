using Amazon.S3;
using Amazon.S3.Model;
using BugTrakerAPI.Model.ReturnModel;

namespace BugTrakerAPI.Services.uploadToS3
{
    public class UploadToS3 : IUploadToS3
    {
        private readonly IAmazonS3 _s3Client;
        public UploadToS3(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
        }

        public async Task<UploadReturnFromS3> upload(IFormFile file, string bucketName, List<string> contentTypeAlloweded, float maxSizeAllocated = 1)
        {
            var retrunValue = new UploadReturnFromS3();
            retrunValue.Sucess = false;

            int in1MB = 1048576;
            try
            {
                double length = (double)file.Length / in1MB;
                if (length > maxSizeAllocated)
                {
                    retrunValue.Error = "The file size exceed the limit";
                    return retrunValue;
                }
                if (!contentTypeAlloweded.Contains(file.ContentType))
                {
                    retrunValue.Error = "Content type didn't match";
                    return retrunValue;
                }
                var fileguid = Guid.NewGuid().ToString() + file.FileName;
                var request = new PutObjectRequest()
                {
                    BucketName = bucketName,
                    Key = fileguid,
                    InputStream = file.OpenReadStream()
                };
                request.Metadata.Add("Content-Type", file.ContentType);
                var result = await _s3Client.PutObjectAsync(request);
                var AvatarLink = $"https://bugtrakerimages.s3.ap-south-1.amazonaws.com/" + fileguid;
                retrunValue.AvatarLink = AvatarLink;
                retrunValue.Sucess = true;
                return retrunValue;
            }
            catch (Exception error)
            {
                retrunValue.Error = error.Message;
                return retrunValue;
            }

        }
    }
}