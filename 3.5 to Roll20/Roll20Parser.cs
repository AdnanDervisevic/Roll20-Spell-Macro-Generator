namespace _3._5_to_Roll20
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.XPath;

    public class Roll20Parser
    {
        public List<Spell> ParseXml(IEnumerable<XElement> spellElements)
        {
            var spellList = spellElements.Select(ParseSpells()).ToList();

            foreach (var spell in spellList)
            {
                spell.ClassInformation = ParseLevelAndClass(spell.Level);
            }

            return spellList;
        }

        public List<int> GetAvailableLevels(IEnumerable<Spell> spells)
        {
            return spells
                .Select(e => e.ClassInformation.Level)
                .GroupBy(e => e)
                .Select(e => e.First())
                .ToList();
        }

        public List<SpellClass> GetAvailableSpellClasses(IEnumerable<Spell> spells)
        {
            return spells
                    .Select(e => e.ClassInformation.SpellClass)
                    .GroupBy(e => e)
                    .Select(e => e.First())
                    .ToList();
        }

        public static ClassInformation ParseLevelAndClass(string levelInformation)
        {
            if (levelInformation == null)
            {
                return new ClassInformation();
            }

            var levelAndClasses = levelInformation.Split(',');

            return levelAndClasses.Select(levelAndClass => levelAndClass
                                    .Split(' '))
                                    .Select(temp =>
                                    new ClassInformation
                                    {
                                        SpellClass = GetSpellClass(temp),
                                        Level = int.Parse(temp[1])
                                    }).FirstOrDefault();
        }

        public static SpellClass GetSpellClass(IReadOnlyList<string> temp)
        {
            return (SpellClass)Enum.Parse(typeof(SpellClass), temp[0].Replace("/", string.Empty), true);
        }

        public static Func<XElement, Spell> ParseSpells()
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

        public string AddIfNotNull(string input)
        {
            return IsStringNull(input) ? "" : $" {input}";
        }

        public static bool IsStringNull(string input)
        {
            return input.Substring(input.IndexOf("=", StringComparison.Ordinal)).Length <= 1;
        }

        public string MacroSection(string input)
        {
            return IsStringNull(input) ? "" : $" {{{{{input}}}}}";
        }
    }
}