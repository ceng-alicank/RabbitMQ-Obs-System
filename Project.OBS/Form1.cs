using Newtonsoft.Json;
using Project.Common;
using Project.OBS.DAL.Contexts;
using Project.OBS.DAL.Interfaces;
using Project.OBS.DAL.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Project.OBS
{
    public partial class Form1 : Form
    {
        private readonly ITranscriptModelRepository _transcriptModelRepository;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string createDocument = "CreateDocument";
        private readonly string documentCreated = "DocumentCreated";
        private readonly string createDocumentExchange = "CreateDocumentExchange";
        
        public Form1()
        {
            _connection = GetConnection();
            _channel = GetChannel();
            _transcriptModelRepository = new TranscriptModelRepository(new AppDbContext());
            InitializeComponent();
        }
        
        private void BtnConnect_Click(object sender, EventArgs e)
        {
            ConfigureRabbitMQDefinition();
            var consumerEvent = new EventingBasicConsumer(_channel);
            consumerEvent.Received += (ch, ea) =>
            {
                var modelReceived = JsonConvert.DeserializeObject<PdfCreatorModel>(Encoding.UTF8.GetString(ea.Body.ToArray()));
                MessageBox.Show(modelReceived.PdfUrl);
            };
            _channel.BasicConsume(documentCreated, true, consumerEvent);
        }
        private void BtnSend_Click(object sender, EventArgs e)
        {
            var modelTosend = new TranscriptModel()
            {
                Name = TxtName.Text,
                Grade = Convert.ToInt32(TxtGrade.Text)
            };
            _transcriptModelRepository.AddAsync(modelTosend);
            WriteToQueue(createDocument, modelTosend);
        }
        private IModel GetChannel()
        {
            return _connection.CreateModel();
        }
        private IConnection GetConnection()
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
        private void WriteToQueue(string queueName, TranscriptModel model)
        {
            var messageArr = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model));
            _channel.BasicPublish(createDocumentExchange, queueName, null, messageArr);
        }
        private void ConfigureRabbitMQDefinition()
        {
            _channel.ExchangeDeclare(createDocumentExchange, "direct");
            _channel.QueueDeclare(createDocument, false, false, false);
            _channel.QueueBind(createDocument, createDocumentExchange, createDocument);
            _channel.QueueDeclare(documentCreated, false, false, false);
            _channel.QueueBind(documentCreated, createDocumentExchange, documentCreated);
        }

    }
}