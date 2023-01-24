using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Cursach_OOP
{
    public class Bot
    {
        public int[,] Cart_1 = new int[Form1.size_cart, Form1.size_cart];
        
        public int[,] Cart_2 = new int[Form1.size_cart, Form1.size_cart];

        public Button[,] myButtons = new Button[Form1.size_cart, Form1.size_cart];
        public Button[,] enemyButtons = new Button[Form1.size_cart, Form1.size_cart];

        public Bot(int[,] Cart_1, int[,] Cart_2, Button[,] myButtons, Button[,] enemyButtons)
        {
            this.Cart_1 = Cart_1;
            this.Cart_2 = Cart_2;
            this.enemyButtons = enemyButtons;
            this.myButtons = myButtons;
        }
        
        public bool IsInsideMap(int i, int j)
        {
            if (i < 0 || j < 0 || i >= Form1.size_cart || j >= Form1.size_cart)
            {
                return false;
            }
            return true;
        }

        public bool IsEmpty(int i, int j, int length)
        {
            bool isEmpty = true;

            for (int k = j; k < j + length; k++)
            {
                if (Cart_1[i, k] != 0)
                {
                    isEmpty = false;
                    break;
                }
            }

            return isEmpty;
        }

        public int[,] ConfigureShips()
        {
            int lengthShip = 4;
            int cycleValue = 4;
            int shipsCount = 10;
            Random r = new Random();

            int posX = 0;
            int posY = 0;

            while (shipsCount > 0)
            {
                for (int i = 0; i < cycleValue / 4; i++)
                {
                    posX = r.Next(1, Form1.size_cart);
                    posY = r.Next(1, Form1.size_cart);

                    while (!IsInsideMap(posX, posY + lengthShip - 1) || !IsEmpty(posX, posY, lengthShip))
                    {
                        posX = r.Next(1, Form1.size_cart);
                        posY = r.Next(1, Form1.size_cart);
                    }
                    for (int k = posY; k < posY + lengthShip; k++)
                    {
                        Cart_1[posX, k] = 1;
                    }



                    shipsCount--;
                    if (shipsCount <= 0)
                        break;
                }
                cycleValue += 4;
                lengthShip--;
            }
            return Cart_1;
        }


        public bool Shoot()
        {
            bool hit = false;
            Random r = new Random();

            int posX = r.Next(1, Form1.size_cart);
            int posY = r.Next(1, Form1.size_cart);

            while (enemyButtons[posX, posY].BackColor == Color.Blue || enemyButtons[posX, posY].BackColor == Color.Red)
            {
                posX = r.Next(1, Form1.size_cart);
                posY = r.Next(1, Form1.size_cart);
            }

            if (Cart_2[posX, posY] != 0)
            {
                hit = true;
                Cart_2[posX, posY] = 0;
                enemyButtons[posX, posY].BackColor = Color.Blue;
                enemyButtons[posX, posY].Text = "*_*";
            }
            else
            {
                hit = false;
                enemyButtons[posX, posY].BackColor = Color.Red;
            }
            if (hit)
                Shoot();
            return hit;
        }
    }
}
