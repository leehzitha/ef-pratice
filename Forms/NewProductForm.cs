using System.ComponentModel;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;

public class NewProductForm : Form
{
    async Task Insert()
    {
        string name = Name;
        decimal price = Price;

        var db = await Config.InitiateAsync();
        var product = new ProductItem
        {
            Name = name,
            Price = price
        };
        
        db.Add(product);


    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string Name
    {
        get => tbName.Text;
        set => tbName.Text = value;
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public decimal Price
    {
        get => decimal.Parse(tbPrice.Text);
        set => tbPrice.Text = value.ToString();
    }

    private TextBox tbName;
    private TextBox tbPrice;

    public NewProductForm()
    {
        tbName = new TextBox { Width = 200 };
        tbPrice = new TextBox { Width = 200 };
        Width = 400;
        Height = 400;
        Text = "Editar Produto";
        var btSave = new Button { 
            Text = "Salvar",
            Height = 40, Width = 80
        };
        var btExit = new Button { 
            Text = "Cancelar", 
            Height = 40, Width = 80 
        };

        btExit.Click += (o, e) =>
        {
            DialogResult = DialogResult.Cancel;
            Close();
        };

        btSave.Click += async (o, e) =>
        {
            DialogResult = DialogResult.OK;
            await Insert();
            Close();
        };
        
        var layout = new FlowLayoutPanel {
            Dock = DockStyle.Fill,
            Padding = new Padding(20),
            Controls = {
                new FlowLayoutPanel {
                    FlowDirection = FlowDirection.LeftToRight,
                    Width = 400, Height = 40,
                    Controls = {
                        new Label {
                            Text = "Nome",
                        },
                        tbName
                    }
                },
                new FlowLayoutPanel {
                    FlowDirection = FlowDirection.LeftToRight,
                    Width = 400, Height = 40,
                    Controls = {
                        new Label {
                            Text = "Preço",
                        },
                        tbPrice
                    }
                },
                new FlowLayoutPanel {
                    FlowDirection = FlowDirection.RightToLeft,
                    Width = 300, Height = 120,
                    Anchor = AnchorStyles.Left | AnchorStyles.Right,
                    Controls = {
                        btSave, btExit
                    }
                }
            }
        };

        Controls.Add(layout);
    }
}