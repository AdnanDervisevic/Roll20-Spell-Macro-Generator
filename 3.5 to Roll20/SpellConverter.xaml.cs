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
        private const string Prefix = "&{template:DnD35StdRoll} {{spellflag=true}}";

        public SpellConverter()
        {
            Spells = ParseXml();

            InitializeComponent();

            SpellSource.ItemsSource = Spells.OrderBy(e => e.Name);
            SpellSource.DisplayMemberPath = "Name";
            SpellSource.IsEditable = false;

            ClipboardButton.Background = Brushes.Salmon;
            ConvertButton.Background = Brushes.White;
        }

        public List<Spell> Spells { get; }

        public string SelectedSpellText { get; set; }

        public Spell SelectedSpell { get; set; }

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
                                    CastingTime = spell.XPathSelectElement("casting_time")?.Value,
                                };
        }

        private void ConvertButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (SpellSource.SelectedIndex != -1)
                SelectedSpellText = CraftRoll20Macro();

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

        private string CraftRoll20Macro()
        {
            return Prefix
                + MacroSection($"subtags=casts {SelectedSpell.Name}")
                + MacroSection($"School:={SelectedSpell.School}") + AddIfNotNull($"Descriptor:={SelectedSpell.Descriptor}")
                + MacroSection($"Level:={SelectedSpell.Level}")
                + MacroSection($"Components:={SelectedSpell.Components}")
                + MacroSection($"Casting Time:={SelectedSpell.CastingTime}")
                + MacroSection($"Range:={SelectedSpell.Range}")
                + MacroSection($"Target:={SelectedSpell.Target}")
                + MacroSection($"Duration:={SelectedSpell.Duration}")
                + MacroSection($"Saving Throw:={SelectedSpell.SavingThrow}")
                + MacroSection($"Spell Resist:={SelectedSpell.SpellResistance}")
                + MacroSection($"Saving Throw:={SelectedSpell.SavingThrow}")
                + MacroSection($"Effect:={SelectedSpell.Effect}")
                + MacroSection($"XP Cost:={SelectedSpell.XpCost}")
                + MacroSection($"notes= {SelectedSpell.Description}");
        }

        private static string AddIfNotNull(string input)
        {
            return IsStringNull(input) ? "" : $" {input}";
        }

        private static bool IsStringNull(string input)
        {
            return input.Substring(input.IndexOf("=", StringComparison.Ordinal)).Length > 0;
        }

        private void SpellSource_OnSelected(object sender, RoutedEventArgs e)
        {
            SelectedSpell = Spells[SpellSource.SelectedIndex];
            ClipboardButton.Background = Brushes.White;
        }

        private static string MacroSection(string input)
        {
            return IsStringNull(input) ? "" : $" {{{{{input}}}}}";
        }
    }
}
