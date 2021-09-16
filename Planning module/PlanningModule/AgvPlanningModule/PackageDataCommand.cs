using System;
using System.Collections.Generic;
using System.Linq;
using AgvPlanningModule.DataType;

namespace AgvPlanningModule
{
    public class PackageDataCommand
    {
        // Define hex package element
        public const byte FRAME_HEAD = 0xBA;
        public const byte ACTION_DIRECTMOVE = 0x01;
        public const byte ACTION_ROTATE_90 = 0x02;
        public const byte ACTION_ROTATE_MINUS_90 = 0x03;
        public const byte ACTION_ROTATE_180 = 0x04;
        public const byte ACTION_STOP = 0x05;
        public const byte ACTION_LIFTUP = 0x06;
        public const byte ACTION_LIFTDOWN = 0x07;

        public const byte VELOCITY_1 = 0x01;

        public const byte DUMMY = 0x01;

        private Logging Log = new Logging();
        Database database = new Database();

        public PackageDataCommand() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentPackage"></param>
        /// <param name="actionCode"></param>
        /// <param name="rfid"></param>
        /// <param name="velocity"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public List<byte> AddSingleCommandPackage(List<byte> currentPackage, byte actionCode, List<byte> rfid, byte velocity, byte distance)
        {
            // Check RFID length
            if (rfid.Count != 4) return currentPackage;

            // Add actioncode
            currentPackage.Add(actionCode);
            // Add RFID
            currentPackage.AddRange(rfid);
            // Add velocity
            currentPackage.Add(velocity);
            // Add distance
            currentPackage.Add(distance);

            return currentPackage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionCode"></param>
        /// <param name="rfid"></param>
        /// <param name="velocity"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public List<byte> CreateSingleCommandPackage(byte actionCode, List<byte> rfid, byte velocity, byte distance)
        {
            // Check RFID length
            if (rfid.Count != 4) return null;

            List<byte> newCommand = new List<byte>();
            // Add actioncode
            newCommand.Add(actionCode);
            // Add RFID
            newCommand.AddRange(rfid);
            // Add velocity
            newCommand.Add(velocity);
            // Add distance
            newCommand.Add(distance);

            return newCommand;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentNode"></param>
        /// <param name="nextNode"></param>
        /// <returns></returns>
        private int CalculateRotationAngle(int currentNode, int nextNode)
        {
            int differentAngle = 0;
            Dictionary<int, Map> mapList = database.UpdateMap();
            MapSize mapSize = database.UpdateMapSize();
            /*          0 
                        |
                270---current---90
                        |            
                       180          */
            if (nextNode - currentNode == mapSize.Width)
            {
                differentAngle = 180;
            }
            else if (nextNode - currentNode == 1)
            {
                differentAngle = 90;

            }
            else if (nextNode - currentNode == -mapSize.Width)
            {
                differentAngle = 0;

            }
            else if (nextNode - currentNode == -1)
            {
                differentAngle = 270;
            }

            return differentAngle;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        private List<byte> GetRfid(int nodeid)
        {
            Dictionary<int, Map> mapList = database.UpdateMap();
            string rfidString = mapList[nodeid].Rfid;
            List<byte> rfid = rfidString.Split(' ').Select(s => Convert.ToByte(s, 16)).ToList();
            return rfid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agvId"></param>
        /// <param name="planAgvToPod"></param>
        /// <param name="planPodtoEnd"></param>
        /// <returns></returns>
        public byte[] CreateActionPackageToAgv(int agvId, List<int> planAgvToPod, List<int> planPodtoEnd)
        {
            byte[] actionPackage = new byte[] { };
            List<Byte> actionPackageList = new List<byte>();
            // Frame head
            actionPackageList.Add(FRAME_HEAD);
            // AGV ID
            actionPackageList.Add(Convert.ToByte(agvId));

            // Frame length
            // Dummy and calculate after create frame complete
            short dummyFrameLength = 0;
            byte[] byteLenghtArray = BitConverter.GetBytes(dummyFrameLength);
            List<byte> byteFrameLengthList = new List<byte>(byteLenghtArray);
            actionPackageList.AddRange(byteFrameLengthList);

            // Timestamp
            var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            actionPackageList.AddRange(BitConverter.GetBytes(unixTimestamp).ToList());

            // Waypoint
            actionPackageList.AddRange(CreateWaypointPackage(agvId, planAgvToPod, planPodtoEnd));

            // Calculate frame length after create command package
            short packageLength = Convert.ToInt16(actionPackageList.Count);
            byteLenghtArray = BitConverter.GetBytes(packageLength);
            // Little Endian
            actionPackageList[2] = byteLenghtArray[0];
            actionPackageList[3] = byteLenghtArray[1];

            // Convert to array
            actionPackage = actionPackageList.ToArray();
            return actionPackage;
        }


        public List<byte> CreateWaypointPackage(int agvId, List<int> fromAgvToPod, List<int> fromPodToDest)
        {
            /* DATA(1) --- RFID(4) --- Velocity(1) --- Distance(1) */

            List<byte> waypoint = new List<byte>();
            // Read begin current agv direction (last direction from db)
            Dictionary<int, Agv> agvList = database.UpdateAgv();
            int agvDirection = agvList[agvId].Orientation;

            // Create Waypoint from agv to pod
            int currentMovingDirection = 0;
            if (fromAgvToPod.Count() > 1)
            {
                int processPosition = 0;
                int start = 0;
                int end = 0;
                currentMovingDirection = CalculateRotationAngle(fromAgvToPod.ElementAt(processPosition), fromAgvToPod.ElementAt(processPosition + 1));

                int nextMovingDirection = currentMovingDirection;
                for (processPosition = 0; processPosition <= fromAgvToPod.Count() - 1; processPosition++)
                {
                    // Check and create rotation command
                    var currentRfid = GetRfid(fromAgvToPod.ElementAt(start));
                    var rotateCommand = CheckAndAddRotateCommand(agvDirection, currentMovingDirection, currentRfid);
                    if (rotateCommand != null)
                    {
                        waypoint.AddRange(rotateCommand);
                        agvDirection = currentMovingDirection;
                    }

                    if (processPosition < fromAgvToPod.Count() - 1)
                    {
                        nextMovingDirection = CalculateRotationAngle(fromAgvToPod.ElementAt(processPosition), fromAgvToPod.ElementAt(processPosition + 1));
                        if (currentMovingDirection == nextMovingDirection)
                        {
                            end = processPosition;
                            currentMovingDirection = CalculateRotationAngle(fromAgvToPod.ElementAt(processPosition), fromAgvToPod.ElementAt(processPosition + 1));
                            if (processPosition < fromAgvToPod.Count() - 2)
                            {
                                continue;
                            }
                        }
                    }
                    //TODO: create direct move waypoint
                    int straightWaypointLength = end - start + 1;

                    while (straightWaypointLength > 0)
                    {
                        int currentNodeId = fromAgvToPod.ElementAt(end - straightWaypointLength + 1);
                        List<byte> rfid = GetRfid(currentNodeId);
                        int straightDistance = straightWaypointLength;
                        waypoint.AddRange(CreateSingleCommandPackage(ACTION_DIRECTMOVE, rfid, VELOCITY_1, Convert.ToByte(straightDistance)));
                        straightWaypointLength--;
                    }

                    end = processPosition;
                    start = end;

                    if (processPosition < fromAgvToPod.Count() - 1)
                    {
                        currentMovingDirection = CalculateRotationAngle(fromAgvToPod.ElementAt(processPosition), fromAgvToPod.ElementAt(processPosition + 1));
                    }
                }
            }

            // Create command lift POD up
            // Last node from agv to pod
            int lastNodeIdFromAgvToPod = fromAgvToPod.Last();
            waypoint.AddRange(CreateSingleCommandPackage(ACTION_LIFTUP, GetRfid(lastNodeIdFromAgvToPod), VELOCITY_1, DUMMY));

            // create hex package from pod to destnation
            if (fromPodToDest.Count() > 1)
            {
                int processPosition = 0;
                int start = 0;
                int end = 0;
                currentMovingDirection = CalculateRotationAngle(fromPodToDest.ElementAt(processPosition), fromPodToDest.ElementAt(processPosition + 1));

                int nextMovingDirection = currentMovingDirection;

                for (processPosition = 0; processPosition < fromPodToDest.Count() - 1; processPosition++)
                {
                    // Check and create rotation command
                    var currentRfid = GetRfid(fromPodToDest.ElementAt(start));
                    var rotateCommand = CheckAndAddRotateCommand(agvDirection, currentMovingDirection, currentRfid);
                    if (rotateCommand != null)
                    {
                        waypoint.AddRange(rotateCommand);
                        agvDirection = currentMovingDirection;
                    }

                    if (processPosition < fromPodToDest.Count() - 1)
                    {
                        nextMovingDirection = CalculateRotationAngle(fromPodToDest.ElementAt(processPosition), fromPodToDest.ElementAt(processPosition + 1));
                        if (currentMovingDirection == nextMovingDirection)
                        {
                            end = processPosition;
                            currentMovingDirection = CalculateRotationAngle(fromPodToDest.ElementAt(processPosition), fromPodToDest.ElementAt(processPosition + 1));
                            if (processPosition < fromPodToDest.Count() - 2)
                            {
                                continue;
                            }                            
                        }
                    }

                    //TODO: create direct move waypoint
                    int straightWaypointLength = end - start + 1;

                    while (straightWaypointLength > 0)
                    {
                        int currentNodeId = fromPodToDest.ElementAt(end - straightWaypointLength + 1);
                        List<byte> rfid = GetRfid(currentNodeId);
                        int straightDistance = straightWaypointLength;
                        waypoint.AddRange(CreateSingleCommandPackage(ACTION_DIRECTMOVE, rfid, VELOCITY_1, Convert.ToByte(straightDistance)));
                        straightWaypointLength--;
                    }
                    end = processPosition;
                    start = end;
                    if (processPosition < fromPodToDest.Count() - 1)
                    {
                        currentMovingDirection = CalculateRotationAngle(fromPodToDest.ElementAt(processPosition), fromPodToDest.ElementAt(processPosition + 1));
                    }
                }
            }

            // Lift down pod command
            int lastNodeIdFromPodToDest = fromPodToDest.Last();
            waypoint.AddRange(CreateSingleCommandPackage(ACTION_LIFTDOWN, GetRfid(lastNodeIdFromPodToDest), VELOCITY_1, DUMMY));
            // Stop command
            waypoint.AddRange(CreateSingleCommandPackage(ACTION_STOP, GetRfid(lastNodeIdFromPodToDest), VELOCITY_1, DUMMY));
            return waypoint;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="planAgvToPod"></param>
        /// <param name="planPodtoEnd"></param>
        /// <returns></returns>
        public string UpdateDashboardRoute(List<int> planAgvToPod, List<int> planPodtoEnd)
        {
            try
            {
                string msgUpdateDashboard = "";

                msgUpdateDashboard += string.Join(":", planAgvToPod);
                msgUpdateDashboard += "@";
                msgUpdateDashboard += string.Join(":", planPodtoEnd);

                return msgUpdateDashboard;
            }
            catch (Exception ex)
            {
                Log.PrintAndWriteLog(ex.ToString());
                return null;
            }
        }

        private List<byte> CheckAndAddRotateCommand(int agvDirection, int movingDirection, List<byte> currentRfid)
        {
            List<byte> rotateCommand = new List<byte>();
            int differentAngle = movingDirection - agvDirection;
            if (differentAngle == 0) return null;
            switch (differentAngle)
            {
                case -90:
                    rotateCommand = CreateSingleCommandPackage(ACTION_ROTATE_MINUS_90, currentRfid, VELOCITY_1, DUMMY);
                    break;
                case 90:
                    rotateCommand = CreateSingleCommandPackage(ACTION_ROTATE_90, currentRfid, VELOCITY_1, DUMMY);
                    break;
                case 180:
                    rotateCommand = CreateSingleCommandPackage(ACTION_ROTATE_180, currentRfid, VELOCITY_1, DUMMY);
                    break;
                case -180:
                    rotateCommand = CreateSingleCommandPackage(ACTION_ROTATE_180, currentRfid, VELOCITY_1, DUMMY);
                    break;
                case 270:
                    rotateCommand = CreateSingleCommandPackage(ACTION_ROTATE_MINUS_90, currentRfid, VELOCITY_1, DUMMY);
                    break;
                case -270:
                    rotateCommand = CreateSingleCommandPackage(ACTION_ROTATE_90, currentRfid, VELOCITY_1, DUMMY);
                    break;
                default:
                    break;
            }

            return rotateCommand;
        }

        /// <summary>
        /// Create waypoint hex package from node A to node B
        /// </summary>
        /// <param name="agvId"></param>
        /// <param name="fromNodeToNode"></param>
        /// <returns></returns>
        public List<byte> CreateWaypointBetween2Node(int agvId, List<int> fromNodeToNode)
        {
            /* DATA(1) --- RFID(4) --- Velocity(1) --- Distance(1) */

            List<byte> waypoint = new List<byte>();
            // Read begin current agv direction (last direction from db)
            Dictionary<int, Agv> agvList = database.UpdateAgv();
            int agvDirection = agvList[agvId].Orientation;

            // Create Waypoint from agv to pod
            int currentMovingDirection = 0;
            if (fromNodeToNode.Count() > 1)
            {
                int processPosition = 0;
                int start = 0;
                int end = 0;
                currentMovingDirection = CalculateRotationAngle(fromNodeToNode.ElementAt(processPosition), fromNodeToNode.ElementAt(processPosition + 1));

                int nextMovingDirection = currentMovingDirection;
                for (processPosition = 0; processPosition <= fromNodeToNode.Count() - 1; processPosition++)
                {
                    // Check and create rotation command
                    var currentRfid = GetRfid(fromNodeToNode.ElementAt(start));
                    var rotateCommand = CheckAndAddRotateCommand(agvDirection, currentMovingDirection, currentRfid);
                    if (rotateCommand != null)
                    {
                        waypoint.AddRange(rotateCommand);
                        agvDirection = currentMovingDirection;
                    }

                    if (processPosition < fromNodeToNode.Count() - 1)
                    {
                        nextMovingDirection = CalculateRotationAngle(fromNodeToNode.ElementAt(processPosition), fromNodeToNode.ElementAt(processPosition + 1));
                        if (currentMovingDirection == nextMovingDirection)
                        {
                            end = processPosition;
                            currentMovingDirection = CalculateRotationAngle(fromNodeToNode.ElementAt(processPosition), fromNodeToNode.ElementAt(processPosition + 1));
                            if (processPosition < fromNodeToNode.Count() - 2)
                            {
                                continue;
                            }
                        }
                    }
                    //TODO: create direct move waypoint
                    int straightWaypointLength = end - start + 1;

                    while (straightWaypointLength > 0)
                    {
                        int currentNodeId = fromNodeToNode.ElementAt(end - straightWaypointLength + 1);
                        List<byte> rfid = GetRfid(currentNodeId);
                        int straightDistance = straightWaypointLength;
                        waypoint.AddRange(CreateSingleCommandPackage(ACTION_DIRECTMOVE, rfid, VELOCITY_1, Convert.ToByte(straightDistance)));
                        straightWaypointLength--;
                    }

                    end = processPosition;
                    start = end;

                    if (processPosition < fromNodeToNode.Count() - 1)
                    {
                        currentMovingDirection = CalculateRotationAngle(fromNodeToNode.ElementAt(processPosition), fromNodeToNode.ElementAt(processPosition + 1));
                    }
                }
            }
            int lastNodeIdFromPodToDest = fromNodeToNode.Last();
            // Stop command
            waypoint.AddRange(CreateSingleCommandPackage(ACTION_STOP, GetRfid(lastNodeIdFromPodToDest), VELOCITY_1, DUMMY));
            return waypoint;
        }

        ~PackageDataCommand() { }
    }
}
