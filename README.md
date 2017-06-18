# 3.5 to Roll20
Right now takes in XML file from http://www.andargor.com/ (Huge thanks for publishing this) and spits out a pretty generic SRD-looking spell macro! Nicer than nothing :)

## Usage
To run this software, just use the executable located in 3.5 to Roll20\bin\Debug!

### Future improvements:
--------------------
* Autocomplete
* Rethink the buttons - why not always copy to clipboard?
* Class filtrering
* Spell level filtering
* Choose between different templates
* Construct a damage macro with some easy inputs.


### Future improvements that I probably won't do
---------------------------------------------
* Parse DC of spell resistance for roll20 from character variable
* Parse proper range based on class level
* Any other parsing ideas from roll20 variables!

Because my party doesn't use the built in sheets we don't have variables so tough to get motivated for this :)

#### Output example
![Image](http://i.imgur.com/gNZfliK.png "Image")

```&{template:DnD35StdRoll} {{spellflag=true}} {{subtags=casts Cat's Grace}} {{School:=Transmutation}} {{Level:=Bard 2, Druid 2, Ranger 2, Sorcerer/Wizard 2}} {{Components:=V, S, M}} {{Casting Time:=1 standard action}} {{Range:=Touch}} {{Target:=Creature touched}} {{Duration:=1 min./level}} {{Saving Throw:=Will negates (harmless)}} {{Spell Resist:=Yes}} {{Saving Throw:=Will negates (harmless)}} {{notes= The transmuted creature becomes more graceful, agile, and coordinated. The spell grants a +4 enhancement bonus to Dexterity, adding the usual benefits to AC, Reflex saves, and other uses of the Dexterity modifier.}}```