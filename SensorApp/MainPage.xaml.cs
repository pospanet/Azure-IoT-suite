using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ppatierno.AzureSBLite;
using ppatierno.AzureSBLite.Messaging;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SensorApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const string ConnectionString = "";
        private const string EventHubName = "";

        public MainPage()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            //ServiceBusConnectionStringBuilder builder = new ServiceBusConnectionStringBuilder(ConnectionString)
            //{
            //    TransportType = TransportType.Amqp
            //};

            MessagingFactory factory = MessagingFactory.CreateFromConnectionString(ConnectionString);

            EventHubClient client = factory.CreateEventHubClient(EventHubName);

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof (CustomData));

            CustomData data = new CustomData
            {
                Date = DateTime.UtcNow,
                Message = "Test message"
            };

            string body;
            using (MemoryStream ms = new MemoryStream())
            using (StreamReader sr = new StreamReader(ms))
            {
                serializer.WriteObject(ms, data);
                ms.Seek(0, SeekOrigin.Begin);
                body = sr.ReadToEnd();
            }

            EventData message = new EventData(Encoding.UTF8.GetBytes(body));

            /*  Alternative serialization

            EventData message = new EventData(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data)));

            */
            message.Properties["time"] = DateTime.UtcNow;

            client.Send(message);

            client.Close();
            factory.Close();
        }
    }
}