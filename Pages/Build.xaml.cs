using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ToolsAnalyzer.Pages
{
    /// <summary>
    /// Logique d'interaction pour Build.xaml
    /// </summary>
    public partial class Build : Page
    {
        // Variable global
        const string _dsn = "server=localhost;database=tools_analyzer;username=root;password=;";
        private MySqlConnection _connection = new MySqlConnection(_dsn);
        private MySqlCommand _command;
        private MySqlDataAdapter _adapter;
        private DataTable _dt;

        public Build()
        {
            InitializeComponent();
            Afficher();
        }

        public void Afficher()
        {
            try
            {
                string sqlQuery = @"SELECT 
                                    i.id AS Id,
                                    i.name AS Name,
                                    CONCAT('pack://application:,,,/Assets/Items/', i.full) AS Full,
                                    i.total AS Total,
                                    i.description AS Description,
                                    i.FlatHPPoolMod AS FlatHPPoolMod,
                                    i.rFlatHPModPerLevel AS rFlatHPModPerLevel,
                                    i.FlatMPPoolMod AS FlatMPPoolMod,
                                    i.rFlatMPModPerLevel AS rFlatMPModPerLevel,
                                    i.PercentHPPoolMod AS PercentHPPoolMod,
                                    i.PercentMPPoolMod AS PercentMPPoolMod,
                                    i.FlatHPRegenMod AS FlatHPRegenMod,
                                    i.rFlatHPRegenModPerLevel AS rFlatHPRegenModPerLevel,
                                    i.PercentHPRegenMod AS PercentHPRegenMod,
                                    i.FlatMPRegenMod AS FlatMPRegenMod,
                                    i.rFlatMPRegenModPerLevel AS rFlatMPRegenModPerLevel,
                                    i.PercentMPRegenMod AS PercentMPRegenMod,
                                    i.FlatArmorMod AS FlatArmorMod,
                                    i.rFlatArmorModPerLevel AS rFlatArmorModPerLevel,
                                    i.PercentArmorMod AS PercentArmorMod,
                                    i.rFlatArmorPenetrationMod AS rFlatArmorPenetrationMod,
                                    i.rFlatArmorPenetrationModPerLevel AS rFlatArmorPenetrationModPerLevel,
                                    i.rPercentArmorPenetrationMod AS rPercentArmorPenetrationMod,
                                    i.rPercentArmorPenetrationModPerLevel AS rPercentArmorPenetrationModPerLevel,
                                    i.FlatPhysicalDamageMod AS FlatPhysicalDamageMod,
                                    i.rFlatPhysicalDamageModPerLevel AS rFlatPhysicalDamageModPerLevel,
                                    i.PercentPhysicalDamageMod AS PercentPhysicalDamageMod,
                                    i.FlatMagicDamageMod AS FlatMagicDamageMod,
                                    i.rFlatMagicDamageModPerLevel AS rFlatMagicDamageModPerLevel,
                                    i.PercentMagicDamageMod AS PercentMagicDamageMod,
                                    i.FlatMovementSpeedMod AS FlatMovementSpeedMod,
                                    i.rFlatMovementSpeedModPerLevel AS rFlatMovementSpeedModPerLevel,
                                    i.PercentMovementSpeedMod AS PercentMovementSpeedMod,
                                    i.rPercentMovementSpeedModPerLevel AS rPercentMovementSpeedModPerLevel,
                                    i.FlatAttackSpeedMod AS FlatAttackSpeedMod,
                                    i.PercentAttackSpeedMod AS PercentAttackSpeedMod,
                                    i.rPercentAttackSpeedModPerLevel AS rPercentAttackSpeedModPerLevel,
                                    i.rFlatDodgeMod AS rFlatDodgeMod,
                                    i.rFlatDodgeModPerLevel AS rFlatDodgeModPerLevel,
                                    i.PercentDodgeMod AS PercentDodgeMod,
                                    i.FlatCritChanceMod AS FlatCritChanceMod,
                                    i.rFlatCritChanceModPerLevel AS rFlatCritChanceModPerLevel,
                                    i.PercentCritChanceMod AS PercentCritChanceMod,
                                    i.FlatCritDamageMod AS FlatCritDamageMod,
                                    i.rFlatCritDamageModPerLevel AS rFlatCritDamageModPerLevel,
                                    i.PercentCritDamageMod AS PercentCritDamageMod,
                                    i.FlatBlockMod AS FlatBlockMod,
                                    i.PercentBlockMod AS PercentBlockMod,
                                    i.FlatSpellBlockMod AS FlatSpellBlockMod,
                                    i.rFlatSpellBlockModPerLevel AS rFlatSpellBlockModPerLevel,
                                    i.PercentSpellBlockMod AS PercentSpellBlockMod,
                                    i.FlatEXPBonus AS FlatEXPBonus,
                                    i.PercentEXPBonus AS PercentEXPBonus,
                                    i.rPercentCooldownMod AS rPercentCooldownMod,
                                    i.rPercentCooldownModPerLevel AS rPercentCooldownModPerLevel,
                                    i.rFlatTimeDeadMod AS rFlatTimeDeadMod,
                                    i.rFlatTimeDeadModPerLevel AS rFlatTimeDeadModPerLevel,
                                    i.rPercentTimeDeadMod AS rPercentTimeDeadMod,
                                    i.rPercentTimeDeadModPerLevel AS rPercentTimeDeadModPerLevel,
                                    i.rFlatGoldPer10Mod AS rFlatGoldPer10Mod,
                                    i.rFlatMagicPenetrationMod AS rFlatMagicPenetrationMod,
                                    i.rFlatMagicPenetrationModPerLevel AS rFlatMagicPenetrationModPerLevel,
                                    i.rPercentMagicPenetrationMod AS rPercentMagicPenetrationMod,
                                    i.rPercentMagicPenetrationModPerLevel AS rPercentMagicPenetrationModPerLevel,
                                    i.FlatEnergyRegenMod AS FlatEnergyRegenMod,
                                    i.rFlatEnergyRegenModPerLevel AS rFlatEnergyRegenModPerLevel,
                                    i.FlatEnergyPoolMod AS FlatEnergyPoolMod,
                                    i.rFlatEnergyModPerLevel AS rFlatEnergyModPerLevel,
                                    i.PercentLifeStealMod AS PercentLifeStealMod,
                                    i.PercentSpellVampMod AS PercentSpellVampMod
                                FROM 
                                    item i
                                ";

                _adapter = new MySqlDataAdapter(sqlQuery, _connection);
                _dt = new DataTable();
                _adapter.Fill(_dt);
                //lb1.ItemsSource = _dt.AsDataView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void lb1_MouseMove(object sender, MouseEventArgs e)
        {
            Point mPos = e.GetPosition(null);
            if (e.LeftButton == MouseButtonState.Pressed &&
                Math.Abs(mPos.X) > SystemParameters.MinimumHorizontalDragDistance &&
                Math.Abs(mPos.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                try
                {
                    ListBoxItem selectedItem = (ListBoxItem)lb1.SelectedItem;

                    lb1.Items.Remove(selectedItem);

                    System.Windows.DragDrop.DoDragDrop(this, new DataObject(DataFormats.FileDrop, selectedItem), DragDropEffects.Copy);

                    if (selectedItem.Parent == null)
                        lb1.Items.Add(selectedItem);
                }
                catch { }

            }
        }

        private void lb2_MouseMove(object sender, MouseEventArgs e)
        {
            Point mPos = e.GetPosition(null);
            if (e.LeftButton == MouseButtonState.Pressed &&
                Math.Abs(mPos.X) > SystemParameters.MinimumHorizontalDragDistance &&
                Math.Abs(mPos.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                try
                {
                    ListBoxItem selectedItem = (ListBoxItem)lb2.SelectedItem;

                    lb2.Items.Remove(selectedItem);

                    System.Windows.DragDrop.DoDragDrop(this, new DataObject(DataFormats.FileDrop, selectedItem), DragDropEffects.Copy);

                    if (selectedItem.Parent == null)
                        lb2.Items.Add(selectedItem);
                }
                catch { }

            }
        }

        private void lb2_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(DataFormats.FileDrop) is ListBoxItem listItem)
            {
                lb2.Items.Add(listItem);
            }
        }

        private void lb1_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(DataFormats.FileDrop) is ListBoxItem listItem)
            {
                lb1.Items.Add(listItem);
            }
        }


    }
}
