using OpenToolkit.Graphics.OpenGL4;
using OpenToolkit.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;

namespace A6k
{
    class SpriteMapDrawer
    {
        private int shaderProgram;
        private View view;
        private uint VAO;

        private Dictionary<String, int[]> spriteMap;
        //private int textureID;
        //private Texture2D tex2D;
        public SpriteMapDrawer(View v)
        {
            view = v;
            spriteMap = new Dictionary<string, int[]>();

            float[] vertices = {
                1f,  1f, 0.0f,     1f, 0.0f, // top right
                1f,  0f, 0.0f,     1f, 1f, // bottom right
                0f,  0f, 0.0f,     0.0f, 1f, // bottom left
                0f,  1f, 0.0f,     0.0f, 0.0f  // top left 
            };

            uint[] indices = {
                3, 1, 0, // first triangle
                1, 2, 3  // second triangle
            };

            uint VBO, EBO;
            GL.GenVertexArrays(1, out VAO);
            GL.GenBuffers(1, out VBO);
            GL.GenBuffers(1, out EBO);


            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.NamedBufferStorage(
                       VBO,
                       sizeof(float) * vertices.Length,
                       vertices,
                       BufferStorageFlags.MapWriteBit);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.NamedBufferStorage(
                       EBO,
                       sizeof(uint) * indices.Length,
                       indices,
                       BufferStorageFlags.MapWriteBit);

            int vShader = CompileShader(ShaderType.VertexShader, @"Shaders\shaderMap.vert");
            int fShader = CompileShader(ShaderType.FragmentShader, @"Shaders\shader.frag");

            shaderProgram = GL.CreateProgram();
            GL.AttachShader(shaderProgram, vShader);
            GL.AttachShader(shaderProgram, fShader);
            GL.LinkProgram(shaderProgram);

            // position attribute
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            // color attribute
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), (3 * sizeof(float)));
            GL.EnableVertexAttribArray(1);
            //// texture coord attribute
            //GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), (6 * sizeof(float)));
            //GL.EnableVertexAttribArray(2);

        }



        public void DrawUI(String spriteName, Vector2 position, Vector2 targetSize)
        {
            GL.UseProgram(shaderProgram);
            int colorLoc = GL.GetUniformLocation(shaderProgram, "spriteColor");


            int transformLoc = GL.GetUniformLocation(shaderProgram, "transform");
            spriteMap.TryGetValue(spriteName, out int[] spriteinfo);
            Matrix4 t = Matrix4.Identity;
            t = Matrix4.Mult(t, Matrix4.CreateScale(targetSize.X, targetSize.Y, 1.0f));
            t = Matrix4.Mult(t, Matrix4.CreateTranslation(position.X, position.Y, 0));

            t = Matrix4.Mult(t, view.ApplyTransforms());
            GL.UniformMatrix4(transformLoc, false, ref t);

            int spriteinfoLoc = GL.GetUniformLocation(shaderProgram, "spriteInfo");

            GL.Uniform4(spriteinfoLoc, (float)spriteinfo[0] / spriteinfo[5], (float)spriteinfo[1] / spriteinfo[6], (float)spriteinfo[2] / spriteinfo[5], (float)spriteinfo[3] / spriteinfo[6]);

            GL.UseProgram(shaderProgram);
            Color color = Color.White;
            GL.Uniform3(colorLoc, (float)color.R / 255, (float)color.G / 255, (float)color.B / 255);
            GL.BindVertexArray(VAO);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, spriteinfo[4]);

            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
        }

        public void LoadSpriteSheet(string filePNG, string fileXML, bool flipY, bool flipX)
        {
            

            Bitmap bitmap = new Bitmap(filePNG);

            if (flipY) bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
            if (flipX) bitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
            ;
            // GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);


            int textureID;
            GL.GenTextures(1, out textureID);
            GL.BindTexture(TextureTarget.Texture2D, textureID);

            System.Drawing.Imaging.BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bitmap.UnlockBits(data);


            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            XmlDocument doc = new XmlDocument();
            doc.Load(fileXML);

            XmlNode root = doc.FirstChild;

            if (root.HasChildNodes)
            {
                for (int i = 0; i < root.ChildNodes.Count; i++)
                {
                    spriteMap.Add(root.ChildNodes[i].Attributes.GetNamedItem("name").Value,
                        new int[]{
                            int.Parse(root.ChildNodes[i].Attributes.GetNamedItem("x").Value),
                            int.Parse(root.ChildNodes[i].Attributes.GetNamedItem("y").Value),
                            int.Parse(root.ChildNodes[i].Attributes.GetNamedItem("width").Value),
                            int.Parse(root.ChildNodes[i].Attributes.GetNamedItem("height").Value),
                            textureID,
                            bitmap.Width,
                            bitmap.Height
                            }
                        );

                }
            }

        }

        public Vector2 getViewSize()
        {
            return view.viewSize;
        }

        private int CompileShader(ShaderType type, string path)
        {
            var shader = GL.CreateShader(type);
            var src = File.ReadAllText(path);
            GL.ShaderSource(shader, src);
            GL.CompileShader(shader);
            var info = GL.GetShaderInfoLog(shader);
            if (!string.IsNullOrWhiteSpace(info))
                throw new Exception($"CompileShader {type} had errors: {info}");
            return shader;
        }
    }
}
