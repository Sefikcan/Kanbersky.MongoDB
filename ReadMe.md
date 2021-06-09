# Kanbersky.MongoDB
 * Yüklenilen projenin Startup.cs sınıfının ConfigureServices metoduna 
    services.RegisterKanberskyMongoDB(configuration) eklenir.
 * AppSettings.json dosyasına aşağıdaki formatta ekleme yapılmalıdır.
    "MongoDBSettings": {
        "ConnectionStrings": "mongodb://localhost:27017",
        "DatabaseName": "DatabaseName"
    }
