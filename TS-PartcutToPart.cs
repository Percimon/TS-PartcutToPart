#pragma warning disable 1633 // Unrecognized #pragma directive
#pragma reference "Tekla.Macros.Akit"
#pragma reference "Tekla.Macros.Wpf.Runtime"
#pragma reference "Tekla.Macros.Runtime"
#pragma warning restore 1633 // Unrecognized #pragma directive
using Tekla.Structures.Model;
using TSMUI = Tekla.Structures.Model.UI;
using System.Windows.Forms;

namespace UserMacros {
    public sealed class Macro {
        [Tekla.Macros.Runtime.MacroEntryPointAttribute()]
        public static void Run(Tekla.Macros.Runtime.IMacroRuntime runtime) 
        {
            Tekla.Macros.Akit.IAkitScriptHost akit = runtime.Get<Tekla.Macros.Akit.IAkitScriptHost>();
            Tekla.Macros.Wpf.Runtime.IWpfMacroHost wpf = runtime.Get<Tekla.Macros.Wpf.Runtime.IWpfMacroHost>();

            ConvertBooleanPartToPart();

        }

        public static void ConvertBooleanPartToPart()
        {
            TSMUI.ModelObjectSelector modelObjectSelector = new TSMUI.ModelObjectSelector();
            ModelObjectEnumerator modelObjectEnumarator = modelObjectSelector.GetSelectedObjects();
            int count = 0;
            while (modelObjectEnumarator.MoveNext())
            {
                if (modelObjectEnumarator.Current is BooleanPart)
                {
                    Part operativePart = ((BooleanPart)modelObjectEnumarator.Current).OperativePart;
                    Part booleanPartBody;
                    if (operativePart is Beam)
                    {
                        Beam b = (Beam)operativePart;
                        booleanPartBody = new Beam(b.StartPoint, b.EndPoint)
                        {
                            Name = "CutOperativePartBody",
                            Profile = new Profile { ProfileString = b.Profile.ProfileString },
                            Material = new Material { MaterialString = "Steel_Undefined" },
                            Position = b.Position,
                            Class = "0"
                        };
                        booleanPartBody.Insert();
                        count++;
                    }
                }
            }
            Model model = new Model();
            model.CommitChanges();
            MessageBox.Show("Cuts converted to parts: " + count.ToString());
        }

    }
}
