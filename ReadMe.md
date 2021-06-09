# Kanbersky.MongoDB
 * Yüklenilen projenin Startup.cs sýnýfýnýn ConfigureServices metoduna 
    services.RegisterKanberskyMongoDB(configuration) eklenir.
 * AppSettings.json dosyasýna aþaðýdaki formatta ekleme yapýlmalýdýr.
    "MongoDBSettings": {
        "ConnectionStrings": "mongodb://localhost:27017",
        "DatabaseName": "DatabaseName"
    }
