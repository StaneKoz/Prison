
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Prison.Entities;
using Prison.Humans;
using System;

namespace Prison.Managers
{
    internal class EnemyManager
    {
        private Enemy enemy;
        public EnemyState enemyState = EnemyState.Patrol;
        public Vector2 StartPatrolPoint;
        public Vector2 EndPatrolPoint;
        public Vector2 TargetTravelPoint;
        public float PatrolWaitingTime;
        public float PatrolWaitnigTimer;

        public EnemyManager(Enemy enemy, Vector2 startPatrolPoint, Vector2 endPatrolPoint)
        {
            this.enemy = enemy; 
            StartPatrolPoint = startPatrolPoint;
            TargetTravelPoint = new Vector2(startPatrolPoint.X + new Random().Next(60, 100), endPatrolPoint.Y);
            PatrolWaitingTime = (float)(new Random().NextDouble() * new Random().Next(2, 4) + 1);
            PatrolWaitnigTimer = PatrolWaitingTime;
        }

        public void Patrol(GameTime gameTime)
        {
            if (!enemy.WalkToggle) enemy.SwitchWalkToggle();
            if (Math.Abs(TargetTravelPoint.X - enemy.Position.X) <= 4)
            {
                enemy.Velocity.X = 0;
                if (PatrolWaitnigTimer < 0)
                {
                    (TargetTravelPoint, StartPatrolPoint) = (StartPatrolPoint, TargetTravelPoint);
                    PatrolWaitnigTimer = PatrolWaitingTime;
                }
                else
                    PatrolWaitnigTimer -= gameTime.GetElapsedSeconds();
            }
            else if (enemy.Velocity.X == 0) enemy.MoveX(TargetTravelPoint.X > enemy.Position.X);
        }

        public void Search()
        {

        }

        public void Chase(GameTime gameTime)
        {
            enemy.SwitchWalkToggle();
/*            var playerPos = enemy.Level.Graph.DefineNode(enemy.Level.Player.Position).Position;
            var enemyPos = enemy.Level.Graph.DefineNode(enemy.Level.Player.Position).Position;*/
            //Console.WriteLine($"{enemyPos.X} {enemyPos.Y}");
        }

        public void Notification()
        {

        }

        public void Update(GameTime gameTime)
        {
            switch (enemy.enemyState)
            {
                case EnemyState.Patrol:
                    Patrol(gameTime);
                    break;
                case EnemyState.Chase:
                    Chase(gameTime);
                    break;
                case EnemyState.Search:
                    Search();
                    break;
                case EnemyState.Notification:
                    Notification();
                    break;
            }

        }

        public void NextMove(GameTime gameTime)
        {

        }
    }
}
