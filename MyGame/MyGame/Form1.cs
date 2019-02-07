using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyGame
{                                               
    // Sterowanie -  W A S D, Strzał - Spacja, Start i Restart - Enter                                          
    public partial class Form1 : Form             
    {
        bool goLeft, goRight, goUp, goDown;
        bool isPressed;
        bool isPressedEn;
        int playerSpeed = 10;
        int wave = 1;
        int yBullet1 = 0;
        public Form1()
        {
            InitializeComponent();
        }
        // Sterowanie statkiem gracza
        private void Key_Down(object sender, KeyEventArgs e)
        {
            
            if (e.KeyCode == Keys.A)
            {
                if (Player.Location.X < 0)
                {
                    goLeft = false;
                    Player.Image = Properties.Resources.PlayerLeft;
                    
                }
                else
                    goLeft = true;
                Player.Image = Properties.Resources.PlayerLeft;
            }
            if (e.KeyCode == Keys.D)
            {
                if (Player.Location.X > 837)
                {
                    goRight = false;
                    Player.Image = Properties.Resources.PlayerRight;
                }
                else
                    goRight = true;
                Player.Image = Properties.Resources.PlayerRight;
            }
            if (e.KeyCode == Keys.W)
                if (Player.Location.Y < 0)
                    goUp = false;
                else
                    goUp = true;
            if (e.KeyCode == Keys.S)
                if (Player.Location.Y > 483)
                    goDown = false;
                else
                    goDown = true;
            // Strzał na spacji
            if (e.KeyCode == Keys.Space && !isPressed)
            {
                isPressed = true;               
                makeBullet(2);
            }
            // Restart programu
            if(e.KeyCode == Keys.Enter && !isPressed)
            {
                isPressedEn = true;
                timer1.Enabled = true;
                timer1.Start();
                wave1timer.Enabled = true;
                wave1timer.Start();
                label2.Visible = false;                
                if (label2.Text == "Game Over" || label2.Text == "You Won")
                    Application.Restart();
            }
        }
        // Przerwanie sterowania statkiem gracza
        private void Key_Up(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.A)
            {
                goLeft = false;
                Player.Image = Properties.Resources.Player;
            }
            if (e.KeyCode == Keys.D)
            {
                goRight = false;
                Player.Image = Properties.Resources.Player;
            }
            if (e.KeyCode == Keys.W)
                goUp = false;
            if (e.KeyCode == Keys.S)
                goDown = false;
            // Strzał na spacji
            if (isPressed)
                isPressed = false;
            if (isPressedEn)
                isPressedEn = false;
        }
        // Główny Timer do kierowania graczem, definiuje szybkość statku gracza
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (goLeft)
                Player.Left -= playerSpeed;
            if (goRight)
                Player.Left += playerSpeed;
            if (goUp)
                Player.Top -= playerSpeed;
            if (goDown)
                Player.Top += playerSpeed;

            // Sprawdzenie kolizji statku gracza ze statkiem przeciwnika
            foreach (Control x in this.Controls)
                if (x is PictureBox && x.Tag == "enemy" || x.Tag == "enemy5" || x.Tag == "enemy-5" || x.Tag == "boss")
                    if (((PictureBox)x).Bounds.IntersectsWith(Player.Bounds))
                        gameOver();
            // Sprawdzenie kolizji pocisku gracza ze statkiem przeciwnika
            foreach (Control i in this.Controls)
                foreach (Control j in this.Controls)
                    if (i is PictureBox && i.Tag == "enemy")
                        if (j is PictureBox && j.Tag == "bullet")
                            if (i.Bounds.IntersectsWith(j.Bounds))
                            {
                                this.Controls.Remove(i);
                                // Pociski znikają wraz z przeciwnikiem
                                this.Controls.Remove(j);
                                
                                switch (i.Name) 
                                {
                                    case "60":
                                        enemyAlive = false;
                                        break;
                                    
                                }
                            }
            
             // Trafienie pocisku przeciwnika w statek gracza                 
             foreach (Control i in this.Controls)
                foreach (Control j in this.Controls)
                    if (i is PictureBox && i.Tag == "main")
                        if (j is PictureBox && j.Tag == "bulletEnemy")
                            if (i.Bounds.IntersectsWith(j.Bounds))
                            {
                                this.Controls.Remove(j);
                                gameOver();
                            }
            // Animacja pocisku gracza
            foreach (Control y in this.Controls)
            {
                if (y is PictureBox && y.Tag == "bullet")
                {
                    y.Top -= 20;
                    if (((PictureBox)y).Top < -20)
                        this.Controls.Remove(y);
                }
            }
            // Animacja pocisku przeciwnika
            foreach (Control y in this.Controls)
            {
                if (y is PictureBox && y.Tag == "bulletEnemy")
                {
                    y.Top += 10; // Szybkość pocisków przeciwnika
                    if (((PictureBox)y).Top > 570 || ((PictureBox)y).Top < -50)
                        this.Controls.Remove(y);
                }
            }
            // Ruch przeciwnika
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && x.Tag == "enemy")
                {
                    ((PictureBox)x).Top += 3;
                    if (((PictureBox)x).Top > 600)
                        this.Controls.Remove(x);
                }
            }
            
            // Granice ruchu statku gracza
            if (Player.Location.X < 0)
                goLeft = false;
            if (Player.Location.X > 837)
                goRight = false;
            if (Player.Location.Y < 0)
                goUp = false;
            if (Player.Location.Y > 480)
                goDown = false;
        }
        // Reset pozycji pocisku przeciwnika
        public void BulletMoveWave1(object sender, EventArgs e)
        {
            if (yBullet1 < 600)
                yBullet1 += 95;
        }
        
        // Przeciwnicy w pierwszej fali
        private void Wave1_Timer(object sender, EventArgs e)
        {
            if (wave == 1)
            {
                enemy(60, -20);
                enemy(190, -20);
                enemy(125, -90);
                enemy(760, -20);
                enemy(630, -20);
                enemy(695, -90);
                enemy(350, -20);
                enemy(470, -20);
                enemy(300, -130);
                enemy(520, -130);
                enemy(10, -100);
                enemy(800, -100);
                enemy(410, -100);
            }
            // wave = 2;
            wave1timer.Stop();
            // wave2timer.Enabled = true;
            enemyBulletTimer.Tick += new EventHandler(BulletMoveWave1);
        }
        
        // Pozycje z których wylatuje pocisk przeciwnika
        private void enemy_bullet_timer(object sender, EventArgs e)
        {
            // Wave 1
            if (wave == 2 && enemyAlive)
                bulletEnemyGreenBig(60, -10 + yBullet1);
            if (wave == 2 && enemyAlive1)
                bulletEnemyGreenBig(190, -10 + yBullet1);
            if (wave == 2 && enemyAlive2)
                bulletEnemyGreenBig(125, -80 + yBullet1);
            if (wave == 2 && enemyAlive3)
                bulletEnemyGreenBig(760, -10 + yBullet1);
            if (wave == 2 && enemyAlive4)
                bulletEnemyGreenBig(630, -10 + yBullet1);
            if (wave == 2 && enemyAlive5)
                bulletEnemyGreenBig(695, -80 + yBullet1);
            if (wave == 2 && enemyAlive6)
                bulletEnemyGreenBig(350, -10 + yBullet1);
            if (wave == 2 && enemyAlive7)
                bulletEnemyGreenBig(470, -10 + yBullet1);
            if (wave == 2 && enemyAlive8)
                bulletEnemyGreenBig(300, -120 + yBullet1);
            if (wave == 2 && enemyAlive9)
                bulletEnemyGreenBig(520, -120 + yBullet1);
            if (wave == 2 && enemyAlive10)
                bulletEnemyGreenBig(10, -90 + yBullet1);
            if (wave == 2 && enemyAlive11)
                bulletEnemyGreenBig(800, -90 + yBullet1);
            if (wave == 2 && enemyAlive12)
                bulletEnemyGreenBig(410, -90 + yBullet1);
            
        }
        // Zdefiniowanie hitboxa pocisku gracza
        private void makeBullet(int n)
        {
            PictureBox bullet = new PictureBox();
            bullet.Image = Properties.Resources.bullet_main;
            bullet.Size = new Size(6, 20);
            bullet.SizeMode = PictureBoxSizeMode.Zoom;
            bullet.Tag = "bullet";
            bullet.Left = Player.Left - 3 + Player.Width * n / 4;
            bullet.Top = Player.Top - 20;
            bullet.BackColor = Color.Transparent;
            this.Controls.Add(bullet);
            bullet.BringToFront();
        }
        // Zdefiniowanie hitboxa pocisku przeciwnika
        private void bulletEnemyGreenBig(int x, int y)
        {
            PictureBox bulletEnGrBig = new PictureBox();
            bulletEnGrBig.Image = Properties.Resources.lazer3;
            bulletEnGrBig.Size = new Size(10, 20);
            bulletEnGrBig.SizeMode = PictureBoxSizeMode.Zoom;
            bulletEnGrBig.Tag = "bulletEnemy";
            bulletEnGrBig.Location = new Point(x + 30, y + 60);
            bulletEnGrBig.BackColor = Color.Transparent;
            this.Controls.Add(bulletEnGrBig);
            bulletEnGrBig.BringToFront();
        }
        
        // Zdefiniowanie hitboxa statku przeciwnika
        public void enemy(int x, int y)
        {
            PictureBox enemy = new PictureBox();
            enemy.Image = Properties.Resources.enemy;
            enemy.Size = new Size(70, 60);
            enemy.SizeMode = PictureBoxSizeMode.Zoom;
            enemy.Tag = "enemy";
            enemy.Location = new Point(x, y);
            this.Controls.Add(enemy);
            enemy.BringToFront();
            enemy.BackColor = Color.Transparent;
            enemy.Name = Convert.ToString(x);
        }
        

        // Ilość puntków życia przecinwików
        public bool enemyAlive = true;
        public bool enemyAlive1 = true;
        public bool enemyAlive2 = true;
        public bool enemyAlive3 = true;
        public bool enemyAlive4 = true;
        public bool enemyAlive5 = true;
        public bool enemyAlive6 = true;
        public bool enemyAlive7 = true;
        public bool enemyAlive8 = true;
        public bool enemyAlive9 = true;
        public bool enemyAlive10 = true;
        public bool enemyAlive11 = true;
        public bool enemyAlive12 = true;

        private void label2_Click(object sender, EventArgs e)
        {
            
        }
        // Funkcja GameOver
        private void gameOver()
        {
            timer1.Stop();
            wave1timer.Stop();
            enemyBulletTimer.Stop();
            label2.Visible = true;
            label2.BringToFront();
            label2.Left = 375;
            label2.Text = "Game Over";
        }
        //Funkcja Win
        private void Win()
        {
            timer1.Stop();
            wave1timer.Stop();
            enemyBulletTimer.Stop();
            
            label2.Visible = true;
            label2.BringToFront();
            label2.Left = 375;
            label2.Text = "You Won";
        }        
    }
}
