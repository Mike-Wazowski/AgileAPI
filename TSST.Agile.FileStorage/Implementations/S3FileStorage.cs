using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSST.Agile.FileStorage.Interfaces;
using Amazon;
using Amazon.S3;
using System.IO;

namespace TSST.Agile.FileStorage.Implementations
{
    public class S3FileStorage: IFileStorage
    {
        private IAmazonS3 _s3Client;
        private string _bucketName = string.Empty;

        public S3FileStorage()
        {
            var config = ConfigurationManager.AppSettings;
            _bucketName = config["BucketName"];
            _s3Client = AWSClientFactory.CreateAmazonS3Client(config["AWSAccessKey"], config["AWSSecretKey"], Amazon.RegionEndpoint.EUCentral1);
        }

        public async Task<string> AddFile(int projectId, int taskId, string fileName, MemoryStream ms)
        {
            string S3Key = string.Format("{0}/{1}/{2}", projectId.ToString(), taskId.ToString(), fileName);
            var request = new Amazon.S3.Model.PutObjectRequest();
            request.BucketName = _bucketName;
            request.Key = S3Key;
            request.InputStream = ms;
            await _s3Client.PutObjectAsync(request);
            var response = await _s3Client.PutACLAsync(new Amazon.S3.Model.PutACLRequest()
            {
                CannedACL = S3CannedACL.PublicRead,
                BucketName = _bucketName,
                Key = S3Key
            });
            var url = _s3Client.GetPreSignedURL(new Amazon.S3.Model.GetPreSignedUrlRequest()
            {
                BucketName = _bucketName,
                Key = S3Key,
                Expires = DateTime.Now.AddSeconds(604800)
            });
            return url;
        }
    }
}
