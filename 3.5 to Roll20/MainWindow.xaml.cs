using System;
using System.Windows;
using System.Collections.Generic;
using System.Xml.Linq;

namespace _3._5_to_Roll20
{
    using System.Linq;
    using System.Xml;
    using System.Xml.XPath;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private static readonly XDocument SpellXml = XDocument.Load("../../all-spells.xml");

        private readonly IEnumerable<XElement> spellElements = SpellXml.Elements().Elements("spell");

        public MainWindow()
        {
            Spells = ParseXml();

            InitializeComponent();


            SpellSource.ItemsSource = Spells;
            SpellSource.DisplayMemberPath = "Name";
        }

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
                                };
        }

        public List<Spell> Spells { get; }

        private void ConvertButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException(); // should eventually trigger the parsing to output
        }
    }
}
