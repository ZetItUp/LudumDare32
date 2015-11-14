using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace APEngine
{
    public interface IResizable
    {
        bool Resizable { get; set; }
        bool InProportion { get; set; }
        float ScaleXY { get; set; }
        float ScaleX { get; set; }
        float ScaleY { get; set; }

        void ScaleTo(float width, float height);
        void ScaleTo(float scaleXY);
        void SetScale(float scaleX, float scaleY);
    }
}
