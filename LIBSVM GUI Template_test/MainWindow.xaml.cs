using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xamarin.Forms.PlatformConfiguration;
using MathWorks.MATLAB.NET.Arrays;
using MathWorks.MATLAB.NET.Utility;
using MLApp;
using System.Security.AccessControl;
using System.Security.Principal;
using ForGUI_GridSearch;
using clearmex;
using System.Data.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Diagnostics;
using System.Windows.Media.Animation;

namespace LIBSVM_GUI_Template_test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : ControlWriter
    {

        #region Variables

        int OptimiseValue = 3;
        internal int Running = 0;
        internal int DataManagementRunning = 0;
        public int nfoldint = 0;
        public int ColumnIndex = 2;
        public string C1Export = "0";
        public string C2Export = "0";
        public string C3Export = "0";
        public string C4Export = "0";
        public string C5Export = "0";
        public string C6Export = "0";
        public string DateExport = "0";
        public string LabelsExport = "0";
        public int AttributeIndex = 2;
        public string A1Export = "0";
        public string A2Export = "0";
        public string A3Export = "0";
        public string A4Export = "0";
        public string A5Export = "0";
        public string A6Export = "0";
        public int TAttributeIndex = 2;
        public string TA1Export = "0";
        public string TA2Export = "0";
        public string TA3Export = "0";
        public string TA4Export = "0";
        public string TA5Export = "0";
        public string TA6Export = "0";
        public int TAttributeIndexCache;
        public string TA1Cache;
        public string TA2Cache;
        public string TA3Cache;
        public string TA4Cache;
        public string TA5Cache;
        public string TA6Cache;
        public string A1Cache;
        public string A2Cache;
        public string A3Cache;
        public string A4Cache;
        public string A5Cache;
        public string A6Cache;
        public string Length_Text_Export;
        public string TLength_Text_Export;
        public int initialised = 0;
        public int ConvertRunning = 0;
        public int SplitRunning = 0;
        public int GridSearchRunning = 0;
        public int TrainRunning = 0;
        public int TestRunning = 0;
        private int MatlabPID;
        public string Assembly_Location = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase.Remove(0,8));


        #endregion

        public Features Featuresobj { get; set; }
        public Features_Linked Features_Linkedobj { get; set; }
        public VisibilityClass VisibilityClassobj { get; set; }

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);
        public ImageSource ImageSourceFromBitmap(Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            try
            {
                return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally { DeleteObject(handle); }
        }
        
        public MainWindow()
        {
            InitializeComponent();

            #region Initialise DataContext

            Featuresobj = new Features()
            {
                Train1Value = "1",
                Train2Value = "1",
                Train3Value = "1",
                Train4Value = "1",
                Train5Value = "1",
                Train6Value = "1",
                Test1Value = "1",
                Test2Value = "1",
                Test3Value = "1",
                Test4Value = "1",
                Test5Value = "1",
                Test6Value = "1",
            };
            Features_Linkedobj = new Features_Linked(){ };

            Attribute1Text.DataContext = Featuresobj;
            Attribute2Text.DataContext = Featuresobj;
            Attribute3Text.DataContext = Featuresobj;
            Attribute4Text.DataContext = Featuresobj;
            Attribute5Text.DataContext = Featuresobj;
            Attribute6Text.DataContext = Featuresobj;
            TAttribute1Text.DataContext = Featuresobj;
            TAttribute2Text.DataContext = Featuresobj;
            TAttribute3Text.DataContext = Featuresobj;
            TAttribute4Text.DataContext = Featuresobj;
            TAttribute5Text.DataContext = Featuresobj;
            TAttribute6Text.DataContext = Featuresobj;

            Attribute1.Visibility = Visibility.Visible;
            Attribute2.Visibility = Visibility.Visible;
            Attribute3.Visibility = Visibility.Collapsed;
            Attribute4.Visibility = Visibility.Collapsed;
            Attribute5.Visibility = Visibility.Collapsed;
            Attribute6.Visibility = Visibility.Collapsed;
            Attribute1Text.Visibility = Visibility.Visible;
            Attribute2Text.Visibility = Visibility.Visible;
            Attribute3Text.Visibility = Visibility.Collapsed;
            Attribute4Text.Visibility = Visibility.Collapsed;
            Attribute5Text.Visibility = Visibility.Collapsed;
            Attribute6Text.Visibility = Visibility.Collapsed;
            AttributeMinus.Visibility = Visibility.Visible;
            AttributePlus.Visibility = Visibility.Visible;
            TAttribute1.Visibility = Visibility.Visible;
            TAttribute2.Visibility = Visibility.Visible;
            TAttribute3.Visibility = Visibility.Collapsed;
            TAttribute4.Visibility = Visibility.Collapsed;
            TAttribute5.Visibility = Visibility.Collapsed;
            TAttribute6.Visibility = Visibility.Collapsed;
            TAttribute1Text.Visibility = Visibility.Visible;
            TAttribute2Text.Visibility = Visibility.Visible;
            TAttribute3Text.Visibility = Visibility.Collapsed;
            TAttribute4Text.Visibility = Visibility.Collapsed;
            TAttribute5Text.Visibility = Visibility.Collapsed;
            TAttribute6Text.Visibility = Visibility.Collapsed;
            TAttributeMinus.Visibility = Visibility.Visible;
            TAttributePlus.Visibility = Visibility.Visible;
            Ep.Visibility = Visibility.Visible;
            Ep_Text.Visibility = Visibility.Visible;
            Nu.Visibility = Visibility.Hidden;
            Nu_Text.Visibility = Visibility.Hidden;
            Gamma.Visibility = Visibility.Visible;
            Gamma_Text.Visibility = Visibility.Visible;
            Degree.Visibility = Visibility.Hidden;
            Degree_Text.Visibility = Visibility.Hidden;
            R.Visibility = Visibility.Hidden;
            R_Text.Visibility = Visibility.Hidden;

            //Column1.Visibility = Visibility.Visible;
            Column2.Visibility = Visibility.Visible;
            Column3.Visibility = Visibility.Collapsed;
            Column4.Visibility = Visibility.Collapsed;
            Column5.Visibility = Visibility.Collapsed;
            Column6.Visibility = Visibility.Collapsed;
            Column1Text.Visibility = Visibility.Visible;
            Column2Text.Visibility = Visibility.Visible;
            Column3Text.Visibility = Visibility.Collapsed;
            Column4Text.Visibility = Visibility.Collapsed;
            Column5Text.Visibility = Visibility.Collapsed;
            Column6Text.Visibility = Visibility.Collapsed;
            ColumnMinus.IsEnabled = true;
            ColumnPlus.IsEnabled = true;

            Has_Labels.IsChecked = true;
            Scale_Selected.IsChecked = true;
            TopToBottom.IsChecked = true;
            initialised = 1;

            Grid_Search_Contour_Image.Source = ImageSourceFromBitmap(Properties.Resources.Contour_Template);
            Grid_Search_Prediction_Image.Source = ImageSourceFromBitmap(Properties.Resources.Plot_Template);
            Plot_Image.Source = ImageSourceFromBitmap(Properties.Resources.Plot_Template);


            Console.WriteLine(Assembly_Location);

            Folder_File_Create();

            #endregion


            //Grant Access to Directory Folder
            DirectoryInfo dInfo = new DirectoryInfo(Saves_Directory.Text);
            DirectorySecurity dSecurity = dInfo.GetAccessControl();
            dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null),
                FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, 
                PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
            dInfo.SetAccessControl(dSecurity);

            

        }



        #region ComboBoxes

        public void SVM_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (initialised == 1)
            {
                if (SVM_ComboBox.SelectedIndex == 0)
                {
                    Ep.Visibility = Visibility.Visible;
                    Ep_Text.Visibility = Visibility.Visible;
                    Nu.Visibility = Visibility.Hidden;
                    Nu_Text.Visibility = Visibility.Hidden;
                }
                else
                {
                    Ep.Visibility = Visibility.Hidden;
                    Ep_Text.Visibility = Visibility.Hidden;
                    Nu.Visibility = Visibility.Visible;
                    Nu_Text.Visibility = Visibility.Visible;
                }
            }
        }

        private void Kernel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (initialised == 1)
            {
                if (Kernel_ComboBox.SelectedIndex == 0)
                {
                    Gamma.Visibility = Visibility.Hidden;
                    Gamma_Text.Visibility = Visibility.Hidden;
                    Degree.Visibility = Visibility.Hidden;
                    Degree_Text.Visibility = Visibility.Hidden;
                    R.Visibility = Visibility.Hidden;
                    R_Text.Visibility = Visibility.Hidden;
                }
                else if (Kernel_ComboBox.SelectedIndex == 1)
                {
                    Gamma.Visibility = Visibility.Visible;
                    Gamma_Text.Visibility = Visibility.Visible;
                    Degree.Visibility = Visibility.Visible;
                    Degree_Text.Visibility = Visibility.Visible;
                    R.Visibility = Visibility.Visible;
                    R_Text.Visibility = Visibility.Visible;
                }
                else if (Kernel_ComboBox.SelectedIndex == 2)
                {
                    Gamma.Visibility = Visibility.Visible;
                    Gamma_Text.Visibility = Visibility.Visible;
                    Degree.Visibility = Visibility.Hidden;
                    Degree_Text.Visibility = Visibility.Hidden;
                    R.Visibility = Visibility.Hidden;
                    R_Text.Visibility = Visibility.Hidden;
                }
                else
                {
                    Gamma.Visibility = Visibility.Visible;
                    Gamma_Text.Visibility = Visibility.Visible;
                    Degree.Visibility = Visibility.Hidden;
                    Degree_Text.Visibility = Visibility.Hidden;
                    R.Visibility = Visibility.Visible;
                    R_Text.Visibility = Visibility.Visible;
                }
            }
        }
        private void Accuracy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        #endregion


        #region Grid Search CheckBoxes 

        public void Parameter_Checked(object sender, RoutedEventArgs e)
        {
            OptimiseValue = 1;
            Feature_Selected.IsChecked = false;
            //Optimise_None.IsChecked = false;
        }

        private void Feature_Checked(object sender, RoutedEventArgs e)
        {
            OptimiseValue = 2;
            Parameter_Selected.IsChecked = false;
            //Optimise_None.IsChecked = false;
        }

        #endregion  

        public void Saves_Directory_Button_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new FolderPicker();
            if (dlg.ShowFolderDialog(IntPtr.Zero) == true)
            { Saves_Directory.Text = dlg.ResultPath; }
            Saves_Directory.ScrollToHorizontalOffset(double.PositiveInfinity);
        }

        public void Data_File_Source_Button_Click(object sender, RoutedEventArgs e)
        {
            FilePicker.CSV(Data_File_Source);
            Data_File_Source.ScrollToHorizontalOffset(double.PositiveInfinity);
        }

        public void ColumnMinus_Click(object sender, RoutedEventArgs e)
        {
            ColumnIndex -= 1;
            if (ColumnIndex == 1)
            {
                //Column1.Visibility = Visibility.Visible;
                Column2.Visibility = Visibility.Collapsed;
                Column3.Visibility = Visibility.Collapsed;
                Column4.Visibility = Visibility.Collapsed;
                Column5.Visibility = Visibility.Collapsed;
                Column6.Visibility = Visibility.Collapsed;
                Column1Text.Visibility = Visibility.Visible;
                Column2Text.Visibility = Visibility.Collapsed;
                Column3Text.Visibility = Visibility.Collapsed;
                Column4Text.Visibility = Visibility.Collapsed;
                Column5Text.Visibility = Visibility.Collapsed;
                Column6Text.Visibility = Visibility.Collapsed;
                ColumnMinus.IsEnabled = false;
                ColumnPlus.IsEnabled = true;
            }
            if (ColumnIndex == 2)
            {
                //Column1.Visibility = Visibility.Visible;
                Column2.Visibility = Visibility.Visible;
                Column3.Visibility = Visibility.Collapsed;
                Column4.Visibility = Visibility.Collapsed;
                Column5.Visibility = Visibility.Collapsed;
                Column6.Visibility = Visibility.Collapsed;
                Column1Text.Visibility = Visibility.Visible;
                Column2Text.Visibility = Visibility.Visible;
                Column3Text.Visibility = Visibility.Collapsed;
                Column4Text.Visibility = Visibility.Collapsed;
                Column5Text.Visibility = Visibility.Collapsed;
                Column6Text.Visibility = Visibility.Collapsed;
                ColumnMinus.IsEnabled = true;
                ColumnPlus.IsEnabled = true;
            }
            if (ColumnIndex == 3)
            {
                //Column1.Visibility = Visibility.Visible;
                Column2.Visibility = Visibility.Visible;
                Column3.Visibility = Visibility.Visible;
                Column4.Visibility = Visibility.Collapsed;
                Column5.Visibility = Visibility.Collapsed;
                Column6.Visibility = Visibility.Collapsed;
                Column1Text.Visibility = Visibility.Visible;
                Column2Text.Visibility = Visibility.Visible;
                Column3Text.Visibility = Visibility.Visible;
                Column4Text.Visibility = Visibility.Collapsed;
                Column5Text.Visibility = Visibility.Collapsed;
                Column6Text.Visibility = Visibility.Collapsed;
                ColumnMinus.IsEnabled = true;
                ColumnPlus.IsEnabled = true;
            }
            if (ColumnIndex == 4)
            {
                //Column1.Visibility = Visibility.Visible;
                Column2.Visibility = Visibility.Visible;
                Column3.Visibility = Visibility.Visible;
                Column4.Visibility = Visibility.Visible;
                Column5.Visibility = Visibility.Collapsed;
                Column6.Visibility = Visibility.Collapsed;
                Column1Text.Visibility = Visibility.Visible;
                Column2Text.Visibility = Visibility.Visible;
                Column3Text.Visibility = Visibility.Visible;
                Column4Text.Visibility = Visibility.Visible;
                Column5Text.Visibility = Visibility.Collapsed;
                Column6Text.Visibility = Visibility.Collapsed;
                ColumnMinus.IsEnabled = true;
                ColumnPlus.IsEnabled = true;
            }
            if (ColumnIndex == 5)
            {
                //Column1.Visibility = Visibility.Visible;
                Column2.Visibility = Visibility.Visible;
                Column3.Visibility = Visibility.Visible;
                Column4.Visibility = Visibility.Visible;
                Column5.Visibility = Visibility.Visible;
                Column6.Visibility = Visibility.Collapsed;
                Column1Text.Visibility = Visibility.Visible;
                Column2Text.Visibility = Visibility.Visible;
                Column3Text.Visibility = Visibility.Visible;
                Column4Text.Visibility = Visibility.Visible;
                Column5Text.Visibility = Visibility.Visible;
                Column6Text.Visibility = Visibility.Collapsed;
                ColumnMinus.IsEnabled = true;
                ColumnPlus.IsEnabled = true;
            }
            if (ColumnIndex == 6)
            {
                //Column1.Visibility = Visibility.Visible;
                Column2.Visibility = Visibility.Visible;
                Column3.Visibility = Visibility.Visible;
                Column4.Visibility = Visibility.Visible;
                Column5.Visibility = Visibility.Visible;
                Column6.Visibility = Visibility.Visible;
                Column1Text.Visibility = Visibility.Visible;
                Column2Text.Visibility = Visibility.Visible;
                Column3Text.Visibility = Visibility.Visible;
                Column4Text.Visibility = Visibility.Visible;
                Column5Text.Visibility = Visibility.Visible;
                Column6Text.Visibility = Visibility.Visible;
                ColumnMinus.IsEnabled = true;
                ColumnPlus.IsEnabled = false;
            }
            UpdateLayout();
        }

        public void ColumnPlus_Click(object sender, RoutedEventArgs e)
        {
            ColumnIndex += 1;
            if (ColumnIndex == 1)
            {
                //Column1.Visibility = Visibility.Visible;
                Column2.Visibility = Visibility.Collapsed;
                Column3.Visibility = Visibility.Collapsed;
                Column4.Visibility = Visibility.Collapsed;
                Column5.Visibility = Visibility.Collapsed;
                Column6.Visibility = Visibility.Collapsed;
                Column1Text.Visibility = Visibility.Visible;
                Column2Text.Visibility = Visibility.Collapsed; 
                Column3Text.Visibility = Visibility.Collapsed; 
                Column4Text.Visibility = Visibility.Collapsed; 
                Column5Text.Visibility = Visibility.Collapsed; 
                Column6Text.Visibility = Visibility.Collapsed;
                ColumnMinus.IsEnabled = false;
                ColumnPlus.IsEnabled = true;
            }
            if (ColumnIndex == 2)
            {
                //Column1.Visibility = Visibility.Visible;
                Column2.Visibility = Visibility.Visible;
                Column3.Visibility = Visibility.Collapsed;
                Column4.Visibility = Visibility.Collapsed;
                Column5.Visibility = Visibility.Collapsed;
                Column6.Visibility = Visibility.Collapsed;
                Column1Text.Visibility = Visibility.Visible;
                Column2Text.Visibility = Visibility.Visible;
                Column3Text.Visibility = Visibility.Collapsed;
                Column4Text.Visibility = Visibility.Collapsed;
                Column5Text.Visibility = Visibility.Collapsed;
                Column6Text.Visibility = Visibility.Collapsed;
                ColumnMinus.IsEnabled = true;
                ColumnPlus.IsEnabled = true;
            }
            if (ColumnIndex == 3)
            {
                //Column1.Visibility = Visibility.Visible;
                Column2.Visibility = Visibility.Visible;
                Column3.Visibility = Visibility.Visible;
                Column4.Visibility = Visibility.Collapsed;
                Column5.Visibility = Visibility.Collapsed;
                Column6.Visibility = Visibility.Collapsed;
                Column1Text.Visibility = Visibility.Visible;
                Column2Text.Visibility = Visibility.Visible;
                Column3Text.Visibility = Visibility.Visible;
                Column4Text.Visibility = Visibility.Collapsed;
                Column5Text.Visibility = Visibility.Collapsed;
                Column6Text.Visibility = Visibility.Collapsed;
                ColumnMinus.IsEnabled = true;
                ColumnPlus.IsEnabled = true;
            }
            if (ColumnIndex == 4)
            {
                //Column1.Visibility = Visibility.Visible;
                Column2.Visibility = Visibility.Visible;
                Column3.Visibility = Visibility.Visible;
                Column4.Visibility = Visibility.Visible;
                Column5.Visibility = Visibility.Collapsed;
                Column6.Visibility = Visibility.Collapsed;
                Column1Text.Visibility = Visibility.Visible;
                Column2Text.Visibility = Visibility.Visible;
                Column3Text.Visibility = Visibility.Visible;
                Column4Text.Visibility = Visibility.Visible;
                Column5Text.Visibility = Visibility.Collapsed;
                Column6Text.Visibility = Visibility.Collapsed;
                ColumnMinus.IsEnabled = true;
                ColumnPlus.IsEnabled = true;
            }
            if (ColumnIndex == 5)
            {
                //Column1.Visibility = Visibility.Visible;
                Column2.Visibility = Visibility.Visible;
                Column3.Visibility = Visibility.Visible;
                Column4.Visibility = Visibility.Visible;
                Column5.Visibility = Visibility.Visible;
                Column6.Visibility = Visibility.Collapsed;
                Column1Text.Visibility = Visibility.Visible;
                Column2Text.Visibility = Visibility.Visible;
                Column3Text.Visibility = Visibility.Visible;
                Column4Text.Visibility = Visibility.Visible;
                Column5Text.Visibility = Visibility.Visible;
                Column6Text.Visibility = Visibility.Collapsed;
                ColumnMinus.IsEnabled = true;
                ColumnPlus.IsEnabled = true;
            }
            if (ColumnIndex == 6)
            {
                //Column1.Visibility = Visibility.Visible;
                Column2.Visibility = Visibility.Visible;
                Column3.Visibility = Visibility.Visible;
                Column4.Visibility = Visibility.Visible;
                Column5.Visibility = Visibility.Visible;
                Column6.Visibility = Visibility.Visible;
                Column1Text.Visibility = Visibility.Visible;
                Column2Text.Visibility = Visibility.Visible;
                Column3Text.Visibility = Visibility.Visible;
                Column4Text.Visibility = Visibility.Visible;
                Column5Text.Visibility = Visibility.Visible;
                Column6Text.Visibility = Visibility.Visible;
                ColumnMinus.IsEnabled = true;
                ColumnPlus.IsEnabled = false;
            }
            UpdateLayout();
        }

        public async void Convert_Button_Click(object sender, RoutedEventArgs e)
        {
            #region Prerequesite Checks

            // Checks if Convert is already running
            if (ConvertRunning == 1)
            {
                System.Windows.Forms.MessageBox.Show("Conversion in progress, please wait");
            }

            // Check if the input data and save directory folder exist
            else if (File.Exists(Helper.AddExtension(Data_File_Source.Text, ".csv")) == false & Directory.Exists(Saves_Directory.Text) == false)
            {
                System.Windows.Forms.MessageBox.Show("Please select a valid saves directory and data to convert");
            }
            else if (File.Exists(Helper.AddExtension(Data_File_Source.Text, ".csv")) == false)
            {
                System.Windows.Forms.MessageBox.Show("Please select valid data to convert");
            }
            else if (Directory.Exists(Saves_Directory.Text) == false)
            {
                System.Windows.Forms.MessageBox.Show("Please select a valid saves directory");
            }
            else
            {

            #endregion

                ConvertRunning = 1;
                ColorAnimation animation;
                animation = new ColorAnimation();
                animation.From = Colors.Silver;
                animation.To = Colors.Thistle;
                animation.Duration = new Duration(TimeSpan.FromSeconds(1.5));
                animation.RepeatBehavior = RepeatBehavior.Forever;
                animation.AutoReverse = true;
                Convert_Button.Background = new SolidColorBrush(Colors.DarkTurquoise);
                Convert_Button.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);

                Convert_Button_text.Text = "Converting";

                // Create Saves folder if it doesn't exist
                Directory.CreateDirectory(Saves_Directory.Text + "\\Saves");

                // Exports Directories
                #region Export Convert Directories

                if (ColumnIndex == 1)
                {
                    C1Export = Column1Text.Text;
                    C2Export = "0";
                    C3Export = "0";
                    C4Export = "0";
                    C5Export = "0";
                    C6Export = "0";
                }
                if (ColumnIndex == 2)
                {
                    C1Export = Column1Text.Text;
                    C2Export = Column2Text.Text;
                    C3Export = "0";
                    C4Export = "0";
                    C5Export = "0";
                    C6Export = "0";
                }
                if (ColumnIndex == 3)
                {
                    C1Export = Column1Text.Text;
                    C2Export = Column2Text.Text;
                    C3Export = Column3Text.Text;
                    C4Export = "0";
                    C5Export = "0";
                    C6Export = "0";
                }
                if (ColumnIndex == 4)
                {
                    C1Export = Column1Text.Text;
                    C2Export = Column2Text.Text;
                    C3Export = Column3Text.Text;
                    C4Export = Column4Text.Text;
                    C5Export = "0";
                    C6Export = "0";
                }
                if (ColumnIndex == 5)
                {
                    C1Export = Column1Text.Text;
                    C2Export = Column2Text.Text;
                    C3Export = Column3Text.Text;
                    C4Export = Column4Text.Text;
                    C5Export = Column5Text.Text;
                    C6Export = "0";
                }
                if (ColumnIndex == 6)
                {
                    C1Export = Column1Text.Text;
                    C2Export = Column2Text.Text;
                    C3Export = Column3Text.Text;
                    C4Export = Column4Text.Text;
                    C5Export = Column5Text.Text;
                    C6Export = Column6Text.Text;
                }

                if (Date_Column.Text == null || Date_Column.Text == "" || Date_Column.Text == " ")
                { DateExport = "0"; }
                else
                { DateExport = Date_Column.Text; }

                string DirectoriesPath = Assembly_Location + "\\Required_Files\\Settings\\Directories_Convert.txt";
                string[] DirectoriesLines =
                {
                Saves_Directory.Text,   //1
                File_Name.Text,         //2
                Data_File_Source.Text,  //3
                (Convert.ToInt32(TopToBottom.IsChecked)).ToString(), //1
                ColumnIndex.ToString(), //2
                DateExport,     //3
                C1Export,       //4
                C2Export,       //5
                C3Export,       //6
                C4Export,       //7
                C5Export,       //8
                C6Export,       //9
                (Convert.ToInt32(Has_Labels.IsChecked)).ToString(),   //10
                (Convert.ToInt32(Scale_Selected.IsChecked)).ToString(), //11

            };
                File.WriteAllLines(DirectoriesPath, DirectoriesLines);

                #endregion

                // Run the DataManagement Program
                await Helper.RunDatamanagement(Running, Assembly_Location + "\\Required_Files\\");

                //Update Prepared File Location
                Prepared_File_Location.Text = Saves_Directory.Text + "\\Saves\\" + File_Name.Text + "_Data_Converted";
                Prepared_File_Location.ScrollToHorizontalOffset(double.PositiveInfinity);

                ConvertRunning = 0;
                //Convert_gif1.Visibility = Visibility.Hidden;
                Convert_Button.Background = new SolidColorBrush(Colors.DarkTurquoise);
                Convert_Button_text.Text = "Convert to LIBSVM-Ready File";
            }
        }

        public void Converted_To_Split_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Prepared_File_Location.Text))
            {
                System.Windows.Forms.MessageBox.Show("No file to send");
            }
            else
            {
                Data_Split_Location.Text = Prepared_File_Location.Text;
                Data_Split_Location.ScrollToHorizontalOffset(double.PositiveInfinity);
            }
        }

        public void Converted_To_Train_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Prepared_File_Location.Text))
            {
                System.Windows.Forms.MessageBox.Show("No file to send");
            }
            else
            {
                Training_Data_Location.Text = Prepared_File_Location.Text;
                Training_Data_Location.ScrollToHorizontalOffset(double.PositiveInfinity);
            }
        }

        public void Converted_To_Test_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Prepared_File_Location.Text))
            {
                System.Windows.Forms.MessageBox.Show("No file to send");
            }
            else
            {
                Testing_Data_Location.Text = Prepared_File_Location.Text;
                Testing_Data_Location.ScrollToHorizontalOffset(double.PositiveInfinity);
            }
        }

        public void Data_To_Split_Button_Click(object sender, RoutedEventArgs e)
        {
            FilePicker.MATLAB(Data_Split_Location);
            Data_Split_Location.ScrollToHorizontalOffset(double.PositiveInfinity);
        }

        public async void Split_Button_Click(object sender, RoutedEventArgs e)
        {
            #region Prerequesite Checks

            // Checks if Split is already running
            if (SplitRunning == 1)
            {
                System.Windows.Forms.MessageBox.Show("Split in progress, please wait");
            }

            // Check if the split data and save directory folder exist
            else if (File.Exists(Helper.AddExtension(Data_Split_Location.Text, ".mat")) == false & Directory.Exists(Saves_Directory.Text) == false)
            {
                System.Windows.Forms.MessageBox.Show("Please select a valid saves directory and data to split");
            }
            else if (File.Exists(Helper.AddExtension(Data_Split_Location.Text, ".mat")) == false)
            {
                System.Windows.Forms.MessageBox.Show("Please select valid data to split");
            }
            else if (Directory.Exists(Saves_Directory.Text) == false)
            {
                System.Windows.Forms.MessageBox.Show("Please select a valid saves directory");
            }
            else
            {

            #endregion

                SplitRunning = 1;
                ColorAnimation animation;
                animation = new ColorAnimation();
                animation.From = Colors.Silver;
                animation.To = Colors.Thistle;
                animation.Duration = new Duration(TimeSpan.FromSeconds(1.5));
                animation.RepeatBehavior = RepeatBehavior.Forever;
                animation.AutoReverse = true;
                Split_Button.Background = new SolidColorBrush(Colors.DarkTurquoise);
                Split_Button.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                Split_Button_text.Text = "Splitting";

                // Create Saves folder if it doesn't exist
                Directory.CreateDirectory(Saves_Directory.Text + "\\Saves");

                // Exports Directories
                #region Export Split Settings

                string DirectoriesPath = Assembly_Location + "\\Required_Files\\Settings\\Settings_Split.txt";
                string[] DirectoriesLines =
                {
                    Saves_Directory.Text,       // 1
                    File_Name.Text,             // 2
                    Data_Split_Location.Text,   // 3
                    Percentage_Split.Text,      // 1
                };
                File.WriteAllLines(DirectoriesPath, DirectoriesLines);

                #endregion

                // Run Split Program
                await Helper.RunSplit(Running, Assembly_Location + "\\Required_Files\\");

                //Update Portion File Locations
                First_Portion_Location.Text = Saves_Directory.Text + "\\Saves\\" + File_Name.Text + "_Data_First_Portion";
                Second_Portion_Location.Text = Saves_Directory.Text + "\\Saves\\" + File_Name.Text + "_Data_Second_Portion";
                First_Portion_Location.ScrollToHorizontalOffset(double.PositiveInfinity);
                Second_Portion_Location.ScrollToHorizontalOffset(double.PositiveInfinity);

                SplitRunning = 0;
                Split_Button.Background = new SolidColorBrush(Colors.DarkTurquoise);
                Split_Button_text.Text = "Split";
            }
        }

        public void First_To_Training_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(First_Portion_Location.Text))
            {
                System.Windows.Forms.MessageBox.Show("No file to send");
            }
            else
            {
                Training_Data_Location.Text = First_Portion_Location.Text;
                Training_Data_Location.ScrollToHorizontalOffset(double.PositiveInfinity);
            }
        }

        public void First_To_Testing_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(First_Portion_Location.Text))
            {
                System.Windows.Forms.MessageBox.Show("No file to send");
            }
            else
            {
                Testing_Data_Location.Text = First_Portion_Location.Text;
                Testing_Data_Location.ScrollToHorizontalOffset(double.PositiveInfinity);
            }
        }

        public void Second_To_Training_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Second_Portion_Location.Text))
            {
                System.Windows.Forms.MessageBox.Show("No file to send");
            }
            else
            {
                Training_Data_Location.Text = Second_Portion_Location.Text;
                Training_Data_Location.ScrollToHorizontalOffset(double.PositiveInfinity);
            }
        }

        public void Second_To_Testing_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Second_Portion_Location.Text))
            {
                System.Windows.Forms.MessageBox.Show("No file to send");
            }
            else
            {
                Testing_Data_Location.Text = Second_Portion_Location.Text;
                Testing_Data_Location.ScrollToHorizontalOffset(double.PositiveInfinity);
            }
        }

        public void AttributeMinus_Click(object sender, RoutedEventArgs e)
        {
            AttributeIndex -= 1;

            IfAttributeIndexAll();

            if (Link_CheckBox.IsChecked == true)
            {
                TAttributeMinus_Click(sender, e);
            }

            UpdateLayout();
        }

        public void AttributePlus_Click(object sender, RoutedEventArgs e)
        {
            AttributeIndex += 1;

            IfAttributeIndexAll();

            if (Link_CheckBox.IsChecked == true)
            {
                TAttributePlus_Click(sender, e);
            }

            UpdateLayout();
        }

        public void Parameter_Selection_Button_Click(object sender, RoutedEventArgs e)
        {
            Gamma_Text.Text = G_Result_Text.Text;
            Cost_Text.Text = C_Result_Text.Text;
        }

        public void Feature_Selection_Button_Click(object sender, RoutedEventArgs e)
        {
            Attribute1Text.Text = Att1_Result_Text.Text;
            Attribute2Text.Text = Att2_Result_Text.Text;
        }

        public async void Run_Search_Button_Click(object sender, RoutedEventArgs e)
        {
            #region Prerequesit Checks

            // Sets nfoldint to 1 if n-fold value is an integer
            int nfoldint = 0;
            if (int.TryParse(n_fold_Text.Text, out int value))
            {
                nfoldint = 1;
            }

            // Checks if Grid Search is already running
            if (GridSearchRunning == 1)
            {
                System.Windows.Forms.MessageBox.Show("Grid Search in progress, please wait");
            }

            // Check if the training data and save directory folder exist
            else if (File.Exists(Helper.AddExtension(Training_Data_Location.Text, ".mat")) == false & Directory.Exists(Saves_Directory.Text) == false)
            {
                System.Windows.Forms.MessageBox.Show("Please select a valid saves directory and training data");
            }
            else if (File.Exists(Helper.AddExtension(Training_Data_Location.Text, ".mat")) == false)
            {
                System.Windows.Forms.MessageBox.Show("Please select valid training data");
            }
            else if (Directory.Exists(Saves_Directory.Text) == false)
            {
                System.Windows.Forms.MessageBox.Show("Please select a valid saves directory");
            }

            // Check if None is selected, if so say "Select a Grid Search method"
            else if (Feature_Selected.IsChecked == false & Parameter_Selected.IsChecked == false)
            {
                System.Windows.Forms.MessageBox.Show("Select a Grid Search method");
            }

            // Check if n-fold value is less than two or not an integer
            else if (nfoldint == 0)
            {
                System.Windows.Forms.MessageBox.Show("n-fold value must be an integer greater than 1");
            }
            else if (float.Parse(n_fold_Text.Text) < 2)
            {
                System.Windows.Forms.MessageBox.Show("n-fold value must be an integer greater than 1");
            }

            // Checks if the upper values aren't greater than the lower values by at least one step
            else if (Parameter_Selected.IsChecked == true & (float.Parse(GUpper_Text.Text) - float.Parse(GLower_Text.Text) < float.Parse(GStep_Text.Text)) 
                & (float.Parse(CUpper_Text.Text) - float.Parse(CLower_Text.Text) < float.Parse(CStep_Text.Text)))
            {
                System.Windows.Forms.MessageBox.Show("C and G upper values must be greater than their lower values by at least one step respectively");
            }
            else if (Parameter_Selected.IsChecked == true & (float.Parse(CUpper_Text.Text) - float.Parse(CLower_Text.Text) < float.Parse(CStep_Text.Text)))
            {
                System.Windows.Forms.MessageBox.Show("C upper value must be greater than the lower value by at least one step");
            }
            else if (Parameter_Selected.IsChecked == true & (float.Parse(GUpper_Text.Text) - float.Parse(GLower_Text.Text) < float.Parse(GStep_Text.Text)))
            {
                System.Windows.Forms.MessageBox.Show("G upper value must be greater than the lower value by at least one step");
            }
            else if (Feature_Selected.IsChecked == true & (float.Parse(Att1Upper_Text.Text) - float.Parse(Att1Lower_Text.Text) < float.Parse(Att1Step_Text.Text)) & 
                (float.Parse(Att2Upper_Text.Text) - float.Parse(Att2Lower_Text.Text) < float.Parse(Att2Step_Text.Text)))
            {
                System.Windows.Forms.MessageBox.Show("Attribute upper values must be greater than their lower values by at least one step respectively");
            }
            else if (Feature_Selected.IsChecked == true & (float.Parse(Att1Upper_Text.Text) - float.Parse(Att1Lower_Text.Text) < float.Parse(Att1Step_Text.Text)))
            {
                System.Windows.Forms.MessageBox.Show("Attribute 1 upper value must be greater than the lower value by at least one step");
            }
            else if (Feature_Selected.IsChecked == true & (float.Parse(Att2Upper_Text.Text) - float.Parse(Att2Lower_Text.Text) < float.Parse(Att2Step_Text.Text)))
            {
                System.Windows.Forms.MessageBox.Show("Attribute 2 upper value must be greater than the lower value by at least one step");
            }
                #endregion

            else
            {
                GridSearchRunning = 1;
                ColorAnimation animation;
                animation = new ColorAnimation();
                animation.From = Colors.Silver;
                animation.To = Colors.Thistle;
                animation.Duration = new Duration(TimeSpan.FromSeconds(1.5));
                animation.RepeatBehavior = RepeatBehavior.Forever;
                animation.AutoReverse = true;
                Run_Search_Button.Background = new SolidColorBrush(Colors.DarkTurquoise);
                Run_Search_Button.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                Run_Search_text.Text = "Searching";

                // Create Plots folder if it doesn't exist
                Directory.CreateDirectory(Saves_Directory.Text + "\\Plots");

                // Export Settings
                #region Export Settings

                if (AttributeIndex == 1)
                {
                    A1Export = Attribute1Text.Text;
                    A2Export = "0";
                    A3Export = "0";
                    A4Export = "0";
                    A5Export = "0";
                    A6Export = "0";
                }
                if (AttributeIndex == 2)
                {
                    A1Export = Attribute1Text.Text;
                    A2Export = Attribute2Text.Text;
                    A3Export = "0";
                    A4Export = "0";
                    A5Export = "0";
                    A6Export = "0";
                }
                if (AttributeIndex == 3)
                {
                    A1Export = Attribute1Text.Text;
                    A2Export = Attribute2Text.Text;
                    A3Export = Attribute3Text.Text;
                    A4Export = "0";
                    A5Export = "0";
                    A6Export = "0";
                }
                if (AttributeIndex == 4)
                {
                    A1Export = Attribute1Text.Text;
                    A2Export = Attribute2Text.Text;
                    A3Export = Attribute3Text.Text;
                    A4Export = Attribute4Text.Text;
                    A5Export = "0";
                    A6Export = "0";
                }
                if (AttributeIndex == 5)
                {
                    A1Export = Attribute1Text.Text;
                    A2Export = Attribute2Text.Text;
                    A3Export = Attribute3Text.Text;
                    A4Export = Attribute4Text.Text;
                    A5Export = Attribute5Text.Text;
                    A6Export = "0";
                }
                if (AttributeIndex == 6)
                {
                    A1Export = Attribute1Text.Text;
                    A2Export = Attribute2Text.Text;
                    A3Export = Attribute3Text.Text;
                    A4Export = Attribute4Text.Text;
                    A5Export = Attribute5Text.Text;
                    A6Export = Attribute6Text.Text;
                }

                if (Length_Text.Text == null | Length_Text.Text == "")
                { Length_Text_Export = "0"; }
                else { Length_Text_Export = Length_Text.Text; }

                File.Delete(Assembly_Location + "\\Required_Files\\Settings\\Settings_Text.txt");
                string SettingsPath = Assembly_Location + "\\Required_Files\\Settings\\Settings_Text.txt";
                string[] SettingsLines =
                {
                    "Start- 01, " + Start_Text.Text + " ,",
                    "Length 02, " + Length_Text_Export + " ,",
                    "AccTyp 03, " + Accuracy_ComboBox.SelectedIndex + " ,",
                    "Att1Ft 04, " + A1Export + " ,",
                    "Att2Ft 05, " + A2Export + " ,",
                    "Att3Ft 06, " + A3Export + " ,",
                    "Att4Ft 07, " + A4Export + " ,",
                    "Att5Ft 08, " + A5Export + " ,",
                    "Att6Ft 09, " + A6Export + " ,",
                    "AttInx 10, " + AttributeIndex + " ,",
                    "",
                    "SVMTyp 11, " + SVM_ComboBox.SelectedIndex + " ,",
                    "Kernel 12, " + Kernel_ComboBox.SelectedIndex + " ,",
                    "Gamma- 13, " + Gamma_Text.Text + " ,",
                    "Degree 14, " + Degree_Text.Text + " ,",
                    "R----- 15, " + R_Text.Text + " ,",
                    "Cost-- 16, " + Cost_Text.Text + " ,",
                    "Nu---- 17, " + Nu_Text.Text + " ,",
                    "Ep---- 18, " + Ep_Text.Text + " ,",
                    "Ee---- 19, " + Ee_Text.Text + " ,",
                    "",
                    "Select 20, " + OptimiseValue + " ,", // OptimiseValue for Search, 3 for Train
                    "CLower 21, " + CLower_Text.Text + " ,",
                    "CUpper 22, " + CUpper_Text.Text + " ,",
                    "C_Step 23, " + CStep_Text.Text + " ,",
                    "GLower 24, " + GLower_Text.Text + " ,",
                    "GUpper 25, " + GUpper_Text.Text + " ,",
                    "G_Step 26, " + GStep_Text.Text + " ,",
                    "At1Low 27, " + Att1Lower_Text.Text + " ,",
                    "At1Upp 28, " + Att1Upper_Text.Text + " ,",
                    "At1Stp 29, " + Att1Step_Text.Text + " ,",
                    "At2Low 30, " + Att2Lower_Text.Text + " ,",
                    "At2Upp 31, " + Att2Upper_Text.Text + " ,",
                    "At2Stp 32, " + Att2Step_Text.Text + " ,",
                    "nfold- 33, " + n_fold_Text.Text + " ,",
                    "Scale- 34, " + Convert.ToInt32(Scale_Selected.IsChecked) + " ,",
                    "Projec 35, " + ProjectionTrain.Text + " ,",
                    "Step-- 36, " + Step_Text.Text + " ,",
                    "Cache- 37, " + Cache_text.Text + " ,",
                    "Shrink 38, " + Convert.ToInt32(Shrinking_Selected.IsChecked) + " ,",
                    "Split- 39, " + Split_Preference.Text + " ,",

                };
                File.WriteAllLines(SettingsPath, SettingsLines);

            #endregion

                // Export Grid Search Directories
                #region Export Search And Traing Directories

                string DirectoriesPath = Assembly_Location + "\\Required_Files\\Settings\\Directories_SearchAndTraining.txt";
                string[] DirectoriesLines =
                {
                    Saves_Directory.Text,
                    File_Name.Text,
                    Training_Data_Location.Text,
                };
                File.WriteAllLines(DirectoriesPath, DirectoriesLines);

                #endregion

                // Run the Program
                Console.WriteLine("ID OF MAINWINDOW: " + Process.GetCurrentProcess().Id.ToString());
                await Helper.RunGridSearch(Running, Assembly_Location + "\\Required_Files\\", this);

                // Update Image for Grid Search and Best Prediction
                #region Display Contour and Prediction Plots

                Grid_Search_Contour_Image.Source = null;
                Grid_Search_Contour_Image.UpdateLayout();
                GC.Collect();

                if (File.Exists(Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Grid_Search_Contour.jpg") == true)
                {
                    File.Delete(Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Grid_Search_Contour.jpg"); 
                }
                if (File.Exists(Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Grid_Search_Contour_cache.jpg") == true)
                {
                    File.Move(Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Grid_Search_Contour_cache.jpg", Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Grid_Search_Contour.jpg");
                    string Plot_Contour_Location = Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Grid_Search_Contour.jpg";
                    BitmapImage bitmap_Contour = new BitmapImage();
                    bitmap_Contour.BeginInit();
                    bitmap_Contour.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap_Contour.UriSource = new Uri(Plot_Contour_Location);
                    bitmap_Contour.EndInit();
                    Grid_Search_Contour_Image.Source = bitmap_Contour;
                    bitmap_Contour.UriSource = null;
                    GC.Collect();
                }

                Grid_Search_Prediction_Image.Source = null;
                Grid_Search_Prediction_Image.UpdateLayout();
                GC.Collect();

                if (File.Exists(Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Grid_Search_Prediction.jpg") == true)
                {
                    File.Delete(Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Grid_Search_Prediction.jpg");
                }
                if (File.Exists(Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Grid_Search_Prediction_cache.jpg") == true)
                {
                    File.Move(Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Grid_Search_Prediction_cache.jpg", Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Grid_Search_Prediction.jpg");
                    string Plot_Prediction_Location = Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Grid_Search_Prediction.jpg";
                    BitmapImage bitmap_Prediction = new BitmapImage();
                    bitmap_Prediction.BeginInit();
                    bitmap_Prediction.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap_Prediction.UriSource = new Uri(Plot_Prediction_Location);
                    bitmap_Prediction.EndInit();
                    Grid_Search_Prediction_Image.Source = bitmap_Prediction;
                    bitmap_Prediction.UriSource = null;
                    GC.Collect();

                #endregion

                // Update Grid Search Results
                #region Update Grid Search Results

                    if (Parameter_Selected.IsChecked == true)
                {
                    using (TextReader reader = File.OpenText(Assembly_Location + "\\Required_Files\\MatlabOutputText\\Best_CandG"))
                    {
                        C_Result_Text.Text = Helper.Normalize(decimal.Parse(reader.ReadLine())).ToString();
                        G_Result_Text.Text = Helper.Normalize(decimal.Parse(reader.ReadLine())).ToString();
                    }
                    //string Best_CandG = System.IO.File.ReadAllText(Saves_Directory.Text + "\\MatlabOutputText\\" + "Best_CandG");
                    //C_Result_Text.Text = Helper.Normalize( ToDecimal(Helper.ReadLine(Best_CandG, 1));
                    //G_Result_Text.Text = Helper.ReadLine(Best_CandG, 2);
                }
                else if (Feature_Selected.IsChecked == true)
                {
                    using (TextReader reader = File.OpenText(Assembly_Location + "\\Required_Files\\MatlabOutputText\\Best_Features"))
                    {
                        Att1_Result_Text.Text = Helper.Normalize(decimal.Parse(reader.ReadLine())).ToString();
                        Att2_Result_Text.Text = Helper.Normalize(decimal.Parse(reader.ReadLine())).ToString();
                    }
                }

                #endregion

                // Update Best Prediction Accuracy
                #region Update Best Prediction Accuracy

                string Accuracy_Lines = System.IO.File.ReadAllText(Assembly_Location + "\\Required_Files\\MatlabOutputText\\Grid_Search_Accuracy");
                GridSearch_MSE_text.Text = Helper.ReadLine(Accuracy_Lines, 2);
                GridSearch_SCC_text.Text = Helper.ReadLine(Accuracy_Lines, 3);

                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Error");
                }

                #endregion

                GridSearchRunning = 0;
                Run_Search_Button.Background = new SolidColorBrush(Colors.DarkTurquoise);
                Run_Search_text.Text = "Run Search";
            }
        }

        public void Training_Data_Button_Click(object sender, RoutedEventArgs e)
        {
            FilePicker.MATLAB(Training_Data_Location);
            Training_Data_Location.ScrollToHorizontalOffset(double.PositiveInfinity);
        }

        public async void Train_Model_Button_Click(object sender, RoutedEventArgs e)
        {
            #region Prerequesite Checks
            // Check if training is already running
            if (TrainRunning == 1)
            {
                System.Windows.Forms.MessageBox.Show("Training in progress, please wait");
            }

            // Check if the training data and save directory folder exist
            else if (File.Exists(Helper.AddExtension(Training_Data_Location.Text, ".mat")) == false & Directory.Exists(Saves_Directory.Text) == false)
            {
                System.Windows.Forms.MessageBox.Show("Please select a valid saves directory and training data");
            }
            else if (File.Exists(Helper.AddExtension(Training_Data_Location.Text, ".mat")) == false)
            {
                System.Windows.Forms.MessageBox.Show("Please select valid training data");
            }
            else if (Directory.Exists(Saves_Directory.Text) == false)
            {
                System.Windows.Forms.MessageBox.Show("Please select a valid saves directory");
            }
            else
            {

            #endregion

                TrainRunning = 1;
                ColorAnimation animation;
                animation = new ColorAnimation();
                animation.From = Colors.Silver;
                animation.To = Colors.Thistle;
                animation.Duration = new Duration(TimeSpan.FromSeconds(1.5));
                animation.RepeatBehavior = RepeatBehavior.Forever;
                animation.AutoReverse = true;
                Train_Model_Button.Background = new SolidColorBrush(Colors.DarkTurquoise);
                Train_Model_Button.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                Train_Model_text.Text = "Training";

                // Create Saves folder if it doesn't exist
                Directory.CreateDirectory(Saves_Directory.Text + "\\Saves");

                // Export Settings
                #region Export Settings

                if (AttributeIndex == 1)
                {
                    A1Export = Attribute1Text.Text;
                    A2Export = "0";
                    A3Export = "0";
                    A4Export = "0";
                    A5Export = "0";
                    A6Export = "0";
                }
                if (AttributeIndex == 2)
                {
                    A1Export = Attribute1Text.Text;
                    A2Export = Attribute2Text.Text;
                    A3Export = "0";
                    A4Export = "0";
                    A5Export = "0";
                    A6Export = "0";
                }
                if (AttributeIndex == 3)
                {
                    A1Export = Attribute1Text.Text;
                    A2Export = Attribute2Text.Text;
                    A3Export = Attribute3Text.Text;
                    A4Export = "0";
                    A5Export = "0";
                    A6Export = "0";
                }
                if (AttributeIndex == 4)
                {
                    A1Export = Attribute1Text.Text;
                    A2Export = Attribute2Text.Text;
                    A3Export = Attribute3Text.Text;
                    A4Export = Attribute4Text.Text;
                    A5Export = "0";
                    A6Export = "0";
                }
                if (AttributeIndex == 5)
                {
                    A1Export = Attribute1Text.Text;
                    A2Export = Attribute2Text.Text;
                    A3Export = Attribute3Text.Text;
                    A4Export = Attribute4Text.Text;
                    A5Export = Attribute5Text.Text;
                    A6Export = "0";
                }
                if (AttributeIndex == 6)
                {
                    A1Export = Attribute1Text.Text;
                    A2Export = Attribute2Text.Text;
                    A3Export = Attribute3Text.Text;
                    A4Export = Attribute4Text.Text;
                    A5Export = Attribute5Text.Text;
                    A6Export = Attribute6Text.Text;
                }

                if (Length_Text.Text == null | Length_Text.Text == "")
                { Length_Text_Export = "0"; }
                else { Length_Text_Export = Length_Text.Text; }

                File.Delete(Assembly_Location + "\\Required_Files\\Settings\\Settings_Text.txt");
                string SettingsPath = Assembly_Location + "\\Required_Files\\Settings\\Settings_Text.txt";
                string[] SettingsLines =
                {
                    "Start- 01, " + Start_Text.Text + " ,",
                    "Length 02, " + Length_Text_Export + " ,",
                    "AccTyp 03, " + Accuracy_ComboBox.SelectedIndex + " ,",
                    "Att1Ft 04, " + A1Export + " ,",
                    "Att2Ft 05, " + A2Export + " ,",
                    "Att3Ft 06, " + A3Export + " ,",
                    "Att4Ft 07, " + A4Export + " ,",
                    "Att5Ft 08, " + A5Export + " ,",
                    "Att6Ft 09, " + A6Export + " ,",
                    "AttInx 10, " + AttributeIndex + " ,",
                    "",
                    "SVMTyp 11, " + SVM_ComboBox.SelectedIndex + " ,",
                    "Kernel 12, " + Kernel_ComboBox.SelectedIndex + " ,",
                    "Gamma- 13, " + Gamma_Text.Text + " ,",
                    "Degree 14, " + Degree_Text.Text + " ,",
                    "R----- 15, " + R_Text.Text + " ,",
                    "Cost-- 16, " + Cost_Text.Text + " ,",
                    "Nu---- 17, " + Nu_Text.Text + " ,",
                    "Ep---- 18, " + Ep_Text.Text + " ,",
                    "Ee---- 19, " + Ee_Text.Text + " ,",
                    "",
                    "Select 20, " + OptimiseValue + " ,", // OptimiseValue for Search, 3 for Train
                    "CLower 21, " + CLower_Text.Text + " ,",
                    "CUpper 22, " + CUpper_Text.Text + " ,",
                    "C_Step 23, " + CStep_Text.Text + " ,",
                    "GLower 24, " + GLower_Text.Text + " ,",
                    "GUpper 25, " + GUpper_Text.Text + " ,",
                    "G_Step 26, " + GStep_Text.Text + " ,",
                    "At1Low 27, " + Att1Lower_Text.Text + " ,",
                    "At1Upp 28, " + Att1Upper_Text.Text + " ,",
                    "At1Stp 29, " + Att1Step_Text.Text + " ,",
                    "At2Low 30, " + Att2Lower_Text.Text + " ,",
                    "At2Upp 31, " + Att2Upper_Text.Text + " ,",
                    "At2Stp 32, " + Att2Step_Text.Text + " ,",
                    "nfold- 33, " + n_fold_Text.Text + " ,",
                    "Scale- 34, " + Convert.ToInt32(Scale_Selected.IsChecked) + " ,",
                    "Projec 35, " + ProjectionTrain.Text + " ,",
                    "Step-- 36, " + Step_Text.Text + " ,",
                    "cache- 37, " + Cache_text.Text + " ,",
                    "Shrink 38, " + Convert.ToInt32(Shrinking_Selected.IsChecked) + " ,",
                    "Split- 39, " + Split_Preference.Text + " ,",
                 };
                File.WriteAllLines(SettingsPath, SettingsLines);
                #endregion

                // Export Training Directories
                #region Export Search And Training Directories

                string DirectoriesPath = Assembly_Location + "\\Required_Files\\Settings\\Directories_SearchAndTraining.txt";
                string[] DirectoriesLines =
                {
                        Saves_Directory.Text,
                        File_Name.Text,
                        Training_Data_Location.Text,
                    };
                File.WriteAllLines(DirectoriesPath, DirectoriesLines);

                #endregion

                // Run the Program
                await Helper.RunTraining(Running, Assembly_Location + "\\Required_Files\\");

                // Update the Model Stats
                #region Import Model Stats

                string ModelStats_Lines = System.IO.File.ReadAllText(Assembly_Location + "\\Required_Files\\MatlabOutputText\\Model_Stats");
                iter_text.Text = String.Concat(Helper.ReadLine(ModelStats_Lines, 2).Where(c => !Char.IsWhiteSpace(c))).Split('=').Last();
                nu_text.Text = Decimal.Round(Decimal.Parse(String.Concat(Helper.ReadLine(ModelStats_Lines, 3).Where(c => !Char.IsWhiteSpace(c))).Split('=').Last()), 3, MidpointRounding.AwayFromZero).ToString();
                obj_text.Text = Decimal.Round(Decimal.Parse(Helper.UntilComma(String.Concat(Helper.ReadLine(ModelStats_Lines, 4).Where(c => !Char.IsWhiteSpace(c)))).Split('=').Last()), 3, MidpointRounding.AwayFromZero).ToString();
                rho_text.Text = Decimal.Round(Decimal.Parse(String.Concat(Helper.ReadLine(ModelStats_Lines, 4).Where(c => !Char.IsWhiteSpace(c))).Split(',').Last().Split('=').Last()), 3, MidpointRounding.AwayFromZero).ToString();
                nSV_text.Text = Helper.UntilComma(String.Concat(Helper.ReadLine(ModelStats_Lines, 5).Where(c => !Char.IsWhiteSpace(c)))).Split('=').Last();
                nBSV_text.Text = String.Concat(Helper.ReadLine(ModelStats_Lines, 5).Where(c => !Char.IsWhiteSpace(c))).Split(',').Last().Split('=').Last();

                #endregion

                //Update Model Output location
                Model_Output_Location.Text = Saves_Directory.Text + "\\Saves\\" + File_Name.Text + "_Model";
                Model_Output_Location.ScrollToHorizontalOffset(double.PositiveInfinity);

                TrainRunning = 0;
                Train_Model_Button.Background = new SolidColorBrush(Colors.DarkTurquoise);
                Train_Model_text.Text = "Train Model";
            }
        }

        public void Send_To_Test_Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Model_Output_Location.Text))
            {
                System.Windows.Forms.MessageBox.Show("No file to send");
            }
            else
            {
                Model_File_Location.Text = Model_Output_Location.Text;
                Model_File_Location.ScrollToHorizontalOffset(double.PositiveInfinity);
            }
        }

        public void TAttributeMinus_Click(object sender, RoutedEventArgs e)
        {
            if (Link_CheckBox.IsChecked == true)
            {
                TAttributeIndex = AttributeIndex;
            }
            else
            {
                TAttributeIndex -= 1;
                IfTAttributeIndexPlusMinusIsEnabled();
            }
            IfTAttributeIndexVisibility();

            UpdateLayout();
        }

        public void TAttributePlus_Click(object sender, RoutedEventArgs e)
        {
            if (Link_CheckBox.IsChecked == true)
            {
                TAttributeIndex = AttributeIndex;
            }
            else
            {
                TAttributeIndex += 1;
                IfTAttributeIndexPlusMinusIsEnabled();
            }

            IfTAttributeIndexVisibility();

            UpdateLayout();
        }

        public void Testing_Data_Button_Click(object sender, RoutedEventArgs e)
        {
            FilePicker.MATLAB(Testing_Data_Location);
            Testing_Data_Location.ScrollToHorizontalOffset(double.PositiveInfinity);
        }

        public void Model_Location_Button_Click(object sender, RoutedEventArgs e)
        {
            FilePicker.MATLAB(Model_File_Location);
            Model_File_Location.ScrollToHorizontalOffset(double.PositiveInfinity);
        }

        public async void Test_Model_Button_Click(object sender, RoutedEventArgs e)
        {
            #region Prerequesite Checks

            // Checks if Split is already running
            if (TestRunning == 1)
            {
                System.Windows.Forms.MessageBox.Show("Testing in progress, please wait");
            }

            // Check if the testing and training data and save directory folder exist
            else if (File.Exists(Helper.AddExtension(Testing_Data_Location.Text, ".mat")) == false & Directory.Exists(Saves_Directory.Text) == false)
            {
                System.Windows.Forms.MessageBox.Show("Please select a valid saves directory, testing data, and model file");
            }
            else if (Directory.Exists(Saves_Directory.Text) == false)
            {
                System.Windows.Forms.MessageBox.Show("Please select a valid saves directory");
            }
            else if (File.Exists(Helper.AddExtension(Testing_Data_Location.Text, ".mat")) == false)
            {
                System.Windows.Forms.MessageBox.Show("Please select valid testing data");
            }
            else if (File.Exists(Helper.AddExtension(Model_File_Location.Text, ".mat")) == false)
            {
                System.Windows.Forms.MessageBox.Show("Please select a valid model file");
            }

            #endregion

            else
            {
                TestRunning = 1;
                ColorAnimation animation;
                animation = new ColorAnimation();
                animation.From = Colors.Silver;
                animation.To = Colors.Thistle;
                animation.Duration = new Duration(TimeSpan.FromSeconds(1.5));
                animation.RepeatBehavior = RepeatBehavior.Forever;
                animation.AutoReverse = true;
                Test_Model_Button.Background = new SolidColorBrush(Colors.DarkTurquoise);
                Test_Model_Button.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                Test_Model_text.Text = "Testing";

                // Create Saves and Plots folders if they don't exist
                Directory.CreateDirectory(Saves_Directory.Text + "\\Saves");
                Directory.CreateDirectory(Saves_Directory.Text + "\\Plots");

                // Update directories file with: Saves location, File Name, Testing Data Location, Model File Location
                #region Directories Export Testing

                    string DirectoriesPath = Assembly_Location + "\\Required_Files\\Settings\\Directories_Export_Testing.txt";
                    string[] DirectoriesLines =
                    {
                    Saves_Directory.Text,
                    File_Name.Text,
                    Testing_Data_Location.Text,
                    Model_File_Location.Text,
                };
                    File.WriteAllLines(DirectoriesPath, DirectoriesLines);

                    #endregion

                // Export Testing Settings
                #region Export Testing Settings

                    if (TAttributeIndex == 1)
                    {
                        TA1Export = TAttribute1.Text;
                        TA2Export = "0";
                        TA3Export = "0";
                        TA4Export = "0";
                        TA5Export = "0";
                        TA6Export = "0";
                    }
                    if (TAttributeIndex == 2)
                    {
                        TA1Export = TAttribute1Text.Text;
                        TA2Export = TAttribute2Text.Text;
                        TA3Export = "0";
                        TA4Export = "0";
                        TA5Export = "0";
                        TA6Export = "0";
                    }
                    if (TAttributeIndex == 3)
                    {
                        TA1Export = TAttribute1Text.Text;
                        TA2Export = TAttribute2Text.Text;
                        TA3Export = TAttribute3Text.Text;
                        TA4Export = "0";
                        TA5Export = "0";
                        TA6Export = "0";
                    }
                    if (TAttributeIndex == 4)
                    {
                        TA1Export = TAttribute1Text.Text;
                        TA2Export = TAttribute2Text.Text;
                        TA3Export = TAttribute3Text.Text;
                        TA4Export = TAttribute4Text.Text;
                        TA5Export = "0";
                        TA6Export = "0";
                    }
                    if (TAttributeIndex == 5)
                    {
                        TA1Export = TAttribute1Text.Text;
                        TA2Export = TAttribute2Text.Text;
                        TA3Export = TAttribute3Text.Text;
                        TA4Export = TAttribute4Text.Text;
                        TA5Export = TAttribute5Text.Text;
                        TA6Export = "0";
                    }
                    if (TAttributeIndex == 6)
                    {
                        TA1Export = TAttribute1Text.Text;
                        TA2Export = TAttribute2Text.Text;
                        TA3Export = TAttribute3Text.Text;
                        TA4Export = TAttribute4Text.Text;
                        TA5Export = TAttribute5Text.Text;
                        TA6Export = TAttribute6Text.Text;
                    }

                    if (TLength_Text.Text == null | TLength_Text.Text == "")
                    { TLength_Text_Export = "0"; }
                    else { TLength_Text_Export = TLength_Text.Text; }

                    File.Delete(Assembly_Location + "\\Required_Files\\Settings\\Settings_Testing.txt");
                    string SettingsPath = Assembly_Location + "\\Required_Files\\Settings\\Settings_Testing.txt";
                    string[] SettingsLines =
                    {
                    "Start- 01, " + TStart_Text.Text + " ,",
                    "Length 02, " + TLength_Text_Export + " ,",
                    "Projec 03, " + ProjectionTest.Text + " ,",
                    "Att1Ft 04, " + TA1Export + " ,",
                    "Att2Ft 05, " + TA2Export + " ,",
                    "Att3Ft 06, " + TA3Export + " ,",
                    "Att4Ft 07, " + TA4Export + " ,",
                    "Att5Ft 08, " + TA5Export + " ,",
                    "Att6Ft 09, " + TA6Export + " ,",
                    "AttInx 10, " + TAttributeIndex + " ,",
                    "Step-- 11, " + TStep_Text.Text + " ,",
                    "Output 12, " + File_Type.SelectedIndex + " ,",
                };
                    File.WriteAllLines(SettingsPath, SettingsLines);

                    #endregion

                // Run the Program
                await Helper.RunTesting(Running, Assembly_Location + "\\Required_Files\\");

                // Update the prediction file location
                Prediction_File_Location.Text = Saves_Directory.Text + "\\Saves\\" + File_Name.Text + "_Prediction";
                Prediction_File_Location.ScrollToHorizontalOffset(double.PositiveInfinity);

                // Update the image for prediction
                #region Display Plot

                    Plot_Image.Source = null;
                    Plot_Image.UpdateLayout();
                    GC.Collect();

                    if (File.Exists(Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Plot.jpg") == true)
                    {
                        File.Delete(Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Plot.jpg");
                    }
                    if (File.Exists(Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Plot_cache.jpg") == true)
                    {
                        File.Move(Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Plot_cache.jpg", Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Plot.jpg");
                        string Plot_Location = Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Plot.jpg";
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.UriSource = new Uri(Plot_Location);
                        bitmap.EndInit();
                        Plot_Image.Source = bitmap;
                        bitmap.UriSource = null;
                        GC.Collect();

                        #endregion

                    // Update the accuracy text
                    #region Import Accuracy Stats

                    string Accuracy_Lines = System.IO.File.ReadAllText(Assembly_Location + "\\Required_Files\\MatlabOutputText\\" + "Accuracy");
                    MSE_text.Text = Helper.ReadLine(Accuracy_Lines, 2);
                    SCC_text.Text = Helper.ReadLine(Accuracy_Lines, 3);

                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Error");
                }

                #endregion

                TestRunning = 0;
                Test_Model_Button.Background = new SolidColorBrush(Colors.DarkTurquoise);
                Test_Model_text.Text = "Test Model";
            }
        }

        public async void Train_and_Test_Button_Click(object sender, RoutedEventArgs e)
        {

            #region Prerequesite Checks
            // Check if training or testing are already running
            if (TrainRunning == 1)
            {
                System.Windows.Forms.MessageBox.Show("Training in progress, please wait");
            }
            else if (TestRunning == 1)
            {
                System.Windows.Forms.MessageBox.Show("Testing in progress, please wait");
            }

            // Check if the testing and training data and save directory folder exist
            else if (File.Exists(Helper.AddExtension(Testing_Data_Location.Text, ".mat")) == false & Directory.Exists(Saves_Directory.Text) == false)
            {
                System.Windows.Forms.MessageBox.Show("Please select a valid saves directory, testing data, and model file");
            }
            else if (File.Exists(Helper.AddExtension(Testing_Data_Location.Text, ".mat")) == false)
            {
                System.Windows.Forms.MessageBox.Show("Please select valid testing data");
            }
            else if (File.Exists(Helper.AddExtension(Model_File_Location.Text, ".mat")) == false)
            {
                System.Windows.Forms.MessageBox.Show("Please select a valid model file");
            }
            else if (Directory.Exists(Saves_Directory.Text) == false)
            {
                System.Windows.Forms.MessageBox.Show("Please select a valid saves directory");
            }
            else
            {

            #endregion

                #region Training 

                TrainRunning = 1;
                TrainRunning = 1;
                ColorAnimation animation;
                animation = new ColorAnimation();
                animation.From = Colors.Silver;
                animation.To = Colors.Thistle;
                animation.Duration = new Duration(TimeSpan.FromSeconds(1.5));
                animation.RepeatBehavior = RepeatBehavior.Forever;
                animation.AutoReverse = true;
                Train_Model_Button.Background = new SolidColorBrush(Colors.DarkTurquoise);
                Train_Model_Button.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                Train_Model_text.Text = "Training";

                // Create Saves folder if it doesn't exist
                Directory.CreateDirectory(Saves_Directory.Text + "\\Saves");

                // Export Settings
                #region Export Settings

                if (AttributeIndex == 1)
                {
                    A1Export = Attribute1Text.Text;
                    A2Export = "0";
                    A3Export = "0";
                    A4Export = "0";
                    A5Export = "0";
                    A6Export = "0";
                }
                if (AttributeIndex == 2)
                {
                    A1Export = Attribute1Text.Text;
                    A2Export = Attribute2Text.Text;
                    A3Export = "0";
                    A4Export = "0";
                    A5Export = "0";
                    A6Export = "0";
                }
                if (AttributeIndex == 3)
                {
                    A1Export = Attribute1Text.Text;
                    A2Export = Attribute2Text.Text;
                    A3Export = Attribute3Text.Text;
                    A4Export = "0";
                    A5Export = "0";
                    A6Export = "0";
                }
                if (AttributeIndex == 4)
                {
                    A1Export = Attribute1Text.Text;
                    A2Export = Attribute2Text.Text;
                    A3Export = Attribute3Text.Text;
                    A4Export = Attribute4Text.Text;
                    A5Export = "0";
                    A6Export = "0";
                }
                if (AttributeIndex == 5)
                {
                    A1Export = Attribute1Text.Text;
                    A2Export = Attribute2Text.Text;
                    A3Export = Attribute3Text.Text;
                    A4Export = Attribute4Text.Text;
                    A5Export = Attribute5Text.Text;
                    A6Export = "0";
                }
                if (AttributeIndex == 6)
                {
                    A1Export = Attribute1Text.Text;
                    A2Export = Attribute2Text.Text;
                    A3Export = Attribute3Text.Text;
                    A4Export = Attribute4Text.Text;
                    A5Export = Attribute5Text.Text;
                    A6Export = Attribute6Text.Text;
                }

                if (Length_Text.Text == null | Length_Text.Text == "")
                { Length_Text_Export = "0"; }
                else { Length_Text_Export = Length_Text.Text; }

                File.Delete(Assembly_Location + "\\Required_Files\\Settings\\Settings_Text.txt");
                string SettingsPathTraining = Assembly_Location + "\\Required_Files\\Settings\\Settings_Text.txt";
                string[] SettingsLinesTraining =
                {
                    "Start- 01, " + Start_Text.Text + " ,",
                    "Length 02, " + Length_Text_Export + " ,",
                    "AccTyp 03, " + Accuracy_ComboBox.SelectedIndex + " ,",
                    "Att1Ft 04, " + A1Export + " ,",
                    "Att2Ft 05, " + A2Export + " ,",
                    "Att3Ft 06, " + A3Export + " ,",
                    "Att4Ft 07, " + A4Export + " ,",
                    "Att5Ft 08, " + A5Export + " ,",
                    "Att6Ft 09, " + A6Export + " ,",
                    "AttInx 10, " + AttributeIndex + " ,",
                    "",
                    "SVMTyp 11, " + SVM_ComboBox.SelectedIndex + " ,",
                    "Kernel 12, " + Kernel_ComboBox.SelectedIndex + " ,",
                    "Gamma- 13, " + Gamma_Text.Text + " ,",
                    "Degree 14, " + Degree_Text.Text + " ,",
                    "R----- 15, " + R_Text.Text + " ,",
                    "Cost-- 16, " + Cost_Text.Text + " ,",
                    "Nu---- 17, " + Nu_Text.Text + " ,",
                    "Ep---- 18, " + Ep_Text.Text + " ,",
                    "Ee---- 19, " + Ee_Text.Text + " ,",
                    "",
                    "Select 20, " + OptimiseValue + " ,", // OptimiseValue for Search, 3 for Train
                    "CLower 21, " + CLower_Text.Text + " ,",
                    "CUpper 22, " + CUpper_Text.Text + " ,",
                    "C_Step 23, " + CStep_Text.Text + " ,",
                    "GLower 24, " + GLower_Text.Text + " ,",
                    "GUpper 25, " + GUpper_Text.Text + " ,",
                    "G_Step 26, " + GStep_Text.Text + " ,",
                    "At1Low 27, " + Att1Lower_Text.Text + " ,",
                    "At1Upp 28, " + Att1Upper_Text.Text + " ,",
                    "At1Stp 29, " + Att1Step_Text.Text + " ,",
                    "At2Low 30, " + Att2Lower_Text.Text + " ,",
                    "At2Upp 31, " + Att2Upper_Text.Text + " ,",
                    "At2Stp 32, " + Att2Step_Text.Text + " ,",
                    "nfold- 33, " + n_fold_Text.Text + " ,",
                    "Scale- 34, " + Convert.ToInt32(Scale_Selected.IsChecked) + " ,",
                    "Projec 35, " + ProjectionTrain.Text + " ,",
                    "Step-- 36, " + Step_Text.Text + " ,",
                    "cache- 37, " + Cache_text.Text + " ,",
                    "Shrink 38, " + Convert.ToInt32(Shrinking_Selected.IsChecked) + " ,",
                    "Split- 39, " + Split_Preference.Text + " ,",
                 };
                File.WriteAllLines(SettingsPathTraining, SettingsLinesTraining);
                #endregion

                // Export Training Directories
                #region Export Search And Training Directories

                string DirectoriesPathTraining = Assembly_Location + "\\Required_Files\\Settings\\Directories_SearchAndTraining.txt";
                string[] DirectoriesLinesTraining =
                {
                        Saves_Directory.Text,
                        File_Name.Text,
                        Training_Data_Location.Text,
                    };
                File.WriteAllLines(DirectoriesPathTraining, DirectoriesLinesTraining);

                #endregion

                // Run the Program
                await Helper.RunTraining(Running, Assembly_Location + "\\Required_Files\\");

                // Update the Model Stats
                #region Import Model Stats

                string ModelStats_Lines = System.IO.File.ReadAllText(Assembly_Location + "\\Required_Files\\MatlabOutputText\\Model_Stats");
                iter_text.Text = String.Concat(Helper.ReadLine(ModelStats_Lines, 2).Where(c => !Char.IsWhiteSpace(c))).Split('=').Last();
                nu_text.Text = Decimal.Round(Decimal.Parse(String.Concat(Helper.ReadLine(ModelStats_Lines, 3).Where(c => !Char.IsWhiteSpace(c))).Split('=').Last()), 3, MidpointRounding.AwayFromZero).ToString();
                obj_text.Text = Decimal.Round(Decimal.Parse(Helper.UntilComma(String.Concat(Helper.ReadLine(ModelStats_Lines, 4).Where(c => !Char.IsWhiteSpace(c)))).Split('=').Last()), 3, MidpointRounding.AwayFromZero).ToString();
                rho_text.Text = Decimal.Round(Decimal.Parse(String.Concat(Helper.ReadLine(ModelStats_Lines, 4).Where(c => !Char.IsWhiteSpace(c))).Split(',').Last().Split('=').Last()), 3, MidpointRounding.AwayFromZero).ToString();
                nSV_text.Text = Helper.UntilComma(String.Concat(Helper.ReadLine(ModelStats_Lines, 5).Where(c => !Char.IsWhiteSpace(c)))).Split('=').Last();
                nBSV_text.Text = String.Concat(Helper.ReadLine(ModelStats_Lines, 5).Where(c => !Char.IsWhiteSpace(c))).Split(',').Last().Split('=').Last();

                #endregion

                //Update Model Output location
                Model_Output_Location.Text = Saves_Directory.Text + "\\Saves\\" + File_Name.Text + "_Model";
                Model_Output_Location.ScrollToHorizontalOffset(double.PositiveInfinity);

                TrainRunning = 0;
                Train_Model_Button.Background = new SolidColorBrush(Colors.DarkTurquoise);
                Train_Model_text.Text = "Train Model";


                Model_File_Location.Text = Saves_Directory.Text + "\\Saves\\" + File_Name.Text + "_Model";
                Model_File_Location.ScrollToHorizontalOffset(double.PositiveInfinity);

                #endregion

                #region Testing

                TestRunning = 1;
                Test_Model_Button.Background = new SolidColorBrush(Colors.DarkTurquoise);
                Test_Model_Button.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                Test_Model_text.Text = "Testing";

                // Create Saves and Plots folders if they don't exist
                Directory.CreateDirectory(Saves_Directory.Text + "\\Saves");
                Directory.CreateDirectory(Saves_Directory.Text + "\\Plots");

                // Update directories file with: Saves location, File Name, Testing Data Location, Model File Location
                #region Directories Export Testing

                string DirectoriesPathTesting = Assembly_Location + "\\Required_Files\\Settings\\Directories_Export_Testing.txt";
                string[] DirectoriesLinesTesting =
                {
                    Saves_Directory.Text,
                    File_Name.Text,
                    Testing_Data_Location.Text,
                    Model_File_Location.Text,
                };
                File.WriteAllLines(DirectoriesPathTesting, DirectoriesLinesTesting);

                #endregion

                // Export Testing Settings
                #region Export Testing Settings

                if (TAttributeIndex == 1)
                {
                    TA1Export = TAttribute1.Text;
                    TA2Export = "0";
                    TA3Export = "0";
                    TA4Export = "0";
                    TA5Export = "0";
                    TA6Export = "0";
                }
                if (TAttributeIndex == 2)
                {
                    TA1Export = TAttribute1Text.Text;
                    TA2Export = TAttribute2Text.Text;
                    TA3Export = "0";
                    TA4Export = "0";
                    TA5Export = "0";
                    TA6Export = "0";
                }
                if (TAttributeIndex == 3)
                {
                    TA1Export = TAttribute1Text.Text;
                    TA2Export = TAttribute2Text.Text;
                    TA3Export = TAttribute3Text.Text;
                    TA4Export = "0";
                    TA5Export = "0";
                    TA6Export = "0";
                }
                if (TAttributeIndex == 4)
                {
                    TA1Export = TAttribute1Text.Text;
                    TA2Export = TAttribute2Text.Text;
                    TA3Export = TAttribute3Text.Text;
                    TA4Export = TAttribute4Text.Text;
                    TA5Export = "0";
                    TA6Export = "0";
                }
                if (TAttributeIndex == 5)
                {
                    TA1Export = TAttribute1Text.Text;
                    TA2Export = TAttribute2Text.Text;
                    TA3Export = TAttribute3Text.Text;
                    TA4Export = TAttribute4Text.Text;
                    TA5Export = TAttribute5Text.Text;
                    TA6Export = "0";
                }
                if (TAttributeIndex == 6)
                {
                    TA1Export = TAttribute1Text.Text;
                    TA2Export = TAttribute2Text.Text;
                    TA3Export = TAttribute3Text.Text;
                    TA4Export = TAttribute4Text.Text;
                    TA5Export = TAttribute5Text.Text;
                    TA6Export = TAttribute6Text.Text;
                }

                if (TLength_Text.Text == null | TLength_Text.Text == "")
                { TLength_Text_Export = "0"; }
                else { TLength_Text_Export = TLength_Text.Text; }

                File.Delete(Assembly_Location + "\\Required_Files\\Settings\\Settings_Testing.txt");
                string SettingsPathTesting = Assembly_Location + "\\Required_Files\\Settings\\Settings_Testing.txt";
                string[] SettingsLinesTesting =
                {
                    "Start- 01, " + TStart_Text.Text + " ,",
                    "Length 02, " + TLength_Text_Export + " ,",
                    "Projec 03, " + ProjectionTest.Text + " ,",
                    "Att1Ft 04, " + TA1Export + " ,",
                    "Att2Ft 05, " + TA2Export + " ,",
                    "Att3Ft 06, " + TA3Export + " ,",
                    "Att4Ft 07, " + TA4Export + " ,",
                    "Att5Ft 08, " + TA5Export + " ,",
                    "Att6Ft 09, " + TA6Export + " ,",
                    "AttInx 10, " + TAttributeIndex + " ,",
                    "Step-- 11, " + TStep_Text.Text + " ,",
                    "Output 12, " + File_Type.SelectedIndex + " ,",
                };
                File.WriteAllLines(SettingsPathTesting, SettingsLinesTesting);

                #endregion

                // Run the Program
                await Helper.RunTesting(Running, Assembly_Location + "\\Required_Files\\");

                // Update the prediction file location
                Prediction_File_Location.Text = Saves_Directory.Text + "\\Saves\\" + File_Name.Text + "_Prediction";
                Prediction_File_Location.ScrollToHorizontalOffset(double.PositiveInfinity);

                // Update the image for prediction
                #region Display Plot

                Plot_Image.Source = null;
                Plot_Image.UpdateLayout();
                GC.Collect();

                if (File.Exists(Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Plot.jpg") == true)
                {
                    File.Delete(Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Plot.jpg");
                }
                if (File.Exists(Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Plot_cache.jpg") == true)
                {
                    File.Move(Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Plot_cache.jpg", Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Plot.jpg");
                    string Plot_Location = Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Plot.jpg";
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.UriSource = new Uri(Plot_Location);
                    bitmap.EndInit();
                    Plot_Image.Source = bitmap;
                    bitmap.UriSource = null;
                    GC.Collect();

                    #endregion

                    // Update the accuracy text
                    #region Import Accuracy Stats

                    string Accuracy_Lines = System.IO.File.ReadAllText(Assembly_Location + "\\Required_Files\\MatlabOutputText\\" + "Accuracy");
                    MSE_text.Text = Helper.ReadLine(Accuracy_Lines, 2);
                    SCC_text.Text = Helper.ReadLine(Accuracy_Lines, 3);

                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Error");
                }

                    #endregion

                TestRunning = 0;
                Test_Model_Button.Background = new SolidColorBrush(Colors.DarkTurquoise);
                Test_Model_text.Text = "Test Model";

                #endregion
            }
        }

        public void Link_CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            TA1Cache = TAttribute1Text.Text;
            TA2Cache = TAttribute2Text.Text;
            TA3Cache = TAttribute3Text.Text;
            TA4Cache = TAttribute4Text.Text;
            TA5Cache = TAttribute5Text.Text;
            TA6Cache = TAttribute6Text.Text;

            Features_Linkedobj = new Features_Linked
            {
                Train1Value = Attribute1Text.Text,
                Train2Value = Attribute2Text.Text,
                Train3Value = Attribute3Text.Text,
                Train4Value = Attribute4Text.Text,
                Train5Value = Attribute5Text.Text,
                Train6Value = Attribute6Text.Text,
                Test1Value = Attribute1Text.Text,
                Test2Value = Attribute2Text.Text,
                Test3Value = Attribute3Text.Text,
                Test4Value = Attribute4Text.Text,
                Test5Value = Attribute5Text.Text,
                Test6Value = Attribute6Text.Text,
            };

            Attribute1Text.DataContext = Features_Linkedobj;
            Attribute2Text.DataContext = Features_Linkedobj;
            Attribute3Text.DataContext = Features_Linkedobj;
            Attribute4Text.DataContext = Features_Linkedobj;
            Attribute5Text.DataContext = Features_Linkedobj;
            Attribute6Text.DataContext = Features_Linkedobj;
            TAttribute1Text.DataContext = Features_Linkedobj;
            TAttribute2Text.DataContext = Features_Linkedobj;
            TAttribute3Text.DataContext = Features_Linkedobj;
            TAttribute4Text.DataContext = Features_Linkedobj;
            TAttribute5Text.DataContext = Features_Linkedobj;
            TAttribute6Text.DataContext = Features_Linkedobj;

            TAttributeIndexCache = TAttributeIndex;
            TAttributeIndex = AttributeIndex;

            IfTAttributeIndexVisibility();

            TAttribute1Text.IsReadOnly = true;
            TAttribute2Text.IsReadOnly = true;
            TAttribute3Text.IsReadOnly = true;
            TAttribute4Text.IsReadOnly = true;
            TAttribute5Text.IsReadOnly = true;
            TAttribute6Text.IsReadOnly = true;
            TAttribute1Text.Background = System.Windows.Media.Brushes.WhiteSmoke;
            TAttribute2Text.Background = System.Windows.Media.Brushes.WhiteSmoke;
            TAttribute3Text.Background = System.Windows.Media.Brushes.WhiteSmoke;
            TAttribute4Text.Background = System.Windows.Media.Brushes.WhiteSmoke;
            TAttribute5Text.Background = System.Windows.Media.Brushes.WhiteSmoke;
            TAttribute6Text.Background = System.Windows.Media.Brushes.WhiteSmoke;
            TAttributeMinus.IsEnabled = false;
            TAttributePlus.IsEnabled = false;
            UpdateLayout();
        }

        private void Link_CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Featuresobj = new Features()
            {
                Train1Value = Attribute1Text.Text,
                Train2Value = Attribute2Text.Text,
                Train3Value = Attribute3Text.Text,
                Train4Value = Attribute4Text.Text,
                Train5Value = Attribute5Text.Text,
                Train6Value = Attribute6Text.Text,
                Test1Value = TA1Cache,
                Test2Value = TA2Cache,
                Test3Value = TA3Cache,
                Test4Value = TA4Cache,
                Test5Value = TA5Cache,
                Test6Value = TA6Cache,
            };

            Attribute1Text.DataContext = Featuresobj;
            Attribute2Text.DataContext = Featuresobj;
            Attribute3Text.DataContext = Featuresobj;
            Attribute4Text.DataContext = Featuresobj;
            Attribute5Text.DataContext = Featuresobj;
            Attribute6Text.DataContext = Featuresobj;
            TAttribute1Text.DataContext = Featuresobj;
            TAttribute2Text.DataContext = Featuresobj;
            TAttribute3Text.DataContext = Featuresobj;
            TAttribute4Text.DataContext = Featuresobj;
            TAttribute5Text.DataContext = Featuresobj;
            TAttribute6Text.DataContext = Featuresobj;

            TAttributeIndex = TAttributeIndexCache;

            if (TAttributeIndex == 1)
            {
                TAttribute1.Visibility = Visibility.Visible;
                TAttribute2.Visibility = Visibility.Collapsed;
                TAttribute3.Visibility = Visibility.Collapsed;
                TAttribute4.Visibility = Visibility.Collapsed;
                TAttribute5.Visibility = Visibility.Collapsed;
                TAttribute6.Visibility = Visibility.Collapsed;
                TAttribute1Text.Visibility = Visibility.Visible;
                TAttribute2Text.Visibility = Visibility.Collapsed;
                TAttribute3Text.Visibility = Visibility.Collapsed;
                TAttribute4Text.Visibility = Visibility.Collapsed;
                TAttribute5Text.Visibility = Visibility.Collapsed;
                TAttribute6Text.Visibility = Visibility.Collapsed;
                TAttributeMinus.IsEnabled = false;
                TAttributePlus.IsEnabled = true;
            }
            if (TAttributeIndex == 2)
            {
                TAttribute1.Visibility = Visibility.Visible;
                TAttribute2.Visibility = Visibility.Visible;
                TAttribute3.Visibility = Visibility.Collapsed;
                TAttribute4.Visibility = Visibility.Collapsed;
                TAttribute5.Visibility = Visibility.Collapsed;
                TAttribute6.Visibility = Visibility.Collapsed;
                TAttribute1Text.Visibility = Visibility.Visible;
                TAttribute2Text.Visibility = Visibility.Visible;
                TAttribute3Text.Visibility = Visibility.Collapsed;
                TAttribute4Text.Visibility = Visibility.Collapsed;
                TAttribute5Text.Visibility = Visibility.Collapsed;
                TAttribute6Text.Visibility = Visibility.Collapsed;
                TAttributeMinus.IsEnabled = true;
                TAttributePlus.IsEnabled = true;
            }
            if (TAttributeIndex == 3)
            {
                TAttribute1.Visibility = Visibility.Visible;
                TAttribute2.Visibility = Visibility.Visible;
                TAttribute3.Visibility = Visibility.Visible;
                TAttribute4.Visibility = Visibility.Collapsed;
                TAttribute5.Visibility = Visibility.Collapsed;
                TAttribute6.Visibility = Visibility.Collapsed;
                TAttribute1Text.Visibility = Visibility.Visible;
                TAttribute2Text.Visibility = Visibility.Visible;
                TAttribute3Text.Visibility = Visibility.Visible;
                TAttribute4Text.Visibility = Visibility.Collapsed;
                TAttribute5Text.Visibility = Visibility.Collapsed;
                TAttribute6Text.Visibility = Visibility.Collapsed;
                TAttributeMinus.IsEnabled = true;
                TAttributePlus.IsEnabled = true;
            }
            if (TAttributeIndex == 4)
            {
                TAttribute1.Visibility = Visibility.Visible;
                TAttribute2.Visibility = Visibility.Visible;
                TAttribute3.Visibility = Visibility.Visible;
                TAttribute4.Visibility = Visibility.Visible;
                TAttribute5.Visibility = Visibility.Collapsed;
                TAttribute6.Visibility = Visibility.Collapsed;
                TAttribute1Text.Visibility = Visibility.Visible;
                TAttribute2Text.Visibility = Visibility.Visible;
                TAttribute3Text.Visibility = Visibility.Visible;
                TAttribute4Text.Visibility = Visibility.Visible;
                TAttribute5Text.Visibility = Visibility.Collapsed;
                TAttribute6Text.Visibility = Visibility.Collapsed;
                TAttributeMinus.IsEnabled = true;
                TAttributePlus.IsEnabled = true;
            }
            if (TAttributeIndex == 5)
            {
                TAttribute1.Visibility = Visibility.Visible;
                TAttribute2.Visibility = Visibility.Visible;
                TAttribute3.Visibility = Visibility.Visible;
                TAttribute4.Visibility = Visibility.Visible;
                TAttribute5.Visibility = Visibility.Visible;
                TAttribute6.Visibility = Visibility.Collapsed;
                TAttribute1Text.Visibility = Visibility.Visible;
                TAttribute2Text.Visibility = Visibility.Visible;
                TAttribute3Text.Visibility = Visibility.Visible;
                TAttribute4Text.Visibility = Visibility.Visible;
                TAttribute5Text.Visibility = Visibility.Visible;
                TAttribute6Text.Visibility = Visibility.Collapsed;
                TAttributeMinus.IsEnabled = true;
                TAttributePlus.IsEnabled = true;
            }
            if (TAttributeIndex == 6)
            {
                TAttribute1.Visibility = Visibility.Visible;
                TAttribute2.Visibility = Visibility.Visible;
                TAttribute3.Visibility = Visibility.Visible;
                TAttribute4.Visibility = Visibility.Visible;
                TAttribute5.Visibility = Visibility.Visible;
                TAttribute6.Visibility = Visibility.Visible;
                TAttribute1Text.Visibility = Visibility.Visible;
                TAttribute2Text.Visibility = Visibility.Visible;
                TAttribute3Text.Visibility = Visibility.Visible;
                TAttribute4Text.Visibility = Visibility.Visible;
                TAttribute5Text.Visibility = Visibility.Visible;
                TAttribute6Text.Visibility = Visibility.Visible;
                TAttributeMinus.IsEnabled = true;
                TAttributePlus.IsEnabled = false;
            };

            TAttribute1Text.IsReadOnly = false;
            TAttribute2Text.IsReadOnly = false;
            TAttribute3Text.IsReadOnly = false;
            TAttribute4Text.IsReadOnly = false;
            TAttribute5Text.IsReadOnly = false;
            TAttribute6Text.IsReadOnly = false;
            TAttribute1Text.Background = System.Windows.Media.Brushes.White;
            TAttribute2Text.Background = System.Windows.Media.Brushes.White;
            TAttribute3Text.Background = System.Windows.Media.Brushes.White;
            TAttribute4Text.Background = System.Windows.Media.Brushes.White;
            TAttribute5Text.Background = System.Windows.Media.Brushes.White;
            TAttribute6Text.Background = System.Windows.Media.Brushes.White;

            UpdateLayout();
        }

        private void Features_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        private void Stop_Button_Click(object sender, RoutedEventArgs e)
        {
            Process.GetProcessById(MatlabPID).Kill();
            //SignalHandler(int SIGINT);
            //myProcess.StandardInput.Close();
            //Process p;
            //if (AttachConsole((uint)p.Id))
            //{
            //    SetConsoleCtrlHandler(null, true);
            //    try
            //    {
            //        if (!GenerateConsoleCtrlEvent(CTRL_C_EVENT, p.SessionId))
            //            return false;
            //        p.WaitForExit();
            //    }
            //    finally
            //    {
            //        SetConsoleCtrlHandler(null, false);
            //        FreeConsole();
            //    }
            //    return true;
            //}

        }

        #region On Loaded

        public void Training_Data_Location_Loaded(object sender, RoutedEventArgs e)
        {
            Training_Data_Location.ScrollToHorizontalOffset(double.PositiveInfinity);
            Training_Data_Location.UpdateLayout();
        }

        public void Testing_Data_Location_Loaded(object sender, RoutedEventArgs e)
        {
            Testing_Data_Location.ScrollToHorizontalOffset(double.PositiveInfinity);
            Testing_Data_Location.UpdateLayout();
        }

        private void Model_File_Location_Loaded(object sender, RoutedEventArgs e)
        {
            Model_File_Location.ScrollToHorizontalOffset(double.PositiveInfinity);
            Model_File_Location.UpdateLayout();
        }

        public void Folder_File_Create()
        {
            Directory.CreateDirectory(Assembly_Location + "\\Required_Files");
            Directory.CreateDirectory(Assembly_Location + "\\Required_Files\\Settings");
            Directory.CreateDirectory(Assembly_Location + "\\Required_Files\\MatlabOutputText");
            Helper.CreateEmptyFileIfNotThere(Assembly_Location + "\\Required_Files\\MatlabOutputText\\Accuracy.txt");
            Helper.CreateEmptyFileIfNotThere(Assembly_Location + "\\Required_Files\\MatlabOutputText\\Best_CandG.txt");
            Helper.CreateEmptyFileIfNotThere(Assembly_Location + "\\Required_Files\\MatlabOutputText\\Best_Features.txt");
            Helper.CreateEmptyFileIfNotThere(Assembly_Location + "\\Required_Files\\MatlabOutputText\\Feature_Cross.txt");
            Helper.CreateEmptyFileIfNotThere(Assembly_Location + "\\Required_Files\\MatlabOutputText\\Grid_Search_Accuracy.txt");
            Helper.CreateEmptyFileIfNotThere(Assembly_Location + "\\Required_Files\\MatlabOutputText\\Model_Stats.txt");
            Helper.CreateEmptyFileIfNotThere(Assembly_Location + "\\Required_Files\\MatlabOutputText\\Parameter_Cross.txt");
        }

        #endregion

        #region Inclass Helpers

        public void IfTAttributeIndexVisibility()
        {
            if (TAttributeIndex == 1)
            {
                TAttribute1.Visibility = Visibility.Visible;
                TAttribute2.Visibility = Visibility.Collapsed;
                TAttribute3.Visibility = Visibility.Collapsed;
                TAttribute4.Visibility = Visibility.Collapsed;
                TAttribute5.Visibility = Visibility.Collapsed;
                TAttribute6.Visibility = Visibility.Collapsed;
                TAttribute1Text.Visibility = Visibility.Visible;
                TAttribute2Text.Visibility = Visibility.Collapsed;
                TAttribute3Text.Visibility = Visibility.Collapsed;
                TAttribute4Text.Visibility = Visibility.Collapsed;
                TAttribute5Text.Visibility = Visibility.Collapsed;
                TAttribute6Text.Visibility = Visibility.Collapsed;
            }
            if (TAttributeIndex == 2)
            {
                TAttribute1.Visibility = Visibility.Visible;
                TAttribute2.Visibility = Visibility.Visible;
                TAttribute3.Visibility = Visibility.Collapsed;
                TAttribute4.Visibility = Visibility.Collapsed;
                TAttribute5.Visibility = Visibility.Collapsed;
                TAttribute6.Visibility = Visibility.Collapsed;
                TAttribute1Text.Visibility = Visibility.Visible;
                TAttribute2Text.Visibility = Visibility.Visible;
                TAttribute3Text.Visibility = Visibility.Collapsed;
                TAttribute4Text.Visibility = Visibility.Collapsed;
                TAttribute5Text.Visibility = Visibility.Collapsed;
                TAttribute6Text.Visibility = Visibility.Collapsed;
            }
            if (TAttributeIndex == 3)
            {
                TAttribute1.Visibility = Visibility.Visible;
                TAttribute2.Visibility = Visibility.Visible;
                TAttribute3.Visibility = Visibility.Visible;
                TAttribute4.Visibility = Visibility.Collapsed;
                TAttribute5.Visibility = Visibility.Collapsed;
                TAttribute6.Visibility = Visibility.Collapsed;
                TAttribute1Text.Visibility = Visibility.Visible;
                TAttribute2Text.Visibility = Visibility.Visible;
                TAttribute3Text.Visibility = Visibility.Visible;
                TAttribute4Text.Visibility = Visibility.Collapsed;
                TAttribute5Text.Visibility = Visibility.Collapsed;
                TAttribute6Text.Visibility = Visibility.Collapsed;
            }
            if (TAttributeIndex == 4)
            {
                TAttribute1.Visibility = Visibility.Visible;
                TAttribute2.Visibility = Visibility.Visible;
                TAttribute3.Visibility = Visibility.Visible;
                TAttribute4.Visibility = Visibility.Visible;
                TAttribute5.Visibility = Visibility.Collapsed;
                TAttribute6.Visibility = Visibility.Collapsed;
                TAttribute1Text.Visibility = Visibility.Visible;
                TAttribute2Text.Visibility = Visibility.Visible;
                TAttribute3Text.Visibility = Visibility.Visible;
                TAttribute4Text.Visibility = Visibility.Visible;
                TAttribute5Text.Visibility = Visibility.Collapsed;
                TAttribute6Text.Visibility = Visibility.Collapsed;
            }
            if (TAttributeIndex == 5)
            {
                TAttribute1.Visibility = Visibility.Visible;
                TAttribute2.Visibility = Visibility.Visible;
                TAttribute3.Visibility = Visibility.Visible;
                TAttribute4.Visibility = Visibility.Visible;
                TAttribute5.Visibility = Visibility.Visible;
                TAttribute6.Visibility = Visibility.Collapsed;
                TAttribute1Text.Visibility = Visibility.Visible;
                TAttribute2Text.Visibility = Visibility.Visible;
                TAttribute3Text.Visibility = Visibility.Visible;
                TAttribute4Text.Visibility = Visibility.Visible;
                TAttribute5Text.Visibility = Visibility.Visible;
                TAttribute6Text.Visibility = Visibility.Collapsed;
            }
            if (TAttributeIndex == 6)
            {
                TAttribute1.Visibility = Visibility.Visible;
                TAttribute2.Visibility = Visibility.Visible;
                TAttribute3.Visibility = Visibility.Visible;
                TAttribute4.Visibility = Visibility.Visible;
                TAttribute5.Visibility = Visibility.Visible;
                TAttribute6.Visibility = Visibility.Visible;
                TAttribute1Text.Visibility = Visibility.Visible;
                TAttribute2Text.Visibility = Visibility.Visible;
                TAttribute3Text.Visibility = Visibility.Visible;
                TAttribute4Text.Visibility = Visibility.Visible;
                TAttribute5Text.Visibility = Visibility.Visible;
                TAttribute6Text.Visibility = Visibility.Visible;
            };
        }

        public void IfTAttributeIndexPlusMinusIsEnabled()
        {
            if (TAttributeIndex == 1) { TAttributeMinus.IsEnabled = false; TAttributePlus.IsEnabled = true; }
            if (TAttributeIndex == 2) { TAttributeMinus.IsEnabled = true; TAttributePlus.IsEnabled = true; }
            if (TAttributeIndex == 3) { TAttributeMinus.IsEnabled = true; TAttributePlus.IsEnabled = true; }
            if (TAttributeIndex == 4) { TAttributeMinus.IsEnabled = true; TAttributePlus.IsEnabled = true; }
            if (TAttributeIndex == 5) { TAttributeMinus.IsEnabled = true; TAttributePlus.IsEnabled = true; }
            if (TAttributeIndex == 6) { TAttributeMinus.IsEnabled = true; TAttributePlus.IsEnabled = false; };

        }

        public void IfAttributeIndexAll()
        {
            if (AttributeIndex == 1)
            {
                Attribute1.Visibility = Visibility.Visible;
                Attribute2.Visibility = Visibility.Collapsed;
                Attribute3.Visibility = Visibility.Collapsed;
                Attribute4.Visibility = Visibility.Collapsed;
                Attribute5.Visibility = Visibility.Collapsed;
                Attribute6.Visibility = Visibility.Collapsed;
                Attribute1Text.Visibility = Visibility.Visible;
                Attribute2Text.Visibility = Visibility.Collapsed;
                Attribute3Text.Visibility = Visibility.Collapsed;
                Attribute4Text.Visibility = Visibility.Collapsed;
                Attribute5Text.Visibility = Visibility.Collapsed;
                Attribute6Text.Visibility = Visibility.Collapsed;
                AttributeMinus.IsEnabled = false;
                AttributePlus.IsEnabled = true;
            }
            if (AttributeIndex == 2)
            {
                Attribute1.Visibility = Visibility.Visible;
                Attribute2.Visibility = Visibility.Visible;
                Attribute3.Visibility = Visibility.Collapsed;
                Attribute4.Visibility = Visibility.Collapsed;
                Attribute5.Visibility = Visibility.Collapsed;
                Attribute6.Visibility = Visibility.Collapsed;
                Attribute1Text.Visibility = Visibility.Visible;
                Attribute2Text.Visibility = Visibility.Visible;
                Attribute3Text.Visibility = Visibility.Collapsed;
                Attribute4Text.Visibility = Visibility.Collapsed;
                Attribute5Text.Visibility = Visibility.Collapsed;
                Attribute6Text.Visibility = Visibility.Collapsed;
                AttributeMinus.IsEnabled = true;
                AttributePlus.IsEnabled = true;
            }
            if (AttributeIndex == 3)
            {
                Attribute1.Visibility = Visibility.Visible;
                Attribute2.Visibility = Visibility.Visible;
                Attribute3.Visibility = Visibility.Visible;
                Attribute4.Visibility = Visibility.Collapsed;
                Attribute5.Visibility = Visibility.Collapsed;
                Attribute6.Visibility = Visibility.Collapsed;
                Attribute1Text.Visibility = Visibility.Visible;
                Attribute2Text.Visibility = Visibility.Visible;
                Attribute3Text.Visibility = Visibility.Visible;
                Attribute4Text.Visibility = Visibility.Collapsed;
                Attribute5Text.Visibility = Visibility.Collapsed;
                Attribute6Text.Visibility = Visibility.Collapsed;
                AttributeMinus.IsEnabled = true;
                AttributePlus.IsEnabled = true;
            }
            if (AttributeIndex == 4)
            {
                Attribute1.Visibility = Visibility.Visible;
                Attribute2.Visibility = Visibility.Visible;
                Attribute3.Visibility = Visibility.Visible;
                Attribute4.Visibility = Visibility.Visible;
                Attribute5.Visibility = Visibility.Collapsed;
                Attribute6.Visibility = Visibility.Collapsed;
                Attribute1Text.Visibility = Visibility.Visible;
                Attribute2Text.Visibility = Visibility.Visible;
                Attribute3Text.Visibility = Visibility.Visible;
                Attribute4Text.Visibility = Visibility.Visible;
                Attribute5Text.Visibility = Visibility.Collapsed;
                Attribute6Text.Visibility = Visibility.Collapsed;
                AttributeMinus.IsEnabled = true;
                AttributePlus.IsEnabled = true;
            }
            if (AttributeIndex == 5)
            {
                Attribute1.Visibility = Visibility.Visible;
                Attribute2.Visibility = Visibility.Visible;
                Attribute3.Visibility = Visibility.Visible;
                Attribute4.Visibility = Visibility.Visible;
                Attribute5.Visibility = Visibility.Visible;
                Attribute6.Visibility = Visibility.Collapsed;
                Attribute1Text.Visibility = Visibility.Visible;
                Attribute2Text.Visibility = Visibility.Visible;
                Attribute3Text.Visibility = Visibility.Visible;
                Attribute4Text.Visibility = Visibility.Visible;
                Attribute5Text.Visibility = Visibility.Visible;
                Attribute6Text.Visibility = Visibility.Collapsed;
                AttributeMinus.IsEnabled = true;
                AttributePlus.IsEnabled = true;
            }
            if (AttributeIndex == 6)
            {
                Attribute1.Visibility = Visibility.Visible;
                Attribute2.Visibility = Visibility.Visible;
                Attribute3.Visibility = Visibility.Visible;
                Attribute4.Visibility = Visibility.Visible;
                Attribute5.Visibility = Visibility.Visible;
                Attribute6.Visibility = Visibility.Visible;
                Attribute1Text.Visibility = Visibility.Visible;
                Attribute2Text.Visibility = Visibility.Visible;
                Attribute3Text.Visibility = Visibility.Visible;
                Attribute4Text.Visibility = Visibility.Visible;
                Attribute5Text.Visibility = Visibility.Visible;
                Attribute6Text.Visibility = Visibility.Visible;
                AttributeMinus.IsEnabled = true;
                AttributePlus.IsEnabled = false;
            };
        }

        public void SetMatlabPID(int pid)
        {
            MatlabPID = pid;
        }

        #endregion

        #region Toolbar

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Minimise_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        #endregion
    }
}



#region Helpers
static class Helper
{
    public static string ReadLine(string text, int lineNumber)
    {
        var reader = new StringReader(text);

        string line;
        int currentLineNumber = 0;

        do
        {
            currentLineNumber += 1;
            line = reader.ReadLine();
        }
        while (line != null && currentLineNumber < lineNumber);

        return (currentLineNumber == lineNumber) ? line :
                                                   string.Empty;
    }

    public static string UntilEquals(this string text, string stopAt = "=")
    {
        if (!String.IsNullOrWhiteSpace(text))
        {
            int charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

            if (charLocation > 0)
            {
                return text.Substring(0, charLocation);
            }
        }

        return String.Empty;
    }

    public static string UntilComma(this string text, string stopAt = ",")
    {
        if (!String.IsNullOrWhiteSpace(text))
        {
            int charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

            if (charLocation > 0)
            {
                return text.Substring(0, charLocation);
            }
        }

        return String.Empty;
    }

    public static decimal Normalize(this decimal value)
    {
        return value / 1.000000000000000000000000000000000m;
    }

    public static void CreateEmptyFileIfNotThere(string filename)
    {
        if (File.Exists(filename) == false)
        {
            File.Create(filename).Dispose();
        }
    }

    public static string AddExtension(string filename, string extension)
    {
        string ext = System.IO.Path.GetExtension(filename);
        if (extension == ext)
        {
            return filename;
        }
        else
        {
            return filename + extension;
        }
    }

    public static async Task RunDatamanagement(int Running, string MATLABInput)
    {
        Task MATLABTask = Task.Run(() => {

            if (Running == 0) // Setup only happens once
            {
                DataManagementSample1.Setup();
                Running = 1;
            }
            DataManagementSample1.DataManagementSample(MATLABInput);

        });
        await MATLABTask;
    }

    public static async Task RunGridSearch(int Running, string MATLABInput, object netCallback)
    {

        Task MATLABTask = Task.Run(() => {

            if (Running == 0) // Setup only happens once
            {
                ForGUI_GridSearchSample1.Setup();
                Running = 1;
            }
            ForGUI_GridSearchSample1.ForGUI_GridSearchSample(MATLABInput, netCallback);

        });
        await MATLABTask;
    }

    public static async Task RunTraining(int Running, string MATLABInput)
    {
        Task MATLABTask = Task.Run(() => {

            if (Running == 0) // Setup only happens once
            {
                TrainingSample1.Setup();
                Running = 1;
            }
            TrainingSample1.TrainingSample(MATLABInput);

        });
        await MATLABTask;
    }
    
    public static async Task RunTesting(int Running, string MATLABInput)
    {
        Task MATLABTask = Task.Run(() => {

            if (Running == 0) // Setup only happens once
            {
                TestingSample1.Setup();
                Running = 1;
            }
            TestingSample1.TestingSample(MATLABInput);

        });
        await MATLABTask;
    }

    public static async Task RunSplit(int Running, string MATLABInput)
    {
        Task MATLABTask = Task.Run(() => {

            if (Running == 0) // Setup only happens once
            {
                SplitSample1.Setup();
                Running = 1;
            }
            SplitSample1.SplitSample(MATLABInput);

        });
        await MATLABTask;
    }

}

class FolderPicker
{
    public virtual string ResultPath { get; protected set; }
    public virtual string ResultName { get; protected set; }
    public virtual string InputPath { get; set; }
    public virtual bool ForceFileSystem { get; set; }
    public virtual string Title { get; set; }
    public virtual string OkButtonLabel { get; set; }
    public virtual string FileNameLabel { get; set; }

    protected virtual int SetOptions(int options)
    {
        if (ForceFileSystem)
        {
            options |= (int)FOS.FOS_FORCEFILESYSTEM;
        }
        return options;
    }

    // for WPF support
    public bool? ShowDialog(Window owner = null, bool throwOnError = false)
    {
        owner = System.Windows.Application.Current.MainWindow;
        return ShowDialog(System.Windows.Interop.HwndSource.FromHwnd(owner != null ? new System.Windows.Interop.WindowInteropHelper(owner).Handle : IntPtr.Zero).RootVisual as Window, throwOnError);
    }

    // for all .NET
    public virtual bool? ShowFolderDialog(IntPtr owner, bool throwOnError = false)
    {
        var dialog = (IFileOpenDialog)new FileOpenDialog();
        if (!string.IsNullOrEmpty(InputPath))
        {
            if (CheckHr(SHCreateItemFromParsingName(InputPath, null, typeof(IShellItem).GUID, out var item), throwOnError) != 0)
                return null;

            dialog.SetFolder(item);
        }

        var options = FOS.FOS_PICKFOLDERS;
        options = (FOS)SetOptions((int)options);
        dialog.SetOptions(options);

        if (Title != null)
        {
            dialog.SetTitle(Title);
        }

        if (OkButtonLabel != null)
        {
            dialog.SetOkButtonLabel(OkButtonLabel);
        }

        if (FileNameLabel != null)
        {
            dialog.SetFileName(FileNameLabel);
        }

        if (owner == IntPtr.Zero)
        {
            owner = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
            if (owner == IntPtr.Zero)
            {
                owner = GetDesktopWindow();
            }
        }

        var hr = dialog.Show(owner);
        if (hr == ERROR_CANCELLED)
            return null;

        if (CheckHr(hr, throwOnError) != 0)
            return null;

        if (CheckHr(dialog.GetResult(out var result), throwOnError) != 0)
            return null;

        if (CheckHr(result.GetDisplayName(SIGDN.SIGDN_DESKTOPABSOLUTEPARSING, out var path), throwOnError) != 0)
            return null;

        ResultPath = path;

        if (CheckHr(result.GetDisplayName(SIGDN.SIGDN_DESKTOPABSOLUTEEDITING, out path), false) == 0)
        {
            ResultName = path;
        }
        return true;
    }

    private static int CheckHr(int hr, bool throwOnError)
    {
        if (hr != 0)
        {
            if (throwOnError)
                Marshal.ThrowExceptionForHR(hr);
        }
        return hr;
    }

    [DllImport("shell32")]
    private static extern int SHCreateItemFromParsingName([MarshalAs(UnmanagedType.LPWStr)] string pszPath, System.Runtime.InteropServices.ComTypes.IBindCtx pbc, [MarshalAs(UnmanagedType.LPStruct)] Guid riid, out IShellItem ppv);
    //IBindCtx
    [DllImport("user32")]
    private static extern IntPtr GetDesktopWindow();

#pragma warning disable IDE1006 // Naming Styles
    private const int ERROR_CANCELLED = unchecked((int)0x800704C7);
#pragma warning restore IDE1006 // Naming Styles

    [ComImport, Guid("DC1C5A9C-E88A-4dde-A5A1-60F82A20AEF7")] // CLSID_FileOpenDialog
    private class FileOpenDialog
    {
    }

    [ComImport, Guid("42f85136-db7e-439c-85f1-e4075d135fc8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    private interface IFileOpenDialog
    {
        [System.Runtime.InteropServices.PreserveSig] int Show(IntPtr parent); // IModalWindow
        [System.Runtime.InteropServices.PreserveSig] int SetFileTypes();  // not fully defined
        [System.Runtime.InteropServices.PreserveSig] int SetFileTypeIndex(int iFileType);
        [System.Runtime.InteropServices.PreserveSig] int GetFileTypeIndex(out int piFileType);
        [System.Runtime.InteropServices.PreserveSig] int Advise(); // not fully defined
        [System.Runtime.InteropServices.PreserveSig] int Unadvise();
        [System.Runtime.InteropServices.PreserveSig] int SetOptions(FOS fos);
        [System.Runtime.InteropServices.PreserveSig] int GetOptions(out FOS pfos);
        [System.Runtime.InteropServices.PreserveSig] int SetDefaultFolder(IShellItem psi);
        [System.Runtime.InteropServices.PreserveSig] int SetFolder(IShellItem psi);
        [System.Runtime.InteropServices.PreserveSig] int GetFolder(out IShellItem ppsi);
        [System.Runtime.InteropServices.PreserveSig] int GetCurrentSelection(out IShellItem ppsi);
        [System.Runtime.InteropServices.PreserveSig] int SetFileName([MarshalAs(UnmanagedType.LPWStr)] string pszName);
        [System.Runtime.InteropServices.PreserveSig] int GetFileName([MarshalAs(UnmanagedType.LPWStr)] out string pszName);
        [System.Runtime.InteropServices.PreserveSig] int SetTitle([MarshalAs(UnmanagedType.LPWStr)] string pszTitle);
        [System.Runtime.InteropServices.PreserveSig] int SetOkButtonLabel([MarshalAs(UnmanagedType.LPWStr)] string pszText);
        [System.Runtime.InteropServices.PreserveSig] int SetFileNameLabel([MarshalAs(UnmanagedType.LPWStr)] string pszLabel);
        [System.Runtime.InteropServices.PreserveSig] int GetResult(out IShellItem ppsi);
        [System.Runtime.InteropServices.PreserveSig] int AddPlace(IShellItem psi, int alignment);
        [System.Runtime.InteropServices.PreserveSig] int SetDefaultExtension([MarshalAs(UnmanagedType.LPWStr)] string pszDefaultExtension);
        [System.Runtime.InteropServices.PreserveSig] int Close(int hr);
        [System.Runtime.InteropServices.PreserveSig] int SetClientGuid();  // not fully defined
        [System.Runtime.InteropServices.PreserveSig] int ClearClientData();
        [System.Runtime.InteropServices.PreserveSig] int SetFilter([MarshalAs(UnmanagedType.IUnknown)] object pFilter);
        [System.Runtime.InteropServices.PreserveSig] int GetResults([MarshalAs(UnmanagedType.IUnknown)] out object ppenum);
        [System.Runtime.InteropServices.PreserveSig] int GetSelectedItems([MarshalAs(UnmanagedType.IUnknown)] out object ppsai);
    }

    [ComImport, Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    private interface IShellItem
    {
        [System.Runtime.InteropServices.PreserveSig] int BindToHandler(); // not fully defined
        [System.Runtime.InteropServices.PreserveSig] int GetParent(); // not fully defined
        [System.Runtime.InteropServices.PreserveSig] int GetDisplayName(SIGDN sigdnName, [MarshalAs(UnmanagedType.LPWStr)] out string ppszName);
        [System.Runtime.InteropServices.PreserveSig] int GetAttributes();  // not fully defined
        [System.Runtime.InteropServices.PreserveSig] int Compare();  // not fully defined
    }

#pragma warning disable CA1712 // Do not prefix enum values with type name
    private enum SIGDN : uint
    {
        SIGDN_DESKTOPABSOLUTEEDITING = 0x8004c000,
        SIGDN_DESKTOPABSOLUTEPARSING = 0x80028000,
        SIGDN_FILESYSPATH = 0x80058000,
        SIGDN_NORMALDISPLAY = 0,
        SIGDN_PARENTRELATIVE = 0x80080001,
        SIGDN_PARENTRELATIVEEDITING = 0x80031001,
        SIGDN_PARENTRELATIVEFORADDRESSBAR = 0x8007c001,
        SIGDN_PARENTRELATIVEPARSING = 0x80018001,
        SIGDN_URL = 0x80068000
    }

    [Flags]
    private enum FOS
    {
        FOS_OVERWRITEPROMPT = 0x2,
        FOS_STRICTFILETYPES = 0x4,
        FOS_NOCHANGEDIR = 0x8,
        FOS_PICKFOLDERS = 0x20,
        FOS_FORCEFILESYSTEM = 0x40,
        FOS_ALLNONSTORAGEITEMS = 0x80,
        FOS_NOVALIDATE = 0x100,
        FOS_ALLOWMULTISELECT = 0x200,
        FOS_PATHMUSTEXIST = 0x800,
        FOS_FILEMUSTEXIST = 0x1000,
        FOS_CREATEPROMPT = 0x2000,
        FOS_SHAREAWARE = 0x4000,
        FOS_NOREADONLYRETURN = 0x8000,
        FOS_NOTESTFILECREATE = 0x10000,
        FOS_HIDEMRUPLACES = 0x20000,
        FOS_HIDEPINNEDPLACES = 0x40000,
        FOS_NODEREFERENCELINKS = 0x100000,
        FOS_OKBUTTONNEEDSINTERACTION = 0x200000,
        FOS_DONTADDTORECENT = 0x2000000,
        FOS_FORCESHOWHIDDEN = 0x10000000,
        FOS_DEFAULTNOMINIMODE = 0x20000000,
        FOS_FORCEPREVIEWPANEON = 0x40000000,
        FOS_SUPPORTSTREAMABLEITEMS = unchecked((int)0x80000000)
    }
#pragma warning restore CA1712 // Do not prefix enum values with type name
}

class FilePicker
{
    public static void MATLAB(System.Windows.Controls.TextBox TextBox)
    {
        Microsoft.Win32.OpenFileDialog openFileDialog1 = new Microsoft.Win32.OpenFileDialog
        {
            Title = "Browse MATLAB Data Files",
            DefaultExt = "mat",
            Filter = "mat files (*.mat)|*.mat",
            RestoreDirectory = true,
        };
        if (openFileDialog1.ShowDialog() == Convert.ToBoolean(DialogResult.OK))
        {
            TextBox.Text = openFileDialog1.FileName;
        }
    }
    public static void CSV(System.Windows.Controls.TextBox TextBox)
    {
        Microsoft.Win32.OpenFileDialog openFileDialog1 = new Microsoft.Win32.OpenFileDialog
        {
            Title = "Browse CSV Data Files",
            DefaultExt = "csv",
            Filter = "csv files (*.csv)|*.csv",
            RestoreDirectory = true,
        };
        if (openFileDialog1.ShowDialog() == Convert.ToBoolean(DialogResult.OK))
        {
            TextBox.Text = openFileDialog1.FileName;
        }
    }
}

class clearmexSample1
{

    static clearmex.Class1 class1Instance;

    public static void Setup()
    {
        class1Instance = new clearmex.Class1();
    }

    /// <summary>
    /// Example of using the clearmex function.
    /// </summary>
    public static void ClearmexSample()
    {
        try
        {
            class1Instance.clearmex();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

}

class DataManagementSample1
{

    static DataManagement.Class1 class1Instance;

    public static void Setup()
    {
        class1Instance = new DataManagement.Class1();
    }

    public static void DataManagementSample(string rootInData)
    {
        try
        {
            MWCharArray rootIn = new MWCharArray(rootInData);
            class1Instance.DataManagement(rootIn);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}

class ForGUI_GridSearchSample1 : System.ComponentModel.Component, IDisposable
{

    static ForGUI_GridSearch.Class1 class1Instance;

    public static void Setup()
    {
        class1Instance = new ForGUI_GridSearch.Class1();
    }

    public static void ForGUI_GridSearchSample(string rootInData, object netCallback)
    {
        //using (Process myProcess = new Process())
        //{
        try
        {
                MWCharArray rootIn = new MWCharArray(rootInData);
                class1Instance.ForGUI_GridSearch(rootIn, new MWObjectArray(netCallback));

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        //}
    }
}

class TrainingSample1
{

    static Training.Class1 class1Instance;

    public static void Setup()
    {
        class1Instance = new Training.Class1();
    }

    public static void TrainingSample(string rootInData)
    {
        try
        {
            MWCharArray rootIn = new MWCharArray(rootInData);
            class1Instance.Training(rootIn);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}

class TestingSample1
{

    static Testing.Class1 class1Instance;

    public static void Setup()
    {
        class1Instance = new Testing.Class1();
    }

    public static void TestingSample(string rootInData)
    {
        try
        {
            MWCharArray rootIn = new MWCharArray(rootInData);
            class1Instance.Testing(rootIn);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}

class SplitSample1
{

    static Split.Class1 class1Instance;

    public static void Setup()
    {
        class1Instance = new Split.Class1();
    }

    public static void SplitSample(string rootInData)
    {
        try
        {
            MWCharArray rootIn = new MWCharArray(rootInData);
            class1Instance.Split(rootIn);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

}

#endregion

// To Do:
// Force quit function
// Write a README.txt
// Check if all the numbers are numbers and not invalid characters or empty
// Make a flowchart for using the app
// Add ToolTips for everything that needs it in app




// README.txt



// What I Would Do In The Future:
// Allow CSV files and not just matlab files to be put in the training data and testing data slots
// Add cross-platform
// Allow files to be imported that have incomplete data i.e. if some columns have fewer data points than others
// Remake the backend so that it either doen't use MATLAB at all, or use the MATLAB coder so that the MATLAB
// runtime isn't needed for the client
// Allow an unrestricted number of attributes be used in testing and training
// Exported the converted data as CSV files
// make the plots interactive 
// Output more informative result when not working such as "not enough data for requested length"
// Implement the .NET or C# wrappers of LIBSVM if possible
// Add saving and using app presets to quickly load settings and directories
// Add a preview of the exported files to the Data Management window
// Ask if the user is sure they want to close the window during training/testing if they click the close button
// Add MATLAB command window text to dropdown