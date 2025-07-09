using System.Data.Common;
using Microsoft.EntityFrameworkCore;

public class ProductForm : Form
{
    async Task<bool> IsAdm(int userId)
    {
        var db = await Config.InitiateAsync();
        var query = await db.Users
            .FirstOrDefaultAsync(u => u.ID == userId);
        if (!query.IsAdm)
        {
            return false; 
        }
        return true;
    }

    async Task LoadData()
    {
        Clear();

        // TODO

        Add(1, "bico", 100);
        Add(2, "injetor", 300);
    }

    async Task DeleteById(int id)
    {
        // TODO
    }

    int userId;
    DataGridView table;
    ToolStripButton btAdd;
    ToolStripButton btShowChart;
    public ProductForm(int userId)
    {
        this.userId = userId;

        Text = "Produtos";
        FormBorderStyle = FormBorderStyle.FixedToolWindow;
        Width = 800;
        Height = 800;
        FormClosed += (o, e) => Application.Exit();
        Shown += FormLoad;

        table = new DataGridView {
            Dock = DockStyle.Fill,
            AllowDrop = false,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AllowUserToOrderColumns = false
        };
        table.Columns.Add(new DataGridViewColumn {
            HeaderText = "ID",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        });
        table.Columns.Add(new DataGridViewColumn {
            HeaderText = "Nome",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        });
        table.Columns.Add(new DataGridViewColumn {
            HeaderText = "Preço",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        });
        table.Columns.Add(new DataGridViewColumn {
            HeaderText = "Editar",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        });
        table.Columns.Add(new DataGridViewColumn {
            HeaderText = "Remover",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        });

        btAdd = new ToolStripButton {
            Text = "Adicionar",
        };

        btShowChart = new ToolStripButton {
            Text = "Ver Gráficos"
        };

        var btExit = new ToolStripButton {
            Text = "Sair",
        };
        btExit.Click += (o, e) => {
            Hide();
            var login = new LoginForm();
            login.Show();
        };

        var menuStrip = new MenuStrip {
            Items = { btAdd, btShowChart, btExit }
        };

        Controls.Add(menuStrip);
        Controls.Add(table);

        btAdd.Click += async (o, e) =>
        {
            var insert = new NewProductForm();
            if (insert.ShowDialog() == DialogResult.Cancel)
                return;
            
            await LoadData();
        };

        btShowChart.Click += (o, e) =>
        {
            var chart = new ChartForm();
            chart.ShowDialog();
        };

        table.CellClick += async (o, e) =>
        {
            var row = table.Rows[e.RowIndex];
            if (e.ColumnIndex == 3)
            {
                var edit = new EditProductForm {
                    ProductId = int.Parse(row.Cells[0].Value.ToString()),
                    Name = row.Cells[1].Value.ToString(),
                    Price = decimal.Parse(row.Cells[2].Value.ToString())
                };
                if (edit.ShowDialog() == DialogResult.Cancel)
                    return;
                
                await LoadData();
                return;
            }
            
            if (e.ColumnIndex == 4)
            {
                await DeleteById(int.Parse(row.Cells[0].Value.ToString()));
                await LoadData();
                return;
            }
        };

        table.CellDoubleClick += (o, e) =>
        {
            var row = table.Rows[e.RowIndex];
            var id = int.Parse(row.Cells[0].Value.ToString());
            var sales = new SalesForm(userId, id);
            sales.ShowDialog();
        };
    }

    void Clear()
        => table.Rows.Clear();
    
    void Add(int id, string name, decimal price)
    {
        var row = new DataGridViewRow();
        row.Cells.Add(new DataGridViewTextBoxCell {
            Value = id
        });
        row.Cells.Add(new DataGridViewTextBoxCell {
            Value = name
        });
        row.Cells.Add(new DataGridViewTextBoxCell {
            Value = price
        });
        var btUpdate = new DataGridViewButtonCell {
            Value = "Editar"
        };
        row.Cells.Add(btUpdate);
        var btDelete = new DataGridViewButtonCell {
            Value = "Deletar"
        };
        row.Cells.Add(btDelete);
        row.Cells[0].ReadOnly = true;
        row.Cells[1].ReadOnly = true;
        row.Cells[2].ReadOnly = true;
        table.Rows.Add(row);
    }

    async void FormLoad(object sender, EventArgs e)
    {
        var isAdm = await IsAdm(userId);
        btAdd.Enabled = isAdm;
        btShowChart.Enabled = isAdm;
        await LoadData();
    }
}