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
        Vector2 keyDisplayPosition; 
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
            keyboardRow1String = "5   6   7   8   9";
            keyboardRow2String = "0   1   2   3   4";
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
        public void DrawPressedKey(SpriteBatch spriteBatch, string key)
        {
            spriteBatch.DrawString(myFont, key, keyDisplayPosition, Color.Yellow);
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
            keyDisplayPosition = new Vector2(keyboardRow0Position.X+KeyboardWidth, keyboardRow0Position.Y);

        }

        // Return key value or empty string
        public string CurrentKey(Vector2 mousePosition)
        {
            string currentKey = "";
            int x=(int)mousePosition.X;
            int y=(int)mousePosition.Y;
            int column;

            if (mousePosition.X <= keyboardRow1Position.X)
                column = 0;
            else
                column = (int)((mousePosition.X - keyboardRow1Position.X) / (keyboardLineLength / 5));

            if (mousePosition.Y >= keyboardRow1Position.Y)
            {
                int valueToAdd = 5;
                if (mousePosition.Y > keyboardRow2Position.Y)
                    valueToAdd = 0;
                currentKey = (column + valueToAdd).ToString();
            }
            return currentKey;
        }

    }
}
