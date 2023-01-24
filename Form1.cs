using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Cursach_OOP
{
    public partial class Form1 : Form
    {
        public const int size_cart = 10;//количество цифр
        public int size_iach = 30;//размер кнопок
        public string alphabet = "ABCDEFGHIJ";//количество букв

        public int[,] Cart_1 = new int[size_cart, size_cart];
        public int[,] Cart_2 = new int[size_cart, size_cart];

        public Button[,] myButtons = new Button[size_cart, size_cart];
        public Button[,] enemyButtons = new Button[size_cart, size_cart];

        public bool isPlaying = false;

        public Bot bot;

        public Form1()
        {
            InitializeComponent();
            this.Text = "Наша игра с редактором!";
            Init();
        }

        public void Init()
        {
            isPlaying = false;
            CreateMaps();
            bot = new Bot(Cart_2, Cart_1, enemyButtons, myButtons);
            Cart_2 = bot.ConfigureShips();
        }

        public void CreateMaps()//создание карт
        {
            this.Width = size_cart * 2 * size_iach + 50;//размер окна(ширина)
            this.Height = (size_cart + 3) * size_iach + 20;//размер окна(высота)
            //цикл прохода и инициализации второго поля 
            for (int i = 0; i < size_cart; i++)
            {
                for (int j = 0; j < size_cart; j++)
                {
                    Cart_1[i, j] = 0;

                    Button button = new Button();
                    button.Location = new Point(j * size_iach, i * size_iach);
                    button.Size = new Size(size_iach, size_iach);
                    button.BackColor = Color.White;
                    if (j == 0 || i == 0)
                    {
                        button.BackColor = Color.Green;
                        if (i == 0 && j > 0)
                            button.Text = alphabet[j - 1].ToString();
                        if (j == 0 && i > 0)
                            button.Text = i.ToString();
                    }
                    else
                    {
                        button.Click += new EventHandler(ConfigureShips);
                    }
                    myButtons[i, j] = button;
                    this.Controls.Add(button);
                }
            }
            //появление лейбы с надписью названий полей(наше)
            Label map1 = new Label();//
            map1.Text = "Карта-редактор";
            map1.Location = new Point(size_cart * size_iach / 2, size_cart * size_iach + 10);
            this.Controls.Add(map1);
            //цикл прохода и инициализации второго поля 
            for (int i = 0; i < size_cart; i++)
            {
                for (int j = 0; j < size_cart; j++)
                {
                    
                    Cart_2[i, j] = 0;

                    Button button = new Button();
                    button.Location = new Point(310 + j * size_iach, i * size_iach);
                    button.Size = new Size(size_iach, size_iach);
                    button.BackColor = Color.White;
                    if (j == 0 || i == 0)//заполнение и окрашивание кнопок с указателями букв и цифр
                    {
                        button.BackColor = Color.Green;
                        if (i == 0 && j > 0)
                            button.Text = alphabet[j - 1].ToString();
                        if (j == 0 && i > 0)
                            button.Text = i.ToString();
                    }
                    else
                    {
                        button.Click += new EventHandler(PlayerShoot);
                    }
                    enemyButtons[i, j] = button;
                    this.Controls.Add(button);
                }
            }

            //появление лейбы с надписью названий полей(наше)
            Label map2 = new Label();
            map2.Text = "Карта бота";
            map2.Location = new Point(350 + size_cart * size_iach / 2, size_cart * size_iach + 10);
            this.Controls.Add(map2);

            Button startButton = new Button();
            startButton.Text = "ЗАПУСК";
            startButton.Click += new EventHandler(Start);
            startButton.Location = new Point(270, size_cart * size_iach + 30);
            this.Controls.Add(startButton);
        }

        public void Start(object sender, EventArgs e)
        {
            isPlaying = true;

        }

        public bool CheckIfMapIsNotEmpty()
        {
            bool isEmpty1 = true;
            bool isEmpty2 = true;
            for (int i = 1; i < size_cart; i++)
            {
                for (int j = 1; j < size_cart; j++)
                {
                    if (Cart_1[i, j] != 0)
                        isEmpty1 = false;
                    if (Cart_2[i, j] != 0)
                        isEmpty2 = false;
                }
            }
            if (isEmpty1 || isEmpty2)
                return false;
            else return true;
        }

        public void ConfigureShips(object sender, EventArgs e)
        {
            Button pressedButton = sender as Button;
            if (!isPlaying)
            {
                if (Cart_1[pressedButton.Location.Y / size_iach, pressedButton.Location.X / size_iach] == 0)
                {
                    pressedButton.BackColor = Color.Yellow;//цвет наших кораблей
                    Cart_1[pressedButton.Location.Y / size_iach, pressedButton.Location.X / size_iach] = 1;
                }
                else
                {
                    pressedButton.BackColor = Color.White;
                    Cart_1[pressedButton.Location.Y / size_iach, pressedButton.Location.X / size_iach] = 0;
                }
            }
        }

        public void PlayerShoot(object sender, EventArgs e)//выстрелы
        {

            Button pressedButton = sender as Button;
            bool playerTurn = Shoot(Cart_2, pressedButton);
            if (!playerTurn)
                bot.Shoot();

            if (!CheckIfMapIsNotEmpty())
            {
                this.Controls.Clear();
                Init();
            }
        }

        public bool Shoot(int[,] map, Button pressedButton)//выстрелы 
        {
            bool hit = false;
            if (isPlaying)
            {
                int delta = 0;
                if (pressedButton.Location.X > 320)
                    delta = 320;
                if (map[pressedButton.Location.Y / size_iach, (pressedButton.Location.X - delta) / size_iach] != 0)
                {
                    hit = true;
                    map[pressedButton.Location.Y / size_iach, (pressedButton.Location.X - delta) / size_iach] = 0;
                    pressedButton.BackColor = Color.Blue;
                    pressedButton.Text = "*=*";
                }
                else
                {
                    hit = false;

                    pressedButton.BackColor = Color.Red;
                }
            }
            return hit;
        }
    }
}
