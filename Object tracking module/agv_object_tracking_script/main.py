import cv2
import sys
import PySimpleGUI as sg
import communication
import time

# Env var
is_running = False
is_tracking = False

# Read video
video = cv2.VideoCapture("http://192.168.1.9/stream")

# Exit if video not opened.
if not video.isOpened():
    print("Could not open video")
    sys.exit()

video_height = int(video.get(4))
video_width = int(video.get(3))

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
        sg.Text('State: ', key='_state_', size=(100, 0), justification="center", font=("Arial", 11, "bold")),
        sg.Text('Time: ', key='_time_', size=(2000, 0), justification="left")
    ],
    [sg.Text("Object tracking robot", size=(200, 1), justification="center", font=("Arial", 11, "bold"))],
    [sg.Image(filename="", key="-IMAGE-", size=(video_width, video_height))],
    [sg.Button("Exit", size=(10, 1))]
]
# Create the window and show it without the plot
window = sg.Window("Object tracking robot", layout, location=(50, 50), size=(video_width, video_height + 200))

(major_ver, minor_ver, subminor_ver) = (cv2.__version__).split('.')

if __name__ == '__main__':
    # Set up tracker.
    # Instead of MIL, you can also use

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

    # Read video
    video = cv2.VideoCapture("http://192.168.1.9/stream")

    # Exit if video not opened.
    if not video.isOpened():
        print("Could not open video")
        sys.exit()

    # Read first frame.
    ok, frame = video.read()
    if not ok:
        print('Cannot read video file')
        sys.exit()

    # Define an initial bounding box
    bbox = (287, 23, 86, 320)

    # Uncomment the line below to select a different bounding box
    bbox = cv2.selectROI(frame, False)

    # Initialize tracker with first frame and bounding box
    ok = tracker.init(frame, bbox)

    while True:
        # Read a new frame
        ok, frame = video.read()
        if not ok:
            break

        # Start timer
        timer = cv2.getTickCount()

        # Update tracker
        ok, bbox = tracker.update(frame)

        # Calculate Frames per second (FPS)
        fps = cv2.getTickFrequency() / (cv2.getTickCount() - timer);

        # Draw bounding box
        if ok:
            # Tracking success
            p1 = (int(bbox[0]), int(bbox[1]))
            p2 = (int(bbox[0] + bbox[2]), int(bbox[1] + bbox[3]))
            cv2.rectangle(frame, p1, p2, (255, 0, 0), 2, 1)
        else:
            # Tracking failure
            cv2.putText(frame, "Tracking failure detected", (100, 80), cv2.FONT_HERSHEY_SIMPLEX, 0.75, (0, 0, 255), 2)

        # Display tracker type on frame
        cv2.putText(frame, tracker_type + " Tracker", (100, 20), cv2.FONT_HERSHEY_SIMPLEX, 0.75, (50, 170, 50), 2);

        # Display FPS on frame
        cv2.putText(frame, "FPS : " + str(int(fps)), (100, 50), cv2.FONT_HERSHEY_SIMPLEX, 0.75, (50, 170, 50), 2);

        # Display result
        cv2.imshow("Tracking", frame)

        # Exit if ESC pressed
        k = cv2.waitKey(1) & 0xff
        if k == 27:
            break