using djack.RogueSurvivor.Engine;
using djack.RogueSurvivor.Gameplay;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading;
using System.Windows.Forms;

namespace djack.RogueSurvivor
{
    public class RogueForm : Game, IRogueUI
    {
        private GraphicsDeviceManager graphics;
        private bool initialized;

        RogueGame m_Game;
        // FIXME
        //Font m_NormalFont;
        //Font m_BoldFont;

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

            // FIXME
            //Logger.WriteLine(Logger.Stage.INIT_MAIN, "create font 1...");
            //m_NormalFont = new Font("Lucida Console", 8.25f, FontStyle.Regular);
            //Logger.WriteLine(Logger.Stage.INIT_MAIN, "create font 2...");
            //m_BoldFont = new Font("Lucida Console", 8.25f, FontStyle.Bold);
        }

        protected override void Initialize()
        {
            base.Initialize();

            Logger.WriteLine(Logger.Stage.INIT_MAIN, "Initializing game...");

            Window.Title = "Rogue Survivor - " + SetupConfig.GAME_VERSION;

            m_Game = new RogueGame(this);

           
        }

        /*protected override void LoadContent()
        {
            

            m_Game.Init();

            SuppressDraw();
        }*/

        protected override void Update(GameTime gameTime)
        {
            if(!initialized)
            {
                Logger.WriteLine(Logger.Stage.INIT_GFX, "loading images...");
                GameImages.LoadResources(this);
                Logger.WriteLine(Logger.Stage.INIT_GFX, "loading images done");
            }
            //if (!m_Game.Update())
            //    EndRun();
        }

        public KeyEventArgs UI_WaitKey()
        {
            // FIXME
            return null;
        }

        public KeyEventArgs UI_PeekKey()
        {
            // FIXME
            return null;
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
                case Keys.ShiftKey:
                case Keys.Shift:
                case Keys.LShiftKey:
                case Keys.RShiftKey:
                case Keys.Control:
                case Keys.ControlKey:
                case Keys.RControlKey:
                case Keys.LControlKey:
                case Keys.Alt:
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
            if (e.KeyCode == Keys.F6)
            {
                if (m_Game.Session != null && m_Game.Session.CurrentMap != null)
                {
                    m_Game.Session.CurrentMap.SetAllAsVisited();
                    UI_Repaint();
                }
            }
            // F7 - DEV - toggle FPS
            if (e.KeyCode == Keys.F7)
            {
                // FIXME
                //m_GameCanvas.ShowFPS = !m_GameCanvas.ShowFPS;
                UI_Repaint();
            }
            // F8 - DEV - resize to normal size
            if (e.KeyCode == Keys.F8)
            {
                // FIXME
                //m_GameCanvas.NeedRedraw = true;
                //SetClientSizeCore(RogueGame.CANVAS_WIDTH, RogueGame.CANVAS_HEIGHT);
                UI_Repaint();
            }
            // F9 - DEV - Show actors stats
            if (e.KeyCode == Keys.F9)
            {
                m_Game.DEV_ToggleShowActorsStats();
                UI_Repaint();
            }
            // F10 - DEV - Show pop graph.
#if DEBUG_STATS
            if (e.KeyCode == Keys.F10)
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
            if (e.KeyCode == Keys.F11)
            {
                m_Game.DEV_TogglePlayerInvincibility();
                UI_Repaint();
            }

            // F12 - DEV - Max trust for all player followers
            if (e.KeyCode == Keys.F12)
            {
                m_Game.DEV_MaxTrust();
                UI_Repaint();
            }

            // alpha10.1
            // INSERT - DEV - Toggle bot mode
            if (e.KeyCode == Keys.Insert)
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

        public MouseButtons? UI_PeekMouseButtons()
        {
            // FIXME
            /*if (!m_HasMouseButtons)
                return null;

            m_HasMouseButtons = false;
            return m_MouseButtons;*/
            return null;
        }

        public void UI_SetCursor(Cursor cursor)
        {
            // FIXME
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

        public void UI_Repaint()
        {
            // FIXME
            //Refresh();
            //Application.DoEvents();
        }

        public void UI_Clear(Color clearColor)
        {
            // FIXME
            //m_GameCanvas.Clear(clearColor);
        }

        public void UI_DrawImage(string imageID, int gx, int gy)
        {
            // FIXME
            //m_GameCanvas.AddImage(GameImages.Get(imageID), gx, gy);
        }

        public void UI_DrawImage(string imageID, int gx, int gy, Color tint)
        {
            // FIXME
            //m_GameCanvas.AddImage(GameImages.Get(imageID), gx, gy, tint);
        }

        public void UI_DrawImageTransform(string imageID, int gx, int gy, float rotation, float scale)
        {
            // FIXME
            //m_GameCanvas.AddImageTransform(GameImages.Get(imageID), gx, gy, rotation, scale);
        }

        public void UI_DrawGrayLevelImage(string imageID, int gx, int gy)
        {
            // FIXME
            //m_GameCanvas.AddImage(GameImages.GetGrayLevel(imageID), gx, gy);
        }

        public void UI_DrawTransparentImage(float alpha, string imageID, int gx, int gy)
        {
            // FIXME
            //m_GameCanvas.AddTransparentImage(alpha, GameImages.Get(imageID), gx, gy);
        }

        public void UI_DrawPoint(Color color, int gx, int gy)
        {
            // FIXME
            //m_GameCanvas.AddPoint(color, gx, gy);
        }

        public void UI_DrawLine(Color color, int gxFrom, int gyFrom, int gxTo, int gyTo)
        {
            // FIXME
            //m_GameCanvas.AddLine(color, gxFrom, gyFrom, gxTo, gyTo);
        }

        public void UI_DrawString(Color color, string text, int gx, int gy, Color? shadowColor)
        {
            // FIXME
            //if (shadowColor != null)
            //    m_GameCanvas.AddString(m_NormalFont, shadowColor.Value, text, gx + 1, gy + 1);
            //m_GameCanvas.AddString(m_NormalFont, color, text, gx, gy);
        }

        public void UI_DrawStringBold(Color color, string text, int gx, int gy, Color? shadowColor)
        {
            // FIXME
            //if (shadowColor != null)
            //    m_GameCanvas.AddString(m_BoldFont, shadowColor.Value, text, gx + 1, gy + 1);
            //m_GameCanvas.AddString(m_BoldFont, color, text, gx, gy);
        }

        public void UI_DrawRect(Color color, Rectangle rect)
        {
            if (rect.Width <= 0 || rect.Height <= 0)
                throw new ArgumentOutOfRangeException("rectangle Width/Height <= 0");

            // FIXME
            //m_GameCanvas.AddRect(color, rect);
        }

        public void UI_FillRect(Color color, Rectangle rect)
        {
            if (rect.Width <= 0 || rect.Height <= 0)
                throw new ArgumentOutOfRangeException("rectangle Width/Height <= 0");

            // FIXME
            //m_GameCanvas.AddFilledRect(color, rect);
        }

        public void UI_DrawPopup(string[] lines, Color textColor, Color boxBorderColor, Color boxFillColor, int gx, int gy)
        {
            // FIXME

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
            // FIXME

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
            // FIXME

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
            // FIXME
            //m_GameCanvas.ClearMinimap(color);
        }

        public void UI_SetMinimapColor(int x, int y, Color color)
        {
            // FIXME
            //m_GameCanvas.SetMinimapColor(x, y, color);
        }

        public void UI_DrawMinimap(int gx, int gy)
        {
            // FIXME
            //m_GameCanvas.DrawMinimap(gx, gy);
        }

        public float UI_GetCanvasScaleX()
        {
            return 1;
            // FIXME
            //return m_GameCanvas.ScaleX;
        }

        public float UI_GetCanvasScaleY()
        {
            return 1;
            // FIXME
            //return m_GameCanvas.ScaleY;
        }

        public string UI_SaveScreenshot(string filePath)
        {
            return "";
            // FIXME
            //return m_GameCanvas.SaveScreenShot(filePath);
        }
    }
}
