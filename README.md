# BasicGraphicsEngine-Project

> Řešení praktických úloh v `C#` s 2D grafickým výstupem.

## Základní architektura

Tento repozitář obsahuje `Visual Studio` řešení `BasicGraphicsEngine-Project`, které dále obsahuje dva projekty:

- `BasicGraphicsEngine` - Statická knihovna umožňující vykreslování jednouché 2D grafiky pomocí specifikace `OpenGL`. Má tři hlavní závislosti (které se **automaticky nainstalují** po spuštění první kompilace pomocí package manageru `NuGet`):
  - `Silk.NET.OpenGL`
  - `Silk.NET.Windowing`
  - `Silk.NET.Input`
- `ProjectApp` - Konzolová aplikace, která se odkazuje na statickou knihovnu `BasicGraphicsEngine` a zároveň obsahuje implementaci její základní třídy `Application`. Základní podoba tohoto projektu obsahuje dva zdrojové soubory:
  - `BGEapp.cs` - Obsahuje třídu `BGEapp`, která implementuje abstraktní třídu `Application` knihovny `BasicGraphicsEngine`. V jednotlivých metodách této třídy je tedy možné implementovat vlastní logiku, jejíž výstupem by měla být 2D grafika.
  - `Program.cs` - Obsahuje `Main` metodu, ve které se definuje objekt třídy `BGEapp` a následně se spouští grafické vykreslování -- otevírá se okno s grafickým výstupem aplikace.

## Základní použití

Pro úspěšné řešení všech připravených úloh není potřeba nijak zasahovat do zdrojového kódu knihovny `BasicGraphicsEngine`. Veškerá logika naší aplikace bude obsažena v implementacích již předpřipravených metod třídy `BGEapp` ve zdrojovém souboru `BGEapp.cs`.

### `BGEapp()`

> Konstruktor naší apliace, který zároveň zajišťuje základní nastavení a spouštění grafického enginu `BasicGraphicsEngine`.

V tomto konstruktoru je možné změnit základní nastavení aplikace přepisem hodnot uložených v proměnných objektu `Settings`. Například změnu barvy pozadí (zde na bílou) je možné uskutečnit příkazem:
```cs
Settings.BackgroundColor = Color.White;
```
**Důležité:** Uvnitř těla konstruktoru **je možné** definovat objekty, ale ještě **není možné** je přidávat do seznamu vykreslovaných objektů pomocí metody: `AddObject()`. Stejně tak zde **není možná** manipulace s kamerou -- k počátečnímu nastavení kamery slouží relevantní proměnné objektu `Settings`.

### `Setup()`

> Metoda, v jejímž těle je možné definovat příkazy, které se mají provést před vykreslením **prvního snímku**.

Zamýšleným využitím této metody tedy je definice a následné přiřazení objektů, které se mají vykreslovat. V `BasicGraphicsEngine` jsou definovány čtyři základní třídy objektů, které je možné vykreslit (tyto třídy dědí přímo z třídy `DrawableObject`):
  - **Částice**: Jednoduchý "bod" určité velikosti a barvy ve tvaru čtverce. Je možné jej definovat takto:
    ```cs
    Particle obj = new Particle(poloha, velikost, barva);
    ```

  - **Kruh**: Kruh o určitém poloměru a barvě. Je možné jej definovat takto:
    ```cs
    Circle obj = new Circle(poloha středu, poloměr, barva);
    ```
    Je ovšem možné nechat vykreslit i ohraničení tohoto kruhu o určité tloušťce a barvě zadáním dodatečných parametrů stejnému konstruktoru:
    ```cs
    Circle obj = new Circle(poloha středu, poloměr, barva kruhu, tloušťka obvodu, barva obvodu);
    ```
    `BasicGraphicsEngine` dokáže kompletně vykreslovat i částečně, nebo zcela průhledné objekty, čehož je možné využít pro vykreslení kružnice -- stačí parametr `barva kruhu` zadat jako RGBA barvu pomocí `Vector4`, jehož čtvrtá souřadnice (alfa kanál) je rovna nule.

  - **Obdélník**: Obdélník o určité šířce, výšce a barvě. Je možné jej definovat takto:
    ```cs
    Quad obj = new Quad(poloha středu, šířka, výška, barva);
    ```

  - **Orientovaná úsečka**: Obdélník o určité tloušťce a barvě, jehož středy podstav jsou určeny počátečním a koncovým bodem (v obou případech `Vector3`). Je možné ji definovat takto:
    ```cs
    Line obj = new Line(počáteční bod, koncový bod, tloušťka, barva);
    ```  

### `Loop()`

> Metoda, v jejímž těle je možné definovat příkazy, které se mají provést před vykreslením **každého snímku**.

Zamýšleným využitím této metody je implementace animace/simulace. V těle této metody máme přístup k proměnné `dt`, ve které je uložen čas (v ms), jak dlouho trvalo vykreslení předchozího snímku (jejíž využití umožňuje vytvářet takzvané "framerate independent" animace/simulace).

Pokud například chceme, aby se objekt `obj` třídy `Particle` (předpokládáme, že referenci na tento objekt máme uloženou jako member variable třídy `BGEapp`) pohyboval stálou rychlostí o velikosti 5 (například $\frac{\text{m}}{\text{s}}$) ve směru osy $x$, tak do těla metody `Loop()` přidáme následující příkazy:
```cs
Vector2 velocity = new Vector2(5, 0);
obj.SetPosition(obj.GetPosition2D() + velocity * dt);
```
Chceme-li dále obdobné chování nastavit i pro kameru, tak její referenci je možné získat pomocí příkazu:
```cs
Camera cam = GetCamera();
```

### Nastavení uživatelského vstupu

Třída `BGEapp` dále obsahuje metody umožňující detekovat a následně i reagovat na uživatelský vstup z klávesnice a myši. Příkazy definované ve všech následujících metodách se provedou, jakmile se detekuje relevantní událost (např. metoda `KeyDownEvent()` se spustí, pokud uživatel stiskne libovolnou klávesu).

#### Vstup klávesnice

V následujících metodách je klávesa, která danou událost vyvolala (jejím stisknutím, nebo uvolněním) uložena v parametru `key` typu `UserKeyboardKey`, což je enumerace definující klávesy, které může `BasicGraphicsEngine` detekovat. 

  - **Stisknutí klávesy** - `KeyDownEvent()`.
  - **Uvolnění klávesy** - `KeyUpEvent()`.

Pokud chceme definovat určitý příkaz, který se má provést po stisknutí, případně uvolnění klávesy `Space`, tak jej definujeme následovně:
```cs
if (key == UserKeyboardKey.Space) { příkaz }
```
I když uživatel drží klávesu `Space` stále stisknutou, příkaz definovaný způsobem výše (v metodě `KeyDownEvent()`) se provede pouze jednou. Chceme-li, aby se tento příkaz neustále opakoval, dokud uživatel danou klávesu neuvolní, musíme použít speciální funkci `IsKeyPressed()`, která přijímá parametr reprezentující konkrétní klávesu (typu `UserKeyboardKey`) a vrací informaci (jako pravdivostní hodnotu) o tom, zdali je tato klávesa právě stisknutá. Tuto funkci je možné zavolat pomocí objektu `UserInput`. Definice našeho příkazu by pak vypadala následovně:
```cs
if (UserInput.IsKeyPressed(UserKeyboardKey.Space)) { příkaz }
```

#### Vstup myši

V následujících metodách je tlačítko myši, které danou událost vyvolalo (jeho stisknutím, nebo uvolněním) uloženo v parametru `button` typu `UserMouseButton`, což je enumerace definující tlačítka myši, která může `BasicGraphicsEngine` detekovat. 

  - **Stisknutí tlačítka myši** - `MouseButtonDownEvent()`.
  - **Uvolnění tlačítka myši** - `MouseButtonUpEvent()`.

Pokud chceme definovat určitý příkaz, který se má provést po stisknutí, případně uvolnění levého tlačítka myši (`Left`), tak jej definujeme následovně:
```cs
if (button == UserMouseButton.Left) { příkaz }
```
I když uživatel drží levé tlačítko stále stisknuté, příkaz definovaný způsobem výše (v metodě `MouseButtonDownEvent()`) se provede pouze jednou. Chceme-li, aby se tento příkaz neustále opakoval, dokud uživatel dané tlačítko neuvolní, musíme použít speciální funkci `IsMouseButtonPressed()`, která přijímá parametr reprezentující konkrétní tlačítko myši (typu `UserMouseButton`) a vrací informaci (jako pravdivostní hodnotu) o tom, zdali je toto tlačítko právě stisknuté. Tuto funkci je možné zavolat pomocí objektu `UserInput`. Definice našeho příkazu by pak vypadala následovně:
```cs
if (UserInput.IsMouseButtonPressed(UserMouseButton.Left)) { příkaz }
```

`BasicGraphicsEngine` dále umožňuje pomocí následujících dvou metod detekovat i pohyb myši a otáčení kolečka myši.

  - **Posunutí kurzoru myši** - `MouseMoveEvent()` - Parametrem této metody je okamžitá poloha myši (typu `Vector2`) v moment detekce události.
  - **Otáčení kolečka myši** - `MouseScrollEvent()` - Parametry této metody jsou dvě racionální čísla reprezentující okamžité natočení kolečka myši v horizontálním a vertikálním směru.