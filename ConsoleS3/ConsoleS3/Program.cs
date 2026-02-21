using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;

Console.InputEncoding = System.Text.Encoding.UTF8;
Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("--Робота з S3--");
var credentials = new BasicAWSCredentials(
    "AKIAY7XUAOGIWNUDR573",
    "2sVViQ/0snugHLDSECIlsLx+D+Yh6ynHy7Pg9Uk1");

var client = new AmazonS3Client(credentials, RegionEndpoint.EUCentral1);

/*
string dir = "images";
string path = Path.Combine(dir, "image.jpg");

string bucketName = "my-p32bucket";

var request = new PutObjectRequest
{
    BucketName = bucketName,
    Key = "images/image.jpg",
    FilePath = path,
    ContentType = "image/jpeg"
};

await client.PutObjectAsync(request);
*/

//string bucketName = "my-p32bucket";
//string downloadsDir = "downloads";
//Directory.CreateDirectory(downloadsDir);
//string path = Path.Combine(downloadsDir, "image.jpg");
//var request = new GetObjectRequest // звертаюся до об'єкта в S3, щоб отримати його вміст
//{
//    BucketName = bucketName,
//    Key = "images/image.jpg",
//};

//try
//{
//    using var response = await client.GetObjectAsync(request);
//    await response.WriteResponseStreamToFileAsync(path, false, new CancellationToken());
//}
//catch (AmazonS3Exception ex)
//{
//    Console.WriteLine($"Помилка при отриманні об'єкта: {ex.Message}");
//}

var request = new ListObjectsV2Request
{
    BucketName = "my-p32bucket"
};
try
{
    var response = await client.ListObjectsV2Async(request);
    Console.WriteLine("Файли в бакеті:");
    foreach (var obj in response.S3Objects)
    {
        Console.WriteLine($"- {obj.Key} (розмір: {obj.Size} байт)");
    }
}
catch (AmazonS3Exception ex)
{
    Console.WriteLine($"Помилка при отриманні списку об'єктів: {ex.Message}");
}



