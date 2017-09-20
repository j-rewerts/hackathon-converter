using Google.Cloud.PubSub.V1;
using System;

namespace Converter.Services.TaskRunner
{
    class Program
    {
        private const string ANALYSIS_TOPIC_NAME = "ConverterAnalysis";
        private const string ANALYSIS_SUBSCRIPTION_NAME = "ConverterAnalysisSubscription";

        static void Main(string[] args)
        {
            // TODO: get projectId from configuration

            // configure subscriber
            string projectId = "jrewerts-project"; //Configuration["Google:ProjectId"];
            if (string.IsNullOrWhiteSpace(projectId))
                throw new InvalidOperationException("Unable to get the projectId from configuration");
            var topicNameString = ANALYSIS_TOPIC_NAME;
            var topicName = new TopicName(projectId, topicNameString);

            var subscriptionClient = SubscriberClient.Create();

            var subscriptionName = new SubscriptionName(projectId, ANALYSIS_SUBSCRIPTION_NAME);

            // create the subscription if it doesn't exist
            try
            {
                subscriptionClient.CreateSubscription(subscriptionName, topicName, new PushConfig(), 60);
            }
            catch (Grpc.Core.RpcException e) {  /* subscription already exists */ }

            while (true)
            {
                var pullResult = subscriptionClient.Pull(subscriptionName, false, 1);
                foreach (var message in pullResult.ReceivedMessages)
                {
                    try
                    {
                        string messageText = System.Text.Encoding.UTF8.GetString(message.Message.Data.ToByteArray());
                    }
                    catch (Exception err)
                    {
                        // TODO: log error and continue
                        
                    }
                }

            }
        }
    }
}
