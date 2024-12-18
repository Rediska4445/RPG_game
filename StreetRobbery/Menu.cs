using System;
using System.Windows.Forms;

namespace StreetRobbery
{
    public partial class Menu : Form
    {
        public int countPassebry = 1;
        public int timePolice = 5;
        public int moneyLeave = 1000;
        public int spawnEnemyTime = 1_000;

        public Menu()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Hide();
            new Setting().Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {   
            Environment.Exit(0);
        }

        private void play_Click(object sender, EventArgs e)
        {
            Hide();
            new Form1(timePolice, countPassebry, moneyLeave, spawnEnemyTime, trackBar2.Value, trackBar3.Value).Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            new Shop(this).Show();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            try
            {
                trackBar2.Value -= 1;
            } catch (Exception) { };
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            try
            {
                trackBar3.Value = trackBar2.Maximum - trackBar2.Value;
            }
            catch (Exception) { }
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            try
            {
                trackBar2.Value = trackBar3.Maximum - trackBar3.Value;
            } catch (Exception) { };
        }
    }
}
