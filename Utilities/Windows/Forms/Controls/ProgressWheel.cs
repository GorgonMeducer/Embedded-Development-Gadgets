using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ESnail.Utilities.Windows.Forms.Controls
{
    public enum PROGRESS_WHEEL_STYLE
    { 
        PW_STYLE_STICKS,
        PW_STYLE_POINTS,
        PW_STYLE_PIE
    }

    public partial class ProgressWheel : UserControl
    {
        public ProgressWheel()
        {
            InitializeComponent();
        }
        
        private Int32 m_DrawIndex = 0;

        private System.Drawing.Color[] c_ColorPanel = new System.Drawing.Color[] 
                            {
                                System.Drawing.Color.FromArgb(255,255,255),
                                System.Drawing.Color.FromArgb(255,255,255),
                                System.Drawing.Color.FromArgb(105,105,105),
                                System.Drawing.Color.FromArgb(145,145,145),
                                System.Drawing.Color.FromArgb(180,180,180),
                                System.Drawing.Color.FromArgb(255,255,255),
                                System.Drawing.Color.FromArgb(255,255,255),
                                System.Drawing.Color.FromArgb(255,255,255),
                                System.Drawing.Color.FromArgb(255,255,255),
                                System.Drawing.Color.FromArgb(255,255,255),
                                System.Drawing.Color.FromArgb(255,255,255),
                                System.Drawing.Color.FromArgb(255,255,255)
                            };

        private void ProgressWheel_Paint(object sender, PaintEventArgs e)
        {
            Point tCenter = new Point(this.Width / 2, this.Height / 2);
            Double tRadius = Math.Min(this.Width, this.Height) / 2;
            Int32 tStartIndex = m_DrawIndex;
            Single tWidth = (Single)(tRadius * 0.15);

            PROGRESS_WHEEL_STYLE tStyle = this.Style;

            switch (tStyle)
            {
                case PROGRESS_WHEEL_STYLE.PW_STYLE_POINTS:
                    for (Int32 n = 0; n < 12; n++)
                    {
                        SolidBrush tBrush = new SolidBrush(c_ColorPanel[(tStartIndex + n) % 12]);
                        Pen tPen = new Pen(tBrush, tWidth);
                        Point tEndian = new Point(tCenter.X, tCenter.Y);
                        tEndian.X += (Int32)(tRadius * MathEx.Sin((Double)n * (Double)30.0) * 0.65);
                        tEndian.Y += (Int32)(tRadius * MathEx.Cos((Double)n * (Double)30.0) * 0.65);

                        e.Graphics.DrawEllipse(tPen, tEndian.X, tEndian.Y, tWidth, tWidth);
                    }
                    break;
                case PROGRESS_WHEEL_STYLE.PW_STYLE_PIE:
                    Rectangle tRec = new Rectangle((Int32)tWidth / 2, (Int32)tWidth / 2,(Int32)tRadius * 2 - (Int32)tWidth, (Int32)tRadius * 2 - (Int32)tWidth);
                    for (Int32 n = 0; n < 12; n++)
                    {
                        SolidBrush tBrush = new SolidBrush(c_ColorPanel[(tStartIndex + n) % 12]);

                        e.Graphics.FillPie(tBrush, tRec,-((float)n * (float)30), (float)25); 
                    }
                    break;
                default:
                case PROGRESS_WHEEL_STYLE.PW_STYLE_STICKS:
                    for (Int32 n = 0; n < 12; n++)
                    {

                        SolidBrush tBrush = new SolidBrush(c_ColorPanel[(tStartIndex + n) % 12]);
                        Pen tPen = new Pen(tBrush, tWidth);



                        Point tEndian = new Point(tCenter.X, tCenter.Y);
                        Point tStart = new Point(tCenter.X, tCenter.Y);
                        tEndian.X += (Int32)(tRadius * MathEx.Sin((Double)n * (Double)30.0));
                        tEndian.Y += (Int32)(tRadius * MathEx.Cos((Double)n * (Double)30.0));
                        tStart.X += (Int32)(tRadius * 0.4 * MathEx.Sin((Double)n * (Double)30.0));
                        tStart.Y += (Int32)(tRadius * 0.4 * MathEx.Cos((Double)n * (Double)30.0));

                        e.Graphics.DrawLine(tPen, tStart, tEndian);

                    }
                    break;
                
            }

            
        }

        private void timerDisplay_Tick(object sender, EventArgs e)
        {
            m_DrawIndex += 1;
            m_DrawIndex %= 12;

            this.Refresh();
        }

        public PROGRESS_WHEEL_STYLE Style
        {
            get;
            set;
        }
        
    }
}
