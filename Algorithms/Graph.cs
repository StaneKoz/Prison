using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using Prison.Collision;
using System.Collections.Generic;
using System.Linq;

namespace Prison.Algorithms
{
    public class Graph
    {
        public Dictionary<Vector2, Node> Nodes = new();
        public Texture2D Point;
        public void BuildGraph(TiledMap tiledMap, List<Wall> walls)
        {
            Point = Global.ContentManager.Load<Texture2D>("point");
            var horizontalOffsetLeft = new Vector2(-tiledMap.TileWidth, -tiledMap.TileHeight);
            var horizontalOffsetRight = new Vector2(tiledMap.TileWidth, -tiledMap.TileHeight);
            var horizontalStep = new Vector2(tiledMap.TileWidth, 0);
            var verticaStep = new Vector2(0,tiledMap.TileHeight);
            foreach (var wall in walls)
            {
                var corners = ((RectangleF)wall.Bounds).GetCorners();
                MarkDirection(corners[0] + horizontalOffsetLeft, corners[1] + horizontalOffsetRight, horizontalStep, false, walls);
                MarkDirection(corners[0] + horizontalOffsetLeft, new Vector2(-1, -1), verticaStep, true, walls);
                MarkDirection(corners[1] + horizontalOffsetRight, new Vector2(-1, -1), verticaStep, true, walls);
            }
        }

        public void MarkDirection(Vector2 startPosition, Vector2 endPosition, Vector2 step, bool breakOnCollision, List<Wall> walls)
        {
            Node previousNode = null;
            Node currentNode = null;
            if (startPosition.X > 1920 || startPosition.X < 0 || startPosition.Y < 0 || startPosition.Y > 1100) return;
            var currentPosition = startPosition;
            while (currentPosition != endPosition)
            {
                if (!Collision(currentPosition, walls))
                {
                    if (Nodes.TryGetValue(currentPosition, out Node node))
                    {
                        previousNode = node;
                        if (currentNode != null)
                        {
                            currentNode.IncidientNodes.Add(previousNode);
                            previousNode.IncidientNodes.Add(currentNode);
                        }
                    }
                    else
                    {
                        currentNode = new Node(currentPosition);
                        Nodes.Add(currentPosition, currentNode);
                        if (previousNode != null)
                        {
                            currentNode.IncidientNodes.Add(previousNode);
                            previousNode.IncidientNodes.Add(currentNode);
                        }
                        previousNode = currentNode;
                    }
                }
                else if (breakOnCollision) return;
                else
                    previousNode = currentNode = null;
                currentPosition += step;
            }
            if (currentPosition == endPosition && previousNode != null)
            {
                currentNode = new Node(currentPosition);
                currentNode.IncidientNodes.Add(previousNode);
                previousNode.IncidientNodes.Add(currentNode);
            }
        }

        private bool Collision(Vector2 position, List<Wall> walls)
        {
            foreach (var wall in walls)
            {
                var corners = ((RectangleF)wall.Bounds).GetCorners();
                if ((corners[0].X <= position.X && corners[2].X >= position.X) && (corners[0].Y <= position.Y && corners[2].Y >= position.Y))
                    return true;
            }
            return false;
        }

        public Node DefineNode(Vector2 currentPosition)
        {
            var x = currentPosition.X + (currentPosition.X % 16 < 7 ? -currentPosition.X % 16 : (16 - currentPosition.X % 16) % 16);
            var nodePosition = new Vector2(x,
                Nodes.Values.Where(t => t.Position.Y >= currentPosition.Y && t.Position.X == x).MinBy(t => t.Position.Y).Position.Y);
            return Nodes[nodePosition];
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var item in Nodes)
            {
                spriteBatch.Draw(Point, item.Key - new Vector2(4, 4), Color.Red);
                foreach (var line in item.Value.IncidientNodes)
                {
                    spriteBatch.DrawLine(item.Key.X, item.Key.Y, line.Position.X, line.Position.Y, Color.Red);
                }
            }
        }
    }

    public class Node
    {
        public List<Node> IncidientNodes = new List<Node>();
        public Vector2 Position;

        public Node(Vector2 position)
        {
            Position = position;
        }
    }
}
