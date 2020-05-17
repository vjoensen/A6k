using OpenToolkit.Mathematics;
using OpenToolkit.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace A6k.UI
{
    class HUD
    {
        SpriteMapDrawer smd;
        View view;
        private Game game;


        FillableBar bar1;

        //Button but1;
        //Button but2;

        Dictionary<string,Button> buttons;

        public HUD(Game game)
        {
            this.game = game;
            game.MouseDown += game_MouseDown;
            game.MouseMove += updateHovered;

            view = new View(Vector2.Zero, new Vector2(640, 480));
            smd = new SpriteMapDrawer(view);
            smd.LoadSpriteSheet("PNG\\Spritesheets\\uipackSpace_sheet.png", "PNG\\Spritesheets\\uipackSpace_sheet.xml", false, false);
            bar1 = new FillableBar(smd, Vector2.Zero);


            Button but1;
            Button but2;

            but1 = new Button(smd, new Vector2(400,00) ,100,100);
            but1.ButtonClicked += testEvent;

            but2 = new Button(smd, new Vector2(500, 00), 100, 100);
            but2.ButtonClicked += testEvent;

            buttons = new Dictionary<string, Button>();
            buttons.Add("Button1", but1);
            buttons.Add("Button2", but2);
        }

        public void setSize(Vector2 newSize)
        {
            view.setSize(newSize);
        }

        public void connectLifeToUI(PlayerStats playerStats)
        {
            playerStats.StatsChanged += bar1.valueChangeEvent;
        }

        public Button GetButton(string buttonName)
        {
            Button value;
            buttons.TryGetValue(buttonName, out value);
            return value;
        }

        public void testEvent(object sender, ButtonClickEventArgs e)
        {
            Console.WriteLine(e.eventmessage);
        }

        public void updateHovered(MouseMoveEventArgs e)
        {
            foreach(KeyValuePair<string, Button> b in buttons)
            {
                if (b.Value.isInside(view.viewSize.X * e.X / game.ClientSize.X * 2, view.viewSize.Y * (game.ClientSize.Y - e.Y) / game.ClientSize.Y * 2)) { b.Value.isHovered = true; } else b.Value.isHovered = false;
            }
            //if (but1.isInside(view.viewSize.X * e.X / game.ClientSize.X * 2, view.viewSize.Y * (game.ClientSize.Y - e.Y) / game.ClientSize.Y * 2)){ but1.isHovered = true; } else but1.isHovered = false;
            //if (but2.isInside(e.X, e.Y)) { but2.isHovered = true; } else but2.isHovered = false;
        }
       
        public void game_MouseDown(MouseButtonEventArgs e)
        {
            
            if(e.Button == OpenToolkit.Windowing.Common.Input.MouseButton.Button1)
            {
                ButtonClickEventArgs bcEvent = new ButtonClickEventArgs();
                bcEvent.eventmessage = "it's alive";

                foreach (KeyValuePair<string, Button> b in buttons)
                {
                    if(b.Value.isHovered) b.Value.OnButtonClick(bcEvent);
                }
                //if (but1.isHovered) but1.OnButtonClick(bcEvent);
                //if (but2.isHovered) but2.OnButtonClick(bcEvent);

                /*
                if(but1.isInside(view.viewSize.X * game.MousePosition.X / game.ClientSize.X * 2, view.viewSize.Y * (game.ClientSize.Y - game.MousePosition.Y) / game.ClientSize.Y * 2)){
                    Console.WriteLine("clicky clicky");
                    
                }
                */
                //Console.WriteLine("mousepos : " +  view.viewSize.X*game.MousePosition.X/ game.ClientSize.X*2 + ", " + view.viewSize.Y * (game.ClientSize.Y - game.MousePosition.Y) / game.ClientSize.Y*2);
                //Console.WriteLine("real mousepos : " + game.MousePosition.X + ", " + (game.ClientSize.Y - game.MousePosition.Y));
            }
        }

        public void Draw()
        {
            foreach (KeyValuePair<string, Button> b in buttons)
            {
                b.Value.Draw();
            }
 
            bar1.Draw();
        }
    }
}
