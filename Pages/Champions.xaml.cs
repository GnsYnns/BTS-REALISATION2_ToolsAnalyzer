using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Globalization;

namespace ToolsAnalyzer.Pages
{
    /// <summary>
    /// Logique d'interaction pour Champions.xaml
    /// </summary>
    public partial class Champions : Page
    {
        // Variable global
        const string _dsn = "server=localhost;database=tools_analyzer;username=root;password=;";
        private MySqlConnection _connection = new MySqlConnection(_dsn);
        private MySqlCommand _command;
        private MySqlDataAdapter _adapter;
        private DataTable _dt;
        private ObservableCollection<int> level;

        public Champions()
        {
            this.Resources.Add("ThreeQuartersConverter", new ThreeQuartersConverter());
            InitializeComponent();
            Afficher();

            // Gestion liste
            level = new ObservableCollection<int>();
            level.Add(1);
            level.Add(2);
            level.Add(3);
            level.Add(4);
            level.Add(5);
            level.Add(6);
            level.Add(7);
            level.Add(8);
            level.Add(9);
            level.Add(10);
            level.Add(11);
            level.Add(12);
            level.Add(13);
            level.Add(14);
            level.Add(15);
            level.Add(16);
            level.Add(17);
            level.Add(18);
            levelSelector.ItemsSource = level;

        }

        public void Afficher()
        {
            try
            {
                string sqlQuery = @"SELECT 
                                    c.name AS ChampionName,
                                    c.title AS ChampionTitle,
                                    CONCAT('pack://application:,,,/Assets/Champions/', c.full) AS IconFull,
                                    c.partype AS ChampionPartype,
                                    c.hp AS HP,
                                    c.hpperlevel AS HPPerLevel,
                                    c.mp AS MP,
                                    c.mpperlevel AS MPPerLevel,
                                    c.movespeed AS MoveSpeed,
                                    c.armor AS Armor,
                                    c.armorperlevel AS ArmorPerLevel,
                                    c.spellblock AS SpellBlock,
                                    c.spellblockperlevel AS SpellBlockPerLevel,
                                    c.attackrange AS AttackRange,
                                    c.hpregen AS HPRegen,
                                    c.hpregenperlevel AS HPRegenPerLevel,
                                    c.crit AS Crit,
                                    c.critperlevel AS CritPerLevel,
                                    c.attackdamage AS AttackDamage,
                                    c.attackdamageperlevel AS AttackDamagePerLevel,
                                    c.attackspeed AS AttackSpeed,
                                    c.attackspeedperlevel AS AttackSpeedPerLevel,
                                    c.mpregen AS MPRegen,
                                    c.mpregenperlevel AS MPRegenPerLevel
                                FROM 
                                    Champion c";

                _adapter = new MySqlDataAdapter(sqlQuery, _connection);
                _dt = new DataTable();
                _adapter.Fill(_dt);
                selectorChamp.ItemsSource = _dt.AsDataView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void loadStatistic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((DataRowView)selectorChamp.SelectedItem != null)
            {
                DataRowView _drv = (DataRowView)selectorChamp.SelectedItem;

                // Icone
                string imageName = _drv.Row["IconFull"].ToString();
                Uri uri = new Uri(imageName, UriKind.Absolute);
                ImageSource imageSource = new BitmapImage(uri);
                championIcone.ImageSource = imageSource;
                championName.Text = _drv.Row["ChampionName"].ToString();

                //Stats
                labelHealth.Content = _drv.Row["HP"].ToString();
                labelMana.Content = _drv.Row["MP"].ToString();
                labelHealthRegen.Content = _drv.Row["HPRegen"].ToString();
                labelManaRegen.Content = _drv.Row["MPRegen"].ToString();
                labelAD.Content = _drv.Row["AttackDamage"].ToString();
                labelAR.Content = _drv.Row["Armor"].ToString();
                labelMR.Content = _drv.Row["SpellBlock"].ToString();
                labelCrit.Content = "150%";
                labelRange.Content = _drv.Row["AttackRange"].ToString();
                labelMS.Content = _drv.Row["MoveSpeed"].ToString();
                labelBaseAS.Content = _drv.Row["AttackSpeed"].ToString();
                labelBonAS.Content = 0 + "%";
            }
        }

        private void levelSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Fonction pour le Changement de champions
            DataRowView _drv = (DataRowView)selectorChamp.SelectedItem;
            object selectedItem = levelSelector.SelectedItem;
            int level = (int)selectedItem;
            const double Var1 = 0.7025;
            float multiplicateur1 = (float)Var1;
            const double Var2 = 0.0175;
            float multiplicateur2 = (float)Var2;


            //Variable stat supp
            float hpPlus = float.Parse(_drv.Row["HPPerLevel"].ToString());
            float manaPlus = float.Parse(_drv.Row["MPRegenPerLevel"].ToString());
            float hp5Plus = float.Parse(_drv.Row["HPRegenPerLevel"].ToString());
            float mana5Plus = float.Parse(_drv.Row["MPRegenPerLevel"].ToString());
            float adPlus = float.Parse(_drv.Row["AttackDamagePerLevel"].ToString());
            float arPlus = float.Parse(_drv.Row["ArmorPerLevel"].ToString());
            float mrPlus = float.Parse(_drv.Row["SpellBlockPerLevel"].ToString());
            float asPlus = float.Parse(_drv.Row["AttackSpeedPerLevel"].ToString());

            // Changement
            labelHealth.Content = Math.Round(float.Parse(_drv.Row["HP"].ToString()) + (hpPlus * (level - 1) * (multiplicateur1 + multiplicateur2 * (level - 1))), 2);
            labelMana.Content = Math.Round(float.Parse(_drv.Row["MP"].ToString()) + (manaPlus * (level - 1) * (multiplicateur1 + multiplicateur2 * (level - 1))), 2);
            labelHealthRegen.Content = Math.Round(float.Parse(_drv.Row["HPRegen"].ToString()) + (hp5Plus * (level - 1) * (multiplicateur1 + multiplicateur2 * (level - 1))), 2);
            labelManaRegen.Content = Math.Round(float.Parse(_drv.Row["MPRegen"].ToString()) + (mana5Plus * (level - 1) * (multiplicateur1 + multiplicateur2 * (level - 1))), 2);
            labelAD.Content = Math.Round(float.Parse(_drv.Row["AttackDamage"].ToString()) + (adPlus * (level - 1) * (multiplicateur1 + multiplicateur2 * (level - 1))), 2);
            labelAR.Content = Math.Round(float.Parse(_drv.Row["Armor"].ToString()) + (arPlus * (level - 1) * (multiplicateur1 + multiplicateur2 * (level - 1))), 2);
            labelMR.Content = Math.Round(float.Parse(_drv.Row["SpellBlock"].ToString()) + (mrPlus * (level - 1) * (multiplicateur1 + multiplicateur2 * (level - 1))), 2);
            labelCrit.Content = "150%";
            labelRange.Content = _drv.Row["AttackRange"].ToString();
            labelMS.Content = _drv.Row["MoveSpeed"].ToString();
            labelBaseAS.Content = _drv.Row["AttackSpeed"].ToString();
            labelBonAS.Content = Math.Round(asPlus * (level - 1) * (multiplicateur1 + multiplicateur2 * (level - 1)), 2) + "%";
        }


        public class ThreeQuartersConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is double doubleValue)
                {
                    return doubleValue * 0.72; // 3/4 de la valeur - margin
                }
                return value;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

    }


}
