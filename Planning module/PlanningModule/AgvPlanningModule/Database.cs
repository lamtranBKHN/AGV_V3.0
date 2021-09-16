using System;
using System.IO;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using AgvPlanningModule.DataType;

namespace AgvPlanningModule
{
    public class Database
    {
        public string m_db_server;
        public string m_db_name;
        private string m_db_user;
        private string m_db_password;
        private Logging Log         = new Logging();
        private Settings appConfig  = new Settings();

        public Dictionary<int, Agv> m_agvList                  = new Dictionary<int, Agv>();
        public Dictionary<int, Barrier> m_barrierList          = new Dictionary<int, Barrier>();
        public Dictionary<int, Charger> m_chargerList          = new Dictionary<int, Charger>();
        public Dictionary<int, Map> m_mapList                  = new Dictionary<int, Map>();
        public Dictionary<int, Pod> m_podList                  = new Dictionary<int, Pod>();
        public Dictionary<int, Station> m_stationList          = new Dictionary<int, Station>();
        public Dictionary<int, WaitingZone> m_waitingZoneList  = new Dictionary<int, WaitingZone>();
        public MapSize m_mapSize                               = new MapSize(0, 0);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db_server"></param>
        /// <param name="db_name"></param>
        /// <param name="db_user"></param>
        /// <param name="db_password"></param>
        public Database()
        {            
            this.m_db_server    = appConfig.DbServer;
            this.m_db_name      = appConfig.DbName;
            this.m_db_user      = appConfig.DbUser;
            this.m_db_password  = appConfig.DbPassword;
            try
            {
                IsConnect();
                GetAllData();
            }
            catch (Exception e)
            {
                Log.PrintAndWriteLog(e.ToString());
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private MySqlConnection connection = null;
        /// <summary>
        /// 
        /// </summary>
        public MySqlConnection Connection
        {
            get { return connection; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsConnect()
        {
            try
            {
                if (Connection == null)
                {
                    if (string.IsNullOrEmpty(m_db_name))
                        return false;
                    string connstring = string.Format("Server={0}; database={1}; UID={2}; password={3}", this.m_db_server, this.m_db_name, this.m_db_user, this.m_db_password);
                    this.connection = new MySqlConnection(connstring);
                    connection.Open();
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.PrintAndWriteLog(ex.ToString());
                return false;
            }            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public MySqlDataReader GetData(string query)
        {
            try
            {
                if (IsConnect())
                {
                    MySqlCommand cmd = new MySqlCommand(query, Connection);
                    return cmd.ExecuteReader();
                }
                return null;
            }
            catch (Exception ex)
            {
                Log.PrintAndWriteLog(ex.ToString());
                return null;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        public void GetAllData()
        {
            m_agvList           = UpdateAgv();
            m_barrierList       = UpdateBarrier();
            m_chargerList       = UpdateCharger();
            m_mapList           = UpdateMap();
            m_podList           = UpdatePod();
            m_stationList       = UpdateStation();
            m_waitingZoneList   = UpdateWaitingZone();
            m_mapSize           = UpdateMapSize();
        }
        /// <summary>
        ///   
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Agv> UpdateAgv()
        {
            Dictionary<int, Agv> newAgvList = new Dictionary<int, Agv>();
            try
            {
                string query = "SELECT * FROM agv;";
                var reader = GetData(query);
                while (reader.Read())
                {
                    int id          = reader.GetInt32(0);
                    string ip       = reader.GetString(1);
                    int port        = reader.GetInt32(2);
                    string camIp    = reader.GetString(3);
                    int orientation = reader.GetInt32(4);
                    int currentPod  = reader.GetInt32(5);
                    int battery     = reader.GetInt32(6);
                    bool isRunning  = false;
                    if (reader.GetInt32(7) == 1)
                    {
                        isRunning = true;
                    }
                    int nodeId = reader.GetInt32(8);
                    bool isAvailable = true;
                    if (reader.GetInt32(9) == 0)
                    {
                        isAvailable = false;
                    }
                    int speed = reader.GetInt32(10);
                    Agv newAgv = new Agv(id, ip, port, camIp, orientation, currentPod, battery, isRunning, nodeId, isAvailable, speed);
                    newAgvList.Add(id, newAgv);                    
                }
                CloseDatabase();
            }
            catch (Exception ex)
            {
                Log.PrintAndWriteLog(ex.ToString());
            }
            return newAgvList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Barrier> UpdateBarrier()
        {
            Dictionary<int, Barrier> newBarrierList = new Dictionary<int, Barrier>();
            try
            {
                string query = "SELECT * FROM barrier;";
                var reader = GetData(query);
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    int nodeId = reader.GetInt32(1);
                    Barrier newBarrier = new Barrier(id, nodeId);
                    newBarrierList.Add(id, newBarrier);
                }
                CloseDatabase();
            }
            catch (Exception ex)
            {
                Log.PrintAndWriteLog(ex.ToString());
            }
            return newBarrierList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Charger> UpdateCharger()
        {
            Dictionary<int, Charger> newChargerList = new Dictionary<int, Charger>();
            try
            {
                string query = "SELECT * FROM charger;";
                var reader = GetData(query);
                while (reader.Read())
                {
                    int id          = reader.GetInt32(0);
                    int nodeId      = reader.GetInt32(1);
                    int orientation = reader.GetInt32(2);
                    bool isCharging = false;

                    if (reader.GetInt32(3) == 1)
                    {
                        isCharging = true;
                    }
                    int agvId = reader.GetInt32(4);

                    Charger newCharger = new Charger(id, nodeId, orientation, isCharging, agvId);                    
                    newChargerList.Add(id, newCharger);
                }
                CloseDatabase();
            }
            catch (Exception ex)
            {
                Log.PrintAndWriteLog(ex.ToString());
            }
            return newChargerList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Map> UpdateMap()
        {
            Dictionary<int, Map> newMapList = new Dictionary<int, Map>();
            try
            {
                string query = "SELECT * FROM map;";
                var reader = GetData(query);
                while (reader.Read())
                {
                    int id      = reader.GetInt32(0);
                    string rfid = reader.GetString(1);
                    int x = reader.GetInt32(2);
                    int y       = reader.GetInt32(3);

                    bool isBlocked = false;
                    if (reader.GetInt32(4) == 1)
                    {
                        isBlocked = true;
                    }

                    bool isActive = false;
                    if (reader.GetInt32(5) == 1)
                    {
                        isActive = true;
                    }

                    Map newMap = new Map(id, rfid, x, y, isBlocked, isActive);
                    newMapList.Add(id, newMap);
                }
                CloseDatabase();
            }
            catch(Exception ex)
            {
                Log.PrintAndWriteLog(ex.ToString());
            }
            return newMapList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Pod> UpdatePod()
        {
            Dictionary<int, Pod> newPodList = new Dictionary<int, Pod>();
            try
            {
                string query    = "SELECT * FROM pod;";
                var reader      = GetData(query);
                while (reader.Read())
                {
                    int id          = reader.GetInt32(0);
                    int productId   = reader.GetInt32(1);
                    int quantity    = reader.GetInt32(2);
                    int nodeId      = reader.GetInt32(3);
                    int load        = reader.GetInt32(4);                
                    Pod newPod      = new Pod(id, productId, quantity, nodeId, load);
                    newPodList.Add(id, newPod);
                }
                CloseDatabase();
            }
            catch (Exception ex)
            {
                Log.PrintAndWriteLog(ex.ToString());
            }
            return newPodList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Station> UpdateStation()
        {
            Dictionary<int, Station> newStationList = new Dictionary<int, Station>();
            try
            {
                string query = "SELECT * FROM station;";
                var reader = GetData(query);
                while (reader.Read())
                {
                    int id              = reader.GetInt32(0);
                    int nodeId          = reader.GetInt32(1);
                    bool isAvailable    = false;
                    if (reader.GetInt32(2) == 1)
                    {
                        isAvailable = true;
                    }
                    Station newStation = new Station(id, nodeId, isAvailable);
                    newStationList.Add(id, newStation);
                }
                CloseDatabase();
            }
            catch (Exception ex)
            {
                Log.PrintAndWriteLog(ex.ToString());
            }
            return newStationList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, WaitingZone> UpdateWaitingZone()
        {
            Dictionary<int, WaitingZone> newWaitingZoneList = new Dictionary<int, WaitingZone>();
            try
            {
                string query = "SELECT * FROM waiting_zone;";
                var reader = GetData(query);
                while (reader.Read())
                {
                    int id          = reader.GetInt32(0);
                    int nodeId      = reader.GetInt32(1);
                    bool isInQuere  = false;
                    if (reader.GetInt32(2) == 1)
                    {
                        isInQuere = true;
                    }
                    WaitingZone newWaitingZone = new WaitingZone(id, nodeId, isInQuere);
                    newWaitingZoneList.Add(id, newWaitingZone);
                }
                CloseDatabase();
            }
            catch (Exception ex)
            {
                Log.PrintAndWriteLog(ex.ToString());
            }
            return newWaitingZoneList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public MapSize UpdateMapSize()
        {
            MapSize mapSize = new MapSize(0, 0);
            try
            {
                string query    = "SELECT MAX(x), MAX(y) FROM map;";
                var reader      = GetData(query);
                while (reader.Read())
                {
                    int width = reader.GetInt32(0);
                    int height = reader.GetInt32(1);
                    mapSize.Width = width + 1;
                    mapSize.Height = height + 1;
                }
                CloseDatabase();
            }
            catch (Exception ex)
            {
                Log.PrintAndWriteLog(ex.ToString());
            }
            return mapSize;
        }

        /// <summary>
        /// Update database
        /// </summary>
        /// <param name="table"></param>
        /// <param name="updateCol"></param>
        /// <param name="updateColValue"></param>
        /// <param name="updateRow"></param>
        /// <param name="updateRowValue"></param>
        public void Update(string table, string updateCol, string updateColValue, string updateRow, string updateRowValue)
        {
            string query = $"UPDATE {table} SET {updateCol}={updateColValue} WHERE {updateRow}={updateRowValue}";
            try
            {
                if (IsConnect())
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandText = query;
                    cmd.Connection = Connection;
                    cmd.ExecuteNonQuery();
                    CloseDatabase();
                }
            }
            catch (Exception ex) 
            {
                Log.PrintAndWriteLog(ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CloseDatabase()
        {
            try
            {
                connection.Close();
                connection = null;
            }
            catch (Exception ex)
            {
                Log.PrintAndWriteLog(ex.ToString());
            }
        }
    }
}
