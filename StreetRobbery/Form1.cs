using StreetRobbery.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StreetRobbery
{
    public partial class Form1 : Form
    {
        Robber Player;
        Robber Gangsta_Enemy;

        public const int maxLocation = 200;
        public const int minLocation = 490;
        public const int BulletSpeed = 10;

        public int speed_d;

        public int dura;

        public bool cooldownShoot;

        public int seconds_int;
        public int minutes_int;

        public const int diaposoneInteractive = 150;
        public const int pleaseDontKillMe = 2000;
        public const int RUN_MOTHERFUCKER = 75;

        public bool movePassebry;
        public const int step = 15;

        public Form1(int timerPolice, int countPassebry, int moneyForLeave, int spawnEnemyTime, int power, int speed)
        {
            InitializeComponent();

            cooldownShoot = true;
            speed_d = speed;

            seconds.Text = seconds_int.ToString();
            minutes.Text = minutes_int.ToString();

            Player = new Robber(Resources.gangsta, Bands.Crips, new Random().Next(
                100, 700), minLocation);
            Player.Form(this);
            Player.money = 100;
            Player.power = power;
            Player.health = 100;
            Player.current_gun = Guns.Glock;
            Player.Current_Ammo = 1;
            label9.Text = "ХП: " + Player.health;

            Background.Controls.Add(Player.pictureBox);

            Gangsta_Enemy = new Robber(Resources.gangsta1, Bands.Bloods, 
                Background.Image.Width, maxLocation);
            Gangsta_Enemy.Form(this);
            Background.Controls.Add(Gangsta_Enemy.pictureBox);

            label2.Text = "Money: " + Player.money.ToString() + " @";
            timer1.Start();

            timer2.Interval = 2000 - Player.level;
            timer3.Interval = spawnEnemyTime;
            timer3.Start();

            new Task(() =>
            {
                while (true)
                {
                    if (Gangsta_Enemy.isKilled)
                    {
                        timer2.Stop();
                        
                        label2.Text = "Money: " + Player.money + " @";
                        label10.Text = "Level of Hero: " + Player.level;
                    }

                    if (Player.isKilled || Player.health <= 0)
                        End("Ты погиб от руки мага!", "Ты умер");
                }
            }).Start();
        }

        public void SpawnEnemy()
        {
            Gangsta_Enemy = new Robber(Resources.gangsta1, Bands.Bloods,
                 Background.Image.Width, maxLocation);
            Gangsta_Enemy.current_gun = Guns.Glock;
            Gangsta_Enemy.Form(this);
            Gangsta_Enemy.pictureBox.Click += (s, e) =>
            {
                try
                {
                    if(cooldownShoot)
                    {
                        Player.Shoot(Gangsta_Enemy, BulletSpeed);
                        label2.Text = "Money: " + Player.money + " @";
                        label10.Text = "Level of Hero: " + Player.level;
                    }
                    new Thread(() =>
                    {
                        cooldownShoot = false;
                        Ammo.Text ="0 : 1";
                        Thread.Sleep(speed_d * 1000);
                        Ammo.Text = "1 : 1";
                        cooldownShoot = true;
                    }).Start();
                } catch(IndexOutOfRangeException) {}
                label7.Text = Gangsta_Enemy.health.ToString();
            };

            Background.Controls.Add(Gangsta_Enemy.pictureBox);
            MoveEnemyX(Background.Width, new Random(5).Next(Gangsta_Enemy.pictureBox.Width, Background.Width / 2)).Start();
        }

        public Task MoveEnemyX(int from, int to)
        {
            return new Task(() =>
            {
                if (to > from)
                {
                    for (int x1 = from; x1 < to; x1 += step / 3)
                    {
                        Thread.Sleep(10);
                        Gangsta_Enemy.SetLocation(x1, Gangsta_Enemy.pictureBox.Location.Y);
                    }
                }
                else
                {
                    for (int x1 = from; x1 > to; x1 -= step / 3)
                    {
                        Thread.Sleep(10);
                        Gangsta_Enemy.SetLocation(x1, Gangsta_Enemy.pictureBox.Location.Y);
                    }
                }
                MoveEnemyY(Player.pictureBox.Location.Y, Gangsta_Enemy.pictureBox.Location.Y, step / 3).Start();
            });
        }

        public Task MoveEnemyY(int playerY, int GangstaY, int steps)
        {
            return new Task(() =>
            {
                if (playerY > GangstaY)
                {
                    for (int x1 = GangstaY; x1 < playerY; x1 += steps)
                    {
                        Thread.Sleep(10);
                        Gangsta_Enemy.SetLocation(Gangsta_Enemy.pictureBox.Location.X, x1);
                    }
                }
                else
                {
                    for (int x1 = GangstaY; x1 > playerY; x1 -= steps)
                    {
                        Thread.Sleep(10);
                        Gangsta_Enemy.SetLocation(Gangsta_Enemy.pictureBox.Location.X, x1);
                    }
                }
                Gangsta_Enemy.pictureBox.Image = Resources.gangsta_interact1;
            });
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!Background.Controls.Contains(Gangsta_Enemy.pictureBox))
            {
                timer2.Stop();
                timer3.Start();
            }
        }

        public void End(string msg, string msgTitle)
        {
            MessageBox.Show(
                msgTitle,
                msg,
                MessageBoxButtons.OK);
            Hide();
            new Menu().Show();
            timer1.Stop();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (StreamWriter writer = new StreamWriter("data.txt", false))
            {
                writer.WriteLineAsync("money:" + Player.money + "\ngun:" + Player.current_gun
                 + "\ncurrent_ammo:" + Player.Current_Ammo + "\nammo:" + Player.Ammo
                 + "\nhealth:" + Player.health + "\nlevel:" + Player.level);
            }
            Hide();
            new Menu().Show();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                if (Player.pictureBox.Location.Y > maxLocation)
                    Player.SetLocation(Player.pictureBox.Location.X, Player.pictureBox.Location.Y - step);
            }
            if (e.KeyCode == Keys.A)
            {
                Player.pictureBox.Image = Resources.gangsta_back;
                if (Player.pictureBox.Location.X > 0)
                    Player.SetLocation(Player.pictureBox.Location.X - step, Player.pictureBox.Location.Y);
            }
            if (e.KeyCode == Keys.S)
            {
                if (Player.pictureBox.Location.Y < Background.Height - Player.pictureBox.Height)
                    Player.SetLocation(Player.pictureBox.Location.X, Player.pictureBox.Location.Y + step);
            }
            if (e.KeyCode == Keys.D)
            {
                Player.pictureBox.Image = Resources.gangsta;
                if (Player.pictureBox.Location.X < Background.Width - Player.pictureBox.Width)
                    Player.SetLocation(Player.pictureBox.Location.X + step, Player.pictureBox.Location.Y);
            }
            if (!(Gangsta_Enemy.pictureBox.Location.Y + Gangsta_Enemy.pictureBox.Height >= Player.pictureBox.Location.Y
                 && Gangsta_Enemy.pictureBox.Location.Y + Gangsta_Enemy.pictureBox.Height <= Player.pictureBox.Location.Y + Player.pictureBox.Height))
            {
                if (Gangsta_Enemy.Location.X < Player.Location.X)
                    Gangsta_Enemy.pictureBox.Image = Resources.gangsta1;
                else
                    Gangsta_Enemy.pictureBox.Image = Resources.gangsta_back1;
            } 
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            using (StreamWriter writer = new StreamWriter("data.txt", false))
            {
                writer.WriteLine("money:" + Player.money + "\ngun:" + Player.current_gun
                    + "\ncurrent_ammo:" + Player.Current_Ammo + "\nammo:" + Player.Ammo
                    + "\nhealth:" + Player.health + "\nlevel" + Player.level);
            }
            Environment.Exit(0);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (Gangsta_Enemy.pictureBox.Location.Y + Gangsta_Enemy.pictureBox.Height >= Player.pictureBox.Location.Y
                 && Gangsta_Enemy.pictureBox.Location.Y <= Player.pictureBox.Location.Y + Player.pictureBox.Height)
            {
                Gangsta_Enemy.Shoot(Player, BulletSpeed);
                label9.Text = "ХП: " + Player.health;
            }
            else
                MoveEnemyY(Player.pictureBox.Location.Y, Gangsta_Enemy.pictureBox.Location.Y, step / 3).Start();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            SpawnEnemy();
            timer2.Start();
            timer3.Stop();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (StreamWriter writer = new StreamWriter("data.txt", false))
            {
                writer.WriteLine("money:" + Player.money + "\ngun:" + Player.current_gun
                 + "\ncurrent_ammo:" + Player.Current_Ammo + "\nammo:" + Player.Ammo
                 + "\nhealth:" + Player.health);
            }
            Hide();
            new Shop(this).Show();
        }
    }

    public enum Bands
    {
        Bloods,
        Crips
    }

    public enum Guns
    {
        Glock,
        Sawed_Off,
        MAC_10,
        Deagle
    }

    public enum Drugs
    {
        Sativa, // trava
        Crack,
        Cocaine
    }

    public class Robber : Entity
    {
        public Bands banda;
        public int health;
        public int level;
        public bool isKilled;
        public int Current_Ammo;
        public int Ammo;
        public PictureBox pictureBox;
        public Point Location;
        public int money;
        public Guns current_gun;

        public int power;

        private PictureBox drug_smoke;
        public bool isSmoking;

        Form1 form;

        public void Form(Form1 form)
        {
            this.form = form;
        }

        public Robber(Image image, Bands banda, int x, int y) // size = 88:204
        {
            this.banda = banda;
            current_gun = Guns.Glock;
            isKilled = false;
            money = 10;
            health = 5;
            level = 1;
            pictureBox = new PictureBox();
            pictureBox.Image = image;
            pictureBox.Size = new Size(80, 100);
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.BackColor = Color.Transparent;
            pictureBox.Location = new Point(x, y);
        }

        public Bullet Shoot(Robber to, int bulletSpeed)
        {
            Bullet bullet = new Bullet(new Size(50, 50), Resources.bullet);
            bullet.speed = bulletSpeed;
            bullet.bullet.Location = new Point(pictureBox.Location.X, pictureBox.Location.Y);
            form.Background.Controls.Add(bullet.bullet);

            PictureBox blood = new PictureBox();
            blood.Image = Resources.blood;
            blood.Location = new Point(form.Background.Width, form.Background.Height);
            blood.Size = new Size(bullet.bullet.Size.Width, bullet.bullet.Size.Height);
            blood.SizeMode = PictureBoxSizeMode.Zoom;
            blood.BackColor = Color.Transparent;
            form.Background.Controls.Add(blood);

            CheckRotateSkin(to);
            if (pictureBox.Location.X < to.pictureBox.Location.X)
                bullet.bullet.Image = Resources.bullet;
            else 
                bullet.bullet.Image = Resources.bullet_back;

            new Task(() =>
            {
                int xTo = pictureBox.Location.X > to.pictureBox.Location.X ? -bullet.speed : bullet.speed;
                for (int x = pictureBox.Location.X; x != to.pictureBox.Location.X; x += xTo)
                {
                    Thread.Sleep(10);
                    bullet.bullet.Location = new Point(x, bullet.bullet.Location.Y);
                    if (bullet.bullet.Location.X >= to.pictureBox.Location.X
                             && bullet.bullet.Location.X <= to.pictureBox.Location.X + to.pictureBox.Width
                             && bullet.bullet.Location.Y >= to.pictureBox.Location.Y
                             && bullet.bullet.Location.Y <= to.pictureBox.Location.Y + to.pictureBox.Height)
                    {
                        if (to.health <= 1) { 
                            to.Kill(blood, this); 
                        } else { 
                            switch(current_gun)
                            {
                                case Guns.Glock:
                                    to.health -= 1 + power;
                                    break;
                                case Guns.Sawed_Off:
                                    to.health -= 3 + power;
                                    break;
                                case Guns.Deagle:
                                    to.health -= 5 + power;
                                    break;
                            }
                        }
                        break;
                    }
                }
                bullet.bullet.Dispose();
                CheckRotateSkin(to);
            }).Start();
            return bullet;
        }

        public void Kill(PictureBox blood, Robber whoKilled)
        {
            pictureBox.Dispose();
            Task.Run(() =>
            {
                blood.BringToFront();
                blood.Location = new Point(this.pictureBox.Location.X, this.pictureBox.Location.Y);
                whoKilled.money += new Random().Next(25, 100);
                whoKilled.level += 1;
                Thread.Sleep(1500);
                blood.Dispose();
            });
            isKilled = true;
        }

        private void CheckRotateSkin(Robber to)
        {
            switch (banda) {
                case Bands.Bloods:
                    if (pictureBox.Location.X < to.pictureBox.Location.X)
                        pictureBox.Image = Resources.gangsta_interact1;
                    else
                        pictureBox.Image = Resources.gangsta_interact_back1;
                    break;
                case Bands.Crips:
                    if (pictureBox.Location.X < to.pictureBox.Location.X)
                    {
                        if(isSmoking)
                            pictureBox.Image = Resources.gangsta_smoking_interact;
                        else
                            pictureBox.Image = Resources.gangsta_interact;
                    }
                    else
                    { 
                        if (isSmoking)
                            pictureBox.Image = Resources.gangsta_smoking_interact_back;
                        else
                            pictureBox.Image = Resources.gangsta_interact_back;
                    }
                    break;
            } 
        }

        public void SetLocation(int x, int y)
        {
            pictureBox.Location = new Point(x, y);
            if (isSmoking)
                drug_smoke.Location = new Point(x + 5, y - 50);
        }
    }

    public class Bullet
    {
        public int speed;
        public PictureBox bullet;
         
        public Bullet(Size size, Image image)
        {
            bullet = new PictureBox();
            bullet.Image = image;
            bullet.Size = size;
            bullet.SizeMode = PictureBoxSizeMode.Zoom;
            bullet.BackColor = Color.Transparent;
        }
    }

    interface Entity
    {
        void SetLocation(int x, int y);
    }
}