using System;
using System.Windows;
using System.Windows.Controls;

namespace LIBSVM_GUI_Template_test
{
    public class VisibilityClass : ObservableObject
    {
        public const Visibility Collapsed = Visibility.Collapsed;
        public const Visibility Visible = Visibility.Visible;
        public Visibility train1V;     //     = Visible   
        public Visibility train2V;     //     =Visible    
        public Visibility train3V;     //     =Collapsed  
        public Visibility train4V;     //     =Collapsed  
        public Visibility train5V;     //     =Collapsed  
        public Visibility train6V;     //     =Collapsed  
        public Visibility train1TextV;     //     =Visible   
        public Visibility train2TextV;     //     =Visible   
        public Visibility train3TextV;     //     =Collapsed 
        public Visibility train4TextV;     //     =Collapsed 
        public Visibility train5TextV;     //     =Collapsed 
        public Visibility train6TextV;     //     =Collapsed 
        public Visibility trainMinusV;     //     =Visible   
        public Visibility trainPlusV;     //     =Visible   
        public Visibility test1V;     //     =Visible     
        public Visibility test2V;     //     =Visible     
        public Visibility test3V;     //     =Collapsed   
        public Visibility test4V;     //     =Collapsed   
        public Visibility test5V;     //     =Collapsed   
        public Visibility test6V;     //     =Collapsed   
        public Visibility test1TextV;     //     =Visible   
        public Visibility test2TextV;     //     =Visible   
        public Visibility test3TextV;     //     =Collapsed 
        public Visibility test4TextV;     //     =Collapsed 
        public Visibility test5TextV;     //     =Collapsed 
        public Visibility test6TextV;     //     =Collapsed 
        public Visibility testMinusV;     //     =Visible   
        public Visibility testPlusV;     //     =Visible

        public Visibility Train1V
        {
            get { return train1V; }
            set
            {
                if (train1V != value)
                {
                    train1V = value;
                    OnPropertyChanged();
                }
            }
        }
        public Visibility Train2V
        {
            get { return train2V; }
            set
            {
                if (train2V != value)
                {
                    train2V = value;
                    OnPropertyChanged();
                }
            }
        }
        public Visibility Train3V
        {
            get { return train3V; }
            set
            {
                if (train3V != value)
                {
                    train3V = value;
                    OnPropertyChanged();
                }
            }
        }
        public Visibility Train4V
        {
            get { return train4V; }
            set
            {
                if (train4V != value)
                {
                    train4V = value;
                    OnPropertyChanged();
                }
            }
        }
        public Visibility Train5V
        {
            get { return train5V; }
            set
            {
                if (train5V != value)
                {
                    train5V = value;
                    OnPropertyChanged();
                }
            }
        }
        public Visibility Train6V
        {
            get { return train6V; }
            set
            {
                if (train6V != value)
                {
                    train6V = value;
                    OnPropertyChanged();
                }
            }
        }
        public Visibility Train1TextV
        {
            get { return train1TextV; }
            set
            {
                if (train1TextV != value)
                {
                    train1TextV = value;
                    OnPropertyChanged();
                }
            }
        }
        public Visibility Train2TextV
        {
            get { return train2TextV; }
            set
            {
                if (train2TextV != value)
                {
                    train2TextV = value;
                    OnPropertyChanged();
                }
            }
        }
        public Visibility Train3TextV
        {
            get { return train3TextV; }
            set
            {
                if (train3TextV != value)
                {
                    train3TextV = value;
                    OnPropertyChanged();
                }
            }
        }
        public Visibility Train4TextV
        {
            get { return train4TextV; }
            set
            {
                if (train4TextV != value)
                {
                    train4TextV = value;
                    OnPropertyChanged();
                }
            }
        }
        public Visibility Train5TextV
        {
            get { return train5TextV; }
            set
            {
                if (train5TextV != value)
                {
                    train5TextV = value;
                    OnPropertyChanged();
                }
            }
        }
        public Visibility Train6TextV
        {
            get { return train6TextV; }
            set
            {
                if (train6TextV != value)
                {
                    train6TextV = value;
                    OnPropertyChanged();
                }
            }
        }
        public Visibility TrainMinusV
        {
            get { return trainMinusV; }
            set
            {
                if (trainMinusV != value)
                {
                    trainMinusV = value;
                    OnPropertyChanged();
                }
            }
        }
        public Visibility TrainPlusV
        {
            get { return trainPlusV; }
            set
            {
                if (trainPlusV != value)
                {
                    trainPlusV = value;
                    OnPropertyChanged();
                }
            }
        }
        public Visibility Test1V
        {
            get { return test1V; }
            set
            {
                if (test1V != value)
                {
                    test1V = value;
                    OnPropertyChanged();
                }
            }
        }
        public Visibility Test2V
        {
            get { return test2V; }
            set
            {
                if (test2V != value)
                {
                    test2V = value;
                    OnPropertyChanged();
                }
            }
        }
        public Visibility Test3V
        {
            get { return test3V; }
            set
            {
                if (test3V != value)
                {
                    test3V = value;
                    OnPropertyChanged();
                }
            }
        }
        public Visibility Test4V
        {
            get { return test4V; }
            set
            {
                if (test4V != value)
                {
                    test4V = value;
                    OnPropertyChanged();
                }
            }
        }
        public Visibility Test5V
        {
            get { return test5V; }
            set
            {
                if (test5V != value)
                {
                    test5V = value;
                    OnPropertyChanged();
                }
            }
        }
        public Visibility Test6V
        {
            get { return test6V; }
            set
            {
                if (test6V != value)
                {
                    test6V = value;
                    OnPropertyChanged();
                }
            }
        }
        public Visibility Test1TextV
        {
            get { return test1TextV; }
            set
            {
                if (test1TextV != value)
                {
                    test1TextV = value;
                    OnPropertyChanged();
                }
            }
        }
        public Visibility Test2TextV
        {
            get { return test2TextV; }
            set
            {
                if (test2TextV != value)
                {
                    test2TextV = value;
                    OnPropertyChanged();
                }
            }
        }
        public Visibility Test3TextV
        {
            get { return test3TextV; }
            set
            {
                if (test3TextV != value)
                {
                    test3TextV = value;
                    OnPropertyChanged();
                }
            }
        }
        public Visibility Test4TextV
        {
            get { return test4TextV; }
            set
            {
                if (test4TextV != value)
                {
                    test4TextV = value;
                    OnPropertyChanged();
                }
            }
        }
        public Visibility Test5TextV
        {
            get { return test5TextV; }
            set
            {
                if (test5TextV != value)
                {
                    test5TextV = value;
                    OnPropertyChanged();
                }
            }
        }
        public Visibility Test6TextV
        {
            get { return test6TextV; }
            set
            {
                if (test6TextV != value)
                {
                    test6TextV = value;
                    OnPropertyChanged();
                }
            }
        }
        public Visibility TestMinusV
        {
            get { return testMinusV; }
            set
            {
                if (testMinusV != value)
                {
                    testMinusV = value;
                    OnPropertyChanged();
                }
            }
        }
        public Visibility TestPlusV
        {
            get { return testPlusV; }
            set
            {
                if (testPlusV != value)
                {
                    testPlusV = value;
                    OnPropertyChanged();
                }
            }
        }

    }
}
