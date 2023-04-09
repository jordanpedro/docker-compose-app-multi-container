// See https://aka.ms/new-console-template for more information
using Dapper;
using MongoDB.Bson;
using MongoDB.Driver;
using StackExchange.Redis;
using System.Data.SqlClient;
using System.Text.Json;

#region Connecting to Redis
var redisDatabase = ConnectionMultiplexer.Connect("redis:6379").GetDatabase();
#endregion

#region Connecting to SQL Server
var connectionString = @"Server=sqlserver,1433;Database=TestDB;User ID=sa;Password=SqlServer2@23;Trusted_Connection=False; TrustServerCertificate=True;";
using var sqlServerConnection = new SqlConnection(connectionString);
sqlServerConnection.Open();
#endregion

#region Connecting to MongoDB
MongoClient dbClient = new MongoClient("mongodb://root:Mongo2023!@mongo:27017/");
var mongoDatabase = dbClient.GetDatabase("TestDB");
var mongoCollection = mongoDatabase.GetCollection<User>("Users");
#endregion

var count = 0;
var limit = 1000;

while (count < limit)
{
    var guid = Guid.NewGuid().ToString();

    await redisDatabase.StringSetAsync($"Keys:name:{guid}", $"Jordan Code {guid}");

    await sqlServerConnection.ExecuteAsync("INSERT INTO [Users] VALUES(@FirstName, @LastName)", new { FirstName = "Jordan", LastName = $"Code {guid}" });

    await mongoCollection.InsertOneAsync(new User { FirstName = "Jordan", LastName = $"Code {guid}" });

    Console.WriteLine($"LOOP: {count} , GUID: {guid}");

    count++; 

    await Task.Delay(1000);
}
public class User
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}