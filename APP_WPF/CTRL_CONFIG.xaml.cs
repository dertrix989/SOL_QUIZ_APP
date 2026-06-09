using LIB_BDD_JSON;
using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace APP_WPF
{
    /// <summary>
    /// Logique d'interaction pour CTRL_CONFIG.xaml
    /// </summary>
    public partial class CTRL_CONFIG : UserControl
    {
        private MainWindow _main_window;
        public BDD_JSON Base;
        public DATA Mon_Quiz;
        public CTRL_CONFIG(MainWindow P_Main_Window)
        {
            InitializeComponent();

            _main_window = P_Main_Window;

            Base = new BDD_JSON();
            Base.Initialiser_Categories();
            Mon_Quiz = new DATA();

            Cmb_Categories.ItemsSource = Base.Dico_Categorie;

        }

        private void Btn_Retour_Click(object sender, RoutedEventArgs e)
        {
            _main_window.Retour_Acceuil();
        }

        private void Btn_Lancer_Quiz_Click(object sender, RoutedEventArgs e)
        {

            if (Cmb_Categories.SelectedItem!= null)
            {
                var cle_select = (KeyValuePair<string, int>)Cmb_Categories.SelectedItem;
                string Categorie = cle_select.Key.ToString();
                int.TryParse(TB_nb_questions.Text, out int nbr_questions);


                if (Btn_Facile.IsChecked == true)
                {
                  Mon_Quiz = Base.Get_Data_Quiz(Categorie, "easy", nbr_questions);
                }

                if (Btn_Moyen.IsChecked == true)
                {
                   Mon_Quiz = Base.Get_Data_Quiz(Categorie, "medium", nbr_questions);

                }

                if (Btn_Difficile.IsChecked == true)
                {
                   Mon_Quiz = Base.Get_Data_Quiz(Categorie, "hard", nbr_questions);
                }

                _main_window.Zone_Application.Content = new CTRL_VIEW(_main_window, this);

            }



        }

        private void Slider_Questions_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TB_nb_questions.Text = Slider_Questions.Value.ToString();
        }
    }
}
