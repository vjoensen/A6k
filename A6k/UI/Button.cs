using OpenToolkit.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace A6k.UI
{
    class Button : UIelement
    {
        SpriteMapDrawer smd;

        private Vector2 pos;
        private int width;
        private int height;
        private int border = 4;
        public bool isHovered { get; set; }
        String color;
        public Button(SpriteMapDrawer smd, Vector2 pos, int width = 400, int height = 50, String color = "blue")
        {
            this.smd = smd;
            this.pos = pos;
            this.width = width;
            this.height = height;
            this.color = color;
            this.isHovered = false;

        }

        public void Draw()
        {
            
            smd.DrawUI("barHorizontal_white_left.png", new Vector2(-smd.getViewSize().X, -smd.getViewSize().Y)+this.pos, new Vector2(6, height));
            smd.DrawUI("barHorizontal_white_mid.png", new Vector2(-smd.getViewSize().X + 6, -smd.getViewSize().Y)+this.pos, new Vector2((width - 12), height));
            smd.DrawUI("barHorizontal_white_right.png", new Vector2(-smd.getViewSize().X + 6 + (width - 12), -smd.getViewSize().Y)+this.pos, new Vector2(6, height));

            if (isHovered)
            {
                smd.DrawUI("barHorizontal_shadow_left.png", new Vector2(-smd.getViewSize().X, -smd.getViewSize().Y) + this.pos, new Vector2(6, height));
                smd.DrawUI("barHorizontal_shadow_mid.png", new Vector2(-smd.getViewSize().X + 6, -smd.getViewSize().Y) + this.pos, new Vector2((width - 12), height));
                smd.DrawUI("barHorizontal_shadow_right.png", new Vector2(-smd.getViewSize().X + 6 + (width - 12), -smd.getViewSize().Y) + this.pos, new Vector2(6, height));
            }

            //for additional bar in the bar, change code to have additional vlaue somewhere
            //smd.DrawUI("barHorizontal_shadow_left.png", new Vector2(-smd.getViewSize().X + border, -smd.getViewSize().Y + border), new Vector2(6, height - border * 2));
            //smd.DrawUI("barHorizontal_shadow_mid.png", new Vector2(-smd.getViewSize().X + border + 6, -smd.getViewSize().Y + border), new Vector2(value * (width - 12), height - border * 2));
            //smd.DrawUI("barHorizontal_shadow_right.png", new Vector2(-smd.getViewSize().X + border + 6 + value * (width - 12), -smd.getViewSize().Y + border), new Vector2(6, height - border * 2));


            //smd.DrawUI("barHorizontal_" + color + "_left.png", new Vector2(-smd.getViewSize().X + border, -smd.getViewSize().Y + border), new Vector2(6 * marginValue, height - border * 2));

        }

        public bool isInside(float x, float y)
        {
            /*
            if(x >= pos.X & y >= pos.Y & x < pos.X + width & y < pos.Y + height)
            {
                ButtonClickEventArgs bcEvent = new ButtonClickEventArgs();
                bcEvent.eventmessage = "it's alive";
                OnButtonClick(bcEvent);
            }
            */
            return (x >= pos.X & y >= pos.Y & x < pos.X + width & y < pos.Y + height);
        }

        public virtual void OnButtonClick(ButtonClickEventArgs e)
        {
            ButtonClicked?.Invoke(this, e);
        }

        public event EventHandler<ButtonClickEventArgs> ButtonClicked;
    }
}
