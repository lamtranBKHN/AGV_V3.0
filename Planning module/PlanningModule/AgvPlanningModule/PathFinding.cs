using System;
using System.IO;
using System.Linq;
using System.Collections;
using AgvPlanningModule.DataType;
using System.Collections.Generic;

namespace AgvPlanningModule
{

    public class PathFinding : Database
    {
        private Logging Log = new Logging();
        public PathFinding() : base() { }

        ~PathFinding() { }
        public class NodeInfo
        {
            public int node;
            public double f, g, h;
            // f = g + h
            public NodeInfo parent;
            public NodeInfo(int node, double f, double g, double h)
            {
                this.node = node;
                this.f = f;
                this.g = g;
                this.h = h;
                this.parent = null;
            }
        }

        // Utility Functions
        /// <summary>
        /// Check Blocked node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private bool CheckBlocked(Map node)
        {
            if (node.IsBlocked) return true;
            else return false;
        }
        /// <summary>
        /// Check Blocked node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private bool CheckBlocked(int node)
        {
            if (this.m_mapList[node].IsBlocked) return true;
            else return false;
        }

        // calculate h(n) cost
        /// <summary>
        /// Calculate H Cost
        /// </summary>
        /// <param name="FirstNode"></param>
        /// <param name="SecondNode"></param>
        /// <returns></returns>
        private double CalculateHCost(Map FirstNode, Map SecondNode)
        {
            try
            {
                double Hcost = Math.Sqrt((FirstNode.X - SecondNode.X) * (FirstNode.X - SecondNode.X) + (FirstNode.Y - SecondNode.Y) * (FirstNode.Y - SecondNode.Y));
                return Hcost;
            }
            catch (Exception e)
            {
                Log.PrintAndWriteLog($"Invalid node {e.ToString()}");
                return 0;
            }
        }
        /// <summary>
        /// Calculate H Cost
        /// </summary>
        /// <param name="FirstNode"></param>
        /// <param name="SecondNode"></param>
        /// <returns></returns>
        private double CalculateHCost(int FirstNode, int SecondNode)
        {
            try
            {
                double Hcost = Math.Sqrt((this.m_mapList[FirstNode].X - this.m_mapList[SecondNode].X) * (this.m_mapList[FirstNode].X - this.m_mapList[SecondNode].X) + (this.m_mapList[FirstNode].Y - this.m_mapList[SecondNode].Y) * (this.m_mapList[FirstNode].Y - this.m_mapList[SecondNode].Y));
                return Hcost;
            }
            catch (Exception e)
            {
                Log.PrintAndWriteLog($"Invalid node {e.ToString()}");
                return 0;
            }
        }

        // check neighbor
        /// <summary>
        /// Validate Neighbor
        /// </summary>
        /// <param name="current"></param>
        /// <param name="neighbor"></param>
        /// <returns></returns>
        private bool CheckNeighbor(Map current, Map neighbor)
        {
            if (!this.m_mapList.ContainsKey(neighbor.Id)) return false;
            if (CheckBlocked(neighbor)) return false;
            // neighboor is at the edge of the map
            if (neighbor.X == 0 && neighbor.X == (this.m_mapSize.Width - 1)) return false;
            return true;
        }
        /// <summary>
        /// Validate Neighbor
        /// </summary>
        /// <param name="current"></param>
        /// <param name="neighbor"></param>
        /// <returns></returns>
        private bool CheckNeighbor(int current, int neighbor)
        {
            if (!this.m_mapList.ContainsKey(neighbor) || !this.m_mapList.ContainsKey(current)) return false;
            if (CheckBlocked(neighbor)) return false;
            // neighboor is at the edge of the map
            if (this.m_mapList[current].X == 0 && this.m_mapList[neighbor].X == (this.m_mapSize.Width - 1)) return false;
            if (this.m_mapList[current].X == (this.m_mapSize.Width - 1) && this.m_mapList[neighbor].X == 0) return false;
            return true;
        }

        /* 
        Generating all the 8 successor of this cell 

            N.W   N   N.E 
              \   |   / 
               \  |  / 
            W----Cell----E 
                 / | \ 
               /   |  \ 
            S.W    S   S.E

        Cell-->Popped Cell (i, j) 
        N -->  North       (i-1, j) 
        S -->  South       (i+1, j) 
        E -->  East        (i, j+1) 
        W -->  West           (i, j-1) 
        N.E--> North-East  (i-1, j+1) 
        N.W--> North-West  (i-1, j-1) 
        S.E--> South-East  (i+1, j+1) 
        S.W--> South-West  (i+1, j-1)*/

        public List<int> FindRoute(int start_node, int end_node, bool safetyMode)
        {
            this.m_mapList = UpdateMap();
            if (!safetyMode)
            {
                this.m_mapList[end_node].IsBlocked = false;
                this.m_mapList[start_node].IsBlocked = false;
            }
            if (!this.m_mapList.ContainsKey(start_node))
            {                
                Log.PrintAndWriteLog($"Invalid begin node{start_node}");
                return null;
            }
            if (!this.m_mapList.ContainsKey(end_node))
            {                
                Log.PrintAndWriteLog($"Invalid end node {end_node}");
                return null;
            }

            if (CheckBlocked(end_node))
            {
                Log.PrintAndWriteLog($"End node is blocked {end_node}");
                return null;
            }
            int i;

            List<NodeInfo> node = new List<NodeInfo>();

            for (i = 0; i < this.m_mapList.Count; i++)
            {
                NodeInfo thisNode = new NodeInfo(i, int.MaxValue, int.MaxValue, int.MaxValue);
                node.Add(thisNode);
            }

            i = start_node;
            node[i].node = i;
            node[i].f = 0;
            node[i].g = 0;
            node[i].h = 0;

            List<NodeInfo> openList = new List<NodeInfo>();
            openList.Add(node[i]);

            while (openList.Any())
            {
                int current = openList.First().node;
                double fMin = int.MaxValue;
                for (i = 0; i < openList.Count(); i++)
                {
                    if (openList[i].f < fMin)
                    {
                        fMin = openList[i].f;
                        current = openList[i].node;
                    }
                }
                int northNeighbor = current - this.m_mapSize.Width;
                int southNeighbor = current + this.m_mapSize.Width;
                int westNeighbor = current - 1;
                int eastNeighbor = current + 1;
                for (i = 0; i < openList.Count(); i++)
                {
                    if (openList[i].node == current) openList.RemoveAt(i);
                }

                //closedList[current] = true;
                if (current == end_node)
                {
                    //((ArrayList)this.mapList[end_node])[3] = 1;
                    Log.PrintAndWriteLog($"Found best route which distance is: {node[current].f}");
                    string waypointLog = "Route path is:";
                    NodeInfo temp = node[end_node];
                    List<int> plan = new List<int>();
                    while (temp.parent != null)
                    {
                        waypointLog += $" {temp.node} <- ";
                        plan.Add(temp.node);
                        temp = temp.parent;
                    }
                    waypointLog += $"{start_node}";
                    // Print Log
                    Log.PrintAndWriteLog(waypointLog);
                    plan.Add(start_node);
                    plan.Reverse();
                    return plan;
                }
                double fNew, gNew, hNew;

                // North//
                if (CheckNeighbor(current, northNeighbor))
                {
                    gNew = node[current].g + 1;
                    hNew = CalculateHCost(northNeighbor, end_node);
                    fNew = gNew + hNew;

                    if (node[northNeighbor].f == int.MaxValue || node[northNeighbor].f > fNew)
                    {
                        node[northNeighbor].f = fNew;
                        node[northNeighbor].g = gNew;
                        node[northNeighbor].h = hNew;
                        node[northNeighbor].parent = node[current];
                        openList.Add(node[northNeighbor]);
                    }
                }

                // South
                if (CheckNeighbor(current, southNeighbor))
                {
                    gNew = node[current].g + 1;
                    hNew = CalculateHCost(southNeighbor, end_node);
                    fNew = gNew + hNew;

                    if (node[southNeighbor].f == int.MaxValue || node[southNeighbor].f > fNew)
                    {
                        node[southNeighbor].f = fNew;
                        node[southNeighbor].g = gNew;
                        node[southNeighbor].h = hNew;
                        node[southNeighbor].parent = node[current];
                        openList.Add(node[southNeighbor]);
                    }
                }

                // West
                if (CheckNeighbor(current, westNeighbor))
                {
                    gNew = node[current].g + 1;
                    hNew = CalculateHCost(westNeighbor, end_node);
                    fNew = gNew + hNew;

                    if (node[westNeighbor].f == int.MaxValue || node[westNeighbor].f > fNew)
                    {
                        node[westNeighbor].f = fNew;
                        node[westNeighbor].g = gNew;
                        node[westNeighbor].h = hNew;
                        node[westNeighbor].parent = node[current];
                        openList.Add(node[westNeighbor]);
                    }
                }
                // East
                if (CheckNeighbor(current, eastNeighbor))
                {
                    gNew = node[current].g + 1;
                    hNew = CalculateHCost(eastNeighbor, end_node);
                    fNew = gNew + hNew;

                    if (node[eastNeighbor].f == int.MaxValue || node[eastNeighbor].f > fNew)
                    {
                        node[eastNeighbor].f = fNew;
                        node[eastNeighbor].g = gNew;
                        node[eastNeighbor].h = hNew;
                        node[eastNeighbor].parent = node[current];
                        openList.Add(node[eastNeighbor]);
                    }
                }

            }
            Log.PrintAndWriteLog($"Can't find route: {start_node} : {end_node}");
            return null;
        } 


        // Calculate Shortest Distance between two nodes

        public double FindShortestDistance(int start_node, int end_node)
        {
            try
            {
                this.m_mapList = UpdateMap();
                this.m_mapList[end_node].IsBlocked = false;
                this.m_mapList[start_node].IsBlocked = false;
                if (!this.m_mapList.ContainsKey(start_node))
                {
                    Log.PrintAndWriteLog($"Invalid begin node{start_node}");
                    return Int32.MaxValue;
                }
                if (!this.m_mapList.ContainsKey(end_node))
                {
                    Log.PrintAndWriteLog($"Invalid end node{end_node}");
                    return Int32.MaxValue;
                }

                int i;
                List<NodeInfo> node = new List<NodeInfo>();

                for (i = 0; i < this.m_mapList.Count; i++)
                {
                    NodeInfo thisNode = new NodeInfo(i, int.MaxValue, int.MaxValue, int.MaxValue);
                    node.Add(thisNode);
                }

                i = start_node;
                node[i].node = i;
                node[i].f = 0;
                node[i].g = 0;
                node[i].h = 0;

                List<NodeInfo> openList = new List<NodeInfo>();
                openList.Add(node[i]);

                while (openList.Any())
                {
                    int current = openList.First().node;
                    double fMin = int.MaxValue;
                    for (i = 0; i < openList.Count(); i++)
                    {
                        if (openList[i].f < fMin)
                        {
                            fMin = openList[i].f;
                            current = openList[i].node;
                        }
                    }
                    int northNeighbor = current - this.m_mapSize.Width;
                    int southNeighbor = current + this.m_mapSize.Width;
                    int westNeighbor = current - 1;
                    int eastNeighbor = current + 1;
                    for (i = 0; i < openList.Count(); i++)
                    {
                        if (openList[i].node == current) openList.RemoveAt(i);
                    }

                    //closedList[current] = true;
                    if (current == end_node)
                    {
                        Log.PrintAndWriteLog($"Shortest Distance from {start_node} to {end_node}: {node[current].f}");
                        return node[current].f;
                    }
                    double fNew, gNew, hNew;

                    // North//
                    if (CheckNeighbor(current, northNeighbor))
                    {
                        gNew = node[current].g + 1;
                        hNew = CalculateHCost(northNeighbor, end_node);
                        fNew = gNew + hNew;

                        if (node[northNeighbor].f == int.MaxValue || node[northNeighbor].f > fNew)
                        {
                            node[northNeighbor].f = fNew;
                            node[northNeighbor].g = gNew;
                            node[northNeighbor].h = hNew;
                            node[northNeighbor].parent = node[current];
                            openList.Add(node[northNeighbor]);
                        }
                    }

                    // South
                    if (CheckNeighbor(current, southNeighbor))
                    {
                        gNew = node[current].g + 1;
                        hNew = CalculateHCost(southNeighbor, end_node);
                        fNew = gNew + hNew;

                        if (node[southNeighbor].f == int.MaxValue || node[southNeighbor].f > fNew)
                        {
                            node[southNeighbor].f = fNew;
                            node[southNeighbor].g = gNew;
                            node[southNeighbor].h = hNew;
                            node[southNeighbor].parent = node[current];
                            openList.Add(node[southNeighbor]);
                        }
                    }

                    // West
                    if (CheckNeighbor(current, westNeighbor))
                    {
                        gNew = node[current].g + 1;
                        hNew = CalculateHCost(westNeighbor, end_node);
                        fNew = gNew + hNew;

                        if (node[westNeighbor].f == int.MaxValue || node[westNeighbor].f > fNew)
                        {
                            node[westNeighbor].f = fNew;
                            node[westNeighbor].g = gNew;
                            node[westNeighbor].h = hNew;
                            node[westNeighbor].parent = node[current];
                            openList.Add(node[westNeighbor]);
                        }
                    }
                    // East
                    if (CheckNeighbor(current, eastNeighbor))
                    {
                        gNew = node[current].g + 1;
                        hNew = CalculateHCost(eastNeighbor, end_node);
                        fNew = gNew + hNew;

                        if (node[eastNeighbor].f == int.MaxValue || node[eastNeighbor].f > fNew)
                        {
                            node[eastNeighbor].f = fNew;
                            node[eastNeighbor].g = gNew;
                            node[eastNeighbor].h = hNew;
                            node[eastNeighbor].parent = node[current];
                            openList.Add(node[eastNeighbor]);
                        }
                    }
                }
                Log.PrintAndWriteLog($"Can't find route: {start_node} : {end_node}");
                return Int32.MaxValue;
            }
            catch (Exception ex)
            {
                Log.PrintAndWriteLog($"{ex}");
                return Int32.MaxValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="podId"></param>
        /// <returns></returns>
        public Dictionary<int, double> FindDistanceToAllAgv(int podId)
        {
            try
            {
                this.m_mapList = UpdateMap();
                this.m_podList = UpdatePod();
                this.m_agvList = UpdateAgv();

                if (!this.m_podList.ContainsKey(podId))
                {
                    string logData = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + $"\tCan not find POD {podId}.\n";
                    Console.WriteLine(logData);
                    File.AppendAllText("log.log", logData);
                    Log.PrintAndWriteLog($"Can not find POD {podId}.\n");
                    return null;
                }
                int podNodeId = this.m_podList[podId].NodeId;
                Dictionary<int, double> distanceToAgvs = new Dictionary<int, double>();
                foreach (int eachAgv in this.m_agvList.Keys)
                {
                    if (!this.m_agvList[eachAgv].IsAvailable || !this.m_agvList[eachAgv].RunningStatus)
                    {
                        //distanceToAgvs.Add(eachAgv, Int32.MaxValue);
                        continue;
                    }
                    PathFinding database1 = new PathFinding();
                    distanceToAgvs.Add(eachAgv, database1.FindShortestDistance(podNodeId, this.m_agvList[eachAgv].NodeId));
                }
                return distanceToAgvs;
            }
            catch (Exception ex)
            {
                Log.PrintAndWriteLog($"{ex}");
                return null;
            }
        }
    }
}