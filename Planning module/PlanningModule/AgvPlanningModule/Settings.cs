using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace AgvPlanningModule
{
    public class Settings
    {
        private string dbServer;
        private string dbName;
        private string dbUser;
        private string dbPassword;

        private string mqttBrokerIp;
        private string mqttUser;
        private string mqttPassword;

        public string DbServer { get => dbServer; set => dbServer = value; }
        public string DbUser { get => dbUser; set => dbUser = value; }
        public string DbName { get => dbName; set => dbName = value; }
        public string DbPassword { get => dbPassword; set => dbPassword = value; }
        public string MqttBrokerIp { get => mqttBrokerIp; set => mqttBrokerIp = value; }
        public string MqttUser { get => mqttUser; set => mqttUser = value; }
        public string MqttPassword { get => mqttPassword; set => mqttPassword = value; }

        public Settings()
        {
            this.dbServer = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("database")["dbServer"].ToString();
            this.dbName = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("database")["dbName"].ToString();
            this.dbUser = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("database")["dbUser"].ToString();
            this.dbPassword = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("database")["dbPassword"].ToString();

            this.mqttBrokerIp = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("mqtt")["brokerIp"].ToString();
            this.mqttUser = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("mqtt")["user"].ToString();
            this.mqttPassword = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("mqtt")["password"].ToString();
        }
        ~Settings() { }

    }
}
