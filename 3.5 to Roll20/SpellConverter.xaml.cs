using System.Windows;
using System.Collections.Generic;
using System.Xml.Linq;

namespace _3._5_to_Roll20
{
    using System.Linq;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SpellConverter
    {
        private static readonly XDocument SpellXml = XDocument.Load("../../all-spells.xml");

        private readonly IEnumerable<XElement> spellElements = SpellXml.Elements().Elements("spell");
        private const string Prefix = "&{template:DnD35StdRoll} {{spellflag=true}}";

        public SpellConverter()
        {
            Parser = new Roll20Parser();
            Spells = Parser.ParseXml(spellElements);
            AvailableLevels = Parser.GetAvailableLevels(Spells);
            AvailableSpellClasses= Parser.GetAvailableSpellClasses(Spells);

            InitializeComponent();

            SpellSource.ItemsSource = Spells.OrderBy(e => e.Name);
            SpellSource.DisplayMemberPath = "DisplayString";
            SpellSource.IsEditable = false;

            ClipboardButton.Background = Brushes.Salmon;
            ConvertButton.Background = Brushes.White;
        }


        public List<SpellClass> AvailableSpellClasses { get; set; }

        public List<int> AvailableLevels { get; set; }

        public List<Spell> Spells { get; }

        public string SelectedSpellText { get; set; }

        public Spell SelectedSpell { get; set; }

        private Roll20Parser Parser { get; }

        private void ConvertButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (SpellSource.SelectedIndex != -1)
                SelectedSpellText = CraftRoll20Macro();

            textBox.Text = SelectedSpellText;
            ClipboardButton.Background = Brushes.White;
        }

        private string CraftRoll20Macro()
        {
            return Prefix
                   + Parser.MacroSection($"subtags=casts {SelectedSpell.Name}")
                   + Parser.MacroSection($"School:={SelectedSpell.School}") + Parser.AddIfNotNull($"Descriptor:={SelectedSpell.Descriptor}")
                   + Parser.MacroSection($"Level:={SelectedSpell.Level}")
                   + Parser.MacroSection($"Components:={SelectedSpell.Components}")
                   + Parser.MacroSection($"Casting Time:={SelectedSpell.CastingTime}")
                   + Parser.MacroSection($"Range:={SelectedSpell.Range}")
                   + Parser.MacroSection($"Target:={SelectedSpell.Target}")
                   + Parser.MacroSection($"Duration:={SelectedSpell.Duration}")
                   + Parser.MacroSection($"Saving Throw:={SelectedSpell.SavingThrow}")
                   + Parser.MacroSection($"Spell Resist:={SelectedSpell.SpellResistance}")
                   + Parser.MacroSection($"Saving Throw:={SelectedSpell.SavingThrow}")
                   + Parser.MacroSection($"Effect:={SelectedSpell.Effect}")
                   + Parser.MacroSection($"XP Cost:={SelectedSpell.XpCost}")
                   + Parser.MacroSection($"notes= {SelectedSpell.Description}");
        }

        private void Clipboard_OnClick(object sender, RoutedEventArgs e)
        {
            if (SelectedSpellText != null)
                Clipboard.SetText(SelectedSpellText);
            else
                return;

            ClipboardButton.Background = new SolidColorBrush(Colors.LightGreen);
        }

        private void SpellSource_OnSelected(object sender, RoutedEventArgs e)
        {
            SelectedSpell = (Spell) SpellSource.SelectedItem;
            ClipboardButton.Background = Brushes.White;
        }
    }
}
