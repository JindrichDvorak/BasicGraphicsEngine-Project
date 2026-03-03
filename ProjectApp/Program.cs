namespace ProjectApp;

internal class Program
{
    static void Main(string[] args)
    {
        // Toto by měly být poslední dva řádky této Main metody -- Definují a následně spouštějí grafickou implementaci.
        //     --> Je možné definovat vlastní příkazy i "pod" těmito řádky, ale k jejich provedení dojde až po uzavření grafického okna.
        BGEapp app = new BGEapp("BasicGraphicsEngine Application", 800, 600);
        app.StartApplication();
    }
}