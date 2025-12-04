using Document.API.Models.Request;
using Document.API.Models.Responses;
using Minio.DataModel;

namespace Document.API.Services
{
    public interface IFileServices
    {
        Task<ResponsesObjectJson> CreateBucketAsync(string bucket);
        Task<ResponsesObjectJson> CreateObjectAsync(CreateObjectDto model);
        Task<ResponsesObjectJson> GeBucketAsync(string bucket);
        Task<ResponsesObjectJson> GetObjectAsync(string bucket, string objectName);
        Task<(byte[] fileBytes, string contentType, string fileName)> DownloadObject(string bucket,string objectName);
    }
}
