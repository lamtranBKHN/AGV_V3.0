using System;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace AgvPlanningModule
{
    /// <summary>
    ///  mqtt class
    /// </summary>
    public class AgvCommunication
    {
        public MqttClient m_client;
        private Logging Log = new Logging();
        Settings appConfig  = new Settings();

        private string Broker_adress;
        private string User_name;
        private string Password;
        /// <summary>
        /// constructor for mqtt class
        /// </summary>
        /// <param name="broker_adress"></param>
        /// <param name="user_name"></param>
        /// <param name="password"></param>
        public AgvCommunication()
        {
            this.Broker_adress = appConfig.MqttBrokerIp;
            this.User_name = appConfig.MqttUser;
            this.Password = appConfig.MqttPassword;
            Init();
        }
        /// <summary>
        /// destructor and destroy connections
        /// </summary>
        ~AgvCommunication()
        {
            m_client.Disconnect();
        }

        /// <summary>
        /// init function
        /// </summary>
        private void Init()
        {
            try
            {
                m_client = new MqttClient(Broker_adress);
                m_client.Connect(Guid.NewGuid().ToString(), User_name, Password);
                m_client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            }
            catch (Exception ex)
            {
                Log.PrintAndWriteLog(ex.ToString());
            }            
        }

        /// <summary>
        /// publish message
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="msg"></param>
        public void Publish(string topic, string msg)
        {
            try
            {
                ushort msgId = m_client.Publish(topic, // topic
                            Encoding.UTF8.GetBytes(msg), // message body
                            MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, // QoS level
                            false); // retained
            }
            catch (Exception ex)
            {
                Log.PrintAndWriteLog(ex.ToString());
            }
            
        }

        public void Publish(string topic, byte[] msg)
        {
            try
            {
                ushort msgId = m_client.Publish(topic, // topic
                                msg, // message body
                                MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, // QoS level
                                true); // retained
            }
            catch (Exception ex)
            {
                Log.PrintAndWriteLog(ex.ToString());
            }
        }

        /// <summary>
        /// Publish retained msg option
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="msg"></param>
        public void PublishRetainedMsg(string topic, string msg)
        {
            try
            {
                ushort msgId = m_client.Publish(topic, // topic
                                Encoding.UTF8.GetBytes(msg), // message body
                                MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, // QoS level
                                true); // retained
            }
            catch (Exception ex)
            {
                Log.PrintAndWriteLog(ex.ToString());
            }
        }

        /// <summary>
        /// subcribe topic
        /// </summary>
        /// <param name="topic"></param>
        public void SubcribeMsg(string topic)
        {
            try
            {
                m_client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            }
            catch (Exception ex)
            {
                Log.PrintAndWriteLog(ex.ToString());
            }
        }

        /// <summary>
        /// unsubcribe topic
        /// </summary>
        /// <param name="topic"></param>
        public void UnsubcribeMsg(string topic)
        {
            try
            {
                m_client.Unsubscribe(new string[] { topic });
            }
            catch (Exception ex)
            {
                Log.PrintAndWriteLog(ex.ToString());
            }
        }

        /// <summary>
        /// event print recieve msg
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            Log.PrintAndWriteLog($"{Encoding.UTF8.GetString(e.Message)}  on topic: {e.Topic}\n");
        }
    }
}

