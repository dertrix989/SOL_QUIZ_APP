using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Web;
using GTranslate.Translators;

namespace LIB_BDD_JSON
{ 
    public class DATA {

        public int response_code { get; set; }

        public List<RESULTAT> results { get; set; }
    }

    public class RESULTAT
    {
        public string type { get; set; }
        public string difficulty { get; set; }
        public string category { get; set; }
        public string question { get; set; }
        public string correct_answer { get; set; }
        public List<string> incorrect_answers { get; set; }
    }

    
 

    public class RESULTAT_MODIF
    {
        public string question { get; set; }
        public string[] liste_reponse { get; set; }
        public string reponse_correct { get; set; }
    }

    public class BDD_JSON
    {
        public Dictionary<string, int> Dico_Categorie = new Dictionary<string, int>();

        public Dictionary<string, int> Initialiser_Categories()
        {   
            // j'associe chaque nom de catégorie a leurs chiffre précis / car dans l'API Open Trivia DB
            // ils la catégorie se fait par chiffre
            Dico_Categorie.Add("Culture générale", 9);
            Dico_Categorie.Add("Livres", 10);
            Dico_Categorie.Add("Films", 11);
            Dico_Categorie.Add("Musique", 12);
            Dico_Categorie.Add("Comédie musicale, Théatre", 13);
            Dico_Categorie.Add("Télévision", 14);
            Dico_Categorie.Add("Jeux vidéos", 15);
            Dico_Categorie.Add("Jeux de sociétés", 16);
            Dico_Categorie.Add("Science et nature", 17);
            Dico_Categorie.Add("Ordinateurs", 18);
            Dico_Categorie.Add("Mathématiques", 19);
            Dico_Categorie.Add("Mythologie", 20);
            Dico_Categorie.Add("Sports", 21);
            Dico_Categorie.Add("Geographie", 22);
            Dico_Categorie.Add("Histoire", 23);
            Dico_Categorie.Add("Politiques", 24);
            Dico_Categorie.Add("Art", 25);
            Dico_Categorie.Add("Célébrités", 26);
            Dico_Categorie.Add("Animaux", 27);
            Dico_Categorie.Add("Véhicules", 28);
            Dico_Categorie.Add("Bandes dessinées", 29);
            Dico_Categorie.Add("Gadgets", 30);
            Dico_Categorie.Add("Anime, Manga", 31);

            return Dico_Categorie;
        }
        public DATA Get_Data_Quiz(string P_categorie, string P_difficulte, int P_quantite)
        {   
            // je répupère les données du quiz Json, les traduient en string et retourne le resultat
            HttpClient Serveur = new HttpClient();

            Dico_Categorie.TryGetValue(P_categorie, out int value);

            var Data_Json = Serveur.GetStringAsync($"https://opentdb.com/api.php?amount={P_quantite}&category={value}&difficulty={P_difficulte}&type=multiple").Result;

            var Data_String = JsonSerializer.Deserialize<DATA>(Data_Json);

            return Data_String;

        }

        public List<RESULTAT_MODIF> Get_Data_Modif(DATA P_data)
        {
            var Liste_Results = P_data.results;

            List<RESULTAT_MODIF> Liste_Resultats_Modifie = new List<RESULTAT_MODIF>();

            Random Generateur = new Random();

            
            foreach( var resultat in Liste_Results)
            {
                RESULTAT_MODIF Resultats_Modif = new RESULTAT_MODIF()
                {
                    question = resultat.question,
                    reponse_correct = resultat.correct_answer,
                    liste_reponse = new string[resultat.incorrect_answers.Count+1] 
                    
                };
                int nbr_reponses = resultat.incorrect_answers.Count + 1;

                int index_CA = Generateur.Next(nbr_reponses); // je génère la position de l'index de la bonne réponse dans liste pour que ce soit aléatoire 

                Resultats_Modif.liste_reponse[index_CA] = resultat.correct_answer;

                List<int> Liste_Index = new List<int>(); 

                Liste_Index.Add(index_CA); // j'ajoute l'index de la bonne réponse a une liste d'index


                // tant que ma liste d'index n'est pas rempli je généres des index aléatoire chacun différent
                // et je m'assure qu'ils soient différents en regardant si l'index génére = la valeur d'un autre index
                while (Liste_Index.Count < nbr_reponses)
                {
                    int Index = Generateur.Next(nbr_reponses);
                    bool ok = true;

                    foreach (int i in Liste_Index)
                    {
                        if (Index == i)
                        {
                            ok = false;
                        }
                    }

                    if (ok == true)
                    {
                        Liste_Index.Add(Index);
                    }
                }

                // j'initialise index_2 = 1 car 0 est déja pris par la bonne réponse
                // et je parcours chaque index de la liste_index (position aléatoire pour tous )
                // et je parcours toute les mauvaises réponses en leurs assigants une position aléatoire 
                int index_2 = 1;
                foreach (var reponse in resultat.incorrect_answers)
                {
                    Resultats_Modif.liste_reponse[Liste_Index[index_2]]= reponse;
                    index_2++;
                }

                Liste_Resultats_Modifie.Add(Resultats_Modif);



            }

            return Liste_Resultats_Modifie;


        }



        // je vais parcourir tout les elements de la liste modifié et les traduire en français en utilise Gltranslate (package nugget)
        public async Task<List<RESULTAT_MODIF>> Traduire_Data(List<RESULTAT_MODIF> P_data)
        {

            var Traducteur = new BingTranslator();

            foreach (var resultat in P_data)
            {
                resultat.question = (await Traducteur.TranslateAsync(resultat.question, "fr")).Translation;

                resultat.reponse_correct = (await Traducteur.TranslateAsync(resultat.reponse_correct, "fr")).Translation;

                for (int index = 0; index < resultat.liste_reponse.Length; index++)
                {
                    resultat.liste_reponse[index] = (await Traducteur.TranslateAsync(resultat.liste_reponse[index], "fr")).Translation;
                }
            }

            return P_data;        
        
        }



        // je vais récuper question par question afin en fonction de l'index choisie / j'incrémente l'index dans mon app wpf a chaque fois 
        // que la réponse a la question est chosie / ainsi je fais question par question
        public  RESULTAT_MODIF Affiche_Question_Par_Question(int P_index, List<RESULTAT_MODIF> P_Quiz)
        {
            RESULTAT_MODIF Data_Precise;
            if (P_index < P_Quiz.Count)
            {
                Data_Precise = new RESULTAT_MODIF()
                {
                    question = P_Quiz[P_index].question,
                    liste_reponse = P_Quiz[P_index].liste_reponse,
                    reponse_correct = P_Quiz[P_index].reponse_correct


                };

                return Data_Precise;
            }
            else
            {
                Data_Precise = new RESULTAT_MODIF()
                {
                    question = P_Quiz[P_Quiz.Count - 1].question,
                    liste_reponse = P_Quiz[P_Quiz.Count - 1].liste_reponse,
                    reponse_correct = P_Quiz[P_Quiz.Count - 1].reponse_correct
                };

                return Data_Precise;
            }


        }


        // decodage html
        public List<RESULTAT_MODIF> decode_html(List<RESULTAT_MODIF> liste_resultat)
        {
            foreach (var resultat in liste_resultat)
            {
                resultat.question = HttpUtility.HtmlDecode(resultat.question);
                resultat.reponse_correct = HttpUtility.HtmlDecode(resultat.reponse_correct);
                

                for (int index = 0; index < resultat.liste_reponse.Length; index++)
                {
                    resultat.liste_reponse[index] = HttpUtility.HtmlDecode(resultat.liste_reponse[index]);
                }
            }

            return liste_resultat;
        }

       
    }
}
