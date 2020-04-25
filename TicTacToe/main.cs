using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class main : Form
    {
        private bool vsCpu;
        // Флаг, указывающий на то, кому принадлежит следующий ход
        private bool sym;
        private bool fin;
        private Random r;
        private int xResultVal;
        private int oResultVal;
        public main()
        {
            InitializeComponent();
            init();
        }
        // Инициализатор класса
        private void init()
        {
            //Ставим результаты побед в 0
            xResultVal = oResultVal = 0;
            r = new Random();
            fin = false;
            //По умолчанию первым будет ходить 'крестик'
            sym = false;
            //По умолчанию, мы играем против человека
            vsCpu = false;
            initButtonsEvents();
        }
        private void initButtonsEvents()
        {
            foreach (Button but in gameFieldPanel.Controls)
                but.Click += But_Click;
        }
        // Обработчик события клика по кнопке на игровом поле
        private void But_Click(object sender, EventArgs e)
        {
            //Если игра всё ещё идёт
            if (!fin)
            {
                Button but = (Button)sender;
                //Проставляем на кнопке соответствующее значение
                setButtonValue(but);
                //Проверка завершения игры на этом ходе
                checkFinal();
                //Если у нас игра ещё не закончена, и идёт против ПК
                if (!fin && vsCpu)
                    //Делаем ход компьютера
                    cpuWork();
            }
        }
        // Клик по кнопке запуска игры против бота
        private void startVsPlayer_Click(object sender, EventArgs e)
        {
            //Очищаем игровое поле
            clearGameField();
            //Указываем, против кого играем
            vsCpu = false;
            //Указываем, что игра началась
            fin = false;
            //По умолчанию первым будет ходить 'крестик'
            sym = false;
        }
        private void startVsCpu_Click(object sender, EventArgs e)
        {
            int x;
            //Очищаем игровое поле
            clearGameField();
            //Указываем, против кого играем
            vsCpu = true;
            //Указываем, что игра началась
            fin = false;
            //По умолчанию первым будет ходить 'крестик'
            sym = false;
        }
        // Очищаем игровое поле
        private void clearGameField()
        {
            //Проходимся по всем контроллам, на панели с кнопками
            foreach (Button but in gameFieldPanel.Controls)
            {
                //Сбрасываем текст
                but.Text = "";
                //Сбрасываем цвет
                but.ForeColor = Color.Black;
            }
        }
        // Проставляем значение на кнопке
        private void setButtonValue(Button but)
        {
            //Если на этой кнопке уже есть значение
            if (but.Text.Length > 0)
                //Выводим сообщение об этом
                MessageBox.Show("Сюда поставить нельзя!");
            //Если кнопка "пустая"
            else
            {
                //Если "Крестик"
                if (sym)
                {
                    //Ставим крестик на кнопке
                    but.Text = "X";
                    //Красим текст на ней в синий цвет (чисто для красоты)
                    but.ForeColor = Color.Blue;
                }
                //Если "Нолик"
                else
                {
                    //Ставим нолик на кнопке
                    but.Text = "0";
                    //Красим текст на ней в красный цвет (чисто для красоты)
                    but.ForeColor = Color.Red;
                }

                //Меняем значение на противоположное
                sym = !sym;
            }
        }
        // Проверка завершения игры на этом ходе
        private void checkFinal()
        {
            /*
            Так вот, мы собираем все координаты выбранных ноликов и 
            крестиков, и просто сравниваем их со списком уже известных решений.
            */

            //Инициализируем массив выигрышных комбинаций
            string[][] winResults = new string[][]
            {
                new string[] {"00", "01", "02"},
                new string[] {"10", "11", "12"},
                new string[] {"20", "21", "22"},
                new string[] {"00", "10", "20"},
                new string[] {"01", "11", "21"},
                new string[] {"02", "12", "22"},
                new string[] {"00", "11", "22"},
                new string[] {"02", "11", "20"}
            };

            //Результаты нажатий
            List<string> xValues = new List<string>();
            List<string> OValues = new List<string>();

            //Проходимся по всем контроллам, на панели с кнопками
            foreach (Button but in gameFieldPanel.Controls)
            {
                //Если на кнопке крестик
                if (but.Text == "X")
                    //Добавляем координаты из названия в список крестика
                    xValues.Add(but.Name.Replace("field_", ""));
                //Если на кнопке нолик
                else if (but.Text == "0")
                    //Добавляем координаты из названия в список нолика
                    OValues.Add(but.Name.Replace("field_", ""));
            }

            //Если победили крестики
            if(checkWin(winResults, xValues))
            {
                //Игра завершена
                fin = true;
                //Выводим сообщение о победе
                MessageBox.Show("Победили крестики!");
                //Увеличиваем количество побед крестиков
                xResultVal++;
                //Обновляем значения результатов побед
                updateVinResults();
            }
            //Если победили нолики
            else if(checkWin(winResults, OValues))
            {
                //Игра завершена
                fin = true;
                //Выводим сообщение о победе
                MessageBox.Show("Победили нолики!");
                //Увеличиваем количество побед ноликов
                oResultVal++;
                //Обновляем значения результатов побед
                updateVinResults();
            }
            //Если наставлено уже 9 знаков
            else if(OValues.Count + xValues.Count == 9)
            {
                //Игра завершена
                fin = true;
                //Выводим сообщение о победе
                MessageBox.Show("Ничья!");
            }
            //В остальных случаях - игра идёт дальеше
        }
        // Обновляем значения результатов побед
        private void updateVinResults()
        {
            //Обновляем значения результатов побед
            xResult.Text = xResultVal.ToString();
            oResult.Text = oResultVal.ToString();            
        }
        // Проверяем существующие комбинации с выигрышными
        private bool checkWin(string[][] winResults, List<string> values)
        {
            bool ex = true;
            
            //Проходимся по списку выигрышных комбинаций
            for(int i = 0; i < winResults.Length; i++)
            {
                //Ставим флаг в true
                ex = true;
                //Проходимся по всем цифрам комбинации
                for (int j = 0; j < winResults[i].Length; j++)
                {
                    //Проверяем нахождение комбинации, 
                    //при помощи логического сложения
                    ex = ex && values.Contains(winResults[i][j]);
                }
                //Если хоть одной цифры из комбинации не найдётся среди 
                //отмеченных, это будет считаться проигрышем

                //Если все цифры комбинации мы нашли в списке
                if (ex)
                    //Выходим из массива
                    break;
            }

            return ex;
        }
        // Ход "Компьютера"
        private void cpuWork()
        {
            Button but;

            do
            {
                //Получаем случайную кнопку, из списка тех, 
                //что расположены на игровой панели
                but = (Button)gameFieldPanel.Controls[r.Next(0, gameFieldPanel.Controls.Count)];

                //Цикл идёт до тех пор, пока игра не закончится 
                //(чисто на всякий случай проверка)
                //Либо, не будет найдена случайная,
                //свободная от значка кнопка
            } while (!fin && (but.Text.Length > 0));

            //Проставляем на кнопке соответствующее значение
            setButtonValue(but);
            //Проверка завершения игры на этом ходе
            checkFinal();
        }
    }
}
