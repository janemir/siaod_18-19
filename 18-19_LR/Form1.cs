using System;
using System.Windows.Forms;

namespace _18_19_LR
{
    public partial class Form1 : Form
    {

        private int[] priorityQueue = new int[16];
        private int queueSize = 0;
        private Random random = new Random();

        public Form1()
        {
            InitializeComponent();
            InitializeDataGridViews();
        }

        private void InitializeDataGridViews()
        {
            dataGridView1.RowCount = 1;
            dataGridView1.ColumnCount = 15;
            dataGridView1.ColumnHeadersVisible = false;
            dataGridView1.RowHeadersVisible = false;
            for (int i = 0; i < 15; i++) dataGridView1.Columns[i].Width = 40;

            dataGridView2.RowCount = 4;
            dataGridView2.ColumnCount = 15;
            dataGridView2.ColumnHeadersVisible = false;
            dataGridView2.RowHeadersVisible = false;
            for (int i = 0; i < 15; i++) dataGridView2.Columns[i].Width = 40;

            dataGridView3.RowCount = 1;
            dataGridView3.ColumnCount = 1;
            dataGridView3.ColumnHeadersVisible = false;
            dataGridView3.RowHeadersVisible = false;
            dataGridView3.Columns[0].Width = 80;
        }

        // создать очередь
        private void button1_Click(object sender, EventArgs e)
        {
            CreateQueue(); 
            Print(priorityQueue); 
            textBox1.Text = "Очередь создана и отображена";
        }

        // очистить очередь
        private void button2_Click(object sender, EventArgs e)
        {
            Clear_Tab(); 
            textBox1.Text = "Все массивы очищены";
        }

        // извлечь максимальный
        private void button3_Click(object sender, EventArgs e)
        {
            if (queueSize == 0)
            {
                textBox1.Text = "Очередь пуста";
                return;
            }

            // извлечение максимального элемента (корня пирамиды)
            int max = priorityQueue[1];
            dataGridView3.Rows[0].Cells[0].Value = max.ToString();

            // перемещение последнего элемента в корень
            priorityQueue[1] = priorityQueue[queueSize];
            priorityQueue[queueSize] = 0;
            queueSize--;

            // восстановление свойств пирамиды
            if (queueSize > 0) fixDown(1);

            UpdateArrayGridView(); 
            Print(priorityQueue); 
            textBox1.Text = $"Извлечен максимальный элемент: {max}";
        }

        // добавить элемент
        private void button4_Click(object sender, EventArgs e)
        {
            if (queueSize >= 15)
            {
                textBox1.Text = "Очередь переполнена";
                return;
            }

            // добавление нового элемента в конец очереди
            int newElement = (int)numericUpDown1.Value;
            queueSize++;
            priorityQueue[queueSize] = newElement;
            fixUp(queueSize); 

            UpdateArrayGridView();
            Print(priorityQueue); 
            textBox1.Text = $"Добавлен элемент: {newElement}";
        }

        // изменить приоритет
        private void button5_Click(object sender, EventArgs e)
        {
            if (queueSize == 0)
            {
                textBox1.Text = "Очередь пуста";
                return;
            }

            // получение индекса и нового значения приоритета
            int index = (int)numericUpDown2.Value;
            int newValue = (int)numericUpDown3.Value;

            if (index < 1 || index > queueSize)
            {
                textBox1.Text = "Некорректный индекс";
                return;
            }

            // изменение приоритета элемента
            int oldValue = priorityQueue[index];
            priorityQueue[index] = newValue;

            // восстановление свойств пирамиды
            if (newValue > oldValue) fixUp(index);
            else if (newValue < oldValue) fixDown(index);

            UpdateArrayGridView(); 
            Print(priorityQueue); 
            textBox1.Text = $"Элемент {index} изменен: {oldValue} -> {newValue}";
        }

        // создание очереди из 15 случайных элементов
        private void CreateQueue()
        {
            queueSize = 15;
            for (int i = 1; i <= 15; i++)
                priorityQueue[i] = random.Next(10, 100);

            BuildHeap(); 
            UpdateArrayGridView(); 
        }

        // построение пирамиды из массива
        private void BuildHeap()
        {
            // проход по всем узлам, начиная с середины, и восстановление свойств пирамиды
            for (int i = queueSize / 2; i >= 1; i--)
                fixDown(i);
        }

        // восстановление свойств пирамиды вверх (при увеличении приоритета)
        private void fixUp(int index)
        {
            // пока текущий элемент больше родителя, меняем их местами
            while (index > 0 && priorityQueue[index] > priorityQueue[index / 2])
            {
                Swap(index, index / 2);
                index /= 2;
            }
        }

        // восстановление свойств пирамиды вниз (при уменьшении приоритета)
        private void fixDown(int index)
        {
            while (2 * index <= queueSize) // пока есть потомки
            {
                int j = 2 * index; // левый потомок
                // потомок существует и больше левого? выбираем его
                if (j < queueSize && priorityQueue[j] < priorityQueue[j + 1]) j++;
                // текущ эл-т больше или равен наибольшему потомку? 
                if (priorityQueue[index] >= priorityQueue[j]) break;
                Swap(index, j); // иначе меняем местами с наибольшим потомком
                index = j; // переходим к потомку
            }
        }

        // обмен элементов массива
        private void Swap(int i, int j)
        {
            int temp = priorityQueue[i];
            priorityQueue[i] = priorityQueue[j];
            priorityQueue[j] = temp;
        }

        // вывод массива в таблицы
        private void Print(int[] A)
        {
            Clear_Tab(); 

            for (int i = 1; i <= queueSize; i++)
            {
                dataGridView1.Rows[0].Cells[i - 1].Value = A[i].ToString();
            }

            int[,] positions =
            {
                {0, 7},         
                {1, 3}, {1, 11}, 
                {2, 1}, {2, 5}, {2, 9}, {2, 13}, 
                {3, 0}, {3, 2}, {3, 4}, {3, 6}, {3, 8}, {3, 10}, {3, 12}, {3, 14} 
            };

            for (int i = 1; i <= queueSize; i++)
            {
                if (i - 1 >= positions.GetLength(0)) break; 
                int row = positions[i - 1, 0];
                int col = positions[i - 1, 1];
                dataGridView2.Rows[row].Cells[col].Value = A[i].ToString();
            }
        }

        private void Clear_Tab()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
                foreach (DataGridViewCell cell in row.Cells)
                    cell.Value = "";

            foreach (DataGridViewRow row in dataGridView2.Rows)
                foreach (DataGridViewCell cell in row.Cells)
                    cell.Value = "";

            dataGridView3.Rows[0].Cells[0].Value = "";
        }

        private void UpdateArrayGridView()
        {
            for (int i = 0; i < 15; i++)
                dataGridView1.Rows[0].Cells[i].Value = (i + 1 <= queueSize) ? priorityQueue[i + 1].ToString() : "";
        }

        private void button6_Click_1(object sender, EventArgs e) => this.Close();
        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
    }
}