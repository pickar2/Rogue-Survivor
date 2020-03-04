using djack.RogueSurvivor.Engine;
using djack.RogueSurvivor.Gameplay;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueSurvivor.Extensions;
using RogueSurvivor.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace djack.RogueSurvivor
{
    public class RogueForm : Game, IRogueUI
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private List<IDrawItem> drawItems = new List<IDrawItem>();
        private int frame = 0;
        private Texture2D m_MinimapTexture;
        private Color[] m_MinimapColors = new Color[RogueGame.MAP_MAX_WIDTH * RogueGame.MAP_MAX_HEIGHT];

        RogueGame m_Game;
        SpriteFont m_NormalFont;
        SpriteFont m_BoldFont;

        internal RogueGame Game
        {
            get { return m_Game; }
        }

        public GraphicsDeviceManager Graphics => graphics;

        public RogueForm()
        {
            Logger.WriteLine(Logger.Stage.INIT_MAIN, "Creating main form...");

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = RogueGame.CANVAS_WIDTH;
            graphics.PreferredBackBufferHeight = RogueGame.CANVAS_HEIGHT;
            graphics.HardwareModeSwitch = false;
            //graphics.IsFullScreen = true; 
            // !FIXME
        }

        protected override void Initialize()
        {
            base.Initialize();

            Logger.WriteLine(Logger.Stage.INIT_MAIN, "Initializing game...");

            Window.Title = "Rogue Survivor - " + SetupConfig.GAME_VERSION;
            IsMouseVisible = true;

            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

            m_MinimapTexture = new Texture2D(graphics.GraphicsDevice, RogueGame.MAP_MAX_WIDTH, RogueGame.MAP_MAX_HEIGHT);

            Content.RootDirectory = "Resources/Content";
            m_NormalFont = Content.Load<SpriteFont>("NormalFont");
            m_BoldFont = Content.Load<SpriteFont>("BoldFont");

            m_Game = new RogueGame(this);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            switch (frame)
            {
                case 0:
                    // do nothing on first frame
                    break;
                case 1:
                    Logger.WriteLine(Logger.Stage.INIT_GFX, "loading images...");
                    GameImages.LoadResources(this);
                    Logger.WriteLine(Logger.Stage.INIT_GFX, "loading images done");

                    m_Game.Init();
                    break;
                default:
                    if (!m_Game.Update())
                        Exit();
                    break;
            }

            ++frame;
        }

        protected override bool BeginDraw()
        {
            return false;
        }

        public Key UI_WaitKey()
        {
            while (true)
            {
                Key key = UI_PeekKey();
                if (key != Key.None)
                    return key;
                Thread.Sleep(10);
            }
        }

        private KeyboardState prevKeyboardState;
        public Key UI_PeekKey()
        {
            System.Windows.Forms.Application.DoEvents();

            KeyboardState keyboardState = Keyboard.GetState();

            Keys[] keys = keyboardState.GetPressedKeys();
            if (keys.Length == 0)
            {
                prevKeyboardState = keyboardState;
                return Key.None;
            }

            Key key = Key.None;
            bool control = false, alt = false, shift = false;
            foreach (Keys k in keys)
            {
                switch (k)
                {
                    case Keys.LeftControl:
                    case Keys.RightControl:
                        control = true;
                        break;
                    case Keys.LeftShift:
                    case Keys.RightShift:
                        shift = true;
                        break;
                    case Keys.LeftAlt:
                    case Keys.RightAlt:
                        alt = true;
                        break;
                    default:
                        key = (Key)k;
                        break;
                }
            }

            if (key != Key.None)
            {
                if (prevKeyboardState.GetPressedKeys().Contains((Keys)key))
                    key = Key.None;
                else
                {
                    if (control)
                        key |= Key.Control;
                    if (alt)
                        key |= Key.Alt;
                    if (shift)
                        key |= Key.Shift;
                }
            }

            prevKeyboardState = keyboardState;
            return key;
        }

        // !FIXME
        /*public void UI_PostKey(KeyEventArgs e)
        {
            ///////////
            // Cheats
            ///////////
#if DEBUG
            // F6 - CHEAT - reveal all
            if (e.KeyCode == Key.F6)
            {
                if (m_Game.Session != null && m_Game.Session.CurrentMap != null)
                {
                    m_Game.Session.CurrentMap.SetAllAsVisited();
                    UI_Repaint();
                }
            }
            // F7 - DEV - toggle FPS
            if (e.KeyCode == Key.F7)
            {
                // FIXME
                //m_GameCanvas.ShowFPS = !m_GameCanvas.ShowFPS;
                UI_Repaint();
            }
            // F8 - DEV - resize to normal size
            if (e.KeyCode == Key.F8)
            {
                // FIXME
                //m_GameCanvas.NeedRedraw = true;
                //SetClientSizeCore(RogueGame.CANVAS_WIDTH, RogueGame.CANVAS_HEIGHT);
                UI_Repaint();
            }
            // F9 - DEV - Show actors stats
            if (e.KeyCode == Key.F9)
            {
                m_Game.DEV_ToggleShowActorsStats();
                UI_Repaint();
            }
            // F10 - DEV - Show pop graph.
#if DEBUG_STATS
            if (e.KeyCode == Key.F10)
            {
                District d = m_Game.Player.Location.Map.District;

                UI_Clear(Color.Black);
                // axis
                UI_DrawLine(Color.White, 0, 0, 0, RogueGame.CANVAS_HEIGHT);
                UI_DrawLine(Color.White, 0, RogueGame.CANVAS_HEIGHT, RogueGame.CANVAS_WIDTH, RogueGame.CANVAS_HEIGHT);
                // plot.
                int prevL = 0;
                int prevU = 0;
                const int XSCALE = WorldTime.TURNS_PER_HOUR;
                const int YSCALE = 10;
                for (int turn = 0; turn < m_Game.Session.WorldTime.TurnCounter; turn += XSCALE)
                {
                    if (turn % WorldTime.TURNS_PER_DAY == 0)
                        UI_DrawLine(Color.White, turn / XSCALE, RogueGame.CANVAS_HEIGHT, turn / XSCALE, 0);

                    Session.DistrictStat.Record? r = m_Game.Session.GetStatRecord(d, turn);
                    if (r == null) break;
                    int L = r.Value.livings;
                    UI_DrawLine(Color.Green, 
                        (turn - 1)/XSCALE, RogueGame.CANVAS_HEIGHT - YSCALE * prevL, 
                        turn/XSCALE, RogueGame.CANVAS_HEIGHT - YSCALE * L);
                    int U = r.Value.undeads;
                    UI_DrawLine(Color.Red, 
                        (turn - 1)/XSCALE, RogueGame.CANVAS_HEIGHT - YSCALE * prevU, 
                        turn/XSCALE, RogueGame.CANVAS_HEIGHT - YSCALE * U);
                    prevL = L;
                    prevU = U;
                }
                UI_Repaint();
                UI_WaitKey();
            }
#endif

            // F11 - DEV - Toggle player invincibility
            if (e.KeyCode == Key.F11)
            {
                m_Game.DEV_TogglePlayerInvincibility();
                UI_Repaint();
            }

            // F12 - DEV - Max trust for all player followers
            if (e.KeyCode == Key.F12)
            {
                m_Game.DEV_MaxTrust();
                UI_Repaint();
            }

            // alpha10.1
            // INSERT - DEV - Toggle bot mode
            if (e.KeyCode == Key.Insert)
            {
                m_Game.BotToggleControl();
                UI_Repaint();
            }
#endif
        }*/

        private object window;
        private MethodInfo updateMouseState;

        private void RefreshMouse()
        {
            if (updateMouseState == null)
            {
                object platform = GetType()
                    .GetField("Platform", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField)
                    .GetValue(this);
                window = platform.GetType()
                    .GetField("_window", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField)
                    .GetValue(platform);
                updateMouseState = window.GetType().GetMethod("UpdateMouseState", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod);
            }

            updateMouseState.Invoke(window, new object[0]);
        }

        public Point UI_GetMousePosition()
        {
            RefreshMouse();
            return Mouse.GetState().Position;
        }

        public MouseButton UI_PeekMouseButtons()
        {
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
                return MouseButton.Left;
            else if (mouseState.RightButton == ButtonState.Pressed)
                return MouseButton.Right;
            else
                return MouseButton.None;
        }

        public void UI_Wait(int msecs)
        {
            UI_Repaint();
            Thread.Sleep(msecs);
        }

        public void UI_Repaint()
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            foreach (IDrawItem drawItem in drawItems)
            {
                switch (drawItem)
                {
                    case DrawTextItem drawText:
                        if (drawText.shadowColor.HasValue)
                            spriteBatch.DrawString(drawText.font, drawText.text, new Vector2(drawText.pos.X + 1, drawText.pos.Y + 1), drawText.shadowColor.Value);
                        spriteBatch.DrawString(drawText.font, drawText.text, drawText.pos, drawText.color);
                        break;
                    case DrawLineItem drawLine:
                        spriteBatch.DrawLine(drawLine.from, drawLine.to, drawLine.color);
                        break;
                    case DrawImageItem drawImage:
                        if (drawImage.transform)
                        {
                            spriteBatch.Draw(drawImage.image, drawImage.pos, null, drawImage.tint, drawImage.rotation,
                                Vector2.Zero, drawImage.scale, SpriteEffects.None, 0.0f);
                        }
                        else
                            spriteBatch.Draw(drawImage.image, drawImage.pos, drawImage.tint);
                        break;
                    case DrawRectangleItem drawRectangle:
                        if (drawRectangle.filled)
                            spriteBatch.DrawRectangle(drawRectangle.rectangle, drawRectangle.color);
                        else
                        {
                            Rectangle rect = drawRectangle.rectangle;
                            spriteBatch.DrawLine(new Vector2(rect.Left, rect.Bottom), new Vector2(rect.Right, rect.Bottom), drawRectangle.color);
                            spriteBatch.DrawLine(new Vector2(rect.Left, rect.Top), new Vector2(rect.Right, rect.Top), drawRectangle.color);
                            spriteBatch.DrawLine(new Vector2(rect.Left, rect.Bottom), new Vector2(rect.Left, rect.Top), drawRectangle.color);
                            spriteBatch.DrawLine(new Vector2(rect.Right, rect.Bottom), new Vector2(rect.Right, rect.Top), drawRectangle.color);
                        }
                        break;
                }
            }

            spriteBatch.End();

            EndDraw();
        }

        public void UI_Clear(Color clearColor)
        {
            graphics.GraphicsDevice.Clear(clearColor);
            drawItems.Clear();
        }

        public void UI_DrawImage(string imageID, int gx, int gy)
        {
            drawItems.Add(new DrawImageItem
            {
                image = GameImages.Get(imageID),
                pos = new Vector2(gx, gy),
                tint = Color.White
            });
        }

        public void UI_DrawImage(string imageID, int gx, int gy, Color tint)
        {
            drawItems.Add(new DrawImageItem
            {
                image = GameImages.Get(imageID),
                pos = new Vector2(gx, gy),
                tint = tint
            });
        }

        public void UI_DrawImageTransform(string imageID, int gx, int gy, float rotation, float scale)
        {
            drawItems.Add(new DrawImageItem
            {
                image = GameImages.Get(imageID),
                pos = new Vector2(gx, gy),
                tint = Color.White,
                rotation = rotation,
                scale = scale,
                transform = true
            });
        }

        public void UI_DrawGrayLevelImage(string imageID, int gx, int gy)
        {
            drawItems.Add(new DrawImageItem
            {
                image = GameImages.GetGrayLevel(imageID),
                pos = new Vector2(gx, gy),
                tint = Color.White
            });
        }

        public void UI_DrawTransparentImage(float alpha, string imageID, int gx, int gy)
        {
            drawItems.Add(new DrawImageItem
            {
                image = GameImages.Get(imageID),
                pos = new Vector2(gx, gy),
                tint = new Color(1.0f, 1.0f, 1.0f, alpha)
            });
        }

        public void UI_DrawLine(Color color, int gxFrom, int gyFrom, int gxTo, int gyTo)
        {
            drawItems.Add(new DrawLineItem
            {
                color = color,
                from = new Vector2(gxFrom, gyFrom),
                to = new Vector2(gxTo, gyTo)
            });
        }

        public void UI_DrawString(Color color, string text, int gx, int gy, Color? shadowColor)
        {
            drawItems.Add(new DrawTextItem
            {
                color = color,
                text = text,
                font = m_NormalFont,
                pos = new Vector2(gx, gy),
                shadowColor = shadowColor
            });
        }

        public void UI_DrawStringBold(Color color, string text, int gx, int gy, Color? shadowColor)
        {
            drawItems.Add(new DrawTextItem
            {
                color = color,
                text = text,
                font = m_BoldFont,
                pos = new Vector2(gx, gy),
                shadowColor = shadowColor
            });
        }

        public void UI_DrawRect(Color color, Rectangle rect)
        {
            if (rect.Width <= 0 || rect.Height <= 0)
                throw new ArgumentOutOfRangeException("rectangle Width/Height <= 0");
            drawItems.Add(new DrawRectangleItem
            {
                color = color,
                rectangle = rect,
                filled = false
            });
        }

        public void UI_FillRect(Color color, Rectangle rect)
        {
            if (rect.Width <= 0 || rect.Height <= 0)
                throw new ArgumentOutOfRangeException("rectangle Width/Height <= 0");
            drawItems.Add(new DrawRectangleItem
            {
                color = color,
                rectangle = rect,
                filled = true
            });
        }

        public void UI_DrawPopup(string[] lines, Color textColor, Color boxBorderColor, Color boxFillColor, int gx, int gy)
        {
            /////////////////
            // Measure lines
            /////////////////
            int longestLineWidth = 0;
            int totalLineHeight = 0;
            Point[] linesSize = new Point[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                linesSize[i] = m_BoldFont.MeasureString(lines[i]).ToPoint();
                if (linesSize[i].X > longestLineWidth)
                    longestLineWidth = linesSize[i].X;
                totalLineHeight += linesSize[i].Y;
            }

            ///////////////////
            // Setup popup box
            ///////////////////
            const int BOX_MARGIN = 2;
            Point boxPos = new Point(gx, gy);
            Point boxSize = new Point(longestLineWidth + 2 * BOX_MARGIN, totalLineHeight + 2 * BOX_MARGIN);
            Rectangle boxRect = new Rectangle(boxPos, boxSize);

            //////////////////
            // Draw popup box
            //////////////////
            UI_FillRect(boxFillColor, boxRect);
            UI_DrawRect(boxBorderColor, boxRect);

            //////////////
            // Draw lines
            //////////////
            int lineX = boxPos.X + BOX_MARGIN;
            int lineY = boxPos.Y + BOX_MARGIN;
            for (int i = 0; i < lines.Length; i++)
            {
                UI_DrawStringBold(textColor, lines[i], lineX, lineY, null);
                lineY += linesSize[i].Y;
            }
        }

        public void UI_DrawPopupTitle(string title, Color titleColor, string[] lines, Color textColor, Color boxBorderColor, Color boxFillColor, int gx, int gy)
        {
            /////////////////
            // Measure lines
            /////////////////
            int longestLineWidth = 0;
            int totalLineHeight = 0;
            Point[] linesSize = new Point[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                linesSize[i] = m_BoldFont.MeasureString(lines[i]).ToPoint();
                if (linesSize[i].X > longestLineWidth)
                    longestLineWidth = linesSize[i].X;
                totalLineHeight += linesSize[i].Y;
            }

            Point titleSize = m_BoldFont.MeasureString(title).ToPoint();
            if (titleSize.X > longestLineWidth)
                longestLineWidth = titleSize.X;
            totalLineHeight += titleSize.Y;
            const int TITLE_BAR_LINE = 1;
            totalLineHeight += TITLE_BAR_LINE;

            ///////////////////
            // Setup popup box
            ///////////////////
            const int BOX_MARGIN = 2;
            Point boxPos = new Point(gx, gy);
            Point boxSize = new Point(longestLineWidth + 2 * BOX_MARGIN, totalLineHeight + 2 * BOX_MARGIN);
            Rectangle boxRect = new Rectangle(boxPos, boxSize);

            //////////////////
            // Draw popup box
            //////////////////
            UI_FillRect(boxFillColor, boxRect);
            UI_DrawRect(boxBorderColor, boxRect);

            //////////////
            // Draw title
            //////////////
            int titleX = boxPos.X + BOX_MARGIN + (longestLineWidth - titleSize.X) / 2;
            int titleY = boxPos.Y + BOX_MARGIN;
            int titleLineY = titleY + titleSize.Y + TITLE_BAR_LINE;
            UI_DrawStringBold(titleColor, title, titleX, titleY, null);
            UI_DrawLine(boxBorderColor, boxRect.Left, titleLineY, boxRect.Right, titleLineY);

            //////////////
            // Draw lines
            //////////////
            int lineX = boxPos.X + BOX_MARGIN;
            int lineY = titleLineY + TITLE_BAR_LINE;

            for (int i = 0; i < lines.Length; i++)
            {
                UI_DrawStringBold(textColor, lines[i], lineX, lineY, null);
                lineY += linesSize[i].Y;
            }
        }

        public void UI_DrawPopupTitleColors(string title, Color titleColor, string[] lines, Color[] colors, Color boxBorderColor, Color boxFillColor, int gx, int gy)
        {
            /////////////////
            // Measure lines
            /////////////////
            int longestLineWidth = 0;
            int totalLineHeight = 0;
            Point[] linesSize = new Point[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                linesSize[i] = m_BoldFont.MeasureString(lines[i]).ToPoint();
                if (linesSize[i].X > longestLineWidth)
                    longestLineWidth = linesSize[i].X;
                totalLineHeight += linesSize[i].Y;
            }

            Point titleSize = m_BoldFont.MeasureString(title).ToPoint();
            if (titleSize.X > longestLineWidth)
                longestLineWidth = titleSize.X;
            totalLineHeight += titleSize.Y;
            const int TITLE_BAR_LINE = 1;
            totalLineHeight += TITLE_BAR_LINE;

            ///////////////////
            // Setup popup box
            ///////////////////
            const int BOX_MARGIN = 2;
            Point boxPos = new Point(gx, gy);
            Point boxSize = new Point(longestLineWidth + 2 * BOX_MARGIN, totalLineHeight + 2 * BOX_MARGIN);
            Rectangle boxRect = new Rectangle(boxPos, boxSize);

            //////////////////
            // Draw popup box
            //////////////////
            UI_FillRect(boxFillColor, boxRect);
            UI_DrawRect(boxBorderColor, boxRect);

            //////////////
            // Draw title
            //////////////
            int titleX = boxPos.X + BOX_MARGIN + (longestLineWidth - titleSize.X) / 2;
            int titleY = boxPos.Y + BOX_MARGIN;
            int titleLineY = titleY + titleSize.Y + TITLE_BAR_LINE;
            UI_DrawStringBold(titleColor, title, titleX, titleY, null);
            UI_DrawLine(boxBorderColor, boxRect.Left, titleLineY, boxRect.Right, titleLineY);

            //////////////
            // Draw lines
            //////////////
            int lineX = boxPos.X + BOX_MARGIN;
            int lineY = titleLineY + TITLE_BAR_LINE;

            for (int i = 0; i < lines.Length; i++)
            {
                UI_DrawStringBold(colors[i], lines[i], lineX, lineY, null);
                lineY += linesSize[i].Y;
            }
        }

        public void UI_ClearMinimap(Color color)
        {
            for (int i = 0; i < RogueGame.MAP_MAX_HEIGHT * RogueGame.MAP_MAX_WIDTH; ++i)
                m_MinimapColors[i] = color;
        }

        public void UI_SetMinimapColor(int x, int y, Color color)
        {
            m_MinimapColors[x + y * RogueGame.MAP_MAX_WIDTH] = color;
        }

        public void UI_DrawMinimap(int gx, int gy)
        {
            m_MinimapTexture.SetData(m_MinimapColors);
            drawItems.Add(new DrawImageItem
            {
                image = m_MinimapTexture,
                pos = new Vector2(gx, gy),
                tint = Color.White,
                rotation = 0,
                scale = RogueGame.MINITILE_SIZE,
                transform = true
            });
        }

        public float UI_GetCanvasScaleX()
        {
            // !FIXME
            return 1;
            //return m_GameCanvas.ScaleX;
        }

        public float UI_GetCanvasScaleY()
        {
            // !FIXME
            return 1;
            //return m_GameCanvas.ScaleY;
        }

        public string UI_SaveScreenshot(string filePath)
        {
            filePath += ".png";

            try
            {
                int w = GraphicsDevice.PresentationParameters.BackBufferWidth,
                    h = GraphicsDevice.PresentationParameters.BackBufferHeight;
                RenderTarget2D screenshot = new RenderTarget2D(GraphicsDevice, w, h, false, SurfaceFormat.Bgra32, DepthFormat.None);
                GraphicsDevice.SetRenderTarget(screenshot);
                UI_Repaint();
                GraphicsDevice.Present();
                GraphicsDevice.SetRenderTarget(null);

                using (FileStream file = new FileStream(filePath, FileMode.Create))
                {
                    screenshot.SaveAsPng(file, w, h);
                }

                return filePath;
            }
            catch (Exception ex)
            {
                Logger.WriteLine(Logger.Stage.RUN_GFX, String.Format("exception when taking screenshot : {0}", ex.ToString()));
                return null;
            }
        }
    }
}
