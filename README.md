# BasicGraphicsEngine-Project

> Řešení praktických úloh v `C#` s 2D grafickým výstupem.

## Základní architektura

Tento repozitář obsahje `Visual Studio` řešení `BasicGraphicsEngine-Project`, které dále obsahuje dva projekty:

- `BasicGraphicsEngine` - Statická knihovna umožňující vykreslování jednouché 2D grafiky pomocí specifikace `OpenGL`. Má tři hlavní závislosti (které se **automaticky nainstalují** po spuštění první kompilace pomocí package manageru `NuGet`):
  - `Silk.NET.OpenGL`
  - `Silk.NET.Windowing`
  - `Silk.NET.Input`
- `ProjectApp` - Projekt, který se odkazuje na statickou knihovnu `BasicGraphicsEngine` a zároveň obsahuje implementaci její základní třídy `Application`. Základní podoba tohoto projektu obsahuje dva zdrojové soubory:
  - `BGEapp.cs` - Obsahuje třídu `BGEapp`, která implementuje abstraktní třídu `Application` knihovny `BasicGraphicsEngine`. V jednotlivých metodách této třídy je tedy možné implementovat vlastní logiku, jejíž výstupem by měla být 2D grafika.
  - `Program.cs` - Obsahuje `Main` metodu, ve které se definuje objekt třídy `BGEapp` a následně se spouští grafické vykreslování -- otevírá se okno s grafickým výstupem aplikace.

> Do tohoto souboru bude postupně přidávána dokumentace popisující jednotlivé metody třídy `BGEapp`, pomocí kterých je celá výsledná aplikace "ovládána". Prozatím se alespoň základní verze této dokumentace nachází na relevantních místech v komentářích v obou zdrojových souborech projektu `ProjectApp`. 
> 
> **Pro úspěšné řešení všech připravených úloh není potřeba nijak zasahovat do zdrojového kódu knihovny `BasicGraphicsEngine`**.

## Základní použití

Pro úspěšné řešení všech připravených úloh není potřeba nijak zasahovat do zdrojového kódu knihovny `BasicGraphicsEngine`. Veškerá logika naší aplikace bude obsažena v implementacích již předpřipravených metod třídy `BGEapp` ve zdrojovém souboru `BGEapp.cs`.

### `BGEapp()`

### `Setup()`

### `Loop()`

### Nastavení uživatelského vstupu

#### Vstup klávesnice



#### Vstup myši
