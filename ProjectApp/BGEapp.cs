using BasicGraphicsEngine; // Grafické vykreslování.
using System.Numerics;     // Integrovaná implementace vektorů, matic a jejich operací. 
using System.Drawing;      // Integrovaná implementace barev kompatibilní s BasicGraphicsEngine.


namespace ProjectApp;

internal class BGEapp : Application
{
    // Konstruktor vaší aplikace, která zároveň zajišťuje základní nastavení a spuštění grafického enginu BasicGraphicsEngine:
    // ==> Některá základní nastavení je možné zvolit pomocí objektu Settings:
    //     --> Například barvu pozadí je možné nastavit (zde na bílou) takto: Settings.BackgroundColor = Color.White;
    // ==> V těle tohoto konstruktoru NENÍ MOŽNÉ psát logiku, která zajišťuje vykreslování. K tomuto účelu slouží metoda Setup().
    public BGEapp(string title, uint viewportWidth, uint viewportHeight) : base(title, viewportWidth, viewportHeight)
    {

    }

    // -------------- ZÁKLADNÍ LOGIKA APLIKACE -------------- //

    // Příkazy, které se provedou před vykreslením PRVNÍHO snímku:
    // ==> Hlavním i zamýšleným účelem této funkce je DEFINICE OBJEKTŮ, které je možné vykreslit. Následující "částečný pseudokód" 
    //     ilustruje definici základních objektů, které je možné vykreslit. 
    //     --> Částice: Particle obj = new Particle(poloha, velikost, barva);
    //     --> Úsečka: Line obj = new Line(počáteční bod -- poloha, koncový bod -- poloha, tloušťka, barva);
    //     --> Obdélník: Quad obj = new Quad(poloha středu, šířka, výška, barva);
    //     --> Kruh: Circle obj = new Circle(poloha středu, poloměr, barva);
    //     --> Kruh s zvýrazněným obvodem: Circle obj = new Circle(poloha středu, poloměr, barva kruhu, tloušťka obvodu, barva obvodu);
    //         --> Kružnici (tedy pouze obvod určité šířky) je možné vykreslit tak, že za "barvu kruhu" zvolíte libovolnou barvu
    //             s alpha = 0 (což je čtvrtý parametr v systému RGBA -- například: Vector4(0, 0, 0, 0)).
    // ==> Jakmile máte definovaný základní objekt "obj", tak jeho VYKRESLENÍ zajistíte příkazem: AddObject(obj);
    public override void Setup()
    {
        Circle circ1 = new Circle(new Vector3(0, 0, 0), 1, new Vector4(1, 0, 0, 0f), 0.2f, Color.HotPink);
        Circle circ2 = new Circle(new Vector3(0, 0, 1), 0.5f, new Vector4(0, 1, 0, 0f), 0.1f, Color.Green);
        Quad quad = new Quad(new Vector3(1, 0, -1), 2.3f, 2.3f, Color.Black);

        AddObject(quad);
        AddObject(circ2);
        AddObject(circ1);
    }

    // Příkazy, které se provádí před vykreslením KAŽDÉHO snímku:
    // ==> Proměnná dt odpovídá času (v ms), jak dlouho trvalo vykreslení předchozího snímku.
    // ==> Pokud chcete kameru ovládat programaticky, tak její referenci je možné získat příkazem: GetCamera()
    public override void Loop(float dt)
    {
        
    }

    // -------------- UŽIVATELSKÝ INPUT --------------------- //

    // KLÁVESNICE:

    // Příkazy, které se provádí před vykreslením KAŽDÉHO snímku, ALE umožňují detekovat input KLÁVESNICE:
    // ==> V KeyDownEvent() detekujeme STISKNUTÍ klávesy.
    // ==> V KeyUpEvent() detekujeme UVOLNĚNÍ klávesy.
    // ==> Klávesy, které je možné detekovat definuje enumerace UserKeyboardKey.
    // ==> Pro příkazy, které se mají provést PRÁVĚ PO DETEKOVÁNÍ dané stisknuté/uvoněné klávesy platí:
    //     --> Například pro klávesu "Space": if (key == UserKeyboardKey.Space) { příkaz }
    //     --> Po detekci se daný příkaz provede POUZE JEDNOU.
    // ==> Pro příkazy, které se mají provádět, když je daná klávesa STÁLE STISKNUTÁ platí:
    //     --> Například pro klávesu "A": if (UserInput.IsKeyPressed(UserKeyboardKey.A)) { příkaz }
    //     --> Daný příkaz se NEUSTÁLE OPAKUJE, dokud je daná klávesa stisknutá (ale neblokuje vykreslování).
    //     --> Tuto detekci je možné provádět i mimo následující dvě funkce, ale zde to "dává smysl".
    public override void KeyDownEvent(UserKeyboardKey key)
    {

    }

    public override void KeyUpEvent(UserKeyboardKey key)
    {

    }

    // MYŠ:

    // Příkazy, které se provádí před vykreslením KAŽDÉHO snímku, ALE umožňují detekovat tlačítkový input MYŠI:
    // ==> V MouseButtonDownEvent() detekujeme STISKNUTÍ tlačítka myši.
    // ==> V MouseButtonUpEvent() detekujeme UVOLNĚNÍ tlačítka myši.
    // ==> Tlačítka myši, která je možné detekovat definuje enumerace UserMouseButton.
    // ==> Pro příkazy, které se mají provést PRÁVĚ PO DETEKOVÁNÍ daného stisknutého/uvoněného tlačítka platí:
    //     --> Například pro levé tlačítko: if (button == UserMouseButton.Left) { příkaz }
    //     --> Po detekci se daný příkaz provede POUZE JEDNOU.
    // ==> Pro příkazy, které se mají provádět, když je daná klávesa STÁLE STISKNUTÁ platí:
    //     --> Například pro klávesu "A": if (UserInput.IsKeyPressed(UserKeyboardKey.A)) { příkaz }
    //     --> Daný příkaz se NEUSTÁLE OPAKUJE, dokud je daná klávesa stisknutá (ale neblokuje vykreslování).
    //     --> Tuto detekci je možné provádět i mimo následující dvě funkce, ale zde to "dává smysl".
    public override void MouseButtonDownEvent(UserMouseButton button)
    {

    }

    public override void MouseButtonUpEvent(UserMouseButton button)
    {

    }

    // V této metodě je možné definovat příkazy, které se mají provést, když myš změní svou polohu:
    // ==> Proměnná position obsahuje okamžitou polohu myši.
    public override void MouseMoveEvent(Vector2 position)
    {

    }

    // V této metodě je možné definovat příkazy, které se mají provést, když se změní natočení kolečka myši:
    // ==> Proměnná mouseWheelX reprezentuje horizontální naklonění kolečka myši -- prakticky není potřeba používat.
    // ==> Proměnná mouseWheelY reprezentuje vertikální natočení kolečka myši (tedy "dopředu", nebo "dozadu").
    // ==> Natočení kolečka myši je v BasicGraphicsEngine provázáno s přibližováním a oddalováním kamery (zoom).
    //     Tento fakt vám ovšem NEBRÁNÍ definovat pro input z kolečka myši dodatečnou logiku.
    public override void MouseScrollEvent(float mouseWheelX, float mouseWheelY)
    {
        
    }
}
