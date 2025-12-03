
using Document.API.Database.Entity;
using Document.API.Models.Request;
using Document.API.Models.Responses;
using Document.API.Repository;
using Minio;
using Minio.DataModel.Args;
using System.Net;

namespace Document.API.Services.Implementations
{
    public class FileServices : IFileServices
    {
        private readonly IMinioClient _minio;
        private readonly IFileRepository _fileRepository;

        public FileServices(IMinioClient minio, IFileRepository fileRepository)
        {
            _minio = minio;
            _fileRepository = fileRepository;
        }

        public async Task<string> CreateBucketAsync(string objectKey)
        {
            bool exists = await _minio.BucketExistsAsync(new BucketExistsArgs().WithBucket(objectKey));

            if (!exists)
            {
                await _minio.MakeBucketAsync(new MakeBucketArgs().WithBucket(objectKey));

                await _fileRepository.AddBucket(new BucketEntity { Name = objectKey });

                return $"Bucket '{objectKey}' creado correctamente.";
            }

            return $"Bucket '{objectKey}' ya existe.";
        }

        public async Task<ResponsesObjectJson> CreateObjectAsync(CreateObjectDto model)
        {
            bool exists = await _minio.BucketExistsAsync(new BucketExistsArgs().WithBucket(model.Bucket));

            if (!exists)
                throw new Exception($"El bucket '{model.Bucket}' no existe.");

            var bucketDb = await _fileRepository.GetBucketByName(model.Bucket);

            if (bucketDb == null)
                throw new Exception($"El bucket '{model.Bucket}' no está registrado en la base de datos.");

            var objectName = $"{model.FilePath}/{model.File.FileName}";
            string contentType = model.File.ContentType ?? "application/octet-stream";

            using (var stream = model.File.OpenReadStream())
            {
                await _minio.PutObjectAsync(
                    new PutObjectArgs()
                        .WithBucket(model.Bucket)
                        .WithObject(objectName)
                        .WithStreamData(stream)
                        .WithObjectSize(stream.Length)
                        .WithContentType(contentType)
                );
            }

            var document = new DocumentEntity
            {
                bucketId = bucketDb.Id,
                Url = objectName,
                CreatAt = DateTime.UtcNow
            };

            await _fileRepository.AddDocument(document);

            return new ResponsesObjectJson
            {
                Code = (int)HttpStatusCode.OK,
                Message = "Archivo subido correctamente.",
                Response = objectName
            };
        }

        public async Task<ResponsesObjectJson> GeBucketAsync(string bucket)
        {
            bool bucketExists = await _minio.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucket));

            var bucketInfo = new
            {
                Name = bucket,
                Exists = bucketExists,
                CreationDate = await GetBucketCreationDate(bucket)
            };

            return new ResponsesObjectJson
            {
                Code = (int)HttpStatusCode.OK,
                Message = "Información del bucket obtenida correctamente.",
                Response = bucketInfo
            };
        }

        public async Task<ResponsesObjectJson> GetObjectAsync(string bucket, string objectName)
        {
            var statArgs = new StatObjectArgs()
                .WithBucket(bucket)
                .WithObject(objectName);

            var objectStat = await _minio.StatObjectAsync(statArgs);

            var objectInfo = new
            {
                Name = objectStat.ObjectName,
                Size = objectStat.Size,
                ContentType = objectStat.ContentType,
                LastModified = objectStat.LastModified,
                ETag = objectStat.ETag,
                VersionId = objectStat.VersionId,
                Metadata = objectStat.MetaData?.ToDictionary(x => x.Key, x => x.Value) ?? new Dictionary<string, string>(),
                Bucket = bucket,
                ObjectPath = objectName
            };

            return new ResponsesObjectJson
            {
                Code = (int)HttpStatusCode.OK,
                Message = "Información del objeto obtenida correctamente.",
                Response = objectInfo
            };
        }

        public async Task<(byte[] fileBytes, string contentType, string fileName)> DownloadObject(string bucket, string objectName)
        {
            var stat = await _minio.StatObjectAsync(
                new StatObjectArgs()
                    .WithBucket(bucket)
                    .WithObject(objectName)
            );

            using var memoryStream = new MemoryStream();

            var args = new GetObjectArgs()
                .WithBucket(bucket)
                .WithObject(objectName)
                .WithCallbackStream(stream => stream.CopyTo(memoryStream));

            await _minio.GetObjectAsync(args);

            return (
                fileBytes: memoryStream.ToArray(),
                contentType: stat.ContentType ?? "application/octet-stream",
                fileName: objectName
            );
        }

        private async Task<DateTime?> GetBucketCreationDate(string bucket)
        {
            var buckets = await _minio.ListBucketsAsync();
            var targetBucket = buckets.Buckets.FirstOrDefault(b => b.Name == bucket);
            return targetBucket?.CreationDateDateTime;
        }
    }
}

