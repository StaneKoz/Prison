using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using Prison.Collision;
using Prison.Algorithms;
using Prison.Entities;
using Prison.Managers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prison.Levels
{
    public class Level : IEntity
    {
        private TiledMap tiledMap;
        private TiledMapRenderer tiledMapRender;
        private List<Character> characters;
        private bool isVisiblePathsEnemy;
        private InputManager inputManager;
        public Player Player;
        public CollisionComponent collisionComponent;
        public CollisionComponent viewComponent;
        public Graph Graph;
        public List<Wall> Walls;
        public Level()
        {
            collisionComponent = new(new(0, 0, 2000, 2000));
            viewComponent = new(new(0, 0, 2000, 2000));
            tiledMap = Global.ContentManager.Load<TiledMap>("Levels/Level1/1");
            tiledMapRender = new(Global.GraphicsDeviceManager.GraphicsDevice, tiledMap);
            Walls = new List<Wall>();

            var player = new Player(new(16, 40), new Vector2(48, 40));
            characters = new List<Character>() { player };
            Player = player;

            inputManager = new(player);
            foreach (TiledMapPolygonObject obj in tiledMap.ObjectLayers.FirstOrDefault(l => l.Name == "Wall").Objects)
            {
                var points = obj.Points.OrderBy(t => t.X).ThenBy(t => t.Y).ToArray();
                var wall = new Wall(new Size2(Math.Abs(points[0].X - points[3].X), Math.Abs(points[0].Y - points[3].Y)), obj.Position + points[0]);
                Walls.Add(wall);    
                viewComponent.Insert(wall);
                collisionComponent.Insert(wall);
            }

            Graph = new();
            Graph.BuildGraph(tiledMap, Walls);

            foreach (var obj in tiledMap.ObjectLayers.FirstOrDefault(l => l.Name == "Enemy").Objects)
            {
                var enemy = new Enemy(obj.Position, obj.Position + new Vector2(100, 0), new Size2(16, 40), new Vector2(48, 40), this, 4);
                characters.Add(enemy);
            }


            foreach (var character in characters)
            {
                collisionComponent.Insert(character);
            }
        }

        public void LoadContent()
        {
            foreach (var character in characters)
            {
                character.LoadContent();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            tiledMapRender.Draw();
            foreach(var entity in characters)
            {
                entity.Draw(spriteBatch);
            }
            if (isVisiblePathsEnemy) Graph.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            isVisiblePathsEnemy = Keyboard.GetState().IsKeyDown(Keys.J);
            inputManager.Update(gameTime);  
            foreach (var entity in characters)
            {
                entity.Update(gameTime);
            }
            viewComponent.Update(gameTime);
            collisionComponent.Update(gameTime);
        }
    }
}
