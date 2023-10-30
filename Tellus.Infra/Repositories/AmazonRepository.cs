using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Tellus.Domain.Interfaces;

namespace Tellus.Infra.Repositories
{
    public class AmazonRepository : IAmazonRepository
    {
        public AmazonS3Client Authenntication(string accessKeyId, string secretAccessKey)
        {
            // Crie uma instância de AmazonS3Client com suas chaves de acesso
            return new AmazonS3Client(accessKeyId, secretAccessKey, Amazon.RegionEndpoint.SAEast1);
        }
        public async Task<long> GetFolderSizeInBytes(string accessKeyId, string secretAccessKey, string bucketName, string folderName)
        {
            var s3Client = Authenntication(accessKeyId, secretAccessKey);

            var listRequest = new ListObjectsRequest
            {
                BucketName = bucketName,
                Prefix = folderName + "/"
            };

            var listResponse = await s3Client.ListObjectsAsync(listRequest);

            long totalSize = 0;

            foreach (var s3Object in listResponse.S3Objects)
            {
                totalSize += s3Object.Size;
            }

            return totalSize;
        }

        public async Task<decimal> GetFolderSizeInMB(string accessKeyId, string secretAccessKey, string bucketName, string folderName)
        {
            long sizeInBytes = await GetFolderSizeInBytes(accessKeyId, secretAccessKey, bucketName, folderName);
            decimal sizeInMB = ((decimal)sizeInBytes) / (1024 * 1024);
            return sizeInMB;
        }
        public async Task<bool> CreateSubfolder(string accessKeyId, string secretAccessKey, string bucketName, string folderName)
        {
            var s3Client = Authenntication(accessKeyId, secretAccessKey);
            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = folderName + "/",
                ContentBody = string.Empty
            };
            var result = await s3Client.PutObjectAsync(request);
            return !String.IsNullOrEmpty(result.ResponseMetadata.RequestId);
        }
        public async void DeleteFolder(string accessKeyId, string secretAccessKey, string bucketName, string folderName)
        {
            var s3Client = Authenntication(accessKeyId, secretAccessKey);

            var listRequest = new ListObjectsRequest
            {
                BucketName = bucketName,
                Prefix = folderName + "/"
            };

            var listResponse = await s3Client.ListObjectsAsync(listRequest);

            if (listResponse.S3Objects.Count > 0)
            {
                var deleteRequest = new DeleteObjectsRequest
                {
                    BucketName = bucketName,
                    Objects = listResponse.S3Objects.Select(o => new KeyVersion { Key = o.Key }).ToList()
                };

                await s3Client.DeleteObjectsAsync(deleteRequest);
            }
        }
        public bool Upload(string accessKeyId, string secretAccessKey, string bucketName, string base64String, string folderName, string fileName)
        {
            try
            {
                byte[] fileBytes = Convert.FromBase64String(base64String);
                var s3Client = Authenntication(accessKeyId, secretAccessKey);
                var transferUtility = new TransferUtility(s3Client);
                var fileTransferUtilityRequest = new TransferUtilityUploadRequest
                {
                    BucketName = bucketName,
                    InputStream = new MemoryStream(fileBytes),
                    Key = folderName + "/" + fileName,
                    CannedACL = S3CannedACL.Private
                };
                transferUtility.Upload(fileTransferUtilityRequest);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> Delete(string accessKeyId, string secretAccessKey, string bucketName, string folderName, string fileName)
        {
            try
            {
                var s3Client = Authenntication(accessKeyId, secretAccessKey);
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = bucketName,
                    Key = folderName + "/" + fileName
                };
                await s3Client.DeleteObjectAsync(deleteObjectRequest);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> Download(string accessKeyId, string secretAccessKey, string bucketName, string folderName, string fileName)
        {
            try
            {

                var s3Client = Authenntication(accessKeyId, secretAccessKey);
                var request = new GetPreSignedUrlRequest
                {
                    BucketName = bucketName,
                    Key = folderName + "/" + fileName,
                    Verb = HttpVerb.GET,
                    Expires = DateTime.UtcNow.AddHours(5),
                    Protocol = Protocol.HTTP
                };

                return s3Client.GetPreSignedURL(request);
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public async Task<string> SignedUrl(string accessKeyId, string secretAccessKey, string bucketName, string folderName, string fileName)
        {
            try
            {
                var s3Client = Authenntication(accessKeyId, secretAccessKey);
                var key = folderName + "/" + fileName;
                var request = new GetPreSignedUrlRequest
                {
                    BucketName = bucketName,
                    Key = key,
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    //    Protocol = Protocol.HTTPS, // Use HTTPS para segurança
                    Verb = HttpVerb.PUT,
                };

                var signedUrl = s3Client.GetPreSignedURL(request);
                return signedUrl;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}