using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace br.corp.bonus630.ImageRender
{
    public class BitMatrix
    {
        public BitMatrix(bool[,] BitArray,int width,int height)
        {
            this.InternalArray = BitArray;
            this.Width = width;
            this.Height = height;
        }
        private bool[,] InternalArray { get; set; }
        public bool this[int i, int j] { get { return InternalArray[i, j]; }  internal set { InternalArray[i, j] = value; } }

        public int Height { get; protected set; }

        public int Width { get; protected set; }
    }
}
