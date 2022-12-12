// See https://aka.ms/new-console-template for more information
using MongoDB.Driver;
using Newtonsoft.Json;
using Project.Common;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

IMongoCollection<PdfCreatorModel> _collection;
IConnection _connection;
IModel _channel;
string createDocument = "CreateDocument";
string documentCreated = "DocumentCreated";
string createDocumentExchange = "CreateDocumentExchange";



_collection = GetMongoDbCollection();
_connection = GetConnection();
_channel = GetChannel();
ConfigureRabbitMQDefinition();
Console.WriteLine("Connected");



var consumerEvent = new EventingBasicConsumer(_channel);
consumerEvent.Received += (ch, ea) =>
{
    var modelReceived = JsonConvert.DeserializeObject<TranscriptModel>(Encoding.UTF8.GetString(ea.Body.ToArray()));
    Console.WriteLine("Creating pdf");//fake pdf creating process
    var pdfCreatorModel = new PdfCreatorModel()
    {
        PdfUrl = "localhost:1000/doc/requests/user_123123.pdf",
        TranscriptModel = modelReceived
    };
    _collection.InsertOne(pdfCreatorModel);
    WriteToQueue(documentCreated, pdfCreatorModel);
};
_channel.BasicConsume( createDocument , true ,consumerEvent);
Console.ReadLine();



void ConfigureRabbitMQDefinition()
{
    _channel.ExchangeDeclare(createDocumentExchange, "direct");
    _channel.QueueDeclare(createDocument, false, false, false);
    _channel.QueueBind(createDocument, createDocumentExchange, createDocument);
    _channel.QueueDeclare(documentCreated, false, false, false);
    _channel.QueueBind(documentCreated, createDocumentExchange, documentCreated);

}
IModel GetChannel()
{
    return _connection.CreateModel();
}
IConnection GetConnection()
{
    var connectionFactory = new ConnectionFactory()
    {

        HostName = "localhost",
        UserName = "guest",
        Password = "guest",
        VirtualHost = "/",
        Port = -1
    };
    return connectionFactory.CreateConnection();
}
void WriteToQueue(string queueName, object model)
{
    var messageArr = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model));
    _channel.BasicPublish(createDocumentExchange, queueName, null, messageArr);
}


IMongoCollection<PdfCreatorModel> GetMongoDbCollection()
{
    var settings = MongoClientSettings.FromConnectionString("mongodb+srv://username:password@pdfappcluster.uukoqer.mongodb.net/?retryWrites=true&w=majority");
    settings.ServerApi = new ServerApi(ServerApiVersion.V1);
    var client = new MongoClient(settings);
    var database = client.GetDatabase("PdfDb");
    return database.GetCollection<PdfCreatorModel>("Table");
}