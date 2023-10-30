using Amazon.S3;

namespace Tellus.Domain.Interfaces
{
    public interface IAmazonRepository
    {
        Task<long> GetFolderSizeInBytes(string accessKeyId, string secretAccessKey, string bucketName, string folderName);
        Task<decimal> GetFolderSizeInMB(string accessKeyId, string secretAccessKey, string bucketName, string folderName);
        void DeleteFolder(string accessKeyId, string secretAccessKey, string bucketName, string folderName);
        Task<bool> CreateSubfolder(string accessKeyId, string secretAccessKey, string bucketName, string folderName);
        AmazonS3Client Authenntication(string accessKeyId, string secretAccessKey);
        bool Upload(string accessKeyId, string secretAccessKey, string bucketName, string base64String, string folderName, string fileName);
        Task<string> Download(string accessKeyId, string secretAccessKey, string bucketName, string folderName, string fileName);
        Task<bool> Delete(string accessKeyId, string secretAccessKey, string bucketName, string folderName, string fileName);
        Task<string> SignedUrl(string accessKeyId, string secretAccessKey, string bucketName, string folderName, string fileName);
    }
}
