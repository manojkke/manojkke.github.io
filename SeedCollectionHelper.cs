    public static class SeedCollectionHelper
    {
        public static void SeedTestData<T>(IMongoCollection<T> context, string dataFilePath, string filename)
        {
            var filePath = $"{dataFilePath}{filename}.json";
            using (StreamReader file = new StreamReader(filePath))
            {
                List<T> collection = BsonSerializer.Deserialize<List<T>>(file.ReadToEnd());
                foreach (var doc in collection)
                {
                    context.InsertOne(doc);
                }
            }
        }

    }
