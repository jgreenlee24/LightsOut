using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LightsOut
{
    public partial class MainForm : Form
    {
        private const int GRID_OFFSET = 25;
        private const int GRID_LENGTH = 200;
        private int NUM_CELLS = 3;
        private int CELL_LENGTH;

        private bool[,] grid;
        private bool[,] x3grid;
        private bool[,] x4grid;
        private bool[,] x5grid;
        private Random rand;


        public MainForm()
        {
            InitializeComponent();
            rand = new Random();    // Initializes random number generator  
            grid = new bool[NUM_CELLS, NUM_CELLS];
            x3grid = new bool[3, 3];
            x4grid = new bool[4, 4];
            x5grid = new bool[5, 5];

            x3ToolStripMenuItem.Checked = true;
            CELL_LENGTH = GRID_LENGTH / NUM_CELLS;

            // Turn all grids on
            for (int i = 3; i < 6; i++)
            {
                NUM_CELLS = i;
                for (int r = 0; r < i; r++)
                {
                    for (int c = 0; c < i; c++)
                    {
                        GetGrid()[r, c] = true;
                    }
                }
            }
            NUM_CELLS = 3;

            newGame();
        }

        public bool[,] GetGrid()
        {
            switch (NUM_CELLS)
            {
                case 3: return x3grid;
                case 4: return x4grid;
                case 5: return x5grid;
                default: return grid;
            }
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush brush;
            Pen pen;

            for (int r = 0; r < NUM_CELLS; r++)
            {
                for (int c = 0; c < NUM_CELLS; c++)
                {
                    if (GetGrid()[r, c])
                    {
                        pen = Pens.Black;
                        brush = Brushes.White;
                    } else
                    {
                        pen = Pens.White;
                        brush = Brushes.Black;
                    }

                    int x = c * CELL_LENGTH + GRID_OFFSET;
                    int y = r * CELL_LENGTH + GRID_OFFSET;

                    g.DrawRectangle(pen, x, y, CELL_LENGTH, CELL_LENGTH);
                    g.FillRectangle(brush, x + 1, y + 1, CELL_LENGTH - 1, CELL_LENGTH - 1);
                }
            }
        }

        private void MainForm_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.X < GRID_OFFSET || e.X > CELL_LENGTH * NUM_CELLS + GRID_OFFSET ||
                e.Y < GRID_OFFSET || e.Y > CELL_LENGTH * NUM_CELLS + GRID_OFFSET)
                return;

            int r = (e.Y - GRID_OFFSET) / CELL_LENGTH;
            int c = (e.X - GRID_OFFSET) / CELL_LENGTH;

            for (int i = r-1; i <= r+1; i++)
            {
                for (int j = c-1; j<=c+1; j++)
                {
                    if (i>=0 && i <NUM_CELLS && j >=0 && j < NUM_CELLS)
                    {
                        GetGrid()[i, j] = !GetGrid()[i, j];
                    }
                }
            }

            // Redraw grid
            this.Invalidate();

            if (PlayerWon())
            {
                MessageBox.Show(this, "Congratulations! You've won!", "Lights Out",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool PlayerWon() // custom code
        {
            // Loop through grid
            for (int i = 0; i < NUM_CELLS; i++)
            {
                for (int y = 0; y < NUM_CELLS; y++)
                {
                    if (!GetGrid()[i, y])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void newGameButton_Click(object sender, EventArgs e)
        {
            newGame();
        }

        private void newGame()
        {
            for (int r = 0; r < NUM_CELLS; r++)
            {
                for (int c = 0; c < NUM_CELLS; c++)
                {
                    GetGrid()[r, c] = rand.Next(2) == 1;
                }
            }

            this.Invalidate();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newGameButton_Click(sender, e);
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm aboutBox = new AboutForm();
            aboutBox.ShowDialog(this);
        }

        private void exitButton_Click(Object sender, EventArgs e)
        {
            Close();
        }

        private void x3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            x3ToolStripMenuItem.Checked = true;
            x4ToolStripMenuItem.Checked = false;
            x5ToolStripMenuItem.Checked = false;
            if (NUM_CELLS != 3)
            {
                NUM_CELLS = 3;
                CELL_LENGTH = GRID_LENGTH / NUM_CELLS;
                newGame();
            }
        }

        private void x4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            x3ToolStripMenuItem.Checked = false;
            x4ToolStripMenuItem.Checked = true;
            x5ToolStripMenuItem.Checked = false;
            if (NUM_CELLS != 4)
            {
                NUM_CELLS = 4;
                CELL_LENGTH = GRID_LENGTH / NUM_CELLS;
                newGame();
            }
        }

        private void x5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            x3ToolStripMenuItem.Checked = false;
            x4ToolStripMenuItem.Checked = false;
            x5ToolStripMenuItem.Checked = true;
            if (NUM_CELLS != 5)
            {
                NUM_CELLS = 5;
                CELL_LENGTH = GRID_LENGTH / NUM_CELLS;
                newGame();
            }
        }
    }
}
