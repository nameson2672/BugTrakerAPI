using BugTrakerAPI.Model.ReturnModel;

namespace BugTrakerAPI.Services.uploadToS3
{
    public interface IUploadToS3
    {
         public Task<UploadReturnFromS3> upload(IFormFile file, string bucketName, List<string> contentTypeAlloweded, float maxSizeAllocated=1); 
    }
}