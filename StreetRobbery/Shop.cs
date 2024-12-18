using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace StreetRobbery
{
    public partial class Shop : Form
    {
        private static int money;
        private static Guns current_gun;
        private static Drugs drug;
        public static int level;
        public static int health;
        private static int current_ammo;
        private static int ammo;
        private Form form;

        public Shop(Form form)
        {
            InitializeComponent();
            this.form = form;
            if (File.Exists("data.txt"))
            {
                using (StreamReader reader = new StreamReader("data.txt"))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Contains("money"))
                        {
                            money = int.Parse(line.Substring(line.IndexOf(":")).Substring(1));
                            label1.Text = "Ur money: " + money.ToString();
                        }
                        else if (line.Contains("gun"))
                        {
                            string gun = line.Substring(line.IndexOf(":")).Substring(1);
                            switch (gun)
                            {
                                case "Glock":
                                    current_gun = Guns.Glock;
                                    label2.Text = "Ur guns: posoh 1";
                                    break;
                                case "Sawed_Off":
                                    current_gun = Guns.Sawed_Off;
                                    label2.Text = "Ur guns: posho 2";
                                    break;
                                case "Deagle":
                                    current_gun = Guns.Deagle;
                                    label2.Text = "Ur guns: posho 3";
                                    break;
                                default:
                                    current_gun = Guns.Glock;
                                    label2.Text = "Ur guns: posoh 1";
                                    break;
                            }
                        }
                        else if (line.Contains("ammo"))
                        {
                            ammo = int.Parse(line.Substring(line.IndexOf(":")).Substring(1));
                        }
                        else if (line.Contains("current_ammo"))
                        {
                            current_ammo = int.Parse(line.Substring(line.IndexOf(":")).Substring(1));
                        }
                        else if (line.Contains("health"))
                        {
                            health = int.Parse(line.Substring(line.IndexOf(":")).Substring(1));
                        }
                        else if (line.Contains("level"))
                        {
                            level = int.Parse(line.Substring(line.IndexOf(":")).Substring(1));
                        }
                    }
                }
            }
            label3.Text = "Ur buffs: while would be nothing";
        }

        private void Shop_Load(object sender, EventArgs e)
        {

        }

        private void Glock_Click(object sender, EventArgs e)
        {
            if (money >= 750)
            { 
                if(current_gun == Guns.Glock)
                {
                    ammo += 54;
                    money -= 350;
                } else
                {
                    current_gun = Guns.Glock;
                    ammo = 54;
                    current_ammo = 17;
                    money -= 750;
                }
                label2.Text = "Ur guns: " + "Posoh1";
                label1.Text = "Ur money: " + money;
            }
        }

        private void Sawed_off_Click(object sender, EventArgs e)
        {
            if (money >= 1000)
            {
                if (current_gun == Guns.Sawed_Off)
                {
                    ammo += 31;
                    money -= 350;
                }
                else
                {
                    current_gun = Guns.Sawed_Off;
                    ammo = 31;
                    current_ammo = 5;
                    money -= 1000;
                }
                label2.Text = "Ur guns: " + "Posoh2";
                label1.Text = "Ur money: " + money;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (StreamWriter writer = new StreamWriter("data.txt", false))
            {
                writer.WriteLineAsync("money:" + money + "\ngun:" + current_gun
                + "\ncurrent_ammo:" + current_ammo + "\nammo:" + ammo
                + "\nhealth:" + health + "\nlevel:" + level);
            }
            Hide();
            form.Show();
        }

        private void Deagle_Click(object sender, EventArgs e)
        {
            if (money >= 1500)
            {
                if (current_gun == Guns.Deagle)
                {
                    ammo += 26;
                    money -= 350;
                }
                else
                {
                    current_gun = Guns.Deagle;
                    ammo = 26;
                    current_ammo = 7;
                    money -= 1500;
                }

                label2.Text = "Ur guns: Posoh 3";
                label1.Text = "Ur money: " + money;
            }
        }

        private void Shop_FormClosing(object sender, FormClosingEventArgs e)
        {
            using (StreamWriter writer = new StreamWriter("data.txt", false))
            {
                writer.WriteLine("money:" + money + "\ngun:" + current_gun
                                    + "\ncurrent_ammo:" + current_ammo + "\nammo:" + ammo);
            }
            Environment.Exit(0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(money >= 250 && drug != Drugs.Crack && level >= 200)
            {
                drug = Drugs.Crack;
                money -= 250;

                label3.Text = "Ur buff: 2";
                label1.Text = "Ur money: " + money;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (money >= 150 && drug != Drugs.Sativa && level >= 100)
            {
                drug = Drugs.Sativa;
                money -= 150;

                label3.Text = "Ur buffs: 1";
                label1.Text = "Ur money: " + money;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (money >= 500 && drug != Drugs.Cocaine && level >= 500)
            {
                drug = Drugs.Cocaine;
                money -= 500;
                label3.Text = "Ur drug: " + drug;
                label1.Text = "Ur money: " + money;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
