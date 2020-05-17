﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using A6k.Weapons;
using OpenToolkit.Mathematics;
using OpenToolkit.Windowing.Common.Input;

namespace A6k
{
    class PlayerShip : SpaceObject
    {
        //private float posX, posY;


        private float xVel, yVel;

        private float maxSpeed;
        private float acceleration;
        private Texture2D texture;

        private SpaceObject target;

        //private int shootCD = 10;
        //private int currentShootCD = 10;
        Texture2D bulletTexture;

        List<Weapon> weapons;

        MissileLauncher weapon1;
        Weapon weapon2;

        View view;

        private PlayerStats playerStats;

        public PlayerShip(float spawnPosX, float spawnPosY, float spawnRotation, Texture2D shipTexture, Texture2D bulletTexture, View view)
        {
            faction = Faction.Team1;
            pos = new Vector2(spawnPosX, spawnPosY);
            rotation = spawnRotation;
            xVel = 0;
            yVel = 0;
            acceleration = 3;
            maxSpeed = 10;
            texture = shipTexture;
            this.bulletTexture = bulletTexture;
            this.view = view;

            radius = Math.Max(shipTexture.Height, shipTexture.Width) / 2;

            weapons = new List<Weapon>();
            //weapons.Add(new Weapon(this, -20,15 , 0, SpriteDrawer.LoadTexture("PNG\\Parts\\gun09.png", false,false), SpriteDrawer.LoadTexture("PNG\\Lasers\\laserBlue01.png",true, false),2,20));
            //weapons.Add(new Shotgun(this, -20, 15, 0, SpriteDrawer.LoadTexture("PNG\\Parts\\gun04.png", false, false), SpriteDrawer.LoadTexture("PNG\\Lasers\\laserGreen13.png", true, false), 2, 20));
            //weapons.Add(new MissileLauncher(this, -20, 15, 0, SpriteDrawer.LoadTexture("PNG\\Parts\\gun07.png", false, false), SpriteDrawer.LoadTexture("PNG\\Sprites X2\\Missiles\\spaceMissiles_013.png", true, false), 2, 5));
            weapon1 = new MissileLauncher(this, -20, 15, 0, SpriteDrawer.LoadTexture("PNG\\Parts\\gun07.png", false, false), SpriteDrawer.LoadTexture("PNG\\Sprites X2\\Missiles\\spaceMissiles_013.png", true, false), 2, 5);
            weapon2 = new Shotgun(this, -20, 15, 0, SpriteDrawer.LoadTexture("PNG\\Parts\\gun04.png", false, false), SpriteDrawer.LoadTexture("PNG\\Lasers\\laserGreen13.png", true, false), 2, 20);

            playerStats = new PlayerStats();
            playerStats.setLife(1000, 1200);
        }

        public PlayerStats GetPlayerStats()
        {
            return playerStats;
        }

        public void setTarget(SpaceObject newTarget)
        {
            this.target = newTarget;
        }

        override public void Update(List<SpaceObject> newObjects, double time)
        {
            
            if (Input.KeyDown(Key.A))
            {
                xVel -= acceleration;
            }
            else if (Input.KeyDown(Key.D))
            {
                xVel += acceleration;
            }
            else
            {
                xVel = 0;
            }


            if (Input.KeyDown(Key.W))
            {
                yVel += acceleration;
            }
            else if (Input.KeyDown(Key.S))
            {
                yVel -= acceleration;
            }
            else
            {
                yVel = 0;
            }
            
            if (xVel * xVel + yVel * yVel > maxSpeed * maxSpeed)
            {
                float speed = (float)Math.Sqrt(xVel * xVel + yVel * yVel);
                xVel = xVel * maxSpeed / speed;
                yVel = yVel * maxSpeed / speed;
            }
            pos.X += xVel;
            pos.Y += yVel;

            //rotation = (float)Math.Atan2(Input.mousePosition.Y + view.getY() - this.pos.Y, Input.mousePosition.X + view.getX() - this.pos.X);
            rotation = -(float)Math.Atan2( Input.mousePosition.Y - view.viewSize.Y / 2  + view.getY() - this.pos.Y,  Input.mousePosition.X - view.viewSize.X / 2  + view.getX() - this.pos.X);
            view.SetPosition(pos);
            
            if (Input.KeyDown(Key.Space))
            {
                weapon1.Shoot(newObjects, time);

                foreach (Weapon wep in weapons)
                {
                    wep.Shoot(newObjects, time);
                }
            }
            
            weapon1.setTarget(this.target);
            weapon1.Update(newObjects);

            foreach (Weapon wep in weapons)
            {
                wep.Update(newObjects);
            }

        }

        override public void Draw()
        {
            foreach (Weapon wep in weapons)
            {
                wep.Draw();
            }
            SpriteDrawer.Draw(texture, pos, Vector2.One, Color.White, new Vector2(((float)texture.Width) / 2, ((float)texture.Height) / 2), rotation - (float)Math.PI / 2);

        }

        public override void TakeDamage(float damage, SpaceObject source)
        {
            playerStats.addLife(-(int)damage);
            Console.WriteLine("take Damage :" + damage);
            //throw new NotImplementedException();
        }

        public override void Collide(SpaceObject collider)
        {
            //throw new NotImplementedException();
        }
    }
}
