using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace VoidEngine
{
    /// <summary>
    /// The Button class for the Void Engine
    /// </summary>
    public class Button : Sprite
    {
        /// <summary>
        /// This is the button's state
        /// </summary>
        public enum bState { HOVER, UP, DOWN, RELEASED }

        Label label;

        public bState buttonState = new bState();
        public bool mPress = false;
        bool pMPress = false;
        public int mX;
        public int mY;
        Rectangle testColision;

        /// <summary>
        /// Creates a button.
        /// </summary>
        /// <param name="position">The position for the button.</param>
        /// <param name="font">The font for the text in the button.</param>
        /// <param name="text">The text in the button.</param>
        public Button(Vector2 position, SpriteFont font, float scale, Color fontColor, string text) : base(position)
        {
            label = new Label(new Vector2(position.X + 2, position.Y + 2), font, scale, fontColor, text);
        }

        /// <summary>
        /// The test for hitButtonAlpha overload 0.
        /// </summary>
        /// <param name="tx">The texture's x</param>
        /// <param name="ty">The texture's y</param>
        /// <param name="frameTex">the texture's width and height in Point</param>
        /// <param name="x">the x of mouse</param>
        /// <param name="y">the y of mouse</param>
        /// <returns>Boolean</returns>
        Boolean hitButtonAlpha(float tx, float ty, Point frameTex, int x, int y)
        {
            testColision = new Rectangle((int)tx, (int)ty, frameTex.X, frameTex.Y);

            if (testColision.Intersects(new Rectangle(x, y, 1, 1)))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Updates the button without the mouse, exectutes the hitButtonAlpha function too.
        /// </summary>
        public void updButton()
        {
            if (hitButtonAlpha(position.X, position.Y, currentAnimation.frameSize, mX, mY))
            {
                currentAnimation.fps = 0;

                if (mPress)
                {
                    buttonState = bState.DOWN;
                    SetAnimation("PRESSED");
                }
                else if (!mPress && pMPress)
                {
                    if (buttonState == bState.DOWN)
                    {
                        buttonState = bState.RELEASED;
                        SetAnimation("REG");
                    }
                }
                else
                {
                    buttonState = bState.HOVER;
                    SetAnimation("HOVER");
                }
            }
            else
            {
                buttonState = bState.UP;
                SetAnimation("REG");

                if (currentAnimation.fps > 0)
                {
                    currentAnimation.fps = currentAnimation.fps - currentAnimation.fps;
                }
                else
                {
                }
            }
        }

        /// <summary>
        /// Updates the button with the mouse's cords and state
        /// </summary>
        /// <param name="gameTime">The main GameTime</param>
        public override void Update(GameTime gameTime)
        {
            currentAnimation.fps = gameTime.ElapsedGameTime.Milliseconds / 1000;

            MouseState mState = Mouse.GetState();

            mX = mState.X;
            mY = mState.Y;
            pMPress = mPress;
            mPress = mState.LeftButton == ButtonState.Pressed;

            updButton();
            base.Update(gameTime);
        }

        public bool Clicked()
        {
            if (buttonState == bState.RELEASED)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Draws the button
        /// </summary>
        /// <param name="gameTime">The main GameTime</param>
        /// <param name="spriteBatch">The main SpriteBatch</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            label.Draw(gameTime, spriteBatch);
        }

        public override void AddAnimations(Texture2D texture)
        {
            addAnimation("REG", texture, new Point(85, 23), new Point(0, 0), new Point(0, 0), 1000);
            addAnimation("HOVER", texture, new Point(85, 23), new Point(1, 0), new Point(85, 0), 1000);
            addAnimation("PRESSED", texture, new Point(85, 23), new Point(2, 0), new Point(170, 0), 1000);
            SetAnimation("REG");
            base.AddAnimations(texture);
        }
    }
}