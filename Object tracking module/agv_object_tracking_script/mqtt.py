import readConfig
import random
from paho.mqtt import client as mqtt_client
from threading import Thread


class Mqtt:
    broker = ""
    port = ""
    topic = ""
    client_id = ""
    account = ""
    password = ""
    client = None

    def thread_handle_mqtt(self, arg):
        self.client.loop_forever()

    def __init__(self):
        config = readConfig.ReadConfig()
        self.broker = config.mqtt['host']
        self.port = config.mqtt['port']
        self.topic = config.mqtt['topic']
        self.account = config.mqtt['account']
        self.password = config.mqtt['password']
        self.client_id = f'python-mqtt-{random.randint(0, 1000)}'
        self.connect_mqtt()

    def connect_mqtt(self):
        def on_connect(client, userdata, flags, rc):
            if rc == 0:
                print("Connected to MQTT Broker!")
            else:
                print("Failed to connect, return code %d\n", rc)

        # Set Connecting Client ID
        self.client = mqtt_client.Client(self.client_id)
        self.client.username_pw_set(self.account, self.password)
        self.client.on_connect = on_connect
        self.client.connect(self.broker, self.port)
        # handle_mqtt = Thread(target=self.thread_handle_mqtt, args=(10,))
        # handle_mqtt.start()

    def publish(self, msg):
        result = self.client.publish(self.topic, msg)
        status = result[0]
        if status != 0:
            print(f"Failed to send message")
