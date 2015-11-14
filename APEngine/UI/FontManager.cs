using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace APEngine.UI
{
    public static class FontManager
    {
        public static bool IsInitialized { get; private set; }

        public static void LoadContent()
        {
            IsInitialized = true;
        }

        public static Vector2 GetStringSize(string text)
        {
            if (!IsInitialized)
                return Vector2.Zero;

            int width = 0;
            int height = 17;

            foreach(char c in text)
            {
                width += (int)GetCharWidth(c);
            }

            Vector2 retVal = new Vector2(width, height);

            return retVal;
        }

        public static List<Rectangle> GetRectangles(String text)
        {
            List<Rectangle> rects = new List<Rectangle>();

            foreach(char c in text)
            {
                rects.Add(GetCharRect(c));
            }

            return rects;
        }

        public static float GetCharWidth(char c)
        {
            return GetCharRect(c).Width;
        }

        public static float GetCharHeight
        {
            get { return 17f; }
        }

        public static Rectangle GetCharRect(char c)
        {
            int ic = (int)c;

            switch(ic)
            {

                // MISC       32 to 63
                //         Space to ?
                case 32:
                    return new Rectangle(0, 40, 14, 17);
                case 33:
                    return new Rectangle(16, 92, 5, 17);
                case 34:
                    return new Rectangle(25, 92, 9, 17);
                case 35:
                    return new Rectangle(38, 92, 13, 17);
                case 36:
                    return new Rectangle(55, 91, 11, 17);
                case 37:
                    return new Rectangle(71, 92, 16, 17);
                case 38:
                    return new Rectangle(92, 92, 13, 17);
                case 39:
                    return new Rectangle(111, 92, 5, 17);
                case 40:
                    return new Rectangle(121, 92, 7, 17);
                case 41:
                    return new Rectangle(131, 92, 7, 17);
                case 42:
                    return new Rectangle(143, 92, 8, 17);
                case 43:
                    return new Rectangle(157, 92, 11, 17);
                case 44:
                    return new Rectangle(173, 92, 5, 17);
                case 45:
                    return new Rectangle(182, 92, 7, 17);
                case 46:
                    return new Rectangle(193, 92, 5, 17);
                case 47:
                    return new Rectangle(202, 92, 6, 17);
                case 48:
                    return new Rectangle(1, 69, 10, 17);
                case 49:
                    return new Rectangle(18, 69, 8, 17);
                case 50:
                    return new Rectangle(33, 69, 10, 17);
                case 51:
                    return new Rectangle(49, 69, 10, 17);
                case 52:
                    return new Rectangle(65, 69, 11, 17);
                case 53:
                    return new Rectangle(81, 69, 10, 17);
                case 54:
                    return new Rectangle(97, 69, 10, 17);
                case 55:
                    return new Rectangle(113, 69, 10, 17);
                case 56:
                    return new Rectangle(129, 69, 10, 17);
                case 57:
                    return new Rectangle(145, 69, 10, 17);
                case 58:
                    return new Rectangle(161, 69, 5, 17);
                case 59:
                    return new Rectangle(171, 69, 5, 17);
                case 60:
                    return new Rectangle(181, 69, 11, 17);
                case 61:
                    return new Rectangle(197, 69, 10, 17);
                case 62:
                    return new Rectangle(213, 69, 11, 17);
                case 63:
                    return new Rectangle(229, 69, 10, 17);
                // UPPERCASES 64 to 95
                //            @  to _
                case 64:
                    return new Rectangle(0, 0, 14, 17);
                case 65:
                    return new Rectangle(16, 0, 15, 17);
                case 66:
                    return new Rectangle(35, 0, 11, 17);
                case 67:
                    return new Rectangle(52, 0, 11, 17);
                case 68:
                    return new Rectangle(69, 0, 11, 17);
                case 69:
                    return new Rectangle(86, 0, 11, 17);
                case 70:
                    return new Rectangle(103, 0, 10, 17);
                case 71:
                    return new Rectangle(119, 0, 12, 17);
                case 72:
                    return new Rectangle(137, 0, 12, 17);
                case 73:
                    return new Rectangle(155, 0, 5, 17);
                case 74:
                    return new Rectangle(165, 0, 11, 17);
                case 75:
                    return new Rectangle(182, 0, 13, 17);
                case 76:
                    return new Rectangle(200, 0, 10, 17);
                case 77:
                    return new Rectangle(216, 0, 14, 17);
                case 78:
                    return new Rectangle(236, 0, 12, 17);
                case 79:
                    return new Rectangle(254, 0, 12, 17);
                case 80:
                    return new Rectangle(272, 0, 11, 17);
                case 81:
                    return new Rectangle(289, 0, 12, 17);
                case 82:
                    return new Rectangle(307, 0, 12, 17);
                case 83:
                    return new Rectangle(324, 0, 11, 17);
                case 84:
                    return new Rectangle(340, 0, 13, 17);
                case 85:
                    return new Rectangle(358, 0, 12, 17);
                case 86:
                    return new Rectangle(374, 0, 15, 17);
                case 87:
                    return new Rectangle(391, 0, 19, 17);
                case 88:
                    return new Rectangle(412, 0, 15, 17);
                case 89:
                    return new Rectangle(430, 0, 13, 17);
                case 90:
                    return new Rectangle(448, 0, 11, 17);
                case 91:
                    return new Rectangle(465, 0, 7, 17);
                case 92:
                    return new Rectangle(475, 0, 6, 17);
                case 93:
                    return new Rectangle(484, 0, 7, 17);
                case 94:
                    return new Rectangle(496, 0, 10, 17);
                case 95:
                    return new Rectangle(506, 0, 10, 17);
                // LOWERCASE    96 to 127
                //              `  to DEL
                case 96:
                    return new Rectangle(0, 23, 5, 17);
                case 97:
                    return new Rectangle(10, 23, 11, 17);
                case 98:
                    return new Rectangle(26, 23, 11, 17);
                case 99:
                    return new Rectangle(42, 23, 11, 17);
                case 100:
                    return new Rectangle(58, 23, 11, 17);
                case 101:
                    return new Rectangle(74, 23, 11, 17);
                case 102:
                    return new Rectangle(89, 23, 8, 17);
                case 103:
                    return new Rectangle(101, 23, 11, 17);
                case 104:
                    return new Rectangle(117, 23, 11, 17);
                case 105:
                    return new Rectangle(133, 23, 5, 17);
                case 106:
                    return new Rectangle(141, 23, 7, 17);
                case 107:
                    return new Rectangle(153, 23, 12, 17);
                case 108:
                    return new Rectangle(169, 23, 5, 17);
                case 109:
                    return new Rectangle(179, 23, 15, 17);
                case 110:
                    return new Rectangle(200, 23, 11, 17);
                case 111:
                    return new Rectangle(216, 23, 11, 17);
                case 112:
                    return new Rectangle(232, 23, 11, 17);
                case 113:
                    return new Rectangle(248, 23, 11, 17);
                case 114:
                    return new Rectangle(264, 23, 8, 17);
                case 115:
                    return new Rectangle(276, 23, 10, 17);
                case 116:
                    return new Rectangle(290, 23, 8, 17);
                case 117:
                    return new Rectangle(303, 23, 11, 17);
                case 118:
                    return new Rectangle(318, 23, 11, 17);
                case 119:
                    return new Rectangle(333, 23, 17, 17);
                case 120:
                    return new Rectangle(353, 23, 13, 17);
                case 121:
                    return new Rectangle(369, 23, 11, 17);
                case 122:
                    return new Rectangle(385, 23, 9, 17);
                case 123:
                    return new Rectangle(398, 23, 8, 17);
                case 124:
                    return new Rectangle(410, 23, 4, 17);
                case 125:
                    return new Rectangle(418, 23, 8, 17);
                case 126:
                    return new Rectangle(430, 23, 11, 17);
                case 127:
                    return new Rectangle(0, 40, 14, 17);
                default:
                    return new Rectangle(0, 40, 14, 17);
            }
        }
    }
}
