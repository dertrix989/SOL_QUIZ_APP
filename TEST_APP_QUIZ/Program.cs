using LIB_BDD_JSON;

namespace TEST_APP_QUIZ;

internal class Program
{
    static async Task Main(string[] args)
    {
        BDD_JSON Quiz = new BDD_JSON();

        Quiz.Initialiser_Categories();


        var Data = Quiz.Get_Data_Quiz("Jeux vidéos", "easy", 5);

        var Data_modif = Quiz.Get_Data_Modif(Data);

        List<RESULTAT_MODIF> Data_modif_traduite = await Quiz.Traduire_Data(Data_modif);

        var Data_1 = Quiz.Affiche_Question_Par_Question(0, Data_modif_traduite);

        var Data_4 = Quiz.Affiche_Question_Par_Question(4, Data_modif_traduite);

        Console.WriteLine(Data_1.question);
        Console.WriteLine(Data_4.question);

    }
}
