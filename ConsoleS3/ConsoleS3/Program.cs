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



