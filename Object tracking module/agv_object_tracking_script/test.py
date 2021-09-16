import cv2
import sys
import PySimpleGUI as sg
import numpy as np
import communication
import time
import threading
import readConfig

# env variables
cam_ip = readConfig.ReadConfig().cam_ip
is_running = False
is_tracking = False
app_status = "OFF"
video = None
video_height = 480
video_width = 640
error = 0
# Off picture
default_img = cv2.imread("test.jpg")
default_img = cv2.resize(default_img, (video_width, video_height))

# Config GUI
sg.theme("Dark")
# Define the window layout
layout = [
    [
        sg.Button("Start", button_color=('white', 'green'), size=(10, 1), enable_events=True, border_width=3),
        sg.Button("Stop", button_color=('white', 'red'), size=(10, 1), enable_events=False, border_width=3),
        sg.Text('Status: OFF', key='_status_', size=(2000, 0), justification="right", font=("Arial", 11, "bold"))
    ],
    [sg.Text("Object tracking robot", size=(200, 1), justification="center", font=("Arial", 11, "bold"))],
    [sg.Image(filename="", key="-IMAGE-", size=(video_width, video_height))],
    [
        sg.Button("Select Object", size=(10, 1)),
        sg.Button("Exit", size=(10, 1))
    ]
]
# Create the window and show it without the plot
window = sg.Window("Object tracking robot", layout, location=(50, 50), size=(video_width, video_height + 120))

# Get cv2 version
(major_ver, minor_ver, subminor_ver) = cv2.__version__.split('.')

########################### Config tracker #############################
tracker_types = ['BOOSTING', 'MIL', 'KCF', 'TLD', 'MEDIANFLOW', 'CSRT', 'MOSSE']
tracker_type = tracker_types[5]

if int(minor_ver) < 3:
    tracker = cv2.Tracker_create(tracker_type)
else:
    if tracker_type == 'BOOSTING':
        tracker = cv2.TrackerBoosting_create()
    if tracker_type == 'MIL':
        tracker = cv2.TrackerMIL_create()
    if tracker_type == 'KCF':
        tracker = cv2.TrackerKCF_create()
    if tracker_type == 'TLD':
        tracker = cv2.TrackerTLD_create()
    if tracker_type == 'MEDIANFLOW':
        tracker = cv2.TrackerMedianFlow_create()
    if tracker_type == 'CSRT':
        tracker = cv2.TrackerCSRT_create()
    if tracker_type == 'MOSSE':
        tracker = cv2.TrackerMOSSE_create()
########################################################################

# Connect to broker
mqtt = communication.Communication()


def send_update():
    while True:
        if is_tracking:
            mqtt.send_update(error)
            time.sleep(0.5)


update_error_thread = threading.Thread(target=send_update)

if __name__ == "__main__":
    bbox = (287, 23, 86, 320)
    update_error_thread.daemon = True
    update_error_thread.start()
    while True:
        event, values = window.read(timeout=0)
        if event == "Start":
            is_running = True
        elif event == "Stop":
            is_running = False
            is_tracking = False
        elif event == "Select Object" and is_running and not is_tracking:
            bbox = cv2.selectROI(frame, False)
            ok = tracker.init(frame, bbox)
            is_tracking = True
            cv2.destroyAllWindows()
        if event == sg.WIN_CLOSED or event == 'Exit':
            is_tracking = False
            is_running = False
            sys.exit()
            break
        window.Element('_status_').Update(f"Status: {app_status}")
        if not is_running:
            app_status = "OFF"
            img_bytes = cv2.imencode('.png', default_img)[1].tobytes()
            window['-IMAGE-'].update(data=img_bytes)
            continue
        if not video or not video.isOpened():
            try:
                video = cv2.VideoCapture(cam_ip)
            except:
                print("Can not connect to AGV. Retrying")
            app_status = "Connecting to AGV"
        else:
            app_status = "Connected to AGV"

        if is_running and not is_tracking:
            ok, frame = video.read()
            img_bytes = cv2.imencode('.png', frame)[1].tobytes()
            window["-IMAGE-"].update(data=img_bytes)
        if is_tracking:
            ok, frame = video.read()
            # Start timer
            timer = cv2.getTickCount()

            ok, bbox = tracker.update(frame)
            if ok:
                # Tracking success
                p1 = (int(bbox[0]), int(bbox[1]))
                p2 = (int(bbox[0] + bbox[2]), int(bbox[1] + bbox[3]))
                center_width = int(bbox[0] + bbox[2] // 2)
                error = int(int(video_width / 2 - center_width) * 100 / (video_width // 2))
                print (error)
                cv2.rectangle(frame, p1, p2, (255, 0, 0), 2, 1)
            else:
                # Tracking failure
                cv2.putText(frame, "Tracking failure detected", (100, 80), cv2.FONT_HERSHEY_SIMPLEX, 0.75, (0, 0, 255),
                            2)
                # Calculate Frames per second (FPS)
            fps = cv2.getTickFrequency() / (cv2.getTickCount() - timer)

            # Display tracker type on frame
            cv2.putText(frame, tracker_type + " Tracker", (100, 20), cv2.FONT_HERSHEY_SIMPLEX, 0.75, (50, 170, 50),
                        2)

            # Display FPS on frame
            cv2.putText(frame, "FPS : " + str(int(fps)), (100, 50), cv2.FONT_HERSHEY_SIMPLEX, 0.75, (50, 170, 50),
                        2)

            img_bytes = cv2.imencode('.png', frame)[1].tobytes()
            window["-IMAGE-"].update(data=img_bytes)
        window.refresh()
    window.close()
