using System;
using System.Windows;
using System.Collections.Generic;
using System.Xml.Linq;

namespace _3._5_to_Roll20
{
    using System.Linq;
    using System.Windows.Media;
    using System.Xml;
    using System.Xml.XPath;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SpellConverter
    {
        private static readonly XDocument SpellXml = XDocument.Load("../../all-spells.xml");

        private readonly IEnumerable<XElement> spellElements = SpellXml.Elements().Elements("spell");

        public SpellConverter()
        {
            Spells = ParseXml();

            InitializeComponent();

            SpellSource.ItemsSource = Spells;
            SpellSource.DisplayMemberPath = "Name";
            SpellSource.IsEditable = false;

            ClipboardButton.Background = Brushes.Salmon;
            ConvertButton.Background = Brushes.White;
        }

        public List<Spell> Spells { get; }

        public string SelectedSpellText { get; set; }
        
        private List<Spell> ParseXml()
        {
            var spellList = spellElements.Select(ParseSpells()).ToList();

            return spellList;
        }

        private static Func<XElement, Spell> ParseSpells()
        {
            return spell => new Spell
                                {
                                    Name = spell.XPathSelectElement("name")?.Value,
                                    Components = spell.XPathSelectElement("components")?.Value,
                                    Description = spell.XPathSelectElement("description")?.Value,
                                    Descriptor = spell.XPathSelectElement("descriptor")?.Value,
                                    Duration = spell.XPathSelectElement("duration")?.Value,
                                    Fulltext = spell.XPathSelectElement("full_text")?.Value,
                                    Level = spell.XPathSelectElement("level")?.Value,
                                    Range = spell.XPathSelectElement("range")?.Value,
                                    SavingThrow = spell.XPathSelectElement("saving_throw")?.Value,
                                    SpellResistance = spell.XPathSelectElement("spell_resistance")?.Value,
                                    Target = spell.XPathSelectElement("target")?.Value,
                                    XpCost = spell.XPathSelectElement("xp_cost")?.Value,
                                    MaterialComponents = spell.XPathSelectElement("material_components")?.Value,
                                    School = spell.XPathSelectElement("school")?.Value,
                                    Effect = spell.XPathSelectElement("effect")?.Value,
                                };
        }

        private void ConvertButton_OnClick(object sender, RoutedEventArgs e)
        {

            if (SpellSource.SelectedIndex != -1)
                SelectedSpellText = $"{Spells[SpellSource.SelectedIndex].Description}";

            textBox.Text = SelectedSpellText;
            ClipboardButton.Background = Brushes.White;
        }


        private void Clipboard_OnClick(object sender, RoutedEventArgs e)
        {
            if (SelectedSpellText != null)
                Clipboard.SetText(SelectedSpellText);
            else
                return;
            
            ClipboardButton.Background = new SolidColorBrush(Colors.LightGreen);
        }
    }
}
