import mqtt


class Communication:
    mqttConnection = mqtt.Mqtt()

    def __init__(self):
        print("Communication module started")

    def send_update(self, error):
        is_positive = "01"
        if error < 0:
            is_positive = "00"
        error = abs(error)
        error_in_hex = hex(error)[2:]
        if error < 16:
            error_in_hex = f"0{error_in_hex}"
        hex_str = f"ba 01 0F 00 11 11 11 11 15 AA AA AA AA AA {is_positive} {error_in_hex}"
        msg = bytearray.fromhex(hex_str)
        try:
            self.mqttConnection.publish(msg)
        except:
            print("Exception at send update to AGV")

    def __del__(self):
        print("Communication module ended")
