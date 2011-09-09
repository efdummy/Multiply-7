using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Multiply7
{
    class Missile
    {
        Vector2 startPoint;
        Vector2 myCurrentPosition;
        float height;
        float width;
        public Missile(float screenHeight, float screenWidth)
        {
            startPoint = new Vector2((screenWidth/2), screenHeight-1);
            myCurrentPosition = startPoint;
            height = screenHeight;
            width = screenWidth;
        }

        public bool isOut(Vector2 targetPosition)
        {
            bool outOfTheScreen=false;
            if (myCurrentPosition.Y < 0) 
                outOfTheScreen = true;
            if ((myCurrentPosition.Y < targetPosition.Y))
                outOfTheScreen = true;

            return outOfTheScreen;
        }

        public void Draw(SpriteBatch batch, Texture2D missileTexture, Color col, Vector2 targetPosition)
        {
            Rectangle destRect = new Rectangle((int)myCurrentPosition.X, (int)myCurrentPosition.Y, 8, 8);
            //Vector2 origin = new Vector2(0.0f, 0.0f);
            // Draw the missile
            batch.Draw(missileTexture, destRect, col);
            // Update the missile's position
            Vector2 diff = myCurrentPosition - targetPosition;
            float len;
            len = diff.Length() / 16;
            myCurrentPosition.X -= diff.X / len;
            myCurrentPosition.Y -= diff.Y / len;
        }
    }
}
