using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

// Startup class

namespace Multiply7
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        // My game state
        GameState state;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Support Portrait orientation
            graphics.SupportedOrientations = DisplayOrientation.Portrait;
            graphics.PreferredBackBufferWidth = 480;
            graphics.PreferredBackBufferHeight = 800;
            graphics.IsFullScreen = true;

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>

        SpriteFont myFont;

        protected override void Initialize()
        {
            // Initialize game state
            state = new GameState(graphics);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        
        // The slate
        Texture2D mySlateTexture;
        Texture2D myMissileTexture;

        // The sound effects to use
        SoundEffect myLaserSound;
        SoundEffect myExplodeSound;
        SoundEffect myBravoSound;

        // The custom keyboard
        Keyboard keyboard;

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load images
            mySlateTexture = Content.Load<Texture2D>("Images\\ardoise04");
            myMissileTexture = Content.Load<Texture2D>("Images\\missile");

            // Load sounds   
            myLaserSound = Content.Load<SoundEffect>("Audio\\Waves\\tir1");
            myExplodeSound = Content.Load<SoundEffect>("Audio\\Waves\\explode");
            myBravoSound = Content.Load<SoundEffect>("Audio\\Waves\\tarzan");

            // Load fonts
            myFont = Content.Load<SpriteFont>("Segoe");
            
            // Load the keyboard
            keyboard = new Keyboard(state.ScreenHeight,
                                    state.ScreenWidth,
                                    myFont, state.Options.rm.GetString("TYPERESULT"));

            // Enable gesture interface for Tap
            TouchPanel.EnabledGestures = GestureType.Tap;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        protected override void Update(GameTime gameTime)
        {
            // Process the hardware back button according to the technical certification requirements
            // number 5.2.4.1 – Back Button: Previous Pages
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                switch (state.DisplayState)
                {
                    case State.MainMenu:
                        this.Exit();
                        break;
                    case State.GameRunning:
                        state.DisplayState = State.MainMenu;
                        break;
                    case State.GameResult:
                        state.DisplayState = State.MainMenu;
                        break;
                    default:
                        this.Exit();
                        break;
                }
        

            //state.SetWidthAndHeight(graphics, mySlateTexture.Width, mySlateTexture.Height+(int)keyboard.KeyboardHeight);
            state.SetWidthAndHeight(graphics, mySlateTexture.Width, mySlateTexture.Height);

            switch (state.DisplayState)
            {
                case State.GameRunning:
                    // Move the slate sprite by speed, scaled by elapsed time.
                    state.UpdateSlatePosition(gameTime);

                    // Generate new operation if the player loses
                    if (state.slatePosition.Y > state.MaxY)
                    {
                        state.AddError();
                        state.GenerateNewOperation();
                    }
                    else
                    // Remove missiles arrived or out of screen
                    foreach (Missile missile in state.Missiles())
                    {
                        if (missile.isOut(state.slateCenterPosition))
                        {
                            state.Missiles().Remove(missile);
                            state.GenerateNewOperation();
                            myExplodeSound.Play();
                            break;
                        }
                    }
                  break;
            }


            // Handle touchscreen
            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample gesture = TouchPanel.ReadGesture();

                if (gesture.GestureType == GestureType.Tap)
                {
                    Vector2 touchPosition = gesture.Position;
                    state.SetMousePosition(touchPosition.X, touchPosition.Y);

                    switch (state.DisplayState)
                    {
                        case State.MainMenu:
                            if (touchPosition.Y > state.ScreenHeight / 1.55)
                            {
                                // Touch bottom of screen to change language
                                state.Options.SwitchCulture();
                                keyboard.ChangeText(state.Options.rm.GetString("TYPERESULT"));
                            }
                            else
                            {
                                state.RestartGame();
                                state.GenerateNewOperation();
                                state.DisplayState = State.GameRunning;
                            }
                            break;
                        
                        case State.GameRunning:
                            state.FireGestureHandling();
                            myLaserSound.Play();
                            break;

                        case State.GameResult:
                          //  if (touchPosition.Y < state.ScreenHeight / 1.40)
                                state.DisplayState = State.MainMenu;
                            break;
                    }
                }
            }

            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black); //GraphicsDevice.Clear(Color.CornflowerBlue);
            string msg="";
            float rotation = 0f; // -1.5f;

            // Draw the sprite.
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            switch(state.DisplayState)
            {
                case State.GameRunning:
                    // Display the current mult in background
                    spriteBatch.DrawString(myFont, state.CurrentMult(), new Vector2(state.ScreenWidth / 2, state.ScreenHeight / 2), Color.White, rotation, myFont.MeasureString(state.CurrentMult()), 3.0f, SpriteEffects.None, 0.0f);

                    // Draw the keyboard
                    keyboard.Draw(spriteBatch, rotation);

                    // Draw the slate
                    Rectangle destRect = new Rectangle((int)state.slatePosition.X, (int)state.slatePosition.Y, mySlateTexture.Width, mySlateTexture.Height);
                    Vector2 origin = new Vector2(0.0f, 0.0f);
                    spriteBatch.Draw(mySlateTexture, destRect, Color.White);
                    // Draw the operation string
                    String operationString = state.OperationString();
                    spriteBatch.DrawString(myFont, operationString, state.operationPosition, Color.White);

                    // Draw missiles
                    foreach (Missile missile in state.Missiles())
                    {
                        missile.Draw(spriteBatch, myMissileTexture, Color.WhiteSmoke, state.slateCenterPosition);
                    }

                    if (state.isNewGestureToHandler())
                    {
                        string currentKey = keyboard.CurrentKey(state.MousePosition);
                        if (!String.IsNullOrEmpty(currentKey))
                        {
                            // Draw the current pressed key
                            keyboard.DrawPressedKey(spriteBatch, currentKey, state.previousKey);

                            // If result is found
                            if (state.isResultFound(currentKey))
                            {
                                // Load a new missile
                                state.AddMissile(new Missile(state.ScreenHeight, state.ScreenWidth));
                            }
                        }
                        state.GestureHandled();
                    }
                    break;
                case State.GameResult:
                    string errorerrors="";
                    switch (state.ErrorCount())
                    {
                        case 0:
                            msg = String.Format(state.Options.rm.GetString("BRAVO"), "\n");
                            if (state.ICanPlayBravo()) myBravoSound.Play();
                            break;
                        case 1:
                            errorerrors = String.Format(state.Options.rm.GetString("ERROR"), "\n");
                            break;
                        default:
                            errorerrors = String.Format(state.Options.rm.GetString("ERRORS"), "\n");
                            break;
                    }
                    if (state.ErrorCount() > 0)
                    {
                        msg = state.ErrorCount() + " " + errorerrors + "\n";
                        msg += String.Format(state.Options.rm.GetString("LEARN"), "\n");
                        msg += state.ErrorListString();
                    }
                    
                    msg += "\n\n" + String.Format(state.Options.rm.GetString("TOUCH"), "\n");
                    spriteBatch.DrawString(myFont, msg, new Vector2(50, 50), Color.White);
                    break;
                case State.MainMenu:
                    msg= String.Format(state.Options.rm.GetString("EXPLANATIONTEXT"), "\n")+"\n\n\n";
                    msg+= String.Format(state.Options.rm.GetString("TOUCHTOSTART"), "\n");
                    spriteBatch.DrawString(myFont, msg, new Vector2(50, 50), Color.White);
                    break;
            }
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
