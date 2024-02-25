using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class Dijkstra
{
    class NodeRecord : IComparable<NodeRecord>
    {
        public Node node;
        public Connection connection;
        public float costSoFar;

        public int CompareTo(NodeRecord other)
        {
            if (other == null)
            {
                return 1;
            }

            //we want to sort lowestr costSoFar to highest...
            //if costSoFar is lower than other, return negative, if same, return 0, if larger, return positive
            return (int)(costSoFar - other.costSoFar);
        }
    }

        class PathFindingList
        {
            List<NodeRecord> nodeRecords = new List<NodeRecord>();

            public void add(NodeRecord n)
            {
                nodeRecords.Add(n);
            }

            public void remove(NodeRecord n)
            {
                nodeRecords.Remove(n);
            }

            public NodeRecord smallestElement()
            {
                nodeRecords.Sort();
                return nodeRecords[0];
            }

            public int length()
            {
                return nodeRecords.Count;
            }

            public bool contains(Node node)
            {
                foreach (NodeRecord n in nodeRecords)
                {
                    if (n.node == node)
                    {
                        return true;
                    }
                }

                return false;
            }

            public NodeRecord find(Node node)
            {
                foreach(NodeRecord n in nodeRecords)
                {
                    if (n.node == node)
                    {
                        return n;
                    }
                }

                return null;
            }
        }
    public static List<Connection> pathFind(Graph graph, Node start, Node goal)
    {
        //Initialize the record for the start node
        NodeRecord startRecord = new NodeRecord();
        startRecord.node = start;
        startRecord.connection = null;
        startRecord.costSoFar = 0;

        //Initialize the open and closed lists
        PathFindingList open = new PathFindingList();
        open.add(startRecord);
        PathFindingList closed = new PathFindingList();

        //Iterate through processing each node
        NodeRecord current = new NodeRecord();
        while (open.length() > 0)
        {
            //Find the smallest element in the open list
            current = open.smallestElement();

            //If it is the goal node, terminate
            if (current.node == goal)
            {
                break;
            }

            //Otherwise get its outgoing connections.
            List<Connection> connections = graph.getConnections(current.node);

            //Loop through each connection in turn
            foreach (Connection connection in connections)
            {
                Node endNode = connection.getToNode();
                float endNodeCost = current.costSoFar + connection.getCost();

                NodeRecord endNodeRecord = new NodeRecord();

                //Skip if the node is closed
                if (closed.contains(endNode))
                {
                    continue;
                }

                //...or if it's open and we've found a worse route
                else if (open.contains(endNode))
                {
                    //here we find the record in the open list corresponding to the endNode
                    endNodeRecord = open.find(endNode);

                    if (endNodeRecord != null && endNodeRecord.costSoFar < endNodeCost)
                    {
                        continue;
                    }
                }

                //otherwise we know we've got an unvisted node, so make a record for it
                else
                {
                    endNodeRecord = new NodeRecord();
                    endNodeRecord.node = endNode;
                }

                //We're here if we need to update the bide. Update the cost and connection
                endNodeRecord.costSoFar = endNodeCost;
                endNodeRecord.connection = connection;

                //And add it to the open list
                if (!open.contains(endNode))
                {
                    open.add(endNodeRecord);
                }
            }

            //We've finished looking at the connections for the current node, so add it
            //to the closed list and remove it from the open list
            open.remove(current);
            closed.add(current);
        }

        //We're here if we've either found the goal, or if we've no more nodes to search,
        //find which
        if (current.node != goal)
        {
            //we've run out of nodes without finding the goal, so there's no solution
            return null;
        }

        else
        {
            //Compile the list of connections in the path
            List<Connection> path = new List<Connection>();

            //Work back along the path, accumulating connections
            while (current.node != start)
            {
                path.Add(current.connection);
                Node fromNode = current.connection.getFromNode();
                current = closed.find(fromNode);
            }

            //Reverse the path and return it
            path.Reverse();
            return path;
        }
    }
}
