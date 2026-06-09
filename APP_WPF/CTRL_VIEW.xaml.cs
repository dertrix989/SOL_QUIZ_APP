using LIB_BDD_JSON;
using System;
using System.Collections.Generic;
using System.Net.Quic;
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
    /// Logique d'interaction pour CTRL_VIEW.xaml
    /// </summary>
    /// 
    public partial class CTRL_VIEW : UserControl
    {
        public MainWindow _main_window;
        public BDD_JSON Base;

        public List<RESULTAT_MODIF> quiz;
        public List<RESULTAT_MODIF> quiz_traduit;
        public int index = 0;
        public int compteur_question = 1;
        public int index_tab = 0;
        public string[] tab_rep;
        public double Progress_Par_Rep;
        public CTRL_VIEW(MainWindow P_MainWindow, CTRL_CONFIG P_config)
        {   

            InitializeComponent();
            _main_window = P_MainWindow;
            Base = new BDD_JSON();
            quiz = Base.Get_Data_Modif(P_config.Mon_Quiz);
            quiz = Base.decode_html(quiz);
            tbx_q_total.Text = $" / {quiz.Count}";
            tbx_difficulte.Text = $"⚡ {P_config.Mon_Quiz.results[0].difficulty}";
            tbx_categorie.Text = $" 🎮 {P_config.Mon_Quiz.results[0].category}";
            tab_rep = new string[quiz.Count];
            Progress_Par_Rep = 725 / quiz.Count;
            brd_progress_bar.Width = 0;


            Charger_Contenu();
        }

        public void Charger_Contenu()
        {
            var Data= Base.Affiche_Question_Par_Question(index, quiz);

            TB_question.Text = Data.question;
            Tb_Rep_A.Text = Data.liste_reponse[0];
            TB_Rep_B.Text = Data.liste_reponse[1];
            TB_Rep_C.Text = Data.liste_reponse[2];
            TB_Rep_D.Text = Data.liste_reponse[3];
            index++;
            Btn_Suivant.IsEnabled = false;


        }
        
        public void Augmenter_Progress_Bar()
        {
            //brd_progress_bar.
        }
        private void Btn_Suivant_Click(object sender, RoutedEventArgs e)
        {   
            Charger_Contenu();

            if (compteur_question == quiz.Count)
            {
                _main_window.Zone_Application.Content = new CTRL_RESULTAT(_main_window, this);
            }
            compteur_question++;
            index_tab++;
            tbx_Q_acutellle.Text = compteur_question.ToString();
            brd_progress_bar.Width += Progress_Par_Rep;
            Charger_Couleur_Normal(Btn_B);
            Charger_Couleur_Normal(Btn_C);
            Charger_Couleur_Normal(Btn_D);
            Charger_Couleur_Normal(Btn_A);
        }

        public void Charger_Couleur_Click(Button P_Btn_Selectione)
        {
            P_Btn_Selectione.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0F3460"));
            P_Btn_Selectione.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7C3AED"));

        }

        public void Charger_Couleur_Normal(Button P_Btn_Selectione)
        {
            P_Btn_Selectione.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1A1A2E"));
            P_Btn_Selectione.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2D2D4E"));
        }

        private void Btn_A_Click(object sender, RoutedEventArgs e)
        {
            Charger_Couleur_Click(Btn_A);
            Charger_Couleur_Normal(Btn_B);
            Charger_Couleur_Normal(Btn_C);
            Charger_Couleur_Normal(Btn_D);
            Btn_Suivant.IsEnabled = true;
            tab_rep[index_tab] = Tb_Rep_A.Text;
        }

        private void Btn_B_Click(object sender, RoutedEventArgs e)
        {
            Charger_Couleur_Click(Btn_B);
            Charger_Couleur_Normal(Btn_A);
            Charger_Couleur_Normal(Btn_C);
            Charger_Couleur_Normal(Btn_D);
            Btn_Suivant.IsEnabled = true;
            tab_rep[index_tab] = TB_Rep_B.Text;

        }

        private void Btn_C_Click(object sender, RoutedEventArgs e)
        {
            Charger_Couleur_Click(Btn_C);
            Charger_Couleur_Normal(Btn_B);
            Charger_Couleur_Normal(Btn_A);
            Charger_Couleur_Normal(Btn_D);
            Btn_Suivant.IsEnabled = true;
            tab_rep[index_tab] = TB_Rep_C.Text;

        }

        private void Btn_D_Click(object sender, RoutedEventArgs e)
        {
            Charger_Couleur_Click(Btn_D);
            Charger_Couleur_Normal(Btn_B);
            Charger_Couleur_Normal(Btn_C);
            Charger_Couleur_Normal(Btn_A);
            Btn_Suivant.IsEnabled = true;
            tab_rep[index_tab] = TB_Rep_D.Text;
        }
    }
}
