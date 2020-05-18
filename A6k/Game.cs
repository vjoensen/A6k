using OpenToolkit.Graphics.OpenGL4;
using OpenToolkit.Mathematics;
using OpenToolkit.Windowing.Common;
using OpenToolkit.Windowing.Desktop;
using OpenToolkit.Windowing.Common.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using A6k.UI;

namespace A6k
{
    class Game : GameWindow
    {
        View view;
        Vector2 mousePos;
        private SpriteDrawer spritedrawer;

        private double _time;
        private Texture2D texture;


        private HUD hud;

        private PlayerShip player;
        private List<SpaceObject> spaceObjects;

        private AI enemyAI;

        private double globalTime = 0;


        public Game() : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            view = new View(Vector2.Zero, new Vector2(640, 480), 2f, 0.0f);
            spaceObjects = new List<SpaceObject>();
            
        }

        protected override void OnLoad()
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
             
            spritedrawer = new SpriteDrawer(view);

            hud = new HUD(this);


            

            CursorVisible = true;
            Input.Initialize(this);
            mousePos = this.MousePosition; 
            Input.mousePosition = mousePos;



            texture = SpriteDrawer.LoadTexture("PNG\\playerShip1_red.png", false, false);
            player = new PlayerShip(0, 0, 0, texture, SpriteDrawer.LoadTexture("PNG\\Lasers\\laserBlue01.png", true, false), view);

            hud.connectLifeToUI(player.GetPlayerStats());


            enemyAI = new AI(spaceObjects, player);
            Ship enemy1 = new Ship(200, 200, 0, SpriteDrawer.LoadTexture("PNG\\ufoBlue.png", false, false));
            player.setTarget(enemy1);


            enemyAI.takeControl(enemy1);
            spaceObjects.Add(enemy1);




            base.OnLoad();
        }
        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            view.viewSize.X = e.Width;
            view.viewSize.Y = e.Height;
            base.OnResize(e);
        }
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);


            HandleKeyboard();


            mousePos = this.MousePosition;

            Input.mousePosition = mousePos;

            for (int i = spaceObjects.Count - 1; i >= 0; i--)
            {
                if (spaceObjects[i].isDead())
                {
                    spaceObjects.RemoveAt(i);
                }
            }

            enemyAI.Update();

            List<SpaceObject> newSO = new List<SpaceObject>();
            player.Update(newSO, args.Time);
            foreach (SpaceObject so in spaceObjects)
            {
                so.Update(newSO, args.Time);
            }
            CheckCollisions();
            spaceObjects.AddRange(newSO);
            view.Update();
            globalTime += args.Time;

        }

        private void CheckCollisions()
        {

            for (int i = 0; i < spaceObjects.Count; i++)
            {
                if(spaceObjects[i].getFaction() != player.getFaction() && checkCol(spaceObjects[i], player))
                {
                    spaceObjects[i].Collide(player);
                    player.Collide(spaceObjects[i]);
                }
                for (int j = i + 1; j < spaceObjects.Count; j++)
                {
                    if (i != j && spaceObjects[i].getFaction() != spaceObjects[j].getFaction() && checkCol(spaceObjects[i], spaceObjects[j]))
                    {
                        spaceObjects[i].Collide(spaceObjects[j]);
                        spaceObjects[j].Collide(spaceObjects[i]);
                    }
                }
            }
        }
        private bool checkCol(SpaceObject objA, SpaceObject objB)
        {
            /*
            if( DateTime.Now.Second%10 == 0)
            {
                Console.WriteLine("Collision" + objA.getFaction() + objB.getFaction());
                Console.WriteLine(" A: (" + objA.pos.X + "," + objA.pos.Y + ") r: " + objA.getRadius());
                Console.WriteLine(" B: (" + objB.pos.X + "," + objB.pos.Y + ") r: " + objB.getRadius());

            }
            */
            return (objA.pos.X - objB.pos.X) * (objA.pos.X - objB.pos.X) + (objA.pos.Y - objB.pos.Y) * (objA.pos.Y - objB.pos.Y)
                    < (objA.getRadius() + objB.getRadius()) * (objA.getRadius() + objB.getRadius());
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            
            GL.Clear(ClearBufferMask.ColorBufferBit);

            //Code goes here.
            //SpriteDrawer.Draw(texture, new Vector2(-200,20), Vector2.One, Color.White, new Vector2(((float)texture.Width) / 2, ((float)texture.Height) / 2), 0 - (float)Math.PI / 2);
            _time += e.Time;
            Title = //$"{_title}: (Vsync: {VSync}) FPS: {1f / e.Time:0}, "+ 
                "pos:" + view.getX() + "," + view.getY() + " mousePos: " + mousePos.X + "," + mousePos.Y + "shipPos:" + player.pos.X + "," + player.pos.Y;
            Color4 backColor;
            backColor.A = 1.0f;
            backColor.R = 0.1f;
            backColor.G = 0.1f;
            backColor.B = 0.3f;
            GL.ClearColor(backColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.ClearStencil(0);


            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            
            
            foreach (SpaceObject so in spaceObjects)
            {
                so.Draw();
            }
            player.Draw();
            //SpriteDrawer.DrawCursor(cursorTexture, mousePos, Vector2.One, Color.Azure, new Vector2(((float)cursorTexture.Width) / 2, ((float)cursorTexture.Height) / 2));

            hud.Draw();
            

            SwapBuffers();
            base.OnRenderFrame(e);
        }


        protected override void OnUnload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);


            base.OnUnload();
        }

        private void HandleKeyboard()
        {
            if (KeyboardState.IsKeyDown(OpenToolkit.Windowing.Common.Input.Key.Escape))
            {
                this.Close();
                
            }


            /*
            if (Mouse.GetCursorState().IsButtonDown(MouseButton.Left))
            {
                view.SetPosition(new Vector2(this.PointToClient(new Point(Mouse.GetCursorState().X , Mouse.GetCursorState().Y)).X -this.Width/2, this.PointToClient(new Point(Mouse.GetCursorState().X, Mouse.GetCursorState().Y)).Y- this.Height/2));
            }
            */

        }
    }

}
