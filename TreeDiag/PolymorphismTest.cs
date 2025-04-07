using System;
using System.Collections.Generic;
using Xunit;

namespace TreeDiag;

public class ViewModel
{
    public string Name { get; set; }
    public List<ViewModel> Children = [];
}

public class TabViewModel : ViewModel { }

public class TextboxViewModel : ViewModel
{
    public string Text { get; set; }
}

public class TableViewModel : ViewModel
{
    public int RowCount { get; set; }
}

public class ImageViewModel : ViewModel
{
    public int Width { get; set; }
    public int Height { get; set; }
}

public class ViewModelTreeDiagnosticWriter : TreeDiagnosticWriter<ViewModel>
{
    protected override string Format(ViewModel vm) => $"{vm.Name} [{GetShortTypeName(vm)}]{SubclassProperties(vm)}";
    protected override IEnumerable<ViewModel> GetChildren(ViewModel vm) => vm.Children;

    private string SubclassProperties(ViewModel vm)
    {
        return vm is ImageViewModel i ? $" {i.Width}Wx{i.Height}H"
             : vm is TableViewModel t ? $" Rows:{t.RowCount}"
             : "";
    }
}

public class PolymorphismTest
{
    [Fact]
    public void Test()
    {
        var tree = new ViewModel { Name = "Root", Children =
            {
                new TabViewModel { Name = "Input", Children =
                    {
                        new TextboxViewModel { Name = "Control Settings" },
                        new TableViewModel { Name = "Geometry", RowCount = 5 },
                        new TableViewModel { Name = "Material", RowCount = 1 },
                        new TableViewModel { Name = "Additional Loads", RowCount = 10 },
                    }
                },
                new TabViewModel { Name = "Output", Children =
                    {
                        new TextboxViewModel { Name = "Summary" },
                        new ImageViewModel { Name = "3D Model", Width = 800, Height = 600 },
                        new ImageViewModel { Name = "Temperature Graph", Width = 500, Height = 500 },
                        new ImageViewModel { Name = "Pressure Graph", Width = 500, Height = 500 }
                    }
                },
                new TabViewModel { Name = "Comments", Children =
                    {
                        new TextboxViewModel { Name = "Additional Comments" }
                    }
                }
            }
        };

        var expected = """
            {0} Root [ViewModel]
              {0} Input [TabViewModel]
                {0} Control Settings [TextboxViewModel]
                {1} Geometry [TableViewModel] Rows:5
                {2} Material [TableViewModel] Rows:1
                {3} Additional Loads [TableViewModel] Rows:10
              {1} Output [TabViewModel]
                {0} Summary [TextboxViewModel]
                {1} 3D Model [ImageViewModel] 800Wx600H
                {2} Temperature Graph [ImageViewModel] 500Wx500H
                {3} Pressure Graph [ImageViewModel] 500Wx500H
              {2} Comments [TabViewModel]
                {0} Additional Comments [TextboxViewModel]

            """;
        var actual = new ViewModelTreeDiagnosticWriter().Write(tree);
        Assert.Equal(expected, actual);
    }
}
