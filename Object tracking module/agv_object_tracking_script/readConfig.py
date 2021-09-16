import json


class ReadConfig:
    mqtt = {'host': "", 'port': 0, 'account': "", 'password': ""}
    agv_id = ""
    agv_name = ""
    cam_ip = ""

    def __init__(self):
        try:
            config_json = json.load(open('config.json'))
            self.mqtt = config_json['mqtt']
            self.agv_id = config_json['agv_id']
            self.agv_name = config_json['agv_name']
            self.cam_ip = config_json['cam_ip']
        except:
            print("Cannot get config information")

    def mqtt(self):
        return self.mqtt

    def agv_id(self):
        return self.agv_id

    def agv_name(self):
        return self.agv_name

    def cam_ip(self):
        return self.cam_ip

    def __del__(self):
        print("Exit read config")
