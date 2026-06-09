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
    /// Logique d'interaction pour CTRL_RESULTAT.xaml
    /// </summary>
    public partial class CTRL_RESULTAT : UserControl
    {
        public string[] tab_rep;
        public List<RESULTAT_MODIF> Quiz;
        public int compteur_bn_rep = 0;
        public int compteur_fs_rep = 0;
        public double Pourcentage_bn_rep;
        public MainWindow _mainwindow;
        public CTRL_VIEW Data;
        public CTRL_RESULTAT(MainWindow P_MainWindow, CTRL_VIEW P_quiz)
        {
            Data = P_quiz;
            _mainwindow = P_MainWindow;
            InitializeComponent();
            tab_rep = P_quiz.tab_rep; ;
            Quiz = P_quiz.quiz;

            int nb_bn_rep = Return_Nb_Bn_Rep();
            int nb_fs_rep = Return_Fs_Bn_Rep();

            tbx_nb_bnrep.Text = nb_bn_rep.ToString();
            tb_nb_fsrep.Text = nb_fs_rep.ToString();
            tbx_score_petit.Text = $"{nb_bn_rep} / {Quiz.Count}";

            Pourcentage_bn_rep = (nb_bn_rep * 100) / Quiz.Count;
            tbx_pourcentage.Text = $"{Pourcentage_bn_rep}%";

            var tab_mess = Charger_Titre_Message_Result();
            tbx_Titre_result.Text = tab_mess[0];
            tbx_mess_result.Text = tab_mess[1];

            double resultat_progress_bar = (285 / Quiz.Count) * nb_bn_rep;
        

            brd_progress_bar_petit.Width = resultat_progress_bar;

        }

        public int Return_Nb_Bn_Rep()
        {
            for (int index = 0; index < Quiz.Count; index++)
            {
                if (tab_rep[index] == Quiz[index].reponse_correct)
                {
                    compteur_bn_rep++;
                }

            }

            return compteur_bn_rep;
        }

        public int Return_Fs_Bn_Rep()
        {
            for (int index = 0; index < Quiz.Count; index++)
            {
                if (tab_rep[index] != Quiz[index].reponse_correct)
                {
                    compteur_fs_rep++;
                }

            }

            return compteur_fs_rep;
        }

        public string[] Charger_Titre_Message_Result()
        {
            string Titre_Mess = "";
            string mess = "";
            if (Pourcentage_bn_rep == 0) { Titre_Mess = "CATASTROPHIQUE 💩"; mess = "J'ai mal au yeux 👀 carément"; }

            if (Pourcentage_bn_rep >= 1 && Pourcentage_bn_rep < 40) { Titre_Mess = "PAS OUF 👎"; mess = "Change de thème..."; }

            if (Pourcentage_bn_rep >= 40 && Pourcentage_bn_rep < 60) { Titre_Mess = "MOUAIS"; mess = "Peut mieux faire (j'espère)"; }

            if (Pourcentage_bn_rep >= 60 && Pourcentage_bn_rep < 80)
            {
                Titre_Mess = "C'EST PLUTOT BIEN ^_^";
                mess = "T'a un ptit niveau (❁´◡`❁)";
            }

            if (Pourcentage_bn_rep >= 80 && Pourcentage_bn_rep < 100) { Titre_Mess = "WSH T'ES BON"; mess = "OK la y'a du level 👌"; }

            if (Pourcentage_bn_rep == 100) { Titre_Mess = "PARFAIT";  mess = "RIEN A DIRE."; }

            string[] tab_messages = new string[2];

            tab_messages[0] = Titre_Mess;

            tab_messages[1] = mess;

            return tab_messages;

        }

        private void btn_detail_Click(object sender, RoutedEventArgs e)
        {
            _mainwindow.Zone_Application.Content = new CTRL_CORRECTION(_mainwindow, Data);
        }

        private void btn_acceuil_Click(object sender, RoutedEventArgs e)
        {
            _mainwindow.Retour_Acceuil();
        }

        private void btn_rejouer_Click(object sender, RoutedEventArgs e)
        {
            _mainwindow.Zone_Application.Content = new CTRL_CONFIG(_mainwindow);
        }
    }
}
