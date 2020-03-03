using djack.RogueSurvivor.Engine;
using djack.RogueSurvivor.Gameplay;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueSurvivor.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace djack.RogueSurvivor
{
    public class RogueForm : Game, IRogueUI
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

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
            // FIXME

        }

        protected override void Initialize()
        {
            base.Initialize();

            Logger.WriteLine(Logger.Stage.INIT_MAIN, "Initializing game...");

            Window.Title = "Rogue Survivor - " + SetupConfig.GAME_VERSION;

            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

            Content.RootDirectory = "Resources/Content";
            m_NormalFont = Content.Load<SpriteFont>("NormalFont");
            m_BoldFont = Content.Load<SpriteFont>("BoldFont");

            m_Game = new RogueGame(this);


        }

        /*protected override void LoadContent()
        {
            

            

            SuppressDraw();
        }*/

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

        KeyboardState prevKeyboardState;

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

        // FIXME
        /*bool m_HasKey = false;
        KeyEventArgs m_InKey;

        public KeyEventArgs UI_WaitKey()
        {
            m_HasKey = false;
            while (true)
            {
                Application.DoEvents();
                if (m_HasKey)
                    break;
                Thread.Sleep(1);
            }
            return m_InKey;
        }

        public KeyEventArgs UI_PeekKey()
        {
            Thread.Sleep(1);
            Application.DoEvents();
            if (m_HasKey)
            {
                m_HasKey = false;
                return m_InKey;
            }
            else
                return null;
        }*/

        // FIXME
        /*public void UI_PostKey(KeyEventArgs e)
        {
            // ignore Shift/Ctrl/Alt alone.
            switch (e.KeyCode)
            {
                case Key.ShiftKey:
                case Key.Shift:
                case Key.LShiftKey:
                case Key.RShiftKey:
                case Key.Control:
                case Key.ControlKey:
                case Key.RControlKey:
                case Key.LControlKey:
                case Key.Alt:
                    return;
                default:
                    break;
            }

            m_HasKey = true;
            m_InKey = e;
            e.Handled = true;

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

        public Point UI_GetMousePosition()
        {
            return Mouse.GetState().Position;
        }

        // FIXME
        /*bool m_HasMouseButtons = false;
        MouseButtons m_MouseButtons;

        public void UI_PostMouseButtons(MouseButtons buttons)
        {
            m_HasMouseButtons = true;
            m_MouseButtons = buttons;
        }*/

        public MouseButton UI_PeekMouseButtons()
        {
            System.Windows.Forms.Application.DoEvents();
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
                return MouseButton.Left;
            else if (mouseState.RightButton == ButtonState.Pressed)
                return MouseButton.Right;
            else
                return MouseButton.None;
        }

        public void UI_SetCursor(System.Windows.Forms.Cursor cursor)
        {
            throw new NotImplementedException();
            /*if (cursor == Cursor)
                return;

            this.Cursor = cursor;
            Application.DoEvents();*/
        }

        public void UI_Wait(int msecs)
        {
            UI_Repaint();
            Thread.Sleep(msecs);
        }

        private int frame = 0;

        public void UI_Repaint()
        {
            spriteBatch.Begin();

            foreach (DrawItem drawItem in drawItems)
            {
                if (drawItem.shadowColor.HasValue)
                    spriteBatch.DrawString(drawItem.font, drawItem.text, new Vector2(drawItem.pos.X + 1, drawItem.pos.Y + 1), drawItem.shadowColor.Value);
                spriteBatch.DrawString(drawItem.font, drawItem.text, drawItem.pos, drawItem.color);
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
            throw new NotImplementedException();
            //m_GameCanvas.AddImage(GameImages.Get(imageID), gx, gy);
        }

        public void UI_DrawImage(string imageID, int gx, int gy, Color tint)
        {
            throw new NotImplementedException();
            //m_GameCanvas.AddImage(GameImages.Get(imageID), gx, gy, tint);
        }

        public void UI_DrawImageTransform(string imageID, int gx, int gy, float rotation, float scale)
        {
            throw new NotImplementedException();
            //m_GameCanvas.AddImageTransform(GameImages.Get(imageID), gx, gy, rotation, scale);
        }

        public void UI_DrawGrayLevelImage(string imageID, int gx, int gy)
        {
            throw new NotImplementedException();
            //m_GameCanvas.AddImage(GameImages.GetGrayLevel(imageID), gx, gy);
        }

        public void UI_DrawTransparentImage(float alpha, string imageID, int gx, int gy)
        {
            throw new NotImplementedException();
            //m_GameCanvas.AddTransparentImage(alpha, GameImages.Get(imageID), gx, gy);
        }

        public void UI_DrawPoint(Color color, int gx, int gy)
        {
            throw new NotImplementedException();
            //m_GameCanvas.AddPoint(color, gx, gy);
        }

        public void UI_DrawLine(Color color, int gxFrom, int gyFrom, int gxTo, int gyTo)
        {
            throw new NotImplementedException();
            //m_GameCanvas.AddLine(color, gxFrom, gyFrom, gxTo, gyTo);
        }

        class DrawItem
        {
            public Color color;
            public Color? shadowColor;
            public SpriteFont font;
            public string text;
            public Vector2 pos;
        }

        private List<DrawItem> drawItems = new List<DrawItem>();

        public void UI_DrawString(Color color, string text, int gx, int gy, Color? shadowColor)
        {
            drawItems.Add(new DrawItem
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
            drawItems.Add(new DrawItem
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

            throw new NotImplementedException();
            //m_GameCanvas.AddRect(color, rect);
        }

        public void UI_FillRect(Color color, Rectangle rect)
        {
            if (rect.Width <= 0 || rect.Height <= 0)
                throw new ArgumentOutOfRangeException("rectangle Width/Height <= 0");

            throw new NotImplementedException();
            //m_GameCanvas.AddFilledRect(color, rect);
        }

        public void UI_DrawPopup(string[] lines, Color textColor, Color boxBorderColor, Color boxFillColor, int gx, int gy)
        {
            throw new NotImplementedException();

            /////////////////
            // Measure lines
            /////////////////
            /*int longestLineWidth = 0;
            int totalLineHeight = 0;
            Size[] linesSize = new Point[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                linesSize[i] = TextRenderer.MeasureText(lines[i], m_BoldFont);
                if (linesSize[i].Width > longestLineWidth)
                    longestLineWidth = linesSize[i].Width;
                totalLineHeight += linesSize[i].Height;
            }

            ///////////////////
            // Setup popup box
            ///////////////////
            const int BOX_MARGIN = 2;
            Point boxPos = new Point(gx, gy);
            Size boxSize = new Point(longestLineWidth + 2 * BOX_MARGIN, totalLineHeight + 2 * BOX_MARGIN);
            Rectangle boxRect = new Rectangle(boxPos, boxSize);

            //////////////////
            // Draw popup box
            //////////////////
            m_GameCanvas.AddFilledRect(boxFillColor, boxRect);
            m_GameCanvas.AddRect(boxBorderColor, boxRect);

            //////////////
            // Draw lines
            //////////////
            int lineX = boxPos.X + BOX_MARGIN;
            int lineY = boxPos.Y + BOX_MARGIN;
            for (int i = 0; i < lines.Length; i++)
            {
                m_GameCanvas.AddString(m_BoldFont, textColor, lines[i], lineX, lineY);
                lineY += linesSize[i].Height;
            }*/
        }

        // alpha10
        public void UI_DrawPopupTitle(string title, Color titleColor, string[] lines, Color textColor, Color boxBorderColor, Color boxFillColor, int gx, int gy)
        {
            throw new NotImplementedException();

            /////////////////
            // Measure lines
            /////////////////
            /*int longestLineWidth = 0;
            int totalLineHeight = 0;
            Size[] linesSize = new Point[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                linesSize[i] = TextRenderer.MeasureText(lines[i], m_BoldFont);
                if (linesSize[i].Width > longestLineWidth)
                    longestLineWidth = linesSize[i].Width;
                totalLineHeight += linesSize[i].Height;
            }

            Size titleSize = TextRenderer.MeasureText(title, m_BoldFont);
            if (titleSize.Width > longestLineWidth)
                longestLineWidth = titleSize.Width;
            totalLineHeight += titleSize.Height;
            const int TITLE_BAR_LINE = 1;
            totalLineHeight += TITLE_BAR_LINE;

            ///////////////////
            // Setup popup box
            ///////////////////
            const int BOX_MARGIN = 2;
            Point boxPos = new Point(gx, gy);
            Size boxSize = new Point(longestLineWidth + 2 * BOX_MARGIN, totalLineHeight + 2 * BOX_MARGIN);
            Rectangle boxRect = new Rectangle(boxPos, boxSize);

            //////////////////
            // Draw popup box
            //////////////////
            m_GameCanvas.AddFilledRect(boxFillColor, boxRect);
            m_GameCanvas.AddRect(boxBorderColor, boxRect);

            //////////////
            // Draw title
            //////////////
            int titleX = boxPos.X + BOX_MARGIN + (longestLineWidth - titleSize.Width) / 2;
            int titleY = boxPos.Y + BOX_MARGIN;
            int titleLineY = titleY + titleSize.Height + TITLE_BAR_LINE;
            m_GameCanvas.AddString(m_BoldFont, titleColor, title, titleX, titleY);
            m_GameCanvas.AddLine(boxBorderColor, boxRect.Left, titleLineY, boxRect.Right, titleLineY);

            //////////////
            // Draw lines
            //////////////
            int lineX = boxPos.X + BOX_MARGIN;
            int lineY = titleLineY + TITLE_BAR_LINE;

            for (int i = 0; i < lines.Length; i++)
            {
                m_GameCanvas.AddString(m_BoldFont, textColor, lines[i], lineX, lineY);
                lineY += linesSize[i].Height;
            }*/
        }

        // alpha10
        public void UI_DrawPopupTitleColors(string title, Color titleColor, string[] lines, Color[] colors, Color boxBorderColor, Color boxFillColor, int gx, int gy)
        {
            throw new NotImplementedException();

            /////////////////
            // Measure lines
            /////////////////
            /*int longestLineWidth = 0;
            int totalLineHeight = 0;
            Size[] linesSize = new Point[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                linesSize[i] = TextRenderer.MeasureText(lines[i], m_BoldFont);
                if (linesSize[i].Width > longestLineWidth)
                    longestLineWidth = linesSize[i].Width;
                totalLineHeight += linesSize[i].Height;
            }

            Size titleSize = TextRenderer.MeasureText(title, m_BoldFont);
            if (titleSize.Width > longestLineWidth)
                longestLineWidth = titleSize.Width;
            totalLineHeight += titleSize.Height;
            const int TITLE_BAR_LINE = 1;
            totalLineHeight += TITLE_BAR_LINE;

            ///////////////////
            // Setup popup box
            ///////////////////
            const int BOX_MARGIN = 2;
            Point boxPos = new Point(gx, gy);
            Size boxSize = new Point(longestLineWidth + 2 * BOX_MARGIN, totalLineHeight + 2 * BOX_MARGIN);
            Rectangle boxRect = new Rectangle(boxPos, boxSize);

            //////////////////
            // Draw popup box
            //////////////////
            m_GameCanvas.AddFilledRect(boxFillColor, boxRect);
            m_GameCanvas.AddRect(boxBorderColor, boxRect);

            //////////////
            // Draw title
            //////////////
            int titleX = boxPos.X + BOX_MARGIN + (longestLineWidth - titleSize.Width) / 2;
            int titleY = boxPos.Y + BOX_MARGIN;
            int titleLineY = titleY + titleSize.Height + TITLE_BAR_LINE;
            m_GameCanvas.AddString(m_BoldFont, titleColor, title, titleX, titleY);
            m_GameCanvas.AddLine(boxBorderColor, boxRect.Left, titleLineY, boxRect.Right, titleLineY);

            //////////////
            // Draw lines
            //////////////
            int lineX = boxPos.X + BOX_MARGIN;
            int lineY = titleLineY + TITLE_BAR_LINE;

            for (int i = 0; i < lines.Length; i++)
            {
                m_GameCanvas.AddString(m_BoldFont, colors[i], lines[i], lineX, lineY);
                lineY += linesSize[i].Height;
            }*/
        }

        public void UI_ClearMinimap(Color color)
        {
            throw new NotImplementedException();
            //m_GameCanvas.ClearMinimap(color);
        }

        public void UI_SetMinimapColor(int x, int y, Color color)
        {
            throw new NotImplementedException();
            //m_GameCanvas.SetMinimapColor(x, y, color);
        }

        public void UI_DrawMinimap(int gx, int gy)
        {
            throw new NotImplementedException();
            //m_GameCanvas.DrawMinimap(gx, gy);
        }

        public float UI_GetCanvasScaleX()
        {
            //return 1;
            throw new NotImplementedException();
            //return m_GameCanvas.ScaleX;
        }

        public float UI_GetCanvasScaleY()
        {
            //return 1;
            throw new NotImplementedException();
            //return m_GameCanvas.ScaleY;
        }

        public string UI_SaveScreenshot(string filePath)
        {
            //return "";
            throw new NotImplementedException();
            //return m_GameCanvas.SaveScreenShot(filePath);
        }
    }
}
