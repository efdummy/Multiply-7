using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Resources;

namespace Multiply7
{
    public class Keyboard
    {
        public float KeyboardWidth;
        public float KeyboardHeight;
        float ScreenHeight;
        float ScreenWidth;
        Vector2 keyboardRow0Position { get; set; }
        Vector2 keyboardRow1Position { get; set; }
        Vector2 keyboardRow2Position { get; set; }
        float keyboardLineLength { get; set; }
        int keyboardLineHeight { get; set; }
        Vector2 pressedKeyDisplayPosition; 
        String keyboardRow0String;
        String keyboardRow1String;
        String keyboardRow2String;
        SpriteFont myFont;

        public Keyboard(float screenHeight, float screenWidth, SpriteFont font,  string typeResult)
        {
            // Screen dimensions
            ScreenHeight = screenHeight;
            ScreenWidth = screenWidth;

            myFont = font;
            keyboardRow0String = typeResult;
            keyboardRow1String = "0   1   2   3   4";
            keyboardRow2String = "5   6   7   8   9";
            KeyboardWidth = myFont.MeasureString(keyboardRow2String).Length();
            KeyboardHeight= myFont.LineSpacing*3;
            SetPosition();
        }

        public void ChangeText(string newTypeResult)
        {
            keyboardRow0String=newTypeResult;
        }

        public void Draw(SpriteBatch spriteBatch, float rotation)
        {
            spriteBatch.DrawString(myFont, keyboardRow0String, keyboardRow0Position, Color.White);
            spriteBatch.DrawString(myFont, keyboardRow1String, keyboardRow1Position, Color.White);
            spriteBatch.DrawString(myFont, keyboardRow2String, keyboardRow2Position, Color.White);
        }
        public void DrawPressedKey(SpriteBatch spriteBatch, string key, string previousKey)
        {
            string displayText = "";
            if (previousKey == "0") displayText = String.Format("{1}", previousKey, key);
            else displayText = displayText = String.Format("{0}{1} & {1}", previousKey, key);
            spriteBatch.DrawString(myFont, displayText, pressedKeyDisplayPosition, Color.Yellow);
        }

        public void SetPosition()
        {
            int lineHeight=myFont.LineSpacing;
            float x = (ScreenWidth - KeyboardWidth) / 2;
            float y = ScreenHeight;
            keyboardRow0Position = new Vector2(x, y - 3 * lineHeight);
            keyboardRow1Position = new Vector2(x, y - 2 * lineHeight);
            keyboardRow2Position = new Vector2(x, y - lineHeight);
            keyboardLineLength = KeyboardWidth;
            keyboardLineHeight = lineHeight;

            // Pressed code display feedback position
            pressedKeyDisplayPosition = new Vector2(keyboardRow0Position.X+KeyboardWidth/3, keyboardRow0Position.Y-2*lineHeight);

        }

        // Return key value or empty string
        public string CurrentKey(Vector2 mousePosition)
        {
            string currentKey = "";
            int x = (int)mousePosition.X;
            int y = (int)mousePosition.Y;
            int column;
            int line;

            if (mousePosition.X <= keyboardRow1Position.X)
                // At the left of column 0, consider that you are at column 0
                column = 0;
            else
                // Calculate colum position
                column = (int)((mousePosition.X - keyboardRow1Position.X) / (keyboardLineLength / 5));

            line = 1;
            // Case when you tape on the first digit's line or when you tap above this line
            currentKey = (column).ToString();
            if (mousePosition.Y > keyboardRow2Position.Y)
            {
                // You tap on the second line
                line = 2;
                currentKey = (column + 5).ToString();
            }
            // Correction (when your tap at the rigth of 9 or 4 key
            if ((line == 1) && (currentKey == "5")) currentKey = "4";
            if ((line == 2) && (currentKey == "10")) currentKey = "9";

            return currentKey;
        }

    }
}
