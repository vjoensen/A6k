﻿using System;
using System.Collections.Generic;
using System.Text;

namespace A6k
{
    struct Texture2D
    {
        private int id;
        private int width, height;

        /// <summary>
        /// An integer that OpenGL uses to reference a texture stored in memory
        /// </summary>
        public int ID
        {
            get
            {
                return id;
            }
        }
        public int Width
        {
            get { return width; }
        }
        public int Height
        {
            get { return height; }
        }

        public Texture2D(int id, int width, int height)
        {
            this.id = id;
            this.width = width;
            this.height = height;
        }
    }
}
