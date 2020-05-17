using OpenToolkit.Mathematics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace A6k.UI
{
    class FillableBar
    {
        SpriteMapDrawer smd;
        private float value;
        private float marginValue = 1;
        private Vector2 pos;
        private int width;
        private int height;
        private int border = 4;
        String color;
        public FillableBar(SpriteMapDrawer smd, Vector2 pos, int width = 400, int height = 50, String color = "blue")
        {
            this.smd = smd;
            this.pos = pos;
            this.width = width;
            this.height = height;
            this.color = color;
            value = .005f;
            if (value < 0.02) marginValue = value * 50;

        }

        public void valueChangeEvent(object sender, StatChangeArgs e)
        {
            this.value = (float)e.currentValue / e.maxValue;
        }
        private void setValue(float newValue)
        {
            value = newValue;
            if (value < 0) value = 0;
            if (value > 1) value = 1;
            marginValue = 1;
            if (value < 0.02) marginValue = value * 50f;

        }

        public void Draw()
        {

            smd.DrawUI("barHorizontal_white_left.png", new Vector2(-smd.getViewSize().X, -smd.getViewSize().Y), new Vector2(6, height));
            smd.DrawUI("barHorizontal_white_mid.png", new Vector2(-smd.getViewSize().X + 6, -smd.getViewSize().Y), new Vector2((width - 12), height));
            smd.DrawUI("barHorizontal_white_right.png", new Vector2(-smd.getViewSize().X + 6 + (width - 12), -smd.getViewSize().Y), new Vector2(6, height));

            //for additional bar in the bar, change code to have additional vlaue somewhere
            //smd.DrawUI("barHorizontal_shadow_left.png", new Vector2(-smd.getViewSize().X + border, -smd.getViewSize().Y + border), new Vector2(6, height - border * 2));
            //smd.DrawUI("barHorizontal_shadow_mid.png", new Vector2(-smd.getViewSize().X + border + 6, -smd.getViewSize().Y + border), new Vector2(value * (width - 12), height - border * 2));
            //smd.DrawUI("barHorizontal_shadow_right.png", new Vector2(-smd.getViewSize().X + border + 6 + value * (width - 12), -smd.getViewSize().Y + border), new Vector2(6, height - border * 2));
            

            smd.DrawUI("barHorizontal_"+color+"_left.png", new Vector2(-smd.getViewSize().X + border, -smd.getViewSize().Y+border), new Vector2(6* marginValue, height - border*2));
            smd.DrawUI("barHorizontal_"+color+"_mid.png", new Vector2(-smd.getViewSize().X + border + 6 * marginValue-1, -smd.getViewSize().Y+border), new Vector2(value * (width - 11 * marginValue), height - border*2));
            smd.DrawUI("barHorizontal_"+color+"_right.png", new Vector2(-smd.getViewSize().X + border + 6 * marginValue + value * (width - 12 * marginValue)-1, -smd.getViewSize().Y+border), new Vector2(6* marginValue, height - border*2));
        }

    }
}
