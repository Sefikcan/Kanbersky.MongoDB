# Kanbersky.MongoDB
 -> Y�klenilen projenin Startup.cs s�n�f�n�n ConfigureServices metoduna 
    services.RegisterKanberskyMongoDB(configuration) eklenir.
 -> AppSettings.json dosyas�na a�a��daki formatta ekleme yap�lmal�d�r.
    "MongoDBSettings": {
        "ConnectionStrings": "mongodb://localhost:27017",
        "DatabaseName": "DatabaseName"
    }