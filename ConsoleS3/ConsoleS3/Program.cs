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

while (true)
{
    Console.WriteLine("\n--- Меню ---");
    Console.WriteLine("1. Створити бакет");
    Console.WriteLine("2. Переглянути список файлів");
    Console.WriteLine("3. Завантажити файл у S3");
    Console.WriteLine("4. Отримати файл з S3");
    Console.WriteLine("5. Вийти");
    Console.Write("Оберіть дію (1-5): ");

    string? choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            await CreateNewBucketAsync(client);
            break;
        case "2":
            await ListBucketFilesAsync(client);
            break;
        case "3":
            await UploadFileToS3Async(client);
            break;
        case "4":
            await DownloadFileFromS3Async(client);
            break;
        case "5":
            Console.WriteLine("Дякую що відвідали нашу програму");
            return;
        default:
            Console.WriteLine("Невідома команда. Спробуйте ще раз.");
            break;
    }
}

static async Task CreateNewBucketAsync(IAmazonS3 client)
{
    Console.Write("Введіть назву нового бакета: ");
    string? bucketName = Console.ReadLine();

    try
    {
        var request = new PutBucketRequest { BucketName = bucketName };
        await client.PutBucketAsync(request);
        Console.WriteLine($"Бакет успішно створено.");
    }
    catch (AmazonS3Exception ex)
    {
        Console.WriteLine($"Помилка: {ex.Message}");
    }
}

static async Task ListBucketFilesAsync(IAmazonS3 client)
{
    Console.Write("Введіть назву бакета для перегляду: ");
    string? bucketName = Console.ReadLine();

    try
    {
        var request = new ListObjectsV2Request { BucketName = bucketName };
        var response = await client.ListObjectsV2Async(request);

        Console.WriteLine($"\nФайли у бакеті:");
        if (response?.S3Objects == null || response.S3Objects.Count == 0)
        {
            Console.WriteLine("Бакет порожній.");
            return;
        }
        foreach (var obj in response.S3Objects)
        {
            Console.WriteLine($"- {obj.Key} (Розмір: {obj.Size} байт)");
        }
    }
    catch (AmazonS3Exception ex)
    {
        Console.WriteLine($"Помилка: {ex.Message}");
    }
}

static async Task UploadFileToS3Async(IAmazonS3 client)
{
    Console.Write("Введіть назву бакета: ");
    string? bucketName = Console.ReadLine();

    Console.Write("Введіть локальний шлях до файлу: ");
    string? path = Console.ReadLine();

    try
    {
        var request = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = path,
            FilePath = path,
            ContentType = "image/jpeg"
        };

        await client.PutObjectAsync(request);
        Console.WriteLine($"Файл успішно завантажено.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Помилка завантаження: {ex.Message}");
    }
}

static async Task DownloadFileFromS3Async(IAmazonS3 client)
{
    Console.Write("Введіть назву бакета: ");
    string? bucketName = Console.ReadLine();

    Console.Write("Введіть ключ файлу в S3: ");
    string? s3Key = Console.ReadLine();

    string downloadsDir = "downloads";
    Directory.CreateDirectory(downloadsDir);
    string localPath = Path.Combine(downloadsDir, Path.GetFileName(s3Key ?? "downloaded_file"));

    try
    {
        var request = new GetObjectRequest
        {
            BucketName = bucketName,
            Key = s3Key
        };

        using var response = await client.GetObjectAsync(request);
        await response.WriteResponseStreamToFileAsync(localPath, false, new CancellationToken());

        Console.WriteLine($"Файл успішно збережено локально");
    }
    catch (AmazonS3Exception ex)
    {
        Console.WriteLine($"Помилка завантаження: {ex.Message}");
    }
}