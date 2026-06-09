using LIB_BDD_JSON;
using System;
using System.Collections.Generic;
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
    /// Logique d'interaction pour CTRL_CORRECTION.xaml
    /// </summary>
    public partial class CTRL_CORRECTION : UserControl
    {
        public MainWindow _mainwindow;
        public List<RESULTAT_MODIF> Quiz;
        public BDD_JSON Base;
        public int index = 0;
        public int Progress_Par_Rep;
        public int compteur_question = 1;
        public CTRL_VIEW Data;
        public CTRL_CORRECTION(MainWindow P_main_win, CTRL_VIEW P_Data)
        {
            Data = P_Data;
            _mainwindow = P_main_win;
            Base = new BDD_JSON();
            Quiz = P_Data.quiz;
            Quiz = Base.decode_html(Quiz);
            InitializeComponent();
            Charger_Contenu();
            Btn_Suivant.IsEnabled = true;
            Progress_Par_Rep = 725 / Quiz.Count;
            brd_progress_bar.Width = 0;
            tbx_q_total.Text = $" / {Quiz.Count}";
        }


        public void Charger_Contenu()
        {
            var resultat = Base.Affiche_Question_Par_Question(index, Quiz);

            TB_question.Text = resultat.question;
            Tb_Rep_A.Text = resultat.liste_reponse[0];
            TB_Rep_B.Text = resultat.liste_reponse[1];
            TB_Rep_C.Text = resultat.liste_reponse[2];
            TB_Rep_D.Text = resultat.liste_reponse[3];
            tbx_Q_acutellle.Text = compteur_question.ToString();

            if (Tb_Rep_A.Text == resultat.reponse_correct)
            {
                Charger_Couleur_Bn_Rep(Btn_A);
            }
            if (TB_Rep_B.Text == resultat.reponse_correct)
            {
                Charger_Couleur_Bn_Rep(Btn_B);
            }
            if (TB_Rep_C.Text == resultat.reponse_correct)
            {
                Charger_Couleur_Bn_Rep(Btn_C);
            }
            if (TB_Rep_D.Text == resultat.reponse_correct)
            {
                Charger_Couleur_Bn_Rep(Btn_D);
            }

            brd_progress_bar.Width += Progress_Par_Rep;
            if (index+1 < Quiz.Count)
            {
                index++;
                compteur_question++;
            }
        }


        public void Charger_Couleur_Bn_Rep(Button P_btn)
        {
            P_btn.Background = new SolidColorBrush((Color)Colors.Green);
            P_btn.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7C3AED"));
        }

        public void Charger_Couleur_Normal(Button P_btn)
        {
            P_btn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1A1A2E"));
            P_btn.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2D2D4E"));
        }
        private void Btn_Suivant_Click(object sender, RoutedEventArgs e)
        {
            Charger_Couleur_Normal(Btn_A);
            Charger_Couleur_Normal(Btn_B);
            Charger_Couleur_Normal(Btn_C);
            Charger_Couleur_Normal(Btn_D);

            Charger_Contenu();
        }

        private void Btn_Retour_Click(object sender, RoutedEventArgs e)
        {
            _mainwindow.Zone_Application.Content = new CTRL_RESULTAT(_mainwindow, Data);
        }
    }
}
