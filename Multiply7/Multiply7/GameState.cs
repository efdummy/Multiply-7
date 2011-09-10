using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Multiply7
{
    public enum State { MainMenu, GameRunning, GameResult}

    class GameState
    {
        public GameOptions Options;
        public State DisplayState;

        public int ScreenHeight;
        public int ScreenWidth;
        public int MaxX;
        public int MaxY;

        List<int> errors;
        int operationCount;

        Random random;
        int mult;
        int operand;

        // The rocket list
        List<Missile> missiles = new List<Missile>();

        public string previousKey;
        int playerResult1;
        int playerResult2;

        public Vector2 slatePosition { get; set;  }
        public Vector2 slateCenterPosition { get; set; }
        public Vector2 operationPosition { get; set; }

        public Vector2 MousePosition { get; set; }
        bool isGestureToHandle;
        bool isBravoAlreadyPlayed;

        public GameState(GraphicsDeviceManager graphics)
        {
            // Game options
            Options = new GameOptions();

            // Set screen dimensions and max x and y
            SetWidthAndHeight(graphics, 0, 0);

            // Random seed and error list
            random = new Random();
            errors = new List<int>();
        }
        public void RestartGame()
        {
            mult = random.Next(10); while ((mult == 0)||(mult==1)) mult = random.Next(10);
            operationCount = 0;
            errors.Clear();
            missiles.Clear();
            isGestureToHandle = false;
            isBravoAlreadyPlayed = false;
        }
        public void GenerateNewOperation()
        {
            operationCount++;
            if (operationCount > Options.MaxOperationCount) DisplayState = State.GameResult;
            operand = random.Next(10); while ((operand == 0)||(operand == 1)) operand = random.Next(10);
            int x = random.Next(MaxX);
            int y = 0;
            slatePosition = new Vector2(x, y);
            slateCenterPosition = new Vector2(x + 126, y + 83);
            operationPosition = new Vector2(x+70, y+50);
            Options.Speed = new Vector2(100, 100);
            playerResult1 = 0;
            playerResult2 = 0;
            previousKey = "0";
            missiles.Clear();
            isGestureToHandle = false;
            isBravoAlreadyPlayed = false;
        }
        public string CurrentMult()
        {
            return mult.ToString();
        }
        public string OperationString()
        {
            return mult.ToString() + " x " + operand.ToString();
        }
        public void AddError()
        {
            errors.Add(operand);
        }
        public void AddMissile(Missile missile)
        {
            missiles.Add(missile);
        }
        public bool isResultFound(string currentKey)
        {
            bool isresultfound = false;
            playerResult1 = int.Parse((previousKey + currentKey));
            playerResult2 = int.Parse(currentKey);
            if ((playerResult1 == operand * mult) || (playerResult2 == operand * mult))
                isresultfound = true;
            previousKey = currentKey;
            return isresultfound;
        }
        public List<Missile> Missiles()
        {
            return missiles;
        }
        public string ErrorListString()
        {
            string msg="";
            foreach (int i in errors)
            {
                msg += "\n" + mult + " x " + i + " = " + mult * i;
            }
            return msg;
        }
        public int ErrorCount()
        {
            return errors.Count;
        }

        void SetOperation(int multiplication, int number)
        {
            mult=multiplication;
            operand=number;
        }
        public void SetWidthAndHeight(GraphicsDeviceManager graphics, int dX, int dY)
        {
            ScreenWidth = graphics.GraphicsDevice.Viewport.Width;
            MaxX = ScreenWidth - dX;
            ScreenHeight = graphics.GraphicsDevice.Viewport.Height;
            MaxY = ScreenHeight-dY;
        }
        public void UpdateSlatePosition(GameTime gameTime)
        {
            // Move the slate sprite by speed, scaled by elapsed time.
            Vector2 speedVector=Options.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            slatePosition += speedVector;
            slateCenterPosition += speedVector;
            operationPosition += speedVector;

            if (slatePosition.X > MaxX)
            {
                SetSpeed(Options.Speed.X * -1, Options.Speed.Y);
            }
            else if (slatePosition.X < 0)
            {
                SetSpeed(Options.Speed.X * -1, Options.Speed.Y);
            }

            if (slatePosition.Y > MaxY)
            {
                // You lose
            }
        }
        public void SetSpeed(float x, float y)
        {
            Options.Speed = new Vector2(x, y);
        }
        public void SetMousePosition(float x, float y)
        {
            MousePosition = new Vector2(x, y);
        }
        public void FireGestureHandling()
        {
            isGestureToHandle = true;
        }
        public bool isNewGestureToHandler()
        {
            return isGestureToHandle;
        }
        public void GestureHandled()
        {
            isGestureToHandle = false;
        }
        public bool ICanPlayBravo()
        {
            bool rc = isBravoAlreadyPlayed;
            if (!rc) isBravoAlreadyPlayed = true;
            return !rc;
        }

    }
}
